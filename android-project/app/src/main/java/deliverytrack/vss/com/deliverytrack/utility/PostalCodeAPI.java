package deliverytrack.vss.com.deliverytrack.utility;

import android.util.Log;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLEncoder;
import java.util.ArrayList;

/**
 * Created by Adi-Loch on 12/16/2015.
 */
public class PostalCodeAPI {

    private static final String TAG = PostalCodeAPI.class.getSimpleName();

    private static final String PLACES_API_BASE = "https://maps.googleapis.com/maps/api/place/details/json";

    private static final String API_KEY = "AIzaSyCFkcLoC-Rks2J6QNSFrMAYuTSsKd-YUsk";

    public String[] details(String placeId) {

        String [] parts = new String[2];
        HttpURLConnection conn = null;
        StringBuilder jsonResults = new StringBuilder();

        try {
            StringBuilder sb = new StringBuilder(PLACES_API_BASE);
            sb.append("?key=" + API_KEY);
            sb.append("&placeid=" + placeId);

            URL url = new URL(sb.toString());
            conn = (HttpURLConnection) url.openConnection();
            InputStreamReader in = new InputStreamReader(conn.getInputStream());

            Log.d("The url", url.toString() + " ");

            // Load the results into a StringBuilder
            int read;
            char[] buff = new char[1024];
            while ((read = in.read(buff)) != -1) {
                jsonResults.append(buff, 0, read);
            }
        } catch (MalformedURLException e) {
            Log.e(TAG, "Error processing Places API URL", e);
            return parts;
        } catch (IOException e) {
            Log.e(TAG, "Error connecting to Places API", e);
            return parts;
        } finally {
            if (conn != null) {
                conn.disconnect();
            }
        }
        try {
            // Create a JSON object hierarchy from the results
            JSONObject jsonObj = new JSONObject(jsonResults.toString());
            JSONArray addressComponents = jsonObj.getJSONObject("result").getJSONArray("address_components");
            for(int i = 0; i < addressComponents.length(); i++) {
                JSONArray typesArray = addressComponents.getJSONObject(i).getJSONArray("types");
                for (int j = 0; j < typesArray.length(); j++) {
                    if (typesArray.get(j).toString().equalsIgnoreCase("postal_code")) {
                        parts[0] = addressComponents.getJSONObject(i).getString("long_name");
                    }
                    if (typesArray.get(j).toString().equalsIgnoreCase("locality")) {
                        parts[1] = addressComponents.getJSONObject(i).getString("long_name");
                    }
                }
            }

        } catch (JSONException e) {
            Log.e(TAG, "Cannot process JSON results", e);
        }

        return parts;
    }
}
