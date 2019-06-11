package deliverytrack.vss.com.deliverytrack.asynch;

import android.app.ProgressDialog;
import android.os.AsyncTask;
import android.support.v4.app.Fragment;
import android.util.Log;

import com.google.gson.Gson;

import org.apache.http.NameValuePair;
import org.json.JSONObject;

import java.util.List;


import deliverytrack.vss.com.deliverytrack.interfaces.PayOneCallbacks;
import deliverytrack.vss.com.deliverytrack.utility.JSONParser;

/**
 * Created by Adi-Loch on 1/9/2016.
 */
public class DTHTTPURLConnection extends AsyncTask<Void, Void, Object> {
    private final int requestType;
    private String urlStr;
    private List<NameValuePair> object;
    private Fragment fragment;
    private ProgressDialog mProgressDialog;

    public DTHTTPURLConnection(String urlStr, List<NameValuePair> object, Fragment fragment, int requestType) {
        this.urlStr = urlStr;
        this.object = object;
        this.fragment = fragment;
        this.requestType = requestType;
    }

    @Override
    protected JSONObject doInBackground(Void... params) {
        JSONParser parser = new JSONParser();
        JSONObject obj = parser.makeHttpRequest(this.urlStr, "POST", this.object);


//        URL url;
//        HttpURLConnection connection = null;
//        try {
//            //Create connection
//            url = new URL(this.urlStr);
//            Log.d("the url", this.urlStr);
//            connection = (HttpURLConnection) url.openConnection();
//            connection.setRequestMethod("POST");
//            connection.setRequestProperty("Content-Type",
//                    "application/x-www-form-urlencoded");
//
//
//            connection.setUseCaches(false);
//            connection.setDoInput(true);
//            connection.setDoOutput(true);
//
////            //Send request
////            DataOutputStream wr = new DataOutputStream(
////                    connection.getOutputStream());
////            wr.writeBytes(convertString(this.object));
////            wr.flush();
////            wr.close();
//
//            //Get Response
//            InputStream is = connection.getInputStream();
//            BufferedReader rd = new BufferedReader(new InputStreamReader(is));
//            String line;
//            StringBuffer response = new StringBuffer();
//            while ((line = rd.readLine()) != null) {
//                response.append(line);
//                response.append('\r');
//            }
//            rd.close();
//            Log.d("response.toString()", response.toString());
//            return response.toString();
//
//        } catch (Exception e) {
//
//            e.printStackTrace();
//            return null;
//
//        } finally {
//
//            if (connection != null) {
//                connection.disconnect();
//            }
//        }

        return obj;
    }


    @Override
    protected void onPreExecute() {
        super.onPreExecute();
        mProgressDialog = new ProgressDialog(this.fragment.getActivity());
        mProgressDialog.setTitle("Retrieving Data");
        mProgressDialog.setMessage("Loading...");
        mProgressDialog.setIndeterminate(false);
        mProgressDialog.setCanceledOnTouchOutside(false);
        mProgressDialog.setCancelable(false);
        mProgressDialog.show();

    }

    @Override
    protected void onPostExecute(Object s) {
        super.onPostExecute(s);
        mProgressDialog.dismiss();
        ((PayOneCallbacks) this.fragment).onReceiveJSONString(s, this.requestType);

    }
}

