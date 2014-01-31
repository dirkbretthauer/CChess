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
using System.Text;
using CChessCore;
using Microsoft.Practices.Prism.ViewModel;

namespace CChessUI.ViewModels
{
    public class ChessPieceViewModel : NotificationObject
    {
        private Square _position;
        private IPiece _piece;

        public Square Position
        {
            get { return _position; }
            set
            {
                _position = value;
                RaisePropertyChanged(() => Position);
            }
        }

        public IPiece Piece
        {
            get { return _piece;}
            set
            {
                _piece = value;
                RaisePropertyChanged(() => Piece);
            }
        }


        public ChessPieceViewModel()
        {
            
        }

        public ChessPieceViewModel(IPiece piece)
        {
            _piece = piece;
        }
    }
}
