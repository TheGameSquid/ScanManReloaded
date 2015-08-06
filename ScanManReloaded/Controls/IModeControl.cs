using System;

namespace ScanManReloaded
{
    interface IModeControl
    {
        void BarcodeLogic(Barcode barcode);
        void Clear();
        void Print();
    }
}
