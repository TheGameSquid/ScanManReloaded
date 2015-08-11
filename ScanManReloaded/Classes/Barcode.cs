using System;
using System.Text.RegularExpressions;

namespace ScanManReloaded
{
    public class Barcode
    {
        private string command;
        private string value;

        public string Command
        {
            get { return this.command; }
        }

        public string Value
        {
            get { return this.value; }
        }

        public Barcode(string barcodeString)
        {
            string barcodePattern = "[a-zA-Z]{2}_[a-zA-Z]+";
            string assetPattern = "[0-9]+";

            // Instantiate the regular expression object.
            Regex barcodeRegex = new Regex(barcodePattern, RegexOptions.IgnoreCase);
            Regex assetRegex = new Regex(assetPattern, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            Match barcodeMatch = barcodeRegex.Match(barcodeString);
            Match assetMatch = assetRegex.Match(barcodeString);

            if (barcodeMatch.Success)
            {
                this.command = barcodeString.Split('_')[0].ToUpper();
                this.value = barcodeString.Split('_')[1].ToUpper();
            }
            else if (assetMatch.Success)
            {
                this.command = "SN".ToUpper();
                this.value = barcodeString.ToUpper();
            }
            else
            {
                this.command = "";
                this.value = "";
            }
        }
    }
}
