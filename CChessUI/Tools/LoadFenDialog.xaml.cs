using System;
using System.Collections.Generic;
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
using WpfTools.Dialogs;

namespace CChessUI
{
    /// <summary>
    /// Interaction logic for LoadFenDialog.xaml
    /// </summary>
    public partial class LoadFenDialog : Window, IDialog
    {
        public LoadFenDialog()
        {
            InitializeComponent();
        }
    }
}
