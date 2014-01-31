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
using System.Diagnostics;
using System.Threading;

namespace Utilities
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