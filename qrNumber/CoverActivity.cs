using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace qrNumber
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class CoverActivity : Activity
    {
        Intent intent;
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {


            if (requestCode == 1)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                {
                    Toast.MakeText(this, "모든 권한 허용해야 합니다.", ToastLength.Short).Show();
                    Finish();
                }
                else
                {
                    intent = new Intent(this, typeof(MainActivity));
                    //System.Threading.Thread.Sleep(3000);
                    StartActivity(intent);
                    //Toast.MakeText(this, "앱을 재실행해주세요.", ToastLength.Long).Show();

                }
                return;
            }

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.cover);
            
            List<string> permissions = new List<string>();

            // 권한이 있는지 확인할 항목들
            List<string> checkPermissions = new List<string>();
            checkPermissions.Add(Manifest.Permission.ReadExternalStorage);
            checkPermissions.Add(Manifest.Permission.WriteExternalStorage);
            checkPermissions.Add(Manifest.Permission.ReadPhoneState);
            checkPermissions.Add(Manifest.Permission.ReadContacts);
            checkPermissions.Add(Manifest.Permission.WriteContacts);


            // 권한이 있는지 확인하고 없다면 체크할 목록에 추가합니다.
            foreach (var checkPermission in checkPermissions)
            {
                if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, checkPermission) != (int)Permission.Granted)
                {
                    permissions.Add(checkPermission);
                }
            }

            // 권한없는 항목에 대해서 추가 허용을 할지 물어보는 팝업을 띄웁니다.
            if (permissions.Count != 0)
                Android.Support.V4.App.ActivityCompat.RequestPermissions(this, permissions.ToArray(), 1);
            else
            {
                intent = new Intent(this, typeof(MainActivity));
                //System.Threading.Thread.Sleep(3000);
                StartActivity(intent);
            }
            //출처: https://kjun.kr/705 [kjun.kr]
            // Create your application here
        }
    }
}