package deliverytrack.vss.com.deliverytrack.Services;

import android.app.IntentService;
import android.content.Intent;
import android.location.Location;
import android.util.Log;
import com.google.android.gms.location.FusedLocationProviderApi;
import com.parse.ParseException;
import com.parse.ParseGeoPoint;
import com.parse.ParseInstallation;
import com.parse.ParseUser;

import deliverytrack.vss.com.deliverytrack.constants.Constants;

/**
 * An {@link IntentService} subclass for handling asynchronous task requests in
 * a service on a separate handler thread.
 * <p/>
 * TODO: Customize class - update intent actions, extra parameters and static
 * helper methods.
 */
public class LocationUpdateService extends IntentService {


    public LocationUpdateService() {
        super(LocationUpdateService.class.getName());
    }

    Location location;


    @Override
    public int onStartCommand(Intent intent, int flags, int startID) {
        super.onStartCommand(intent, flags, startID);
        Log.d("LocationUpdateService", "Location received");
        return START_REDELIVER_INTENT;
    }

    @Override
    protected void onHandleIntent(Intent intent) {

        if (intent.hasExtra(FusedLocationProviderApi.KEY_LOCATION_CHANGED)) {
            location = intent.getParcelableExtra(FusedLocationProviderApi.KEY_LOCATION_CHANGED);
            Log.d("locationtesting", "accuracy: " + location.getAccuracy() + " lat: " + location.getLatitude() + " lon: " + location.getLongitude());
            Log.d("LocationUpdateService", "Location received");

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

    }

}
