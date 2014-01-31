using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CChessCore;
using CChessCore.Pgn;

namespace CChessDatabase
{
    internal class DatabaseGameParser : BaseChessNotationParser
    {
        private Game _game;

        public DatabaseGameParser(Game game)
        {
            _game = game;
        }

        public override IGame GetGame()
        {
            _currentGame = CChessCore.Game.CreateChessGame();

            var reader = new PgnParser();
            reader.Load(_game.PgnMoves);
            
            foreach (var move in reader.GetMoves().Moves)
            {
                DoMove(move.Move);
                _currentGame.AddComment(_currentGame.Movelist.CurrentHalfMoveNumber, move.Comment);
            }

            _currentGame.Info.White = _game.White.Name;
            _currentGame.Info.Black = _game.Black.Name;
            _currentGame.Info.Event = _game.Event;
            _currentGame.Info.Result = _game.Result;

            return _currentGame;
        }
    }
}
