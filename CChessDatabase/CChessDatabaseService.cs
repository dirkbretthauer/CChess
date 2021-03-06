﻿#region Header
////////////////////////////////////////////////////////////////////////////// 
//The MIT License (MIT)

//Copyright (c) 2013 Dirk Bretthauer

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CChessCore;
using CChessCore.Pgn;

namespace CChessDatabase
{
    public class CChessDatabaseService : ICChessDatabaseService
    {
        public void Save(IGame game)
        {
            var g = new Game();

            g.PgnMoves = game.Movelist.ToPgn();
            g.Result = game.Info.Result;
            g.White.Name = game.Info.White;
            g.Black.Name = game.Info.Black;
            g.Event = game.Info.Event;
            g.Date = game.Info.Date;

            using(var db = new CChessDatabaseContext())
            {
                var white = db.Players.Where(x => x.Name.Equals(game.Info.White)).FirstOrDefault();
                if(white == null)
                {
                    white = new Player() { Name = game.Info.White };
                    db.Players.Add(white);
                }

                var black = db.Players.Where(x => x.Name.Equals(game.Info.Black)).FirstOrDefault();
                if(black == null)
                {
                    black = new Player() { Name = game.Info.Black };
                    db.Players.Add(black);
                }

                db.SaveChanges();

                var existingGame = db.Games.Where(x => x.Black.Id.Equals(black.Id) &&
                                                x.White.Id.Equals(white.Id) &&
                                                x.Event.Equals(g.Event) &&
                                                x.PgnMoves.Equals(g.PgnMoves)).FirstOrDefault();

                if(existingGame == null)
                {
                    g.Black = black;
                    g.White = white;
                    db.Games.Add(g);
                    db.SaveChanges();
                }
            }
        }

        public void Save(PgnGame game)
        {
            var g = new Game();
            var gameInfo = new PgnParser().GetGameInfo(game);

            g.PgnMoves = game.GetMovesAsPgn();

            g.Result = gameInfo.Result;
            g.White.Name = gameInfo.White;
            g.Black.Name = gameInfo.Black ;
            g.Event = gameInfo.Event;
            g.Date = gameInfo.Date;

            using (var db = new CChessDatabaseContext())
            {
                var white = db.Players.Where(x => x.Name.Equals(gameInfo.White)).FirstOrDefault();
                if (white == null)
                {
                    white = new Player() { Name = gameInfo.White };
                    db.Players.Add(white);
                }

                var black = db.Players.Where(x => x.Name.Equals(gameInfo.Black)).FirstOrDefault();
                if (black == null)
                {
                    black = new Player() { Name = gameInfo.Black };
                    db.Players.Add(black);
                }

                db.SaveChanges();

                var existingGame = db.Games.Where(x => x.Black.Id.Equals(black.Id) &&
                                                x.White.Id.Equals(white.Id) &&
                                                x.Event.Equals(g.Event) &&
                                                x.PgnMoves.Equals(g.PgnMoves)).FirstOrDefault();

                if (existingGame == null)
                {
                    g.Black = black;
                    g.White = white;
                    db.Games.Add(g);
                    db.SaveChanges();
                }
            }
        }

        public IGame GetGame()
        {
            using (var db = new CChessDatabaseContext())
            {
                var game = db.Games.First();
                var parser = new DatabaseGameParser(game);

                return parser.GetGame();
            }
        }

        public IGame GetGame(GameInfo gameInfo)
        {
            using (var db = new CChessDatabaseContext())
            {
                var white = db.Players.Where(x => x.Name.Equals(gameInfo.White)).FirstOrDefault();
                var black = db.Players.Where(x => x.Name.Equals(gameInfo.Black)).FirstOrDefault();
                

                var game = db.Games.Where(x => x.Black.Id.Equals(black.Id) &&
                                               x.White.Id.Equals(white.Id) &&
                                               x.Event.Equals(gameInfo.Event)).FirstOrDefault();

                if (game == null)
                    return null;

                var parser = new DatabaseGameParser(game);

                return parser.GetGame();
            }
        }

        public void RemoveGame(GameInfo gameInfo)
        {
            using (var db = new CChessDatabaseContext())
            {
                var game = db.Games.Where(x => x.Black.Name.Equals(gameInfo.Black) &&
                                               x.White.Name.Equals(gameInfo.White) &&
                                               x.Event.Equals(gameInfo.Event)).FirstOrDefault();

                if (game == null)
                    return;

                db.Games.Remove(game);
                db.SaveChanges();
            }
        }

        public void RemoveGames(IEnumerable<GameInfo> gameInfos)
        {
            using(var db = new CChessDatabaseContext())
            {
                foreach(var gameInfo in gameInfos)
                {
                    var game = db.Games.Where(x => x.Black.Name.Equals(gameInfo.Black) &&
                                                   x.White.Name.Equals(gameInfo.White) &&
                                                   x.Event.Equals(gameInfo.Event)).FirstOrDefault();

                    if(game == null)
                        continue;

                    db.Games.Remove(game);
                }
                db.SaveChanges();
            }
        }

        public IEnumerable<GameInfo> GetAllGames()
        {
            using (var db = new CChessDatabaseContext())
            {
                var games = db.Games.Select(x => new GameInfo()
                                                {
                                                    Black = x.Black.Name,
                                                    White = x.White.Name,
                                                    Event = x.Event,
                                                    Result = x.Result,
                                                    Date = x.Date
                                                });

                return games.ToList();
            }
        }


        public void Load()
        {
            using (var db = new CChessDatabaseContext())
            {
                db.Games.Load();
            }
        }
    }
}
