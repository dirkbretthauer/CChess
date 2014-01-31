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
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CChessCore
{
    [TestClass]
    public class SquareTest
    {
        [TestMethod]
        public void TestSmoke()
        {
            Square square = "a1";

            Assert.AreEqual(0, square.X);
            Assert.AreEqual(0, square.Y);
        }

        [TestMethod]
        public void TestToString()
        {
            Square square = "a1";

            Assert.AreEqual("a1", square.ToString());
        }

        [TestMethod]
        public void TestMaxValues()
        {
            Square square = "h8";

            Assert.AreEqual(7, square.X);
            Assert.AreEqual(7, square.Y);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSquare()
        {
            Square square = "h9";

            Assert.AreEqual(7, square.X);
            Assert.AreEqual(7, square.Y);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSquare2()
        {
            Square square = "k1";

            Assert.AreEqual(7, square.X);
            Assert.AreEqual(7, square.Y);
        }

        [TestMethod]
        public void TestYtoCoord()
        {
            Assert.AreEqual('1', Square.YtoCoordinate(0));
            Assert.AreEqual('7', Square.YtoCoordinate(6));
            Assert.AreEqual('6', Square.YtoCoordinate(5));
        }

        [TestMethod]
        public void TestXtoCoord()
        {
            Assert.AreEqual('a', Square.XtoCoordinate(0));
            Assert.AreEqual('h', Square.XtoCoordinate(7));
            Assert.AreEqual('f', Square.XtoCoordinate(5));
        }
    }
}