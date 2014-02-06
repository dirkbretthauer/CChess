#region Header
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