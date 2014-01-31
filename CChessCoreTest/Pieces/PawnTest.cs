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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CChessCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace CChessCoreTest.Pieces
{
    [TestClass]
    public class PawnTest
    {
        private IPiece _pawn;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _pawn = new Pawn(PieceColor.White);
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            _pawn = null;
        }

    }
}