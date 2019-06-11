package deliverytrack.vss.com.deliverytrack.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Filter;
import android.widget.Filterable;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.PlaceAPI;

/**
 * Created by Adi-Loch on 12/15/2015.
 */
public class PlacesAdapter extends ArrayAdapter<String> implements Filterable {

    ArrayList<String> resultList;
    Context mContext;
    int mResource;
    PlaceAPI mPlaceAPI = new PlaceAPI();

    public PlacesAdapter(Context context, int resource) {
        super(context, resource);

        mContext = context;
        mResource = resource;
    }

    @Override
    public int getCount() {
        // Last item will be the footer
        return resultList.size();
    }

    @Override
    public String getItem(int position) {
        return resultList.get(position);
    }

    @Override
    public Filter getFilter() {
        Filter filter = new Filter() {
            @Override
            protected FilterResults performFiltering(CharSequence constraint) {
                FilterResults filterResults = new FilterResults();
                if (constraint != null) {
                    resultList = mPlaceAPI.autocomplete(constraint.toString());

                    filterResults.values = resultList;
                    filterResults.count = resultList.size();
                }

                return filterResults;
            }

            @Override
            protected void publishResults(CharSequence constraint, Filter.FilterResults results) {
                if (results != null && results.count > 0) {
                    notifyDataSetChanged();
                } else {
                    notifyDataSetInvalidated();
                }
            }
        };

        return filter;
    }


    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        View view;
        //if (convertView == null) {
        LayoutInflater inflater = (LayoutInflater) mContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        view = inflater.inflate(R.layout.autocomplete_list, null);
        TextView autocompleteTextView = (TextView) view.findViewById(R.id.autocompleteText);
        autocompleteTextView.setText(DeliveryTrackUtils.splitStrings(resultList.get(position))[0]);
        return view;
    }

}

