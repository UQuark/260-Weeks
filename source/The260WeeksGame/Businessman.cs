﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The260WeeksGame
{
    class Businessman : GameMember
    {
        private string name = "";

        public int ServicePoint;

        public int AbsoluteLoyalty;
        public double AdjustedLoyalty
        {
            get
            {
                return adjust(AbsoluteLoyalty);
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Businessman(string name, int absoluteRating, int absoluteLoyalty, int servicePoint)
        {
            this.name = name;
            AbsoluteRating = absoluteRating;
            AbsoluteLoyalty = absoluteLoyalty;
            ServicePoint = servicePoint;
        }

        public override void Turn() {

        }

        public static Businessman GenerateRandom()
        {
            var firstName = GameCore.RandomObjectFromList(GameCore.FirstNameList);
            var secondName = GameCore.RandomObjectFromList(GameCore.SecondNameList); 

            var fullName = firstName + " " + secondName;
            var rating = GameCore.random.Next(-10, 30);
            int loyalty = 0;

            switch (GameCore.GameDifficulty)
            {
                case GameCore.Difficulty.Easy:
                    loyalty = GameCore.random.Next(5, 20);
                    break;
                case GameCore.Difficulty.Moderate:
                    loyalty = GameCore.random.Next(-5, 20);
                    break;
                case GameCore.Difficulty.Medium:
                    loyalty = GameCore.random.Next(-10, 10);
                    break;
                case GameCore.Difficulty.Complicated:
                    loyalty = GameCore.random.Next(-20, 5);
                    break;
                case GameCore.Difficulty.Hard:
                    loyalty = GameCore.random.Next(-20, -5);
                    break;
            }

            Businessman result = new Businessman(fullName, rating, loyalty, 0);
            GameCore.SecondNameList.Remove(secondName);
            return result;
        }
    }
}
