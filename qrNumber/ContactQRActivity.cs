using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace qrNumber
{
    [Activity(Label = "ContactQRActivity")]
    public class ContactQRActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listQRcode_layout);
            
            Android.Support.V7.Widget.Toolbar toolbar
                = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "연락처 QR코드";// "QRcodeName";
            

            TextView namev = FindViewById<TextView>(Resource.Id.nameview);
            TextView numberv = FindViewById<TextView>(Resource.Id.numberview);
            ImageView qrimageView = FindViewById<ImageView>(Resource.Id.numQRimageview);
            namev.Text = Intent.GetStringExtra("name");
            numberv.Text= Intent.GetStringExtra("number");

            SBconverter sBconverter = new SBconverter();
            Bitmap numqrcode = sBconverter.stringToBitmap(@"{name:" + namev.Text.ToString() + @",number:"+ numberv.Text.ToString() + @"}");
            qrimageView.SetImageBitmap(numqrcode);

            // Create your application here
        }
    }
}