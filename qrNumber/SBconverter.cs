using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using QRCoder;

namespace qrNumber
{
    class SBconverter
    {
        QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
        QRCodeData qRCodeData;
        BitmapByteQRCode bitmapByteQRCode;
        public SBconverter()
        {

        }

        public Bitmap stringToBitmap(string contents)
        {
            qRCodeData = qRCodeGenerator.CreateQrCode(contents, QRCodeGenerator.ECCLevel.H);
            bitmapByteQRCode = new BitmapByteQRCode(qRCodeData);

            return BitmapFactory.DecodeByteArray(bitmapByteQRCode.GetGraphic(20), 0,
                bitmapByteQRCode.GetGraphic(20).Length);
        }
        public Bitmap stringToBitmap(string name,string number)
        {
            string temp = "{\"name\":\""+name+"\",\"number\":\""+number+"\"}";
            qRCodeData = qRCodeGenerator.CreateQrCode(temp, QRCodeGenerator.ECCLevel.H);
            bitmapByteQRCode = new BitmapByteQRCode(qRCodeData);

            return BitmapFactory.DecodeByteArray(bitmapByteQRCode.GetGraphic(20), 0,
                bitmapByteQRCode.GetGraphic(20).Length);
        }
    }
}