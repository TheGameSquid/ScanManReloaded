using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
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
using System.Xml.Linq;
using HtmlAgilityPack;
using Pechkin;
using Pechkin.Util;
using Pechkin.EventHandlers;

namespace ScanManReloaded.Controls
{
    public partial class ModeRequestControl : UserControl, IModeControl
    {
        private ObservableCollection<Asset> assetList;

        public ModeRequestControl()
        {
            InitializeComponent();
            assetList = new ObservableCollection<Asset>();

            assetList.Add(new Asset("PO"));
            SetAsset("130021498");
            assetList.Add(new Asset("PO", "130021498"));
            assetList.Add(new Asset("PO", "130021498b"));
            assetList.Add(new Asset("PO", "130021999"));
            assetList.Add(new Asset("PO", "130021000"));

            this.testList.DataContext = assetList;
        }

        public void Clear()
        {
            assetList.Clear();
        }

        public void Print()
        {
            HtmlDocument docHtml = CreateHtml();
            byte[] pdfBuf = CreatePDF(docHtml);
            File.WriteAllBytes(String.Format("C:\\Data\\Data-{0}.pdf", Guid.NewGuid()), pdfBuf);
        }

        private HtmlDocument CreateHtml()
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;

            string assemblyPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = System.IO.Path.Combine(assemblyPath, "Resources", "Documents", "NMBS.png");

            // Load the HTML doc
            htmlDoc.Load(@"Resources\Documents\Request.html");

            // Fill in the correct path of the logo
            htmlDoc.DocumentNode.SelectSingleNode("//img[@id='logo']").ChildAttributes("src").Single<HtmlAttribute>().Value = logoPath;

            // Replace the person signing the document
            htmlDoc.DocumentNode.SelectSingleNode("//span[@id='signed']").InnerHtml = this.textBoxName.Text;

            // Replace the department
            htmlDoc.DocumentNode.SelectSingleNode("//span[@id='department']").InnerHtml = this.textBoxDepartment.Text;

            // Replace the reason
            htmlDoc.DocumentNode.SelectSingleNode("//span[@id='reason']").InnerHtml = this.textBoxReason.Text;

            // Insert the DateTime
            htmlDoc.DocumentNode.SelectSingleNode("//span[@id='datetime']").InnerHtml = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

            // Add the items of the list to the table
            foreach (Asset asset in this.testList.Items)
            {
                HtmlNode listItem = htmlDoc.CreateElement("tr");
                listItem.InnerHtml = String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", asset.Type, asset.Name, asset.ADPath);
                htmlDoc.DocumentNode.SelectSingleNode("//tbody[@id='assetsbody']").AppendChild(listItem);
            }

            htmlDoc.Save("kfkgjfgjfgjgf.html");

            return htmlDoc;
        }

        private byte[] CreatePDF(HtmlDocument doc)
        {
            // Create global configuration object
            GlobalConfig gc = new GlobalConfig();

            // Set it up using fluent notation
            gc.SetMargins(new Margins(50, 100, 0, 0))
                .SetDocumentTitle("Request")
                .SetPaperSize(PaperKind.A4);

            // Create converter
            IPechkin pechkin = new SimplePechkin(gc);

            // Create document configuration object
            ObjectConfig oc = new ObjectConfig();

            // And set it up using fluent notation
            oc.SetCreateExternalLinks(true)
                .SetFallbackEncoding(Encoding.ASCII)
                .SetZoomFactor(2)
                .SetIntelligentShrinking(true)
                .SetLoadImages(true);

            // Convert document
            return pechkin.Convert(oc, doc.DocumentNode.OuterHtml);    
        }

        public void BarcodeLogic(Barcode barcode)
        {
            switch (barcode.Command) 
            { 
                case "TP":
                    AddAsset(barcode.Value);
                    break;
                case "SN":
                    SetAsset(barcode.Value);
                    break;
                case "NM":
                    textBoxName.Text = barcode.Value;
                    break;
                case "DP":
                    textBoxDepartment.Text = barcode.Value;
                    break;
                case "RN":
                    textBoxDepartment.Text = barcode.Value;
                    break;
                case "WO":
                    textBoxDepartment.Text = barcode.Value;
                    break;
                case "RO":
                    textBoxDepartment.Text = barcode.Value;
                    break;
            }
        }

        private void AddAsset(string type)
        {
            this.assetList.Add(new Asset(type));
        }

        private void SetAsset(string name)
        {
            if (assetList.Count == 0)
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

        private void RemoveAsset(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Asset asset = button.CommandParameter as Asset;
            this.assetList.Remove(asset);
        }

        private bool ValidateForm()
        { 
            if((assetList.Count > 0)
                && (textBoxName.Text != "")
                && (textBoxDepartment.Text != "")
                && (textBoxReason.Text != ""))
            {
                return true;
            }
            return false;
        }
    }
}
