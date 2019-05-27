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

using System.ComponentModel.Composition;
using CChessUI.Views;
using Prism.Mvvm;
using Prism.Regions;

namespace CChessUI.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        public NotationView NotationView { get; private set; }

        public Navigation NavigationView {get; private set;}

        public ShellViewModel(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(ChessBoardView));
            regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(ChessToolbar));
            regionManager.RegisterViewWithRegion(RegionNames.MainToolsRegion, typeof(NotationView));
            regionManager.RegisterViewWithRegion(RegionNames.MainToolsRegion, typeof(Navigation));
        }
    }
}
