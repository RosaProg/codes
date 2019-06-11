

package deliverytrack.vss.com.deliverytrack.Services;

import android.app.Service;
import android.content.Intent;
import android.location.Location;
import android.os.Binder;
import android.os.Bundle;
import android.os.IBinder;
import android.util.Log;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationServices;
import com.parse.ParseException;
import com.parse.ParseGeoPoint;
import com.parse.ParseInstallation;
import com.parse.ParseUser;

import deliverytrack.vss.com.deliverytrack.constants.Constants;


public class TrackLocationService extends Service implements GoogleApiClient.ConnectionCallbacks,
        GoogleApiClient.OnConnectionFailedListener, com.google.android.gms.location.LocationListener {

    private final static String TAG = TrackLocationService.class.getSimpleName();

    private final long MIN_INTERVAL_BETWEEN_LOCATION_UPDATES = 500L;
    private final long INTERVAL_BETWEEN_LOCATION_UPDATES = 1000L;

    private GoogleApiClient mGoogleApiClient;
    private Location location;

    @Override
    public void onCreate() {

        Log.d(TAG, "onCreate");
        super.onCreate();

        mGoogleApiClient = new GoogleApiClient.Builder(getApplicationContext())
                .addConnectionCallbacks(this)
                .addOnConnectionFailedListener(this)
                .addApi(LocationServices.API)
                .build();

        mGoogleApiClient.connect();

    }


    private void startGPSTracker() {
        Log.d(TAG, "startGPSTracker");

        LocationRequest mLocationRequest = new LocationRequest();
        mLocationRequest.setInterval(INTERVAL_BETWEEN_LOCATION_UPDATES);
        mLocationRequest.setFastestInterval(MIN_INTERVAL_BETWEEN_LOCATION_UPDATES);
        mLocationRequest.setPriority(LocationRequest.PRIORITY_HIGH_ACCURACY);

        LocationServices.FusedLocationApi.requestLocationUpdates(mGoogleApiClient, mLocationRequest, this);
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        Log.d(TAG, "onStartCommand");
        return START_STICKY;
    }

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    // Binder given to clients
    private final IBinder mBinder = new LocalBinder();


    @Override
    public void onConnected(Bundle bundle) {
        Log.d(TAG, "onConnected");
        startGPSTracker();
    }

    private class ThreadDemo extends Thread {
        @Override
        public void run() {
            super.run();
            try {

                ThreadDemo.sleep(6000*10);

                if (location != null) {
                    System.out.println("check again");

                    ParseGeoPoint geopoint = new ParseGeoPoint(location.getLatitude(), location.getLongitude());
                    ParseUser user = ParseUser.getCurrentUser();
                    user.put("location", geopoint);
                    ParseInstallation installation = null;
                    installation = ParseInstallation.getCurrentInstallation();
                    installation.put("geoPoint", geopoint);
                    installation.put("GCMSenderId", Constants.GCM_SENDER_ID);
                    installation.put("userId", ParseUser.getCurrentUser().getObjectId());
                    installation.put("userType", ParseUser.getCurrentUser().get("name").toString());
                    installation.put("user", ParseUser.getCurrentUser());

                    try {
                        user.save();// save user
                        installation.save();// save installation

                    } catch (ParseException e) {
                        e.printStackTrace();
                    }
                }

            } catch (Exception e) {
                e.getMessage();
            }

        }
    }


    @Override
    public void onConnectionSuspended(int i) {
        Log.d(TAG, "onConnectionSuspended i: " + i);
    }

    @Override
    public void onConnectionFailed(ConnectionResult connectionResult) {
        Log.d(TAG, "onConnectionFailed connectionResult: " + connectionResult.toString());
    }

    @Override
    public void onLocationChanged(Location location) {
        Log.d(TAG, "onLocationChanged lat: " + location.getLatitude() + " lng: " + location.getLongitude());
        Log.d("locationtesting", "accuracy: " + location.getAccuracy() + " lat: " + location.getLatitude() + " lon: " + location.getLongitude());

        this.location = location;
        new ThreadDemo().start();


    }


    /**
     * Class used for the client Binder.  Because we know this service always
     * runs in the same process as its clients, we don't need to deal with IPC.
     */

    public class LocalBinder extends Binder {
        public TrackLocationService getService() {
            // Return this instance of LocalService so clients can call public methods
            return TrackLocationService.this;
        }
    }


    @Override
    public void onDestroy() {
        Log.d(TAG, "onDestroy");
        super.onDestroy();
    }
}


