package deliverytrack.vss.com.deliverytrack;

import android.graphics.Color;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;

import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.MapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.android.gms.maps.model.PolylineOptions;

import org.w3c.dom.Document;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.utility.MapUtils;


public class MapActivity extends AppCompatActivity {


    LatLng Agent;
    private GoogleMap map;
    Document document;
    MapUtils v2GetRouteDirection;
    LatLng fromPosition;
    LatLng toPosition;
    MarkerOptions markerOptions;
    private boolean isSingle;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_map);

        map = ((MapFragment) getFragmentManager().findFragmentById(R.id.map))
                .getMap();
        isSingle = getIntent().getBooleanExtra("single", false);
        double lat = getIntent().getDoubleExtra("Lat", 0);
        double lon = getIntent().getDoubleExtra("Log", 0);

        double tolat = getIntent().getDoubleExtra("tolat", 0);
        double tolon = getIntent().getDoubleExtra("tolon", 0);

        Log.d("latitude longitude", lat + " " + lon + " " + tolat + " " + tolon + " ");

        if (map != null) {

            if (isSingle) {
                setTitle("Locate Agent");
                Agent = new LatLng(lat, lon);
                map.addMarker(new MarkerOptions().position(Agent)
                        .title("Agent Location"));
                map.moveCamera(CameraUpdateFactory.newLatLngZoom(Agent, 50));
                map.animateCamera(CameraUpdateFactory.zoomTo(10), 2000, null);
            } else {
                Agent = new LatLng(lat, lon);
                setTitle("Get Route");
                v2GetRouteDirection = new MapUtils();
                map.setMyLocationEnabled(true);
                map.getUiSettings().setZoomControlsEnabled(true);
                map.getUiSettings().setCompassEnabled(true);
                map.getUiSettings().setMyLocationButtonEnabled(true);
                map.getUiSettings().setAllGesturesEnabled(true);
                map.setTrafficEnabled(true);
                map.moveCamera(CameraUpdateFactory.newLatLngZoom(Agent, 50));
                map.animateCamera(CameraUpdateFactory.zoomTo(10), 2000, null);
                markerOptions = new MarkerOptions();
                fromPosition = new LatLng(lat, lon);
                toPosition = new LatLng(tolat, tolon);

            }

        }


    }


    @Override
    protected void onResume() {
        super.onResume();

        if (!isSingle) {
            GetRouteTask getRoute = new GetRouteTask();
            getRoute.execute();
        }
    }

    private class GetRouteTask extends AsyncTask<String, Void, String> {

        String response = "";

        @Override
        protected void onPreExecute() {

        }

        @Override
        protected String doInBackground(String... urls) {
            //Get All Route values
            document = v2GetRouteDirection.getDocument(fromPosition, toPosition, MapUtils.MODE_DRIVING);
            response = "Success";
            return response;

        }

        @Override
        protected void onPostExecute(String result) {
            map.clear();
            if (response.equalsIgnoreCase("Success")) {
                ArrayList<LatLng> directionPoint = v2GetRouteDirection.getDirection(document);
                PolylineOptions rectLine = new PolylineOptions().width(10).color(
                        Color.RED);

                for (int i = 0; i < directionPoint.size(); i++) {
                    rectLine.add(directionPoint.get(i));
                }
                // Adding route on the map
                map.addPolyline(rectLine);
                markerOptions.position(toPosition);
                markerOptions.draggable(true);
                markerOptions.title("Customer");
                map.addMarker(markerOptions);

            }

        }
    }

    @Override
    protected void onStop() {
        super.onStop();
        finish();
    }
}



