package deliverytrack.vss.com.deliverytrack.adapters;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.Uri;
import android.os.AsyncTask;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.parse.ParseQuery;
import com.parse.ParseUser;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.asynch.CancelOrder;
import deliverytrack.vss.com.deliverytrack.asynch.FindOrder;
import deliverytrack.vss.com.deliverytrack.asynch.UpdateOrder;
import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.fragments.orders.OrderdetailedFragment;
import deliverytrack.vss.com.deliverytrack.models.Area;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.fragments.areas.AreasForToday.*;

/**
 * Created by Emilse on 3/25/2016.
 */
public class AreasAdapter extends BaseAdapter implements View.OnClickListener {

    private JSONArray areas;
    private Activity context;
    private Fragment mFragment;

    private JSONObject area;

    public AreasAdapter(Activity context, JSONArray areas, Fragment fragment) {
        if(areas != null){
        Log.v("Emilse", areas.toString());}
        this.context = context;
        this.areas = areas;
        this.mFragment = fragment;

        //imageLoader = new ImageLoader(context);

    }

    //public void setItems(ArrayList<String> items) {
    public void setItems(JSONArray items) {
        Log.v("Emm", "entro a set area 2");
        this.areas = items;
    }

    class ViewHolder {
        TextView txt_area_from_date;
        TextView txt_area_to_date;
        TextView txt_area_address;
        TextView area_start_time;
        TextView area_end_time;
        Button btn_accept;
        Button btn_closed;
        ImageView client_logo;
        int position;
    }

    @Override
    public int getCount() {
        return areas.length(); //areas.size();
    }

    @Override
    public JSONObject getItem(int position) {
        JSONObject vv = null;
        try {
            vv = areas.getJSONObject(position);
        } catch (JSONException e) {
            Log.e("JSON Parser4", "Error parsing data " + e.toString());
        }
        return vv;
    }

    @Override
    public long getItemId(int position) {
        return 0;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        Log.v("Emilseeee","getView");
        //final String area = getItem(position);
        //Convert to JSONObject and from this to string
        //final String area = getItem(position);
        final JSONObject area = getItem(position);
        try {
            Log.v("Emi",area.get("id").toString());
        } catch (JSONException e) {
            Log.e("JSON Parser5", "Error parsing data " + e.toString());
        }
        LayoutInflater inflater = context.getLayoutInflater();
        ViewHolder holder = null;
        /*for (String v : areas){
            //Log.v("Iterator Emil", v.id.toString());
        }*/
        if (convertView == null) {
            holder = new ViewHolder();
            convertView = inflater.inflate(R.layout.area_row, null);
            holder.txt_area_from_date = (TextView) convertView.findViewById(R.id.txt_area_from_date);
            holder.txt_area_to_date = (TextView) convertView.findViewById(R.id.txt_area_to_date);
            holder.txt_area_address = (TextView) convertView.findViewById(R.id.txt_area_address);
            holder.area_start_time = (TextView) convertView.findViewById(R.id.area_start_time);
            holder.btn_accept = (Button) convertView.findViewById(R.id.btn_accept);
            holder.btn_closed = (Button) convertView.findViewById(R.id.btn_closed);
            holder.area_end_time = (TextView) convertView.findViewById(R.id.area_end_time);
            holder.client_logo = (ImageView) convertView.findViewById(R.id.client_logo);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }
        try {
            holder.txt_area_from_date.setText(area.get("from_date").toString());
            holder.txt_area_to_date.setText(area.get("to_date").toString());
            holder.txt_area_address.setText(area.get("address").toString());
            holder.area_start_time.setText(area.get("start_time").toString());
            holder.area_end_time.setText(area.get("end_time").toString());

            if(area.get("status").equals("1")){
                holder.btn_closed.setVisibility(View.GONE);
                holder.btn_accept.setVisibility(View.VISIBLE);
            }else{
                holder.btn_closed.setVisibility(View.VISIBLE);
                holder.btn_accept.setVisibility(View.GONE);
            }

        } catch (JSONException e) {
            Log.e("JSON Parser5", "Error parsing data " + e.toString());
        }
        holder.position = position;

        holder.btn_accept.setTag(position);
        holder.btn_accept.setOnClickListener(this);
        holder.btn_closed.setTag(position);
        holder.btn_closed.setOnClickListener(this);
        convertView.setTag(holder);

        return convertView;
    }

    @Override
    public void onClick(View v) {

        int position = (Integer) v.getTag();
        try {
            area = this.areas.getJSONObject(position);
        }catch (JSONException e) {
            Log.e("JSON Parser5", "Error parsing data " + e.toString());
        }

        switch (v.getId()) {

            case R.id.btn_deliver:
                Log.v("Emil botones","btn_delivery");
                break;

            case R.id.btn_pickup:
                Log.v("Emil botones", "btn_pickup");
                break;
            case R.id.btn_accept_pickup:
                Log.v("Emil botones", "btn_accept_pickup");
                break;
            case R.id.btn_arrived:
                Log.v("Emil botones", "btn_arrived");
                break;

            case R.id.btn_detail:
                Log.v("Emil botones", "btn_detail");
                break;

            case R.id.btn_accept:
                Log.v("Emil botones", "btn_accept");
                new AlertDialog.Builder(context)
                        .setTitle("Apply to Area")
                        .setMessage("Are you sure you want to apply to this area? Please have in mind that penalties may apply if accepted and then not done.")
                        .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {

                                new LongOperation().execute("");
                                Toast.makeText(context, "Your area has been assigned. Please remember than no more than one area will be assigned and the first area to be applied to will be the area assigned.", Toast.LENGTH_LONG).show();

                            }
                        })
                        .setNegativeButton(android.R.string.no, new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {
                            }
                        })
                        .setIcon(android.R.drawable.ic_dialog_alert)
                        .show();
                break;

            case R.id.call_customer:
                Log.v("Emil botones", "call_customer");
                break;

            case R.id.btn_closed:
                Log.v("Emil botones", "btn_closed");
                break;

        }

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

    ListView lv = null;
    public AreasAdapter areaAdapter;
    private class LongOperation extends AsyncTask<String, Void, String> {

        JSONArray areasToday;
        String user="";

        @Override
        protected String doInBackground(String... params) {
            String url = "";
            /* IMPORTANT! Replace this when the app migrates from Parse */
            user = ParseUser.getCurrentUser().getUsername().toString();

            try {
                Log.v("doInBackground", area.get("id").toString());
                url = "http://www.vsstechnology.com/deliverytrack/api/?q=area_accepted&user="+user+"&area="+area.get("id").toString();
                Log.v("MiURL", url);
            } catch (JSONException e) {
                Log.e("doInBackground", "Error parsing data " + e.toString());
            }

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

            /*if(result!=null){
                try{
                    areasToday = new JSONArray(result);
                }catch(JSONException e) {
                    Log.e("JSON Parser", "Error parsing data " + e.toString());
                }
            }
            Log.v("rrrr", areasToday.toString());*/
            Log.v("vvvv", "f: " + result);
            Log.v("pppp","zzzzzzzz");
            return "Executed";
        }

        @Override
        protected void onPostExecute(String result) {
            Log.v("vvvv", "" + result);
            // NO FUNCIONA lv.setAdapter(areaAdapter);
            // NO FUNCIONA Toast.makeText(context, "Prueba2", Toast.LENGTH_LONG).show();
        }

        @Override
        protected void onPreExecute() {}

        @Override
        protected void onProgressUpdate(Void... values) {}
    }
}
