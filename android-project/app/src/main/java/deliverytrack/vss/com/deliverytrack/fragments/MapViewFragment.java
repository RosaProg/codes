package deliverytrack.vss.com.deliverytrack.fragments;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.Typeface;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v7.app.AlertDialog;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.google.android.gms.maps.CameraUpdate;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.MapFragment;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.BitmapDescriptorFactory;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;
import com.parse.ParseGeoPoint;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.asynch.FindUser;
import deliverytrack.vss.com.deliverytrack.interfaces.ReceiveUserCallbacks;
import deliverytrack.vss.com.deliverytrack.utility.CustomMarker;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link MapViewFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link MapViewFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class MapViewFragment extends Fragment implements DialogInterface.OnClickListener, ReceiveUserCallbacks {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private LocationListener locListener = new MyLocationListener();
    private ImageView car_img;
    private TextView car_type;
    private TextView estimated_time;
    private HashMap<CustomMarker, Marker> markersHashMap;
    private Iterator<Map.Entry<CustomMarker, Marker>> iter;
    private CameraUpdate cu;
    private CustomMarker customMarkerOne, customMarkerTwo;
    private Location location;


    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private SupportMapFragment fragment;
    private GoogleMap map;
    private LocationManager locManager;
    private boolean gps_enabled;
    private boolean network_enabled;
    public Location location1 = new Location("");
    public Location location2 = new Location("");

    private TextView agentName;
    private double latitu;
    private double longitu;

    public MapViewFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment MapViewFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static MapViewFragment newInstance(String param1, String param2) {
        MapViewFragment fragment = new MapViewFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
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
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_map_view, container, false);

        estimated_time = (TextView) view.findViewById(R.id.EstimatedTime);
        car_type = (TextView) view.findViewById(R.id.CarType);
        agentName = (TextView) view.findViewById(R.id.Username);
        car_img = (ImageView) view.findViewById(R.id.imageView1);
        car_img.setVisibility(View.GONE);


        locManager = (LocationManager) this.getActivity().getSystemService(Context.LOCATION_SERVICE);
        try {
            gps_enabled = locManager.isProviderEnabled(LocationManager.GPS_PROVIDER);
        } catch (Exception ex) {
        }
        try {
            network_enabled = locManager.isProviderEnabled(LocationManager.NETWORK_PROVIDER);
        } catch (Exception ex) {
        }

        // don't start listeners if no provider is enabled
        if (!gps_enabled && !network_enabled) {
            AlertDialog.Builder builder = new AlertDialog.Builder(this.getActivity());
            builder.setTitle("Attention!");
            builder.setMessage("Sorry, location is not determined. Please enable location providers");
            builder.setPositiveButton("OK", this);
            builder.setNeutralButton("Cancel", this);
            builder.create().show();
        }

        if (gps_enabled) {
            locManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, locListener);
        }
        if (network_enabled) {
            locManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER, 0, 0, locListener);
        }

        return view;

    }


    class MyLocationListener implements LocationListener {


        @Override
        public void onLocationChanged(Location location) {
            if (location != null) {
                locManager.removeUpdates(locListener);
                MapViewFragment.this.location = location;
                initilizeMap();
                initializeUiSettings();
                initializeMapLocationSettings();
                initializeMapTraffic();
                initializeMapType();
                initializeMapViewSettings();


            }
        }

        @Override
        public void onStatusChanged(String provider, int status, Bundle extras) {

        }

        @Override
        public void onProviderEnabled(String provider) {

        }

        @Override
        public void onProviderDisabled(String provider) {

        }


    }

    private void initilizeMap() {


        Log.d("the locations", location.getLatitude() + " " + location.getLongitude() + "  ");
        ParseGeoPoint point = new ParseGeoPoint(location.getLatitude(), location.getLongitude());
        ParseQuery<ParseUser> user = ParseUser.getQuery();
        user.whereWithinKilometers("location", point, 2);
        user.whereEqualTo("name", "Agent");
        user.whereEqualTo("userStatus", "Active");
        user.whereEqualTo("loggedIn", true);


        FindUser findUser = new FindUser(user, this.getActivity(), this, 567);
        findUser.execute();

    }


    @Override
    public void getUser(List<ParseUser> parseUsers, int tableId) {

        if (parseUsers != null && parseUsers.size() > 0) {
            for (ParseUser a : parseUsers) {

                Log.d("the users", a.getUsername() + " ");
                // Obteniendo el posicionamiento del Uber
                double ggg = a.getParseGeoPoint("location").getLatitude();
                double ppp = a.getParseGeoPoint("location").getLongitude();
                Log.v("Posicion", "latitud de parse: " + ggg);
                Log.v("Posicion", "longitud de parse: " + ppp);


                Log.v("Leyendo Mapa", "Por Leer");
                Log.v("Leyendo Mapa", "Leido");

                //Location location2 = new Location("");
                location2.setLatitude(ggg);
                location2.setLongitude(ppp);

                float distanceInMeters = location.distanceTo(location2);
                Log.v("Distance", "Distance: " + distanceInMeters);
                int speedIs10MetersPerMinute = 100;
                final float estimatedDriveTimeInMinutes = distanceInMeters / speedIs10MetersPerMinute;
                final int x = (int) estimatedDriveTimeInMinutes;
                Log.v("Estimated Drive Time", " " + estimatedDriveTimeInMinutes);
                // Cargando los datos para la pantalla
                Log.v("Tipo de Vehiculo", " - " + a.getUsername().toString());
                //uber_username.setText("Agent ID: " + a.getUsername().toString());
                estimated_time.setText("Estimated Time: " + x + " Mins.");
                car_type.setText("Vehicle Type: " + a.getString("mode"));
                Log.v("Tipo de Vehiculo", " - " + a.getString("mode"));
                car_img.setVisibility(View.VISIBLE);

                try {
                    switch (a.getString("mode")) {
                        case "Car/3 wheeler":
                            car_img.setImageResource(R.drawable.car);
                            break;
                        case "Moterbike/scooty":
                            car_img.setImageResource(R.drawable.moto);
                            break;
                        case "Rickshaw":
                            car_img.setImageResource(R.drawable.carro);
                            break;
                        case "Bicycle":
                            car_img.setImageResource(R.drawable.bici);
                            break;
                        case "Car":
                            car_img.setImageResource(R.drawable.car);
                            break;
                        case "Walking":
                            car_img.setImageResource(R.drawable.walk);
                            break;
                        default:
                            break;
                    }
                } catch (Exception eer) {
                    eer.printStackTrace();
                }


                map.moveCamera(CameraUpdateFactory.newLatLngZoom(new LatLng(ggg, ppp), 14.0f));

                map.addMarker(new MarkerOptions()
                        .position(new LatLng(ggg, ppp))
                        .title("Agent: " + a.getUsername())
                        .snippet("Vehicle: " + a.getString("mode"))
                        .icon(BitmapDescriptorFactory.fromResource(R.drawable.agent)));
                map.setInfoWindowAdapter(new GoogleMap.InfoWindowAdapter() {

                    @Override
                    public View getInfoWindow(Marker arg0) {
                        return null;
                    }

                    @Override
                    public View getInfoContents(Marker marker) {

                        LinearLayout info = new LinearLayout(getActivity());
                        info.setOrientation(LinearLayout.VERTICAL);
                        TextView title = new TextView(getActivity());
                        title.setTextColor(Color.BLACK);
                        title.setGravity(Gravity.CENTER);
                        title.setTypeface(null, Typeface.BOLD);
                        title.setText(marker.getTitle());

                        TextView snippet = new TextView(getActivity());
                        snippet.setTextColor(Color.GRAY);
                        snippet.setText(marker.getSnippet());

                        info.addView(title);
                        info.addView(snippet);

                        return info;
                    }
                });
            }

            if (mListener != null) {
                mListener.isNearByAgent(true);
            }

        } else {
            DeliveryTrackUtils.showToast(this.getActivity(), "No Agents near by");
            if (mListener != null) {
                mListener.isNearByAgent(false);
            }
        }


    }

    public void initializeUiSettings() {
        map.getUiSettings().setCompassEnabled(true);
        map.getUiSettings().setRotateGesturesEnabled(false);
        map.getUiSettings().setTiltGesturesEnabled(true);
        map.getUiSettings().setZoomControlsEnabled(true);
        map.getUiSettings().setMyLocationButtonEnabled(true);
    }

    public void initializeMapLocationSettings() {
        map.setMyLocationEnabled(true);
    }

    public void initializeMapTraffic() {
        map.setTrafficEnabled(true);
    }

    public void initializeMapType() {
        map.setMapType(GoogleMap.MAP_TYPE_NORMAL);
    }

    public void initializeMapViewSettings() {
        map.setIndoorEnabled(true);
        map.setBuildingsEnabled(false);
    }


    @Override
    public void onClick(DialogInterface dialog, int which) {
        if (which == DialogInterface.BUTTON_NEUTRAL) {
            DeliveryTrackUtils.showToast(this.getActivity(), "Sorry, location is not determined. To fix this please enable location providers");
        } else if (which == DialogInterface.BUTTON_POSITIVE) {
            startActivity(new Intent(android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS));
        }
    }


    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onAttach(Activity context) {
        super.onAttach(context);
        if (context instanceof OnFragmentInteractionListener) {
            mListener = (OnFragmentInteractionListener) context;
        } else {
            throw new RuntimeException(context.toString()
                    + " must implement OnFragmentInteractionListener");
        }
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
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
        void onFragmentInteraction(Uri uri);

        void isNearByAgent(boolean isNearBy);
    }


    @Override
    public void onActivityCreated(Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        FragmentManager fm = getChildFragmentManager();
        fragment = (SupportMapFragment) fm.findFragmentById(R.id.map_container);
        if (fragment == null) {
            fragment = SupportMapFragment.newInstance();
            fm.beginTransaction().replace(R.id.map_container, fragment).commit();
        }


/***at this time google play services are not initialize so get map and add what ever you want to it in onResume() or onStart() **/
    }

    @Override
    public void onResume() {
        super.onResume();
        if (map == null) {
            map = fragment.getMap();
        }

        getActivity()
                .setTitle("Track Location");
    }

}
