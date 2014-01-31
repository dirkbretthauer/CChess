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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CChessCore
{
    [TestClass]
    public class ChessBoardTest
    {
        private ChessBoard _chessBoard;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _chessBoard = new ChessBoard();
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            _chessBoard = null;
        }

        [TestMethod]
        public void TestSmoke()
        {
            
        }
    }
}