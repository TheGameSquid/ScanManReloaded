using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScanManReloaded.Controls
{
    public partial class ModeSelectionControl : UserControl, IModeControl
    {
        public ModeSelectionControl()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            //
        }

        public void Print()
        {
            //
        }

        public void BarcodeLogic(Barcode barcode)
        { 
            //
        }

        private void buttonKitting_Click(object sender, RoutedEventArgs e)
        {
            // Get a handle on the Main window
            DockPanel parent = (DockPanel)this.Parent;
            Grid gridparent = (Grid)parent.Parent;
            MainWindow mainParent = (MainWindow)gridparent.Parent;

            // Change the mode
            mainParent.ChangeMode(new ModeKittingControl());
        }

        private void buttonWIP_Click(object sender, RoutedEventArgs e)
        {
            // Get a handle on the Main window
            DockPanel parent = (DockPanel)this.Parent;
            Grid gridparent = (Grid)parent.Parent;
            MainWindow mainParent = (MainWindow)gridparent.Parent;

            // Change the Mode
            mainParent.ChangeMode(new ModeRequestControl());
        }
    }
}
