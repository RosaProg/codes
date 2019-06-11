package deliverytrack.vss.com.deliverytrack;

import android.os.AsyncTask;
import android.util.Log;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONArray;
import org.json.JSONException;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

/**
 * Created by Emilse on 3/27/2016.
 */
public class LongOperation extends AsyncTask<String, Void, String> {

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

    @Override
    public String doInBackground(String... params) {
        /* IMPORTANT! Replace to this when the app migrates from Parse */
        //String user = ParseUser.getCurrentUser().getUsername().toString();

        String url=params[0];
        Log.v("URL doInBackground", url);
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
            Log.v("Vista", "" + e.toString());
        } finally {
            if (instream != null) {
                try {
                    instream.close();
                } catch (Exception exc) {
                    Log.v("Vista",""+ exc.toString());
                }
            }
        }

        if(result!=null){
            Log.v("RESULT doInBackground", result.toString());
        }
        return "Executed";
    }

    @Override
    protected void onPostExecute(String result) {
        Log.v("vvvv", "" + result);
    }

    @Override
    protected void onPreExecute() {}

    @Override
    protected void onProgressUpdate(Void... values) {}
}
