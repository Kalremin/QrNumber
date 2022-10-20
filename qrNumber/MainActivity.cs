using System;
using System.Collections.Generic;
using System.IO;

using Android;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Database;
using Android.Telephony;
using Android.Content.PM;
using Android.Graphics;

using Xamarin.Essentials;
using Xamarin.Forms;

using SQLite;


using QRCoder;
using System.Text;

namespace qrNumber
{


    [Activity(Label = "MainActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        
        Intent intent;
        ImageView qrimageView;

        //string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //string folder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        //string folder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DataDirectory.ToString()).AbsolutePath;

        string path = (string)Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DataDirectory.ToString()); // /storage/emulated/0/data
        string pathpackage;
        string pathtxt;
        private long time = 0;

        
        
        //Toolbar toolbar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main);
            
           
            Android.Support.V7.Widget.Toolbar toolbar 
                = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "메인";// "QRcodeName";

            

            TextView text = FindViewById<TextView>(Resource.Id.textView2);
            qrimageView = FindViewById<ImageView>(Resource.Id.qrcodeimage);

            if (!File.Exists(getNamePath()))
            {
                DirectoryInfo directory = new DirectoryInfo(pathpackage = System.IO.Path.Combine(path, PackageName));
                if (!directory.Exists)
                    directory.Create();
                setNameData("Nothing");
            }
                
            //출처: https://ghj1001020.tistory.com/9 [혁준 블로그]



            SBconverter sBconverter = new SBconverter();
            Bitmap mainqrcode = sBconverter.stringToBitmap(getNamedata(),GetMyPhoneNumber());
            qrimageView.SetImageBitmap(mainqrcode);
            text.Text = getNamedata();
            
            
            
            
            /*
            StringBuilder sb = new StringBuilder(@"{number:");
            sb.Append(GetMyPhoneNumber());
            */

            
            /*
            StreamWriter textWrite = File.CreateText(folder+@"/test.txt"); //생성
            textWrite.WriteLine("abcdefghijk"); //쓰기
            textWrite.Dispose(); //파일 닫기
            */



        }

        [Java.Interop.Export("insertNameClick")]
        public void insertNameClick(Android.Views.View view)
        {
            TextView text = FindViewById<TextView>(Resource.Id.textView2);
            EditText editText = FindViewById<EditText>(Resource.Id.editNameText);
            setNameData(editText.Text);
            
            text.Text = editText.Text;

            SBconverter sBconverter = new SBconverter();
            Bitmap mainqrcode = sBconverter.stringToBitmap( editText.Text ,GetMyPhoneNumber());
            qrimageView = FindViewById<ImageView>(Resource.Id.qrcodeimage);
            qrimageView.SetImageBitmap(mainqrcode);
        }

        public string GetMyPhoneNumber()
        {

            TelephonyManager mTelephonyMgr;

            mTelephonyMgr = (TelephonyManager)GetSystemService(TelephonyService);

            var Number = mTelephonyMgr.Line1Number;
            return Number;
            //TelephonyManager mgr = Android.App.Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;
            //return mgr.Line1Number;

        }


        public string getNamePath()
        {
            pathpackage = System.IO.Path.Combine(path, PackageName);// /storage/emulated/0/data/App4.App4
            pathtxt = System.IO.Path.Combine(pathpackage, "selfname.txt"); // /storage/emulated/0/data/App4.App4/selfname.txt

            return pathtxt;
        }

        public string getNamedata()
        {

            string[] nameData = File.ReadAllLines(getNamePath());
            return nameData[0];
            /*
            if (!File.Exists(pathtxt))
                File.Create(pathtxt);
            using (FileStream fs = new FileStream(pathtxt, FileMode.OpenOrCreate))
            {

                StreamWriter textWrite = File.read; //생성
                textWrite.WriteLine("abcdefghijk"); //쓰기
                textWrite.Dispose();
            }
            */
        }

        public void setNameData(string namelayout)
        {
            //StreamWriter textWrite = File.CreateText(getNamePath()); //생성
            
            StreamWriter textWrite = new StreamWriter(getNamePath(), false, Encoding.Unicode);
            textWrite.WriteLine(namelayout); //쓰기
            textWrite.Dispose(); //파일 닫기
        }

        
        public override void OnBackPressed()
        {

            if(Java.Lang.JavaSystem.CurrentTimeMillis()-time>=2000)//2초
            {
                time = Java.Lang.JavaSystem.CurrentTimeMillis();
                Toast.MakeText(this, "한번 더 누르시면 앱이 종료됩니다.", ToastLength.Short).Show();
                
            }
            else if (Java.Lang.JavaSystem.CurrentTimeMillis() - time < 2000)
            {
                MoveTaskToBack(true);
            }

        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change main_compat_menu
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.scanQR_main_menu:
                    //intent = new Intent(this, typeof(ScanQRcodeActivity));
                    intent = new Intent(this, typeof(ScangoogleQRActivity));
                    StartActivity(intent);
                    break;
                    
                case Resource.Id.list_main_menu:
                    intent = new Intent(this, typeof(NumberListActivity));
                    StartActivity(intent);
                    break;

                case Resource.Id.explain_main_menu:
                    intent = new Intent(this, typeof(ExplainAppActivity));
                    StartActivity(intent);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
       
    }
}