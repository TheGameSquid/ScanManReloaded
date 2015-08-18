using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
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
using HtmlAgilityPack;
using Pechkin;
using Pechkin.Util;
using Pechkin.EventHandlers;

namespace ScanManReloaded.Controls
{
    public partial class ModeKittingControl : UserControl, IModeControl
    {
        public ModeKittingControl()
        {
            InitializeComponent();
        }

        private HtmlDocument CreateHtml()
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;

            string assemblyPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logoPath = System.IO.Path.Combine(assemblyPath, "Resources", "Documents", "NMBS.png");

            // Load the HTML doc
            htmlDoc.Load(@"Resources\Documents\Checklist.html");

            // Replace the asset name, asset type and engineer name
            htmlDoc.DocumentNode.SelectSingleNode("//span[@id='computername']").InnerHtml = this.textBoxNameAsset.Text;
            htmlDoc.DocumentNode.SelectSingleNode("//span[@id='computertype']").InnerHtml = this.textBoxNameType.Text;
            htmlDoc.DocumentNode.SelectSingleNode("//span[@id='engineername']").InnerHtml = this.textBoxName.Text;          

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

        public void Clear()
        {
            //
        }

        public void Print()
        {
            HtmlDocument docHtml = CreateHtml();
            byte[] pdfBuf = CreatePDF(docHtml);
            File.WriteAllBytes(String.Format("C:\\Data\\Data-{0}.pdf", Guid.NewGuid()), pdfBuf);
        }

        public void BarcodeLogic(Barcode barcode)
        {
            switch (barcode.Command)
            {
                case "TP":
                    this.textBoxNameType.Text = barcode.Value;
                    break;
                case "SN":
                    this.textBoxNameAsset.Text = barcode.Value;
                    break;
                case "NM":
                    this.textBoxName.Text = barcode.Value;
                    break;
            }
        }
    }
}
