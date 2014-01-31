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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CChessCore;

namespace CChessUI.Controls
{
    public class ChessBoardPanel : Canvas
    {
        private const int NumberOfColumnsAndRows = 8;


        #region BlackSquareColor
        /// <summary>
        /// Description
        /// </summary>
        public Brush BlackSquareColor
        {
            get { return (Brush)GetValue(BlackSquareColorProperty); }
            set { SetValue(BlackSquareColorProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for BlackSquareColor.
        /// </summary>
        public static readonly DependencyProperty BlackSquareColorProperty =
            DependencyProperty.Register("BlackSquareColor", typeof(Brush), typeof(ChessBoardPanel), new UIPropertyMetadata(default(SolidColorBrush)));
        #endregion

        #region WhiteSquareColor
        /// <summary>
        /// Description
        /// </summary>
        public Brush WhiteSquareColor
        {
            get { return (Brush)GetValue(WhiteSquareColorProperty); }
            set { SetValue(WhiteSquareColorProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for WhiteSquareColor.
        /// </summary>
        public static readonly DependencyProperty WhiteSquareColorProperty =
            DependencyProperty.Register("WhiteSquareColor", typeof(Brush), typeof(ChessBoardPanel), new UIPropertyMetadata(default(SolidColorBrush)));
        #endregion

        #region ShowCoordinates
        /// <summary>
        /// Description
        /// </summary>
        public bool ShowCoordinates
        {
            get { return (bool) GetValue(ShowCoordinatesProperty); }
            set { SetValue(ShowCoordinatesProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for ShowCoordinates.
        /// </summary>
        public static readonly DependencyProperty ShowCoordinatesProperty =
            DependencyProperty.Register("ShowCoordinates", typeof (bool), typeof (ChessBoardPanel), new UIPropertyMetadata(true));
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ChessBoardPanel"/> class.
        /// </summary>
        public ChessBoardPanel()
        {

        }

        public Square GetPosition(Point point)
        {
            var squareWidth = ActualWidth/NumberOfColumnsAndRows;
            int x = (int)(point.X / squareWidth);

            var squareHeight = ActualHeight / NumberOfColumnsAndRows;
            int y = NumberOfColumnsAndRows - 1 - (int)(point.Y / squareHeight);

            return new Square(x, y);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size availableSize = new Size(constraint.Width / NumberOfColumnsAndRows,
                                          constraint.Height / NumberOfColumnsAndRows);

            double width = 0.0;
            double height = 0.0;

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
                Size desiredSize = child.DesiredSize;
                if(width < desiredSize.Width)
                {
                    width = desiredSize.Width;
                }
                if(height < desiredSize.Height)
                {
                    height = desiredSize.Height;
                }
            }

            return new Size(width * NumberOfColumnsAndRows, height * NumberOfColumnsAndRows);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            double width = arrangeBounds.Width/NumberOfColumnsAndRows;
            double height = arrangeBounds.Height/NumberOfColumnsAndRows;
            
            foreach (UIElement child in InternalChildren)
            {
                var position = ((ChessBoardItem)child).Position;
                if(position == null)
                {
                    child.Visibility = Visibility.Collapsed;
                    continue;
                }
                double yPos = arrangeBounds.Height - (height*(position.Y + 1));
                double xPos = width * position.X;
                child.Arrange(new Rect(xPos, yPos, width, height));
            }

            return arrangeBounds;
        }

        protected override void OnRender(DrawingContext dc)
        {
            Size renderSize = RenderSize;
            double width = renderSize.Width / NumberOfColumnsAndRows;
            double height = renderSize.Height / NumberOfColumnsAndRows;
            for(int x=0; x < NumberOfColumnsAndRows; x++)
            {
                var xIsEvent = x % 2 == 0;

                for(int y=0; y< NumberOfColumnsAndRows; y++)
                {
                    var yIsEven = y % 2 == 0;

                    Brush background;
                    if((yIsEven && xIsEvent) ||
                        (!yIsEven && !xIsEvent))
                    {
                        background = WhiteSquareColor;
                    }
                    else
                    {
                        background = BlackSquareColor;
                    }

                    dc.DrawRectangle(background, null, new Rect(width*x, height*y, width, height));
                }
            }

            base.OnRender(dc);
        }
    }
}
