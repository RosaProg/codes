package deliverytrack.vss.com.deliverytrack.Services;

import android.app.IntentService;
import android.content.Intent;
import com.parse.ParseException;
import com.parse.ParseGeoPoint;
import com.parse.ParseInstallation;
import com.parse.ParseUser;
import deliverytrack.vss.com.deliverytrack.constants.Constants;

/**
 * Created by Adi-Loch on 7/14/2015.
 */
public class RegistrationService extends IntentService {
    /**
     * Creates an IntentService.  Invoked by your subclass's constructor.
     * <p/>
     * .
     */
    public RegistrationService() {
        super("RegistrationService");
    }

    @Override
    protected void onHandleIntent(Intent intent) {

        double latitude = intent.getDoubleExtra("latitude", 0);
        double longitude = intent.getDoubleExtra("longitude", 0);
        String userId = intent.getStringExtra("userId");


            try {
                ParseGeoPoint geoPoint = new ParseGeoPoint(latitude, longitude);
                if (geoPoint != null) {
                    ParseInstallation installation = null;
                    installation = ParseInstallation.getCurrentInstallation();
                    installation.put("geoPoint", geoPoint);
                    installation.put("GCMSenderId", Constants.GCM_SENDER_ID);
                    installation.put("userId", ParseUser.getCurrentUser().getObjectId());
                    installation.put("userType", ParseUser.getCurrentUser().get("name").toString());
                    installation.put("user", ParseUser.getCurrentUser());
                    installation.save();


                    //save locations to the user
                    ParseUser user = ParseUser.getCurrentUser();
                    if(user!=null) {
                        user.put("location", geoPoint);
                        try {
                            user.save();
                        } catch (ParseException e) {
                            e.printStackTrace();
                        }
                    }
                }

            } catch (ParseException e) {
                e.printStackTrace();
            }
        }




}
