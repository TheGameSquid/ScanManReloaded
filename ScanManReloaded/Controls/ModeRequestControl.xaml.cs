using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
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
        private ObservableCollection<Asset> assetList;

        public ModeRequestControl()
        {
            InitializeComponent();
            assetList = new ObservableCollection<Asset>();

            assetList.Add(new Asset("PO", "130021498"));
            assetList.Add(new Asset("PO", "130021498"));
            assetList.Add(new Asset("PO", "130021498b"));
            assetList.Add(new Asset("PO", "130021999"));
            assetList.Add(new Asset("PO", "130021000"));

            this.testList.DataContext = assetList;
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
            switch (barcode.Command) 
            { 
                case "TP":
                    AddAsset(barcode.Value);
                    break;
                case "SN":
                    SetAsset(barcode.Value);
                    break;
            }
        }

        private void AddAsset(string type)
        {
            this.assetList.Add(new Asset(type));
        }

        private void SetAsset(string name)
        {
            if (assetList.Last<Asset>().Type == "")
            {
                // TODO: Diplay error message!
            }
            else
            {
                assetList.Last<Asset>().Name = name;

                // Find the CN of a given asset
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
                {
                    // Find a computer
                    ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, String.Format("{0}{1}", assetList.Last<Asset>().Type, name));
                    if (computer != null)
                    {
                        assetList.Last<Asset>().ADPath = computer.DistinguishedName;
                    }
                    else
                    {
                        assetList.Last<Asset>().ADPath = "Not Found!";
                    }
                }
            }
        }

        private void DeleteAsset(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Asset asset = button.CommandParameter as Asset;
            this.assetList.Remove(asset);
        }

        private void OnAssetTextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            ListViewItem parentItem = textbox.FindParent<ListViewItem>();
            Asset asset = parentItem.Content as Asset;
            asset = assetList.Single<Asset>(a => a == asset);
            asset.Name = textbox.Text;

            textbox.Background = new SolidColorBrush(Colors.Red);
            
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
            {
                // Find a computer
                ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, String.Format("{0}{1}", asset.Type, asset.Name));
                if (computer != null)
                {
                    asset.ADPath = computer.DistinguishedName;
                    //if (asset.ADPath.Contains(Properties.Settings.Default.ErrorOU))
                    //{
                    //    txtLocationAd.BackColor = System.Drawing.Color.OrangeRed;
                    //}
                }
                else
                {
                    asset.ADPath = "Not Found!";
                }
            }

            textbox.Background = new SolidColorBrush(Colors.Red);
            parentItem.Background = new SolidColorBrush(Colors.Red);

            //ICollectionView view = CollectionViewSource.GetDefaultView(assetList);
            //view.Refresh();
        }
    }
}
