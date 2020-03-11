using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUI;
using DAL;
using Domain;
using GameEngine;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MineSweeperConsole
{
    class Program
    {
        private static GameSetting _settings;
        
        private static readonly GameDbContext _context = new GameDbContext(new DbContextOptions<GameDbContext>());

        private static readonly List<Game> games = _context.Games.ToList();

        static void Main(string[] args)
        {
            Console.Clear();


            _settings = new GameSetting();


            Console.WriteLine($"Hello to {_settings.GameName}!");

            var menu2 = new Menu(2)
            {
                Title = "Load games",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
            };
            
            int count = 1;
            foreach (var game in games)
            {
                menu2.MenuItemsDictionary.Add(count.ToString(), new MenuItem()
                {
                    Title = game.GameName,
                    CommandToExecute = s => StartGame(game.GameId.ToString())
                });
                count++;
            }
            
            var gameMenu = new Menu(1)
            {
                Title = $"Start a new game of {_settings.GameName}",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Title = "Start new game",
                            CommandToExecute = s => StartGame("")
                        }
                    }
                }
            };
            
            

            var menu0 = new Menu(0)
            {
                Title = $"{_settings.GameName} Main Menu",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "S", new MenuItem()
                        {
                            Title = "Start game",
                            CommandToExecute = gameMenu.Run
                        }
                    },
                    {
                        "J", new MenuItem()
                        {
                            Title = "Set defaults for game (save to DB)",
                            CommandToExecute = s => SaveSettings()
                        }
                    },
                    {
                        "L", new MenuItem()
                        {
                            Title = "Load Saved games",
                            CommandToExecute = menu2.Run
                        }
                    }
                }
            };


            menu0.Run();
        }


        static string SaveSettings()
        {
            Console.Clear();

            var boardWidth = 0;
            var boardHeight = 0;
            var userCanceled = false;

            (boardWidth, userCanceled) = GetUserIntInput("Enter board width", 9, 20, 0);
            if (userCanceled) return "";

            (boardHeight, userCanceled) = GetUserIntInput("Enter board height", 9, 20, 0);
            if (userCanceled) return "";

            _settings.BoardHeight = boardHeight;
            _settings.BoardWidth = boardWidth;

            _context.Add(_settings);
            _context.SaveChanges();
            return "";
        }

        static string StartGame(string gameId)
        {
            var game = new GameController(_context, _settings);
            if(_context.Games.Find(Int32.Parse(gameId)) != null)
            {
                game = new GameController(_context, Int32.Parse(gameId));
            }
            
            var done = false;
            do
            {
                Console.Clear();
                GameUI.PrintBoard(game);

                var userXint = 0;
                var userYint = 0;
                var userCanceled = false;

                (userXint, userCanceled) = GetUserIntInput("Enter X coordinate", 1, 9, 0);
                if (!userCanceled)
                {
                    (userYint, userCanceled) = GetUserIntInput("Enter Y coordinate", 1, 9, 0);
                }

                if (userCanceled)
                {
                    done = true;
                }
                else
                {
                    game.OpenPosition(userYint - 1, userXint - 1);
                }
            } while (!done);


            return "GAME OVER!!";
        }

        static (int result, bool wasCanceled) GetUserIntInput(string prompt, int min, int max,
            int? cancelIntValue = null, string cancelStrValue = "")
        {
            do
            {
                Console.WriteLine(prompt);
                if (cancelIntValue.HasValue || !string.IsNullOrWhiteSpace(cancelStrValue))
                {
                    Console.WriteLine($"To cancel input enter: {cancelIntValue}" +
                                      $"{(cancelIntValue.HasValue && !string.IsNullOrWhiteSpace(cancelStrValue) ? " or " : "")}" +
                                      $"{cancelStrValue}");
                }

                Console.Write(">");
                var consoleLine = Console.ReadLine();

                if (consoleLine == cancelStrValue) return (0, true);

                if (int.TryParse(consoleLine, out var userInt))
                {
                    return userInt == cancelIntValue ? (userInt, true) : (userInt, false);
                }

                Console.WriteLine($"'{consoleLine}' cant be converted to int value!");
            } while (true);
        }
    }
}