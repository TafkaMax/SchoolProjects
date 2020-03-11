using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;


namespace GameEngine
{
    /// <summary>
    /// MineSweeper
    /// </summary>
    public class GameController
    {
        // null, X, O
        public Cell[,] Board { get; set; }

        public Game CurrentGame { get; set; }

        private const double DefaultDifficultyPercent = 0.16;

        public GameDbContext _context;


        public GameController(GameDbContext ctx, GameSetting settings)
        {
            if (settings.BoardHeight < 8 || settings.BoardWidth < 8)
            {
                throw new ArgumentException("Board size has to be at least 8x8!");
            }

            _context = ctx;
            CurrentGame = new Game
            {
                Height = settings.BoardHeight, Width = settings.BoardWidth, FirstMove = true,
                DifficultyLevel = settings.DifficultyLevel, GameName = settings.GameName
            };
            Board = GenerateNewBoard();
            ctx.Games.Add(CurrentGame);
            ctx.SaveChanges();
        }


        public GameController(GameDbContext ctx, int gameId)
        {
            _context = ctx;
            CurrentGame = ctx.Games.Include(g => g.Cells)
                .Single(g => g.GameId == gameId);
            Board = LoadBoard();
        }

        private Cell[,] LoadBoard()
        {
            Cell[,] board = new Cell[CurrentGame.Height, CurrentGame.Width];
            foreach (var cell in CurrentGame.Cells)
            {
                board[cell.y, cell.x] = cell;
            }

            return board;
        }


        public Cell[,] GenerateNewBoard()
        {
            Cell[,] board = new Cell[CurrentGame.Height, CurrentGame.Width];
            for (int i = 0; i < CurrentGame.Width; i++)
            {
                for (int j = 0; j < CurrentGame.Height; j++)
                {
                    board[j, i] = new Cell();
                    board[j, i].y = j;
                    board[j, i].x = i;
                    CurrentGame.Cells.Add(board[j, i]);
                }
            }

            return board;
        }

        public String GetVisualClass(int y, int x)
        {
            if (Board[y, x].IsOpened)
            {
                if (Board[y, x].IsBomb)
                {
                    return "mine";
                }
                else
                {
                    CountNeighbours(y, x);
                    return "open";
                }
            }
            else
            {
                if (Board[y, x].IsBomb)
                {
                    if (Board[y, x].IsFlag)
                    {
                        if (CurrentGame.GameLost || CurrentGame.GameWon)
                        {
                            return "unopenedbomb";
                        }
                        else
                        {
                            return "flag";
                        }
                    }
                    else
                    {
                        if (CurrentGame.GameLost)
                        {
                            return "unopenedbomb";
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
                else
                {
                    if (Board[y, x].IsFlag)
                    {
                        if (CurrentGame.GameLost)
                        {
                            return "wrongflag";
                        }
                        else
                        {
                            return "flag";
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
            }
        }

        public String GetVisualClassConsole(int y, int x)
        {
            if (Board[y, x].IsOpened)
            {
                if (Board[y, x].IsBomb)
                {
                    return "X";
                }
                else
                {
                    CountNeighbours(y, x);
                    return "\u25A0";
                }
            }
            else
            {
                if (Board[y, x].IsBomb)
                {
                    if (Board[y, x].IsFlag)
                    {
                        if (CurrentGame.GameLost || CurrentGame.GameWon)
                        {
                            return "O";
                        }
                        else
                        {
                            return "F";
                        }
                    }
                    else
                    {
                        if (CurrentGame.GameLost)
                        {
                            return "O";
                        }
                        else
                        {
                            return 	" ";
                        }
                    }
                }
                else
                {
                    if (Board[y, x].IsFlag)
                    {
                        if (CurrentGame.GameLost)
                        {
                            return "W";
                        }
                        else
                        {
                            return "F";
                        }
                    }
                    else
                    {
                        return " ";
                    }
                }
            }
        }

        public int CountNeighbours(int y, int x)
        {
            int count = 0;
            for (int i = y - 1; i <= y + 1; i++)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    if (j >= 0 && j < CurrentGame.Width && i >= 0 && i < CurrentGame.Height)
                    {
                        if (Board[i, j].IsBomb)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        public void OpenPosition(int posY, int posX)
        {
            if (!Board[posY, posX].IsOpened && !Board[posY, posX].IsFlag)
            {
                if (Board[posY, posX].IsBomb)
                {
                    CurrentGame.GameLost = true;
                }

                Board[posY, posX].IsOpened = true;

                if (CurrentGame.FirstMove)
                {
                    GenerateBombs();
                    CurrentGame.FirstMove = false;
                }

                if (CountNeighbours(posY, posX) == 0)
                {
                    for (int i = posY - 1; i <= posY + 1; i++)
                    {
                        for (int j = posX - 1; j <= posX + 1; j++)
                        {
                            if (j >= 0 && j < CurrentGame.Width && i >= 0 && i < CurrentGame.Height)
                            {
                                OpenPosition(i, j);
                            }
                        }
                    }
                }

                CurrentGame.GameWon = isWon();

                _context.SaveChanges();
            }
        }

        private bool isWon()
        {
            foreach (var cell in CurrentGame.Cells)
            {
                if (!cell.IsOpened && !cell.IsBomb)
                {
                    return false;
                }
            }

            return true;
        }

        public void CleanGame()
        {
            _context.Games.Remove(CurrentGame);
            _context.SaveChanges();
        }

        public void ChangeFlag(int posY, int posX)
        {
            if (!Board[posY, posX].IsOpened)
            {
                Board[posY, posX].IsFlag = !Board[posY, posX].IsFlag;
            }

            _context.SaveChanges();
        }

        public void GenerateBombs()
        {
            //kui difficulty lvl 1, panna % lauast pomme, kui lvl 2 suurem % pomme jne, oleneb board sizeist!
            Random bombLocation = new Random();
            int bombs = GetNumberofBombs();
            for (int i = 0; i < bombs; i++)
            {
                int bombX;
                int bombY;
                do
                {
                    bombX = bombLocation.Next(0, CurrentGame.Width);
                    bombY = bombLocation.Next(0, CurrentGame.Height);
                } while (Board[bombY, bombX].IsBomb || Board[bombY, bombX].IsOpened);

                Board[bombY, bombX].IsBomb = true;
            }
        }

        public int GetNumberofBombs()
        {
            int numberofbombs = (int) (CurrentGame.Height * CurrentGame.Width * DefaultDifficultyPercent);


            if (CurrentGame.DifficultyLevel == 2)
            {
                return (int) Math.Round(numberofbombs * 1.5);
            }

            if (CurrentGame.DifficultyLevel == 3)
            {
                return numberofbombs * 2;
            }

            return numberofbombs;
        }
    }
}