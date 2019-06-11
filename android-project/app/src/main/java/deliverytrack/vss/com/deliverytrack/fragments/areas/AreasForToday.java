package deliverytrack.vss.com.deliverytrack.fragments.areas;

/**
 * Created by Emilse on 3/25/2016.
 */
import android.app.Activity;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.content.Context;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import com.parse.ParseObject;
import com.parse.ParseUser;

import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.BaseActivity;
import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.adapters.AreasAdapter;
import deliverytrack.vss.com.deliverytrack.interfaces.AreasCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Area;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.util.Log;

public class AreasForToday extends Fragment implements AreasCallbacks {
    Context thiscontext;
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    public AreasAdapter areaAdapter;

    ListView lv = null;
    ArrayAdapter <String> adapter;
    //private ArrayList<String> areasToday = new ArrayList<>();
    private JSONArray areasToday;
    /*public ArrayList<String> getAreasToday() {
        return areasToday;
    }*/

    //public void setArea(ArrayList<String> areasToday) {
    public void setArea(JSONArray areasToday) {
        this.areasToday = areasToday;
        Log.v("Emm", "entro a set area 1");
        areaAdapter.setItems(areasToday);
        Log.v("Emm", "entro a set area 3");
        areaAdapter.notifyDataSetChanged();
        Log.v("Emm", "entro a set area 4");
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        getActivity()
                .setTitle(getString(R.string.title_areas));
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment OrderDelivered.
     */
    // TODO: Rename and change types and number of parameters
    public static AreasForToday newInstance(String param1, String param2) {
        AreasForToday fragment = new AreasForToday();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public AreasForToday() {
        // Required empty public constructor
    }

    public void getAreasForToday()
    {
        Log.v("Vista", "Entro en getAreasForToday");
        //Not using Parse anymore
        // Starting REST service request
        new LongOperation().execute("");
    }

    public String convertStreamToString(InputStream is) {
        BufferedReader reader = new BufferedReader(new InputStreamReader(is));
        StringBuilder sb = new StringBuilder();
        String line = null;

        try {
            while ((line = reader.readLine()) != null) {
                sb.append(line + "\n");
            }
        } catch (IOException e) {
        } finally {
            try {
                is.close();
            } catch (IOException e) {
            }
        }

        return sb.toString();
    }

    private class LongOperation extends AsyncTask<String, Void, String> {

        @Override
        protected String doInBackground(String... params) {
            /* IMPORTANT! Replace this when the app migrates from Parse */
            //String user = ParseUser.getCurrentUser().getUsername().toString();
            String url = "http://www.vsstechnology.com/deliverytrack/api/?q=available_areas_today";
            HttpClient httpclient = new DefaultHttpClient();
            String result = null;
            HttpGet httpget = new HttpGet(url);
            HttpResponse response = null;
            InputStream instream = null;

            try {
                response = httpclient.execute(httpget);
                HttpEntity entity = response.getEntity();

                if (entity != null) {
                    instream = entity.getContent();
                    result = convertStreamToString(instream);
                }

            } catch (Exception e) {
                Log.v("Vista",""+ e.toString());
            } finally {
                if (instream != null) {
                    try {
                        instream.close();
                    } catch (Exception exc) {
                        Log.v("Vista",""+ exc.toString());
                    }
                }
            }
            Log.v("RESULT", result.toString());
            if(result!=null){
                /*if(result.contains("User")){
                    // Don't add data to the ListView
                    // Add the message
                    Log.v("ENTRO EN EL IF", "ffffffffff");
                    areasToday.put(result);
                }else{
                    Log.v("ELSE", "Entro en el else.");*/
                    try{
                        areasToday = new JSONArray(result);
                    }catch(JSONException e) {
                        Log.e("JSON Parser", "Error parsing data " + e.toString());
                    }
                //}

            }
            Log.v("rrrr", areasToday.toString());
            Log.v("vvvv", "f: " + result);
            areaAdapter.setItems(areasToday);
            try {
              //  areaAdapter.notifyDataSetChanged();
            } catch (Exception e){
                Log.e("DataSetChangedException",e.getMessage());
            }
            return "Executed";
        }

        @Override
        protected void onPostExecute(String result) {
            Log.v("vvvv", "" + result);
            lv.setAdapter(areaAdapter);
        }

        @Override
        protected void onPreExecute() {}

        @Override
        protected void onProgressUpdate(Void... values) {}
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_area_available, container, false);
        Log.v("Vista", "Entrando en la vista");
        thiscontext = container.getContext();

        /*Activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {

                //stuff that updates ui

            }
        });*/
        lv = (ListView) view.findViewById(R.id.area_list);
        getAreasForToday();
        //Log.v("AreasForToday",areasToday.toString());
        areaAdapter = new AreasAdapter(getActivity(), areasToday, this);
        //lv.setAdapter(areaAdapter);
        lv.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> arg0, View arg1,
                                           int pos, long id) {
                final int item = pos;
                System.out.println("Click en areas en item numero: " + item);
                //final Area order = ordersWaitingForAccept.get(item);
                //DeliveryTrackUtils.cancelOrder(order, OrderPendingAccept.this);
                return true;
            }
        });

        return view;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }


    @Override
    public void onResume() {
        super.onResume();
        // Set title
        getActivity()
                .setTitle(getString(R.string.title_areas));
    }

    @Override
    public void getAreas(List<ParseObject> parseObjects, int tableId) {

    }

    @Override
    public void onAreaAccepted(Area area) {

    }

    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p/>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFragmentInteractionListener {
        // TODO: Update argument type and name
        public void onFragmentInteraction(Uri uri);
    }
}
