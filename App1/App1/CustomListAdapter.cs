using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Object = Java.Lang.Object;


namespace App1
{
    public class CustomListAdapter : BaseAdapter<CustomControl>, IFilterable
    {
        private List<CustomControl> _originalData;
        private List<CustomControl> _items;
        private readonly Activity _context;

        //Constuctor

        public CustomListAdapter(Activity activity, IEnumerable<CustomControl> chemicals)
        {
            _items = chemicals.OrderBy(s => s.Name).ToList();
            _context = activity;

            Filter = new CustomlFilter(this);


        }


        //customControl id

        public override long GetItemId(int position)
        {
            return position;
        }

        //return inflated view

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.row_component, null);

            var chemical = _items[position];

            var nameView = view.FindViewById<TextView>(Resource.Id.textView1);
            var imageView = view.FindViewById<ImageView>(Resource.Id.imgPic);
            nameView.Text = chemical.Name;
            MemoryStream ms = new MemoryStream(chemical.Image);
            Bitmap image = BitmapFactory.DecodeStream(ms);
            imageView.SetImageBitmap(image);
            return view;
        }

        //total num of customControl
        public override int Count
        {
            get { return _items.Count; }
        }

        //get position
        public override CustomControl this[int position]
        {
            get { return _items[position]; }
        }


        //create Filter
        public Filter Filter { get; private set; }

        public override void NotifyDataSetChanged()
        {
            // remember to update the indices here!
            base.NotifyDataSetChanged();
        }

        private class CustomlFilter : Filter
        {
            private readonly CustomListAdapter _adapter;
            public CustomlFilter(CustomListAdapter adapter)
            {
                _adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();
                var results = new List<CustomControl>();
                if (_adapter._originalData == null)
                    _adapter._originalData = _adapter._items;

                if (constraint == null) return returnObj;

                if (_adapter._originalData != null && _adapter._originalData.Any())
                {
                    // Compare constraint to all names lowercased. 
                    // It they are contained they are added to results.
                    results.AddRange(
                        _adapter._originalData.Where(
                            chemical => chemical.Name.ToLower().Contains(constraint.ToString())));
                }

                // Nasty piece of .NET to Java wrapping, be careful with this!
                returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;

                constraint.Dispose();

                return returnObj;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    _adapter._items = values.ToArray<Object>()
                        .Select(r => r.ToNetObject<CustomControl>()).ToList();

                _adapter.NotifyDataSetChanged();

                // Don't do this and see GREF counts rising
                constraint.Dispose();
                results.Dispose();
            }


        }
    }
}