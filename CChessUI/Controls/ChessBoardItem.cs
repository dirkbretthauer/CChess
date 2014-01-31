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
using System.Windows;
using System.Windows.Controls;
using CChessCore;

namespace CChessUI.Controls
{
    public class ChessBoardItem : ListBoxItem
    {
        #region Position
        /// <summary>
        /// Description
        /// </summary>
        public Square Position
        {
            get { return (Square) GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Position.
        /// </summary>
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof (Square), typeof (ChessBoardItem), new UIPropertyMetadata(default(Square)));
        #endregion

        #region IsDragged
        /// <summary>
        /// Description
        /// </summary>
        public bool IsDragged
        {
            get { return (bool) GetValue(IsDraggedProperty); }
            set { SetValue(IsDraggedProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsDragged.
        /// </summary>
        public static readonly DependencyProperty IsDraggedProperty =
            DependencyProperty.Register("IsDragged", typeof (bool), typeof (ChessBoardItem), new UIPropertyMetadata(default(bool)));
        #endregion
    }
}
