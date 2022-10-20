using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Vision.Barcodes;
using Android.Gms.Vision;
using Android.Graphics;
using Android.Support.V4.App;
using Android;
using Android.Content.PM;
using static Android.Gms.Vision.Detector;
using Android.Util;
using Android.Provider;

namespace qrNumber
{
    [Activity(Label = "ScangoogleQRActivity")]
    public class ScangoogleQRActivity : AppCompatActivity,ISurfaceHolderCallback,IProcessor
    {
        SurfaceView cameraview;
        TextView txtresult;
        BarcodeDetector barcodeDetector;
        CameraSource cameraSource;
        const int RequestCameraPermissionID = 1001;
        string tempname, tempnumber;
        bool checkdetect=false;
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch(requestCode)
            {
                case RequestCameraPermissionID:
                    {
                        if(grantResults[0]==Permission.Granted)
                        {
                            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
                            {
                                ActivityCompat.RequestPermissions(this, new string[]{Manifest.Permission.Camera}, RequestCameraPermissionID);
                                return;
                            }
                            try
                            {
                                cameraSource.Start(cameraview.Holder);
                            }
                            catch (InvalidOperationException)
                            {

                            }
                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.googlescanlayout);

            cameraview = FindViewById<SurfaceView>(Resource.Id.cameraPreview);
            txtresult = FindViewById<TextView>(Resource.Id.txtresultview);

            barcodeDetector = new BarcodeDetector.Builder(this)
                .SetBarcodeFormats(BarcodeFormat.QrCode)
                .Build();
            cameraSource = new CameraSource
                .Builder(this, barcodeDetector)
                .SetRequestedPreviewSize(640,480)
                .SetAutoFocusEnabled(true)  //----
                .Build();
            // Create your application here
            
            cameraview.Holder.AddCallback(this);
            barcodeDetector.SetProcessor(this);
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
           
            //throw new NotImplementedException();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if(ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[]
                {
                    Manifest.Permission.Camera
                }, RequestCameraPermissionID);
                return;
            }
            try
            {
                cameraSource.Start(cameraview.Holder);
            }
            catch(InvalidOperationException)
            {

            }
            //throw new NotImplementedException();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {

            if (checkdetect)
            {
                try
                {
                    insertContacts(tempname, tempnumber);
                    Toast.MakeText(this, "저장 성공", ToastLength.Short).Show();
                }
                catch
                {
                    Toast.MakeText(this, "저장 실패", ToastLength.Short).Show();
                }
            }
            else
                Toast.MakeText(this, "무관한 QR코드입니다.", ToastLength.Short).Show();

            cameraSource.Stop();
            //throw new NotImplementedException();
        }

        public void ReceiveDetections(Detections detections)
        {
            SparseArray qrcodes = detections.DetectedItems;
            //cameraSource.Stop();
            string[] spstring,spstring2,spstring3;
            
            if(qrcodes.Size()!=0)
            {
                
                spstring = ((Barcode)qrcodes.ValueAt(0)).RawValue.Split("\r\n");

                if (spstring.Count<string>() > 1)    //VCARD
                {
                    if (spstring[0].Contains("VCARD"))
                    {
                        spstring2 = spstring[3].Split(':');
                        tempname = spstring2[1];
                        //txtresult.Text += " ";
                        spstring3 = spstring[5].Split(';');
                        spstring2 = spstring3[2].Split(':');
                        tempnumber = spstring2[1];
                        //txtresult.Text = spstring[3];
                        //insertContacts(tempname, tempnumber);
                        checkdetect = true;
                    }
                    
                }
                else if (spstring.Count<string>() == 1)
                {
                    spstring2 = spstring[0].Split(';');
                    if (spstring2[0].Contains("MECARD:")) //MECARD
                    {
                        spstring3 = spstring2[0].Split(':');
                        tempname = spstring3[2];
                        spstring3 = spstring2[1].Split(':');
                        tempnumber = spstring3[1];
                        //insertContacts(tempname, tempnumber);
                        checkdetect = true;

                    }
                    else if (spstring2[0].Contains("{\"name\""))   //App
                    {
                        Newtonsoft.Json.Linq.JObject jo = Newtonsoft.Json.Linq.JObject.Parse(spstring2[0]);
                        tempname = jo["name"].ToString();
                        tempnumber = jo["number"].ToString();
                        //insertContacts(tempname, tempnumber);
                        checkdetect = true;
                    }
                    
                }
                
                /*
                txtresult.Post(() => {
                    //Vibrator vibrator = (Vibrator)GetSystemService(Context.VibratorService);
                    //vibrator.Vibrate(1000);
                    //txtresult.Text = ((Barcode)qrcodes.ValueAt(0)).RawValue;
                }
                
                ); */
                Finish();
            }
            //throw new NotImplementedException();
        }

        public void Release()
        {
            throw new NotImplementedException();
        }

        public void insertContacts(string cname,string cnumber)
        {
            

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
            builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.DisplayName, cname);
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
            res = this.ContentResolver.ApplyBatch(ContactsContract.Authority, operations);
            /*
            try
            {
                res = this.ContentResolver.ApplyBatch(ContactsContract.Authority, operations);
                Toast.MakeText(this, "Success", ToastLength.Long).Show();
            }
            catch
            {
                Toast.MakeText(this, "Fail", ToastLength.Long).Show();
            }
            */
            //출처: https://ghj1001020.tistory.com/4 [혁준 블로그] Android studio
            //https://forums.xamarin.com/discussion/158608/how-to-get-the-value-of-contactscontract-rawcontacts-aggregation-mode-disabled-in-xamarin  xamarin

        }

        
    }
}