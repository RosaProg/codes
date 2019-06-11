package deliverytrack.vss.com.deliverytrack.utility;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.location.Address;
import android.location.Geocoder;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.provider.Settings;
import java.io.IOException;
import java.util.List;
import java.util.Locale;

import deliverytrack.vss.com.deliverytrack.R;

/**
 * Created by anna on 08.07.15.
 */

public class LocationUtils {

    private LocationManager locationManager;
    private Location mLocation;
    private Context mContext;
    private Geocoder mGeocoder;
    private LocationListener locationListener;

    private static final int MIN_TIME_BW_UPDATES = 0;
    private static final int MIN_DISTANCE_CHANGE_FOR_UPDATES = 0;

    public LocationUtils(Context context) {
        mContext = context;
        locationManager = (LocationManager) mContext
                .getSystemService(Context.LOCATION_SERVICE);

        locationListener = new LocationListener() {
            @Override
            public void onLocationChanged(Location location) {
                mLocation = location;
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
        };
        mGeocoder = new Geocoder(mContext, Locale.getDefault());
        updateLocation();
    }

    private void updateLocation() {
        // getting GPS status
        Boolean isGPSEnabled = locationManager
                .isProviderEnabled(LocationManager.GPS_PROVIDER);

        // getting network status
        Boolean isNetworkEnabled = locationManager
                .isProviderEnabled(LocationManager.NETWORK_PROVIDER);

        if (!isGPSEnabled && !isNetworkEnabled) {
            // no network provider is enabled
            AlertDialog.Builder builder = new AlertDialog.Builder(mContext);
            builder.setMessage(R.string.gps_disabled);
            builder.setCancelable(false);
            builder.setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    Intent settings = new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS);
                    mContext.startActivity(settings);
                    dialog.dismiss();
                }
            });
            builder.setNegativeButton(android.R.string.no, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    dialog.dismiss();
                }
            });
            AlertDialog dialog = builder.create();
            dialog.show();
        } else {
            if (isNetworkEnabled) {
                locationManager.requestLocationUpdates(
                        LocationManager.NETWORK_PROVIDER,
                        MIN_TIME_BW_UPDATES,
                        MIN_DISTANCE_CHANGE_FOR_UPDATES, locationListener);
                if (locationManager != null) {
                    mLocation = locationManager
                            .getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
                    if (mLocation != null) {
                        double latitude = mLocation.getLatitude();
                        double longitude = mLocation.getLongitude();
                    }
                }
            }
            // if GPS Enabled get lat/long using GPS Services
            if (isGPSEnabled) {

                if (mLocation == null) {
                    locationManager.requestLocationUpdates(
                            LocationManager.GPS_PROVIDER,
                            MIN_TIME_BW_UPDATES,
                            MIN_DISTANCE_CHANGE_FOR_UPDATES, locationListener);

                    if (locationManager != null) {
                        mLocation = locationManager
                                .getLastKnownLocation(LocationManager.GPS_PROVIDER);
                        if (mLocation != null) {

                        }
                    }
                }
            }
        }
    }

    public String getCityName() {

        if (mLocation == null) {
            updateLocation();
            if (mLocation == null)
                return null;
        }
        /*----------to get City-Name from coordinates ------------- */
        String cityName = null;
        List<Address> addresses;
        try {
            addresses = mGeocoder.getFromLocation(mLocation.getLatitude(),
                    mLocation.getLongitude(), 1);
            cityName = addresses.get(0).getLocality();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return cityName;
    }

    public Location getLocation() {

        if (mLocation == null)
            updateLocation();
        return mLocation;
    }
}
