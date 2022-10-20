using System;
using System.Collections.Generic;
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
    class NumberListAdapter : BaseAdapter
    {
        private List<ItemContact> listitems;
        private Activity activity;
        public NumberListAdapter(Activity activity,List<ItemContact> listitems)
        {
            this.activity = activity;
            this.listitems = listitems;

        }

        /*
        Context context;

        public NumberListAdapter(Context context)
        {
            this.context = context;
        }
        */

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }


        public override long GetItemId(int position)
        {
            return listitems[position].id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.
                Inflate(Resource.Layout.itemlist,parent, false);

            var nametextview = view.FindViewById<TextView>(Resource.Id.itemName);
            var numbertextview = view.FindViewById<TextView>(Resource.Id.itemNumber);

            nametextview.Text = listitems[position].name;
            numbertextview.Text = listitems[position].phoneNumber;

            return view;
            /*
            var view = convertView;
            NumberListAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as NumberListAdapterViewHolder;

            if (holder == null)
            {
                holder = new NumberListAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                //view = inflater.Inflate(Resource.Layout.item, parent, false);
                //holder.Title = view.FindViewById<TextView>(Resource.Id.text);
                view.Tag = holder;
            }
            

            //fill in your items
            //holder.Title.Text = "new text here";

            return view;
            */
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return listitems.Count;
            }
        }

    }

    class NumberListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}