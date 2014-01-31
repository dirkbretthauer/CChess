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

using CChessCore;
using Microsoft.Practices.Prism.ViewModel;

namespace CChessUI.ViewModels
{
    public class MoveViewModel : NotificationObject
    {
        private bool _isWhiteMove;
        private readonly Move _move;

        public bool IsWhiteMove
        {
            get { return _isWhiteMove; }
            set
            {
                if(_isWhiteMove != value)
                {
                    _isWhiteMove = value;
                    RaisePropertyChanged(() => IsWhiteMove);
                }
            }
        }

        public string MoveNumberText
        {
            get { return _isWhiteMove ? _move.TurnNumber + "." : string.Empty; }
        }

        public int TurnNumber
        {
            get { return _move.TurnNumber; }
        }

        public int HalfMoveNumber
        {
            get { return _move.HalfMoveNumber; }
        }

        public string MoveText
        {
            get { return _move.ToString(); }
        }

        public string Comment
        {
            get { return _move.Comment; }
        }

        public MoveViewModel(Move move)
        {
            _move = move;
            _isWhiteMove = _move.PieceMoved.Color == PieceColor.White;
        }

        public override string ToString()
        {
            return _move.ToString();
        }
    }
}
