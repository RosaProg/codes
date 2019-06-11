package deliverytrack.vss.com.deliverytrack.application;

import android.app.Application;
import android.content.Context;
import android.content.SharedPreferences;
import android.support.multidex.MultiDex;

import com.parse.Parse;
import com.parse.ParseACL;
import com.parse.ParseObject;
import com.parse.ParseUser;

import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.models.AnywallPost;
import deliverytrack.vss.com.deliverytrack.models.OrderConfirmation;
import deliverytrack.vss.com.deliverytrack.utility.ConfigHelper;

/**
 * Created by Adi-Loch on 6/24/2015.
 */
public class DeliveryTrackApplication extends Application {


    // Debugging switch
    public static final boolean APPDEBUG = false;

    // Debugging tag for the application
    public static final String APPTAG = "AnyWall";

    // Used to pass location from MainActivity to PostActivity
    public static final String INTENT_EXTRA_LOCATION = "location";

    // Key for saving the search distance preference
    private static final String KEY_SEARCH_DISTANCE = "searchDistance";

    private static final float DEFAULT_SEARCH_DISTANCE = 250.0f;

    private static SharedPreferences preferences;

    private static ConfigHelper configHelper;

    public DeliveryTrackApplication() {
    }


    @Override
    public void onCreate() {

        super.onCreate();

        Parse.enableLocalDatastore(this);
        Parse.initialize(this, Constants.parseApplicationId,
                Constants.parseClientKey);
        ParseUser.enableRevocableSessionInBackground();
        //specify a Activity to be used for all push notifications by default
//        PushService.setDefaultPushCallback(this, BaseActivity.class);
        //ParseInstallation.getCurrentInstallation().saveInBackground();

        ParseACL defaultACL = new ParseACL();

//        // If you would like all objects to be private by default, remove this line.
//        defaultACL.setPublicReadAccess(true);

        ParseACL.setDefaultACL(defaultACL, true);

        ParseObject.registerSubclass(AnywallPost.class);
        ParseObject.registerSubclass(OrderConfirmation.class);
        preferences = getSharedPreferences("com.parse.anywall", Context.MODE_PRIVATE);
        configHelper = new ConfigHelper();
        configHelper.fetchConfigIfNeeded();


    }

    @Override
    protected void attachBaseContext(Context base) {
        super.attachBaseContext(base);
        MultiDex.install(base);


    }

    public static float getSearchDistance() {
        return preferences.getFloat(KEY_SEARCH_DISTANCE, DEFAULT_SEARCH_DISTANCE);
    }

    public static ConfigHelper getConfigHelper() {
        return configHelper;
    }

    public static void setSearchDistance(float value) {
        preferences.edit().putFloat(KEY_SEARCH_DISTANCE, value).commit();
    }

}

