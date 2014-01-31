#region Header
////////////////////////////////////////////////////////////////////////////// 
//    This file is part of Foobar.
//
//    Foobar is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Foobar is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
///////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;

namespace CChessCore
{
    public interface IGame
    {
        event EventHandler<MoveListChangedEventArgs> MoveListUpdated;

        IGameBoard Board { get; }

        IEnumerable<IGameRule> Rules { get; }

        IPromotionProvider PromotionProvider { get; set; }

        MoveList Movelist { get; }

        GameStatus Status { get; }

        GameInfo Info { get; set; }

        bool TryMove(Square from, Square to, PieceType promoteTo = null);

        bool CanMove(Move move);

        void GoForward();

        void GoBackward();

        void GoToBegin();

        void GoToEnd();

        void GoToTurn(int halfMoveNumber);

        bool IsSquareUnderCheck(Square square, IGameBoard board);

        IEnumerable<Tuple<Square, IPiece>> GetAttackingPieces(Square square, IGameBoard board);

        void SetCastleRight(PieceColor color, MoveType castleType, bool hasTheRight);

        void AddComment(int halfMoveNumber, string comment);
    }
}