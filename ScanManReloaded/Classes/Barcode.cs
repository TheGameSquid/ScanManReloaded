using System;

namespace ScanManReloaded
{
    public class Barcode
    {
        private string value;

        public string Value
        {
            get { return this.value; }
        }

        public Barcode(string barcodeString)
        {
            this.value = barcodeString;
        }
    }
}
