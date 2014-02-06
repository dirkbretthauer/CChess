#region Header
////////////////////////////////////////////////////////////////////////////// 
//    This file is part of $projectname$.
//
//    $projectname$ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    $projectname$ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with $projectname$.  If not, see <http://www.gnu.org/licenses/>.
///////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using CChessCore.Pieces;
using CChessCore.Rules;

namespace CChessCore
{
    public class Game : IGame
    {
        private readonly IGameBoard _originalBoard;

        public IPromotionProvider PromotionProvider { get; set;}

        public IGameBoard Board { get { return _originalBoard; } }

        public IEnumerable<IGameRule> Rules { get; private set; }

        public MoveList Movelist { get; private set; }

        public GameStatus Status { get; private set; }

        public GameInfo Info { get; set; }

        #region events
        public event EventHandler InvalidMove;
        public event EventHandler<MoveListChangedEventArgs> MoveListUpdated;
        #endregion

        private Game(IGameBoard board, IEnumerable<IGameRule> rules)
        {
            _originalBoard = board;
            Rules = rules;
            
            ArgumentGuard.NotNull(board, "board");
            ArgumentGuard.NotNull(rules, "rules");

            Movelist = new MoveList();
            Status = new GameStatus();
            Info = new GameInfo();
        }

        public static IGame CreateChessGame()
        {
            return new Game(new ChessBoard(), new List<IGameRule>()
                                                  {
                                                      new KingHasMovedNoCastlingRule(),
                                                      new KingInCheckRule(),
                                                      new DrawByRepetitionRule(),
                                                      new FiftyMoveRule(),
                                                  });
        }

        public bool TryMove(Square from, Square to, PieceType promoteTo = null)
        {
            Move move = new Move(from, to);
            if (promoteTo != null)
            {
                move.Promotion = promoteTo;
            }

            if (CanMove(move))
            {
                var piece = _originalBoard[from];

                var anotherPossiblePiece =_originalBoard.Where(x => x.Item2.Type == piece.Type &&
                                          x.Item2.Color == piece.Color &&
                                          !x.Item1.Equals(from) &&
                                          x.Item2.CanMove(this, Board.Copy(), new Move(x.Item1, to))).FirstOrDefault();

                if (anotherPossiblePiece != null)
                {
                    move.AvoidAmbiguity = (from.X == anotherPossiblePiece.Item1.X ? Square.YtoCoordinate(from.Y) : Square.XtoCoordinate(from.X)) + "";
                }

                DoMove(piece, move);

                Movelist.Add(move);
                OnMoveListUpdated(new MoveListChangedEventArgs(MoveListChangeReason.Add, Movelist.CurrentHalfMoveNumber, move));
                return true;
            }

            return false;
        }

        public bool CanMove(Move move)
        {
            if (move.From == null || move.To == null)
            {
                return false;
            }

            var piece = _originalBoard[move.From];
            if (piece == null)
            {
                return false;
            }

            if (Status.Turn != piece.Color)
            {
                return false;
            }

            move.PieceMoved = piece;

            if (piece.CanMove(this, Board.Copy(), move))
            {
                IPiece captured = _originalBoard[move.To];
                if (captured != null)
                {
                    move.Type |= MoveType.Capture;
                    move.PieceCaptured = captured;
                }
                else if ((move.Type & MoveType.EnPassent) == MoveType.EnPassent)
                {
                    move.PieceCaptured = Movelist.CurrentMove.PieceMoved;
                }

                foreach (var rule in Rules)
                {
                    var ruleResult = rule.IsMoveLegal(this, Board.Copy(), move);
                    switch (ruleResult)
                    {
                        case RuleResult.Accept:
                            break;
                        case RuleResult.Deny:
                            return false;
                        case RuleResult.Neutral:
                        default:
                            continue;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private void DoMove(IPiece piece, Move move)
        {
            _originalBoard[move.From] = null;
            _originalBoard[move.To] = piece;

            if ((move.Type & MoveType.EnPassent) == MoveType.EnPassent)
            {
                _originalBoard[Movelist.CurrentMove.To] = null;
            }

            if ((move.Type & MoveType.DoublePawn) == MoveType.DoublePawn)
            {
                int enPassentYPosition = piece.Color == PieceColor.White ? move.To.Y - 1 : move.To.Y + 1;
                Status.EnPassentPosition = new Square(move.To.X, enPassentYPosition);
            }
            else
            {
                Status.EnPassentPosition = null;
            }

            if((move.Type & MoveType.Promotion) == MoveType.Promotion)
            {
                if (move.Promotion == null)
                {
                    var newPiece = AskForPromotion();
                    move.Promotion = newPiece;
                }
                _originalBoard[move.To] = PieceFactory.CreatePiece(move.Promotion, Status.Turn);
            }

            if((move.Type & MoveType.ShortCastle) == MoveType.ShortCastle)
            {
                if(piece.Color == PieceColor.White)
                {
                    var rook = _originalBoard["h1"];
                    _originalBoard["h1"] = null;
                    _originalBoard["f1"] = rook;
                }
                else
                {
                    var rook = _originalBoard["h8"];
                    _originalBoard["h8"] = null;
                    _originalBoard["f8"] = rook;
                }
            }

            if ((move.Type & MoveType.LongCastle) == MoveType.LongCastle)
            {
                if (piece.Color == PieceColor.White)
                {
                    var rook = _originalBoard["a1"];
                    _originalBoard["a1"] = null;
                    _originalBoard["d1"] = rook;
                }
                else
                {
                    var rook = _originalBoard["a8"];
                    _originalBoard["a8"] = null;
                    _originalBoard["d8"] = rook;
                }
            }

            move.Position = Fen.GetFen(this, true);

            Status.Turn = piece.Color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            if (IsCheck())
            {
                move.Type |= MoveType.Check;
            }

            if(IsEndOfGame())
            {
                move.Type |= MoveType.CheckMate;   
            }
        }

        private void UndoMove(Move move)
        {
            _originalBoard[move.From] = move.PieceMoved;

            if ((move.Type & MoveType.Capture) == MoveType.Capture)
            {
                _originalBoard[move.To] = move.PieceCaptured;
            }
            else if ((move.Type & MoveType.EnPassent) == MoveType.EnPassent)
            {
                Status.EnPassentPosition = move.To;
                _originalBoard[new Square(move.To.X, move.From.Y)] = move.PieceCaptured;
                _originalBoard[move.To] = null;
            }
            else
            {
                _originalBoard[move.To] = null;
            }

            if ((move.Type & MoveType.ShortCastle) == MoveType.ShortCastle)
            {
                if (move.PieceMoved.Color == PieceColor.White)
                {
                    var rook = _originalBoard["f1"];
                    _originalBoard["f1"] = null;
                    _originalBoard["h1"] = rook;
                }
                else
                {
                    var rook = _originalBoard["f8"];
                    _originalBoard["f8"] = null;
                    _originalBoard["h8"] = rook;
                }
            }

            if ((move.Type & MoveType.LongCastle) == MoveType.LongCastle)
            {
                if (move.PieceMoved.Color == PieceColor.White)
                {
                    var rook = _originalBoard["d1"];
                    _originalBoard["d1"] = null;
                    _originalBoard["a1"] = rook;
                }
                else
                {
                    var rook = _originalBoard["d8"];
                    _originalBoard["d8"] = null;
                    _originalBoard["a8"] = rook;
                }
            }

            Status.Turn = move.PieceMoved.Color;
        }

        private bool IsCheck()
        {
            var king = Board.FirstOrDefault(x => x.Item2.Color == Status.Turn && x.Item2.Type == PieceType.King);
            if (king != null)
            {
                return IsSquareUnderCheck(king.Item1, Board, king.Item2.Color);
            }

            return false;
        }

        private bool IsEndOfGame()
        {
            if (Status.Result != Result.None)
                return true;

            var king = Board.FirstOrDefault(x => x.Item2.Color == Status.Turn && x.Item2.Type == PieceType.King);
            if (king != null)
            {
                if (IsSquareUnderCheck(king.Item1, Board, king.Item2.Color))
                {
                    var possibleMoves = king.Item2.GetValidMoves(this, Board.Copy(), king.Item1);
                    if(possibleMoves.Count() == 0)
                    {
                        var attackingPieces = GetAttackingPieces(king.Item1, Board.Copy());

                        if(attackingPieces.Count() == 1)
                        {
                            Board.Where(item => item.Item2.Color != Status.Turn).Where(item => item.Item2.CanMove(this, Board.Copy(), new Move(item.Item1, attackingPieces.ElementAt(0).Item1)));
                        }

                        Status.Result = Status.Turn == PieceColor.White ? Result.WhiteWon : Result.BlackWon;
                        Info.Result = GameStatus.GetResult(Status.Result);
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnInvalidMove()
        {
            EventRaiser.TryRaiseEvent(InvalidMove, this, EventArgs.Empty);
        }

        private void OnMoveListUpdated(MoveListChangedEventArgs args)
        {
            EventRaiser.TryRaiseEvent(MoveListUpdated, this, args);
        }

        public PieceType AskForPromotion()
        {
            return PromotionProvider != null ? PromotionProvider.AskforPromotion() : PieceType.Queen;
        }

        public void GoForward()
        {
            Move nextMove;
            if(Movelist.TryGoForward(out nextMove))
            {
                DoMove(nextMove.PieceMoved, nextMove);
            }
        }

        public void GoBackward()
        {
            Move previousMove;
            if (Movelist.TryGoBackward(out previousMove))
            {
                UndoMove(previousMove);
            }
        }

        public void GoToBegin()
        {
            Movelist.GoTo(0);
            this._originalBoard.SetToInitialPosition();
            this.Status = new GameStatus();
        }

        public void GoToEnd()
        {
            Movelist.GoTo(Movelist.Count);
            if (Movelist.CurrentMove == null)
                return;

            var game = Fen.GetBoard(Movelist.CurrentMove.Position);
            Status = game.Status;
            _originalBoard.SetPosition(game.Board.GetPosition());
        }

        public void GoToTurn(int halfMoveNumber)
        {
            Movelist.GoTo(halfMoveNumber);
            if (Movelist.CurrentMove == null)
                return;

            var game = Fen.GetBoard(Movelist.CurrentMove.Position);
            Status = game.Status;
            _originalBoard.SetPosition(game.Board.GetPosition());
        }

        public void AddComment(int halfMoveNumber, string comment)
        {
            Movelist.AddComment(halfMoveNumber, comment);
        }

        public bool IsSquareUnderCheck(Square square, IGameBoard board)
        {
            return IsSquareUnderCheck(square, board, Status.Turn);
        }

        public bool IsSquareUnderCheck(Square square, IGameBoard board, PieceColor colorInCheck)
        {
            IGameBoard copy = board.Copy();
            return board.Where(item => item.Item2.Color != colorInCheck)
                                 .Any(item => item.Item2.CanMove(this, copy, new Move(item.Item1, square)));
        }

        public IEnumerable<Tuple<Square, IPiece>> GetAttackingPieces(Square square, IGameBoard board)
        {
            IGameBoard copy = board.Copy();
            return board.Where(item => item.Item2.Color != Status.Turn)
                        .Where(item => item.Item2.CanMove(this, copy, new Move(item.Item1, square)));
        }

        public void SetCastleRight(PieceColor color, MoveType castleType, bool hasTheRight)
        {
            if(color == PieceColor.White)
            {
                if((castleType & MoveType.LongCastle) == MoveType.LongCastle)
                {
                    Status.CanWhiteCastleLong = hasTheRight;    
                }

                if ((castleType & MoveType.ShortCastle) == MoveType.ShortCastle)
                {
                    Status.CanWhiteCastleShort = hasTheRight;
                }
            }
            else if (color == PieceColor.Black)
            {
                if ((castleType & MoveType.LongCastle) == MoveType.LongCastle)
                {
                    Status.CanBlackCastleLong = hasTheRight;
                }

                if ((castleType & MoveType.ShortCastle) == MoveType.ShortCastle)
                {
                    Status.CanBlackCastleShort = hasTheRight;
                }
            }
        }
    }
}