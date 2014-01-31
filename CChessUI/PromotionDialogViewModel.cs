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
using System.Windows.Input;
using CChessCore;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Utilities;
using WpfTools.Dialogs;

namespace CChessUI
{
    public class PromotionDialogViewModel : NotificationObject, IDialogViewModel
    {
        public event EventHandler<RequestCloseEventArgs> RequestClose;

        /// <summary>
        /// Provides the user input of the dialog.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Provides the Result of the dialog.
        /// </summary>
        public object ResultObject { get; set; }

        /// <summary>
        /// Gets the select piece command.
        /// </summary>
        public ICommand SelectPieceCommand { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is white color.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is white color; otherwise, <c>false</c>.
        /// </value>
        public bool IsWhiteColor { get; private set; }

        public string WhiteSquareColor { get; set; }
        
        public string BlackSquareColor { get; set; }

        public string Background { get { return IsWhiteColor ? WhiteSquareColor : BlackSquareColor; } }

        /// <summary>
        /// Gets the queen image.
        /// </summary>
        /// <value>
        /// The queen image.
        /// </value>
        public string QueenImage { get { return IsWhiteColor ? "Images\\ql.png" : "Images\\qd.png"; } }

        /// <summary>
        /// Gets the rook image.
        /// </summary>
        /// <value>
        /// The rook image.
        /// </value>
        public string RookImage { get { return IsWhiteColor ? "Images\\rl.png" : "Images\\rd.png"; } }

        /// <summary>
        /// Gets the bishop image.
        /// </summary>
        /// <value>
        /// The bishop image.
        /// </value>
        public string BishopImage { get { return IsWhiteColor ? "Images\\bl.png" : "Images\\bd.png"; } }

        /// <summary>
        /// Gets the knight image.
        /// </summary>
        /// <value>
        /// The knight image.
        /// </value>
        public string KnightImage { get { return IsWhiteColor ? "Images\\nl.png" : "Images\\nd.png"; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionDialogViewModel"/> class.
        /// </summary>
        public PromotionDialogViewModel(PieceColor color)
        {
            SelectPieceCommand = new DelegateCommand<string>(OnSelectPieceExecuted);
            IsWhiteColor = color == PieceColor.White;
        }

        private void OnSelectPieceExecuted(string piece)
        {
            try
            {
                ResultObject = PieceType.GetPieceType(piece);
                OnRequestClose(true);
            }
            catch (Exception)
            {
                ResultObject = null;
                OnRequestClose(false);
            }
        }

        private void OnRequestClose(bool result)
        {
            EventRaiser.TryRaiseEvent(RequestClose, this, new RequestCloseEventArgs(result));
        }
    }
}
