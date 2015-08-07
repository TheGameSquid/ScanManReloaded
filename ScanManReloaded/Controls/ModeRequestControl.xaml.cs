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
    public partial class ModeRequestControl : UserControl, IModeControl
    {
        public ModeRequestControl()
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
    }
}
