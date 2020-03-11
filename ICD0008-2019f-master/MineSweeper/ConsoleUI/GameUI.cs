using System;
using System.ComponentModel;
using Domain;
using GameEngine;

namespace ConsoleUI
{
    public static class GameUI
    {
        private static readonly string _verticalSeparator = "|";
        private static readonly string _horizontalSeparator = "-";
        private static readonly string _centerSeparator = "+";
        
        public static void PrintBoard(GameController game)
        {
            var board = game.Board;
            for (int yIndex = 0; yIndex < game.CurrentGame.Height; yIndex++)
            {
                var line = "";
                for (int xIndex = 0; xIndex < game.CurrentGame.Width; xIndex++)
                {
                    if (!game.Board[yIndex, xIndex].IsBomb && game.Board[yIndex, xIndex].IsOpened &&
                        game.CountNeighbours(yIndex, xIndex) > 0)
                    {
                        line = line + " " + game.CountNeighbours(yIndex, xIndex) + " ";
                    }
                    else
                    {
                        line = line + " " + game.GetVisualClassConsole(yIndex, xIndex) + " ";    
                    }
                    if (xIndex < game.CurrentGame.Width - 1)
                    {
                        line = line + _verticalSeparator;
                    }
                }
                
                Console.WriteLine(line);

                if (yIndex < game.CurrentGame.Height - 1)
                {
                    line = "";
                    for (int xIndex = 0; xIndex < game.CurrentGame.Width; xIndex++)
                    {
                        line = line + _horizontalSeparator+ _horizontalSeparator+ _horizontalSeparator;
                        if (xIndex < game.CurrentGame.Width - 1)
                        {
                            line = line +_centerSeparator;
                        }
                    }
                    Console.WriteLine(line);
                }

                
            }
        }
    }
}