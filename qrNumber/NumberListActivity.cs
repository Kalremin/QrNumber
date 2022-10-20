using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace qrNumber
{
    [Activity(Label = "NumberListActivity")]
    public class NumberListActivity : AppCompatActivity
    {
        ListView listView;
        List<ItemContact> itemContacts;
        /*
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            if (requestCode != null)
            {

            }
            else
            {

            }
        }
        */
        /*
        Android.Widget.SearchView searchView;
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.search, menu);

            var searchItem = menu.FindItem(Resource.Id.action_search);

            searchView = searchItem.ActionView.JavaCast<Android.Widget.SearchView>();
            
            searchView.QueryTextSubmit += (sender, args) =>
            {
                Toast.MakeText(this, "You searched: " + args.Query, ToastLength.Short).Show();

            };
            

            return base.OnCreateOptionsMenu(menu);
        }

        */
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listNumber_layout);
            // Create your application here
            //Dictionary<string, string> callList = new Dictionary<string, string>();
            //string test = ContactsContract.Contacts.InterfaceConsts.DisplayName;
            listView = FindViewById<ListView>(Resource.Id.listNumberView);

            itemContacts = new List<ItemContact>();
            ItemContact tempitem = new ItemContact();
            Android.Support.V7.Widget.Toolbar toolbar
                = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "연락처 리스트";// "QRcodeName";
            /*
            toolbar.InflateMenu(Resource.Menu.searchmenu);
            toolbar.MenuItemClick+= (object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e) =>
            {

            };

            var search = toolbar.Menu.FindItem(Resource.Id.searchmenu);
            var searchView = search.ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();
            */
            //toolbar.InflateMenu

            using (var phones = Android.App.Application.Context.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null))
            {
                if (phones != null)
                {
                    while (phones.MoveToNext())
                    {
                        try
                        {
                            string name = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                            string phoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                            
                            string[] words = name.Split(' ');
                            var contact = new ItemContact();
                            contact.name = name;
                            contact.phoneNumber = phoneNumber;
                            itemContacts.Add(contact);
                        }
                        catch 
                        {
                            //something wrong with one contact, may be display name is completely empty, decide what to do
                        }
                    }
                    phones.Close();
                }
                // if we get here, we can't access the contacts. Consider throwing an exception to display to the user
            }
            //https://alexdunn.org/2017/09/08/xamarin-tip-read-all-contacts-in-android/
            
            var adapter = new NumberListAdapter(this, itemContacts);
            listView.Adapter = adapter;
            listView.ItemClick += listViewItemClick;
            
        }
        
        private void listViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            listView = FindViewById<ListView>(Resource.Id.listNumberView);
            
            //TextView nametext = FindViewById<TextView>(Resource.Id.itemName);
            //TextView numtext = FindViewById<TextView>(Resource.Id.itemNumber);
            Intent intent = new Intent(this, typeof(ContactQRActivity));
            
            intent.PutExtra("name", itemContacts[e.Position].name);
            intent.PutExtra("number", itemContacts[e.Position].phoneNumber);
            StartActivity(intent);
            
        }
        //https://www.youtube.com/watch?v=WMKrR_5uh0A
    }
}