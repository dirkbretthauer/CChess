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
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using CChessCore;
using WpfTools.DragDrop;

namespace CChessUI.Controls
{
    public class ChessBoardDragDropAdvisor : IDragSourceAdvisor, IDropTargetAdvisor
    {
        private const string _pieceFormat = "ChessPiece";
        private const string _positionFormat = "Position";
        private UIElement _chessBoardPanel;
        private Func<Square, Square, bool> _canMove;
        private Action<Square, Square> _doMove;


        public ChessBoardDragDropAdvisor(Func<Square, Square, bool> canMove, Action<Square, Square> doMove)
        {
            _canMove = canMove;
            _doMove = doMove;
        }


        #region Implementation of IDragSourceAdvisor
        public DataObject GetDataObject(UIElement draggedElt)
        {
            DataObject obj = new DataObject();

            DependencyObject chessBoardItem = draggedElt;
            while (!(chessBoardItem is ChessBoardItem))
            {
                chessBoardItem = VisualTreeHelper.GetParent(draggedElt);
            }
            obj.SetData(_pieceFormat, ((ChessBoardItem)chessBoardItem).DataContext);
            obj.SetData(_positionFormat, ((ChessBoardItem) chessBoardItem).Position);

            draggedElt.Visibility = Visibility.Collapsed;

            return obj;
        }

        public void FinishDrag(UIElement draggedElt, DragDropEffects finalEffects)
        {
            draggedElt.Visibility = Visibility.Visible;
        }

        public bool IsDraggable(UIElement dragElt)
        {
            return (!(dragElt is ChessBoardPanel));
        }

        public UIElement SourceUI
        {
            get { return _chessBoardPanel; }
            set { _chessBoardPanel = value; }
        }

        public DragDropEffects SupportedEffects
        {
            get { return DragDropEffects.Move; }
        }
        #endregion

        #region Implementation of IDropTargetAdvisor
        public bool IsValidDataObject(IDataObject obj)
        {
            return (obj.GetDataPresent(_pieceFormat) || obj.GetDataPresent(_positionFormat));
        }

        public void OnDropCompleted(IDataObject obj, Point dropPoint)
        {
            ChessBoardPanel panel = TargetUI as ChessBoardPanel;
            var newSquare = panel.GetPosition(dropPoint);

            _doMove((Square)obj.GetData(_positionFormat), newSquare);
        }

        public object GetUiContent(IDataObject obj)
        {
            return obj.GetData(_pieceFormat);
        }

        public UIElement TargetUI
        {
            get { return _chessBoardPanel; }
            set { _chessBoardPanel = value; }
        }
        #endregion
    }
}
