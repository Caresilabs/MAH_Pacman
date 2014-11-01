using MAH_Pacman.Entity;
using MAH_Pacman.Entity.Components;
using MAH_Pacman.Entity.Systems;
using MAH_Pacman.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.AI
{
    public abstract class AIController
    {
        public enum State
        {
            SCATTER, CHASE, FRIGHTENED, NORMAL, DEAD
        }

        private Point spawn;
        private Point target;
        private Point lastTurn;
        private State state;
        private GameEntity pacman;
        private GameEntity entity;

        private float stateTime;

        public AIController(GameEntity ai, int x, int y)
        {
            this.target = new Point();
            this.spawn = new Point(x, y);
            this.lastTurn = new Point();
            this.entity = ai;
            this.pacman = World.pacman;
            this.state = State.SCATTER;
            this.stateTime = 0;
            this.ChangedState(state);
        }

        protected abstract void TargetReached();

        protected abstract void ChangedState(State state);

        public virtual void Update(float delta)
        {
            stateTime += delta;

            MovementComponent movement = entity.GetComponent<MovementComponent>();
            TransformationComponent transform = entity.GetComponent<TransformationComponent>();
            AIComponent aic = entity.GetComponent<AIComponent>();

            UpdateAI(movement, transform);
            UpdateAIState(state);
        }

        protected virtual void UpdateAIState(State state)
        {
            switch (state)
            {
                case State.SCATTER:
                    if (stateTime > 5)
                        SetState(State.CHASE);
                    break;
                case State.CHASE:
                    if (stateTime > 7)
                        SetState(State.NORMAL);
                    break;
                case State.FRIGHTENED:
                    break;
                case State.NORMAL:
                    break;
                case State.DEAD:
                    break;
                default:
                    break;
            }
        }

        private void UpdateAI(MovementComponent movement, TransformationComponent transform)
        {
            // 1. Check if turn is available
            if (!(lastTurn.X == transform.GetIntX() && lastTurn.Y == transform.GetIntY())
                    && entity.engine.GetSystem<GridSystem>().HasWalkedHalf(movement.velocity, transform))
            {
                Point? right = null;
                Point? left = null;
                Point? forward = null;

                // Check 'Right'
                if (entity.engine.GetSystem<GridSystem>().IsWalkable(entity, new Vector2(movement.velocity.Y, movement.velocity.X)))
                    right = new Point((int)transform.position.X + Math.Sign(movement.velocity.Y), (int)transform.position.Y + Math.Sign(movement.velocity.X));

                // Check 'Left'
                if (entity.engine.GetSystem<GridSystem>().IsWalkable(entity, new Vector2(-movement.velocity.Y, -movement.velocity.X)))
                    left = new Point((int)transform.position.X + Math.Sign(-movement.velocity.Y), (int)transform.position.Y + Math.Sign(-movement.velocity.X));

                // Check 'forward'
                if (entity.engine.GetSystem<GridSystem>().IsWalkable(entity, new Vector2(movement.velocity.X, movement.velocity.Y)))
                    forward = new Point((int)transform.position.X + Math.Sign(movement.velocity.X), (int)transform.position.Y + Math.Sign(movement.velocity.Y));

                // 2. Check if only one way or 2/3 ways
                TryChangeDirection(movement, transform, right, left, forward);
            }
        }

        private void TryChangeDirection(MovementComponent movement, TransformationComponent transform, Point? right, Point? left, Point? forward)
        {
            if (left != null || right != null || forward != null)
            {
                // 3. Check fastest way to target
                Point fastest = new Point(-1000, 1000);

                if (left != null)
                    if (GetDistance(left.Value, target) < GetDistance(target, fastest)) fastest = left.Value;

                if (right != null)
                    if (GetDistance(right.Value, target) < GetDistance(target, fastest)) fastest = right.Value;

                if (forward != null)
                    if (GetDistance(forward.Value, target) < GetDistance(target, fastest)) fastest = forward.Value;

                // 4. Walk that way!
                Point[] path = entity.engine.GetSystem<GridSystem>().entities[0].GetComponent<GridComponent>().Pathfind(new Point(transform.GetIntX(), transform.GetIntY()), target).ToArray();

                // notify ghost that target is reached
                if (path.Count() == 1) TargetReached();

                if (path.Count() > 1)
                {
                    if (GetDistance(path[0], path[1]) > 1)
                    {
                        if (path[1].X >= World.WIDTH - 1)
                            path[1].X = (path[1].X) - World.WIDTH;

                        if (path[1].Y >= World.HEIGHT - 1)
                            path[1].Y = (path[1].Y) - World.HEIGHT;
                    }

                    movement.velocity.X = Math.Sign(path[1].X - transform.GetIntX()) * entity.GetComponent<AIComponent>().speed;
                    movement.velocity.Y = Math.Sign(path[1].Y - transform.GetIntY()) * entity.GetComponent<AIComponent>().speed;

                    transform.position.X = (int)(transform.position.X + .5f);
                    transform.position.Y = (int)(transform.position.Y + .5f);

                    movement.halt = false;

                    lastTurn.X = (int)transform.position.X;
                    lastTurn.Y = (int)transform.position.Y;
                }
                else if (path.Count() == 0)
                {
                    // Do nearest algorithm
                    if (fastest.X != -1000)
                    {
                        movement.velocity.X = Math.Sign(fastest.X - (int)transform.position.X) * entity.GetComponent<AIComponent>().speed;
                        movement.velocity.Y = Math.Sign(fastest.Y - (int)transform.position.Y) * entity.GetComponent<AIComponent>().speed;

                        transform.position.X = (int)(transform.position.X + .5f);
                        transform.position.Y = (int)(transform.position.Y + .5f);

                        movement.halt = false;

                        lastTurn.X = (int)transform.position.X;
                        lastTurn.Y = (int)transform.position.Y;

                    }
                }
            }
            else
            {
                // Turn around
                movement.velocity.X *= -1;
                movement.velocity.Y *= -1;

                movement.halt = false;

                lastTurn.X = (int)transform.position.X;
                lastTurn.Y = (int)transform.position.Y;
            }
        }

        protected void TargetPacman()
        {
            SetTarget(pacman.GetComponent<TransformationComponent>().GetIntX(), pacman.GetComponent<TransformationComponent>().GetIntY());
        }

        // Return true if pacman dies
        public bool CollideWithPacman()
        {
            switch (state)
            {
                case State.SCATTER:
                    break;
                case State.CHASE:
                    SetState(State.SCATTER);
                    break;
                case State.FRIGHTENED:
                    SetState(State.DEAD);
                    entity.GetComponent<AnimationComponent>().Set("dead");
                    entity.GetComponent<TransformationComponent>().hasCollision = false;
                    return false;
                case State.NORMAL:
                    SetState(State.SCATTER);
                    break;
                case State.DEAD:
                    return false;
                default:
                    break;
            }
            return true;
        }

        private float GetDistance(Point a, Point b)
        {
            Point delta = new Point(a.X - b.X, a.Y - b.Y);
            return (float)Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y));
        }

        protected GameEntity GetPacman()
        {
            return pacman;
        }

        protected GameEntity GetEntity()
        {
            return entity;
        }

        protected Point GetSpawn()
        {
            return spawn;
        }

        public void SetState(State state)
        {
            stateTime = 0;

            // update state
            switch (state)
            {
                case State.SCATTER:
                    entity.GetComponent<AnimationComponent>().Set("walk");
                    break;
                case State.CHASE:
                    entity.GetComponent<AnimationComponent>().Set("walk");
                    break;
                case State.FRIGHTENED:
                    entity.GetComponent<AnimationComponent>().Set("frightened");
                    break;
                case State.NORMAL:
                    entity.GetComponent<AnimationComponent>().Set("walk");
                    break;
                case State.DEAD:
                    entity.GetComponent<AnimationComponent>().Set("dead");
                    break;
                default:
                    break;
            }

            // new State
            this.state = state;
            ChangedState(state);
        }

        public State GetState()
        {
            return state;
        }

        public void SetTarget(int x, int y)
        {
            target.X = x;
            target.Y = y;
        }

        public void SetTarget(Point target)
        {
            this.target.X = target.X;
            this.target.Y = target.Y;
        }


        public void Respawn()
        {
            this.target = new Point();
            this.entity.GetComponent<TransformationComponent>().position = new Vector2(spawn.X, spawn.Y);
            SetState(State.SCATTER);
        }
    }
}
