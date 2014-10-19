﻿using MAH_Pacman.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MAH_Pacman
{
    public static class LevelIO
    {
        public static int[,] ReadLevel(int lvl)
        {
            StreamReader sr = new StreamReader(@"Content/levels.txt");
            string input = sr.ReadToEnd().ToString().Replace("\r", "").Replace("\n", "").Replace(" ", "");
            sr.Close();

            MatchCollection matches = Regex.Matches(input, @"\[[^\]]+\]");

            string map = matches[lvl - 1].Value.Replace("[", "").Replace("]", "");

            string[] stringParts = map.Split(',');
            int[,] loadedMap = new int[World.WIDTH, World.HEIGHT];

            for (int j = 0; j < World.HEIGHT; j++)
            {
                for (int i = 0; i < World.WIDTH; i++)
                {
                    loadedMap[i, j] = int.Parse(stringParts[i + (j * World.WIDTH)]);
                }
            }

            return loadedMap;
        }

        public static void WriteLevel(int[,] levelToWrite)
        {
            //TODO
        }
    }
}