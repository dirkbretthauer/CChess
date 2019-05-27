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
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using CChessCore;
using CChessCore.Pieces;
using CChessUI.Controls;
using CChessUI.ViewModels;
using CChessUI.Views;
using Prism.Common;
using Prism.Mvvm;
using Prism.Regions;
using WpfTools.Dialogs;

namespace CChessUI.ViewModels
{
    public class ChessBoardViewModel : BindableBase, IPromotionProvider
    {
        private IGame _game;
        private string _gameInfo;
        private readonly IDialogService _dialogService;

        public ObservableCollection<ChessPieceViewModel> Pieces { get; private set; }

        public ChessBoardDragDropAdvisor DragDropHandler { get; private set; }

        public IEnumerable<Move> MoveList { get; set; }


        public string WhiteSquareColor { get { return "Beige"; } }

        public string BlackSquareColor { get { return "Brown"; } }


        public string GameInfo
        {
            get { return _gameInfo; }
            set
            {
                SetProperty(ref _gameInfo, value);
            }
        }

        public ChessBoardViewModel(IGameController gameController, IDialogService dialogService, IRegionManager regionManager)
        {
            _dialogService = dialogService;

            DragDropHandler = new ChessBoardDragDropAdvisor(InternalCanMove, InternalDoMove);

            Pieces = new ObservableCollection<ChessPieceViewModel>();

            gameController.NewGameStarted += OnNewGameStarted;
            gameController.StartNewGame();
        }

        private void OnNewGameStarted(object sender, EventArgs e)
        {
            var gameController = sender as IGameController;
            if(gameController == null)
                return;

            if(_game != null)
            {
                _game.Board.PositionChanged -= OnPositionChanged;
                _game = null;
            }
            _game = gameController.Game;
            _game.PromotionProvider = this;
            _game.Board.PositionChanged += OnPositionChanged;

            if (string.IsNullOrWhiteSpace(_game.Info.White) ||
                string.IsNullOrWhiteSpace(_game.Info.Black))
            {
                GameInfo = string.Empty;
            }
            else
            {
                GameInfo = _game.Info.White + " - " + _game.Info.Black;
            }

            Pieces.Clear();
            foreach (var item in _game.Board)
            {
                Pieces.Add(new ChessPieceViewModel(item.Item2) { Position = item.Item1 });
            }
        }

        private void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            if(e == PositionChangedEventArgs.Reset)
            {
                Pieces.Clear();
                foreach (var item in _game.Board)
                {
                    Pieces.Add(new ChessPieceViewModel(item.Item2) { Position = item.Item1 });
                }
            }
            else if(e.OldPiece != null)
            {
                var viewModel = Pieces.FirstOrDefault(x => x.Position.Equals(e.Square));
                if (e.NewPiece != null)
                {
                    viewModel.Piece = e.NewPiece;
                }
                else
                {
                    Pieces.Remove(viewModel);
                }
            }
            else
            {
                if(e.NewPiece != null)
                {
                    Pieces.Add(new ChessPieceViewModel(e.NewPiece) { Position = e.Square});
                }
            }   
        }

        private void InternalDoMove(Square from, Square to)
        {
            _game.TryMove(from, to);
        }

        private bool InternalCanMove(Square from, Square to)
        {
            return _game.CanMove(new Move(from, to));
        }

        public PieceType AskforPromotion()
        {
            var promotionDialogViewModel = new PromotionDialogViewModel(_game.Status.Turn);
            promotionDialogViewModel.WhiteSquareColor = WhiteSquareColor;
            promotionDialogViewModel.BlackSquareColor = BlackSquareColor;

            _dialogService.ShowDialog(typeof (PromotionDialog), promotionDialogViewModel, null);

            if(promotionDialogViewModel.ResultObject != null)
            {
                return (PieceType) promotionDialogViewModel.ResultObject;
            }

            return PieceType.Queen;
        }
    }
}