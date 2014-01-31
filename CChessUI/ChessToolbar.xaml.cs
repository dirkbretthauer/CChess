using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CChessUI
{
    /// <summary>
    /// Interaction logic for ChessToolbar.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ChessToolbar : UserControl
    {
        public ChessToolbar()
        {
            InitializeComponent();
        }

        [Import]
        ChessToolbarViewModel ViewModel
        {
            set
            {
                DataContext = value;
            }
        }
    }
}
