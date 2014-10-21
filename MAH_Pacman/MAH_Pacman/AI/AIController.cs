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
            SCATTER, CHASE, FRIGHTENED, NORMAL
        }

        private Point spawn;
        private Point target;
        private Point lastTurn;
        private State state;
        private GameEntity pacman;
        private GameEntity ai;

        public AIController(GameEntity ai)
        {
            this.target = new Point();
            this.spawn = new Point();
            this.lastTurn = new Point();
            this.ai = ai;
            this.pacman = World.pacman;
            this.state = State.SCATTER;
            ChangedState(state);
        }

        public virtual void Update(float delta)
        {
            MovementComponent movement = ai.GetComponent<MovementComponent>();
            TransformationComponent transform = ai.GetComponent<TransformationComponent>();
            AIComponent aic = ai.GetComponent<AIComponent>();

            SetTarget(pacman.GetComponent<TransformationComponent>().GetIntX(), pacman.GetComponent<TransformationComponent>().GetIntY());

            // 1. Check if turn is available
            if (!(lastTurn.X == transform.GetIntX() && lastTurn.Y == transform.GetIntY())
                    && ai.engine.GetSystem<GridSystem>().HasWalkedHalf(movement.velocity, transform))
            {
                Point? right = null;
                Point? left = null;
                Point? forward = null;

                // Check 'Right'
                if (ai.engine.GetSystem<GridSystem>().IsWalkable(ai, new Vector2(movement.velocity.Y, movement.velocity.X)))
                {
                    right = new Point((int)transform.position.X + Math.Sign(movement.velocity.Y), (int)transform.position.Y + Math.Sign(movement.velocity.X));
                }

                // Check 'Left'
                if (ai.engine.GetSystem<GridSystem>().IsWalkable(ai, new Vector2(-movement.velocity.Y, -movement.velocity.X)))
                {
                    left = new Point((int)transform.position.X + Math.Sign(-movement.velocity.Y), (int)transform.position.Y + Math.Sign(-movement.velocity.X));
                }

                // Check 'forward'
                if (ai.engine.GetSystem<GridSystem>().IsWalkable(ai, new Vector2(movement.velocity.X, movement.velocity.Y)))
                {
                    forward = new Point((int)transform.position.X + Math.Sign(movement.velocity.X), (int)transform.position.Y + Math.Sign(movement.velocity.Y));
                }

                // 2. Check if only one way or 2/3 ways
                if (left != null || right != null || forward != null)
                {
                    // 3. Check fastest way to target
                    Point fastest = new Point(-1000, 1000);

                    if (left != null)
                        if (GetDistance(left.Value, target) < GetDistance(target, fastest)) fastest = left.Value;

                    if (right != null)
                        if (GetDistance(right.Value, target) < GetDistance(target, fastest)) fastest = right.Value;

                    if (forward != null)
                        if (GetDistance(forward.Value, target) < GetDistance(target, fastest))
                            return;

                    // 4. Walk that way!
                    if (fastest.X != -1000)
                    {
                        movement.velocity.X = Math.Sign(fastest.X - (int)transform.position.X) * ai.GetComponent<AIComponent>().speed;
                        movement.velocity.Y = Math.Sign(fastest.Y - (int)transform.position.Y) * ai.GetComponent<AIComponent>().speed;

                        transform.position.X = (int)(transform.position.X + .5f);
                        transform.position.Y = (int)(transform.position.Y + .5f);

                        movement.halt = false;

                        lastTurn.X = (int)transform.position.X;
                        lastTurn.Y = (int)transform.position.Y;

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

        protected abstract void ChangedState(State state);

        public void SetTarget(int x, int y)
        {
            target.X = x;
            target.Y = y;
        }
    }
}
