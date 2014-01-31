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
using System.Windows.Shapes;

namespace CChessUI
{
    /// <summary>
    /// Interaction logic for Navigation.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Navigation : UserControl
    {
        public Navigation()
        {
            InitializeComponent();
        }

        [Import]
        NavigationViewModel ViewModel
        {
            set
            {
                DataContext = value;
            }
        }
    }
}
