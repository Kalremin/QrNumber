using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms.Android;

using Android.Gms.Vision.Barcodes;
using PermissionsHandler = ZXing.Net.Mobile.Forms.Android.PermissionsHandler;

namespace qrNumber
{
    [Activity(Label = "ScanQRcodeActivity")]
    public class ScanQRcodeActivity : Activity
    {
        Intent intent;
        TextView resultview,stringview,jsonview;
        
        /*
        string path = (string)Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DataDirectory.ToString());
        string pathpackage;
        string pathtxt;
        */
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
           PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        /*
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
            }
            else
            {
            }

        }
        */

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.scantestlayout);

            try
            {
                Task.Run(() => ScanQRAsync());
                Toast.MakeText(this, "Success", ToastLength.Long).Show();
            }
            catch
            {
                Toast.MakeText(this, "Fail", ToastLength.Long).Show();
            }
            //BarcodeDetector
            Finish();

            //비동기 작업 필요
            /*
            MobileBarcodeScanner.Initialize(Application);
            
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            ZXing.Result result = scanner.Scan();

            if (result != null)
                textView.Text = ;
            */



            // Create your application here
        }
        
        private async Task ScanQRAsync()
        {
            
            MobileBarcodeScanner.Initialize(Application);
            var scanner = new MobileBarcodeScanner();
            ZXing.Result result = await scanner.Scan();
            resultview = FindViewById<TextView>(Resource.Id.resultview);
            stringview = FindViewById<TextView>(Resource.Id.stringtest);

            resultview.Text = result.Text;
            
            insertContacts(result.Text);
                
            
            /*
            pathpackage = Path.Combine(path, PackageName);// /storage/emulated/0/data/App4.App4
            pathtxt = Path.Combine(pathpackage, "scantemp.txt"); // /storage/emulated/0/data/App4.App4/selfname.txt

            
            StreamWriter streamWriter=new StreamWriter(pathtxt,false,Encoding.Unicode);
            streamWriter.WriteLine(resultview.Text);
            streamWriter.Dispose();
            */
            /*
            FileStream file = new FileStream(pathtxt, FileMode.Create);
            file.Write(result.RawBytes, 0, result.NumBits);
            file.Dispose();
            */
            //textView.Text = result.Text;
            //text2.Text = result.Text;

            //return result.Text;

        }

        public async Task<string> scanQRstring()
        {
            MobileBarcodeScanner.Initialize(Application);
            var scanner = new MobileBarcodeScanner();
            ZXing.Result result = await scanner.Scan();
            return result.Text;
        }

        //https://ghj1001020.tistory.com/4
        public void insertContacts(string contactstext)
        {
            JObject jo = JObject.Parse(contactstext);
            string cname = jo["name"].ToString();
            string cnumber = jo["number"].ToString();
            jsonview = FindViewById<TextView>(Resource.Id.jsontest);
            jsonview.Text = cname + cnumber;
            
            List<ContentProviderOperation> operations = new List<ContentProviderOperation>();
            ContentProviderOperation.Builder builder =
                ContentProviderOperation.NewInsert(ContactsContract.RawContacts.ContentUri);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountType, null);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountName, null);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AggregationMode,
                AggregationMode.Disabled.GetHashCode());
            operations.Add(builder.Build());

            //name
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                              ContactsContract.CommonDataKinds.StructuredName.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.DisplayName,cname);
            operations.Add(builder.Build());

            //Number
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                              ContactsContract.CommonDataKinds.Phone.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.Number, cnumber);
            /*
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Type,
                              ContactsContract.CommonDataKinds.Phone.InterfaceConsts.TypeCustom);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Label, "QRnumber");
            */        
            operations.Add(builder.Build());

            ContentProviderResult[] res;
            try
            {
                res = this.ContentResolver.ApplyBatch(ContactsContract.Authority, operations);
                Toast.MakeText(this, "Success", ToastLength.Long).Show();
            }
            catch
            {
                Toast.MakeText(this, "Fail", ToastLength.Long).Show();
            }
            
            //출처: https://ghj1001020.tistory.com/4 [혁준 블로그] Android studio
            //https://forums.xamarin.com/discussion/158608/how-to-get-the-value-of-contactscontract-rawcontacts-aggregation-mode-disabled-in-xamarin  xamarin

        }
    }
}