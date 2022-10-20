using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace qrNumber
{
    [Activity(Label = "PopupNameActivity1")]
    public class PopupNameActivity : Activity
    {
        //https://ghj1001020.tistory.com/9

        string path = (string)Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DataDirectory.ToString()); // /storage/emulated/0/data
        string pathpackage;
        string pathtxt;
        EditText editText;
        MainActivity mainact;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.popupNamelayout);
            editText = FindViewById<EditText>(Resource.Id.editNameText);

            
            if (File.Exists(pathtxt))
            {
                editText.Text = mainact.getNamedata();
            }
            
            // Create your application here
        }
        [Java.Interop.Export("okclick")]
        public void okclick(View view)
        {
            setNameData(editText.Text);
            TextView text = FindViewById<TextView>(Resource.Id.textView1);
            text.Text = editText.Text;

            SBconverter sBconverter = new SBconverter();
            Android.Graphics.Bitmap mainqrcode = sBconverter.stringToBitmap(@"{name:" + editText.Text + @",number:" + mainact.GetMyPhoneNumber() + @"}");
            ImageView qrimageView = FindViewById<ImageView>(Resource.Id.qrcodeimage);
            qrimageView.SetImageBitmap(mainqrcode);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Outside)
                return false;
            else
                return true;
        }

        public string getNamePath()
        {
            pathpackage = System.IO.Path.Combine(path, PackageName);// /storage/emulated/0/data/App4.App4
            pathtxt = System.IO.Path.Combine(pathpackage, "selfname.txt"); // /storage/emulated/0/data/App4.App4/selfname.txt

            return pathtxt;
        }

        public void setNameData(string namelayout)
        {
            //StreamWriter textWrite = File.CreateText(getNamePath()); //생성
            StreamWriter textWrite = new StreamWriter(getNamePath(), false, Encoding.Unicode);
            textWrite.WriteLine(namelayout); //쓰기
            textWrite.Dispose(); //파일 닫기
        }
    }
}