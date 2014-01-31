﻿#region Header
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

using System.Collections.Generic;


namespace CChessCore
{
    public interface IMoveList : ICollection<Move>
    {
        Move LastMove { get; }

        Move CurrentMove { get; }

        int CurrentHalfMoveNumber { get; }

        bool TryGoBackward(out Move move);

        bool TryGoForward(out Move move);

        bool GoTo(int halfMoveNumber);

        void AddComment(int halfMoveNumber, string comment);

        string ToPgn();
    }
}