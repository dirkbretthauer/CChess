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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CChessCore
{
    /// <summary>Helper class for event raise</summary>
    public static class EventRaiser 
    {
        /// <summary>Raise event - event handler CAN be null</summary>
        /// <typeparam name="T">Event args class</typeparam>
        /// <param name="eh">event handler</param>
        /// <param name="sender">sender object</param>
        /// <param name="args">event arguments</param>
        public static void TryRaiseEvent<T>(EventHandler<T> eh, object sender, T args) where T : EventArgs 
        {
            ArgumentGuard.NotNull(sender, "sender");
            ArgumentGuard.NotNull(args, "args");

            if (eh != null)
                eh(sender, args);
        }

        /// <summary>Raise event - event handler CAN be null</summary>
        /// <param name="eh">event handler</param>
        /// <param name="sender">sender object</param>
        /// <param name="args">event arguments</param>
        public static void TryRaiseEvent(EventHandler eh, object sender, EventArgs args) 
        {
            ArgumentGuard.NotNull(sender, "sender");
            ArgumentGuard.NotNull(args, "args");

            if (eh != null)
                eh(sender, args);
        }
    }
}