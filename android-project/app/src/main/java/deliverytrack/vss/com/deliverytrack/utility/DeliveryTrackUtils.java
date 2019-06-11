package deliverytrack.vss.com.deliverytrack.utility;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.location.Address;
import android.location.Geocoder;
import android.location.Location;
import android.location.LocationManager;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.provider.Settings;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.widget.Toast;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.GooglePlayServicesUtil;
import com.parse.GetCallback;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.io.IOException;
import java.security.MessageDigest;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.asynch.CancelOrder;
import deliverytrack.vss.com.deliverytrack.asynch.UpdateOrder;
import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.models.Order;

/**
 * Created by Adi-Loch on 6/26/2015.
 */
public class DeliveryTrackUtils {


    private static final int PLAY_SERVICES_RESOLUTION_REQUEST = 0;
    public static final String AGENT = "Agent";
    public static final String RESTAURANT = "Restaurant";

    public static void showToast(Context context, String message) {

        Toast.makeText(context, message, Toast.LENGTH_LONG).show();
    }


    public static float getPercentage(int cal, int amount) {
        float res = cal / amount;
        return res * 100;
    }

    public static Date removeTime(Date date) {
        Calendar cal = Calendar.getInstance(); // locale-specific
        cal.setTime(date);
        cal.set(Calendar.HOUR_OF_DAY, 0);
        cal.set(Calendar.MINUTE, 0);
        cal.set(Calendar.SECOND, 0);
        cal.set(Calendar.MILLISECOND, 0);
        return cal.getTime();
    }

    public static Date removeTimeTom(Date date) {
        Calendar cal = Calendar.getInstance(); // locale-specific
        cal.set(Calendar.DATE, +1);
        cal.setTime(cal.getTime());
        cal.set(Calendar.HOUR_OF_DAY, 0);
        cal.set(Calendar.MINUTE, 0);
        cal.set(Calendar.SECOND, 0);
        cal.set(Calendar.MILLISECOND, 0);
        return cal.getTime();
    }

    public static Date removeTimeFoePrevious(Date date) {
        Calendar cal = Calendar.getInstance(); // locale-specific
        cal.set(Calendar.DATE, -30);
        cal.setTime(cal.getTime());
        cal.set(Calendar.HOUR_OF_DAY, 0);
        cal.set(Calendar.MINUTE, 0);
        cal.set(Calendar.SECOND, 0);
        cal.set(Calendar.MILLISECOND, 0);
        return cal.getTime();
    }


    public static ParseQuery getQueryForTodayEarning() {
        ParseUser user = ParseUser.getCurrentUser();
        ParseQuery query = new ParseQuery("OrderTransaction");
        query.whereEqualTo("userId", user.getObjectId());
        query.whereGreaterThan("createdAt", removeTime(new Date()));// gets query only for today
        return query;
    }

    public static double round(double value, int places) {
        if (places < 0) throw new IllegalArgumentException();

        long factor = (long) Math.pow(10, places);
        value = value * factor;
        long tmp = Math.round(value);
        return (double) tmp / factor;
    }


    /**
     * Check the device to make sure it has the Google Play Services APK. If it
     * doesn't, display a dialog that allows users to download the APK from the
     * Google Play Store or enable it in the device's system settings.
     */
    public static boolean checkPlayServices(Activity context) {
        int resultCode = GooglePlayServicesUtil
                .isGooglePlayServicesAvailable(context);
        if (resultCode != ConnectionResult.SUCCESS) {
            if (GooglePlayServicesUtil.isUserRecoverableError(resultCode)) {
                GooglePlayServicesUtil.getErrorDialog(resultCode, context,
                        PLAY_SERVICES_RESOLUTION_REQUEST).show();
            } else {
                DeliveryTrackUtils.showToast(context, "Not Supported");
            }
            return false;
        }
        return true;
    }


    public static Location getLocation(Context context) {
        LocationManager locManager = (LocationManager) context.getSystemService(Context.LOCATION_SERVICE);
        Location location = null;
        if (DeliveryTrackUtils.checkLocationIsOn(context)) {
            location = locManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
            if (location != null) {
                return location;
            }

        }

        return location;
    }

    public static SharedPreferences getSharedPreferences(Context context) {
        SharedPreferences prefs = context.getSharedPreferences("DeliveryTrack",
                Context.MODE_PRIVATE);

        return prefs;
    }

    /**
     * @return Application's version code from the {@code PackageManager}.
     */
    public static int getAppVersion(Context context) {
        try {
            PackageInfo packageInfo = context.getPackageManager()
                    .getPackageInfo(context.getPackageName(), 0);
            Log.d("te version", packageInfo.versionCode + " ");
            return packageInfo.versionCode;
        } catch (PackageManager.NameNotFoundException e) {
            // should never happen
            throw new RuntimeException("Could not get package name: " + e);
        }
    }


    public static String getType() {
        if (ParseUser.getCurrentUser().get("name").toString().equals("Agent")) {
            return "agentId";
        } else {
            return "restaurentId";
        }

    }


    public static String getUserType() {
        if (ParseUser.getCurrentUser().get("name").toString().equals("Agent")) {
            return "Agent";
        } else {
            return "Restaurant";
        }

    }


    public static String getPayOneExpTime() {
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
        Calendar c = Calendar.getInstance();
        c.add(Calendar.DATE, 7);  // number of days to add
        String dt = sdf.format(c.getTime());
        String dt1 = " 00:00:00";
        String dt2 = dt + dt1;
        return dt2;
    }


    public static String getPayOneSignature(String[] inputArray, String glueString) {

        String imp = implodeArray(inputArray, glueString);
        return sha256(imp);

    }

    private static String implodeArray(String[] inputArray, String glueString) {
        String output = "";
        if (inputArray.length > 0) {
            StringBuilder sb = new StringBuilder();
            sb.append(inputArray[0]);
            for (int i = 1; i < inputArray.length; i++) {
                sb.append(glueString);
                sb.append(inputArray[i]);
            }
            output = sb.toString();
        }

        return output;

    }

    private static String sha256(String base) {
        try {
            MessageDigest digest = MessageDigest.getInstance("SHA-256");
            byte[] hash = digest.digest(base.getBytes("UTF-8"));
            StringBuffer hexString = new StringBuffer();

            for (int i = 0; i < hash.length; i++) {
                String hex = Integer.toHexString(0xff & hash[i]);
                if (hex.length() == 1) hexString.append('0');
                hexString.append(hex);
            }
            // return hash.toString();
            return hexString.toString();
        } catch (Exception ex) {
            throw new RuntimeException(ex);
        }
    }


    public static boolean isRestaurentUser() {
        if (ParseUser.getCurrentUser().get("name")!=null &&ParseUser.getCurrentUser().get("name").toString().equals("Restaurant")) {
            return true;
        } else {
            return false;
        }
    }


    public static boolean isAgent() {
        if (ParseUser.getCurrentUser().get("name").toString().equals("Agent")) {
            return true;
        } else {
            return false;
        }
    }


    public static void saveLocale(String lang, Context context) {
        String langPref = "Language";
        SharedPreferences prefs = context.getSharedPreferences("CommonPrefs", Activity.MODE_PRIVATE);
        SharedPreferences.Editor editor = prefs.edit();
        editor.putString(langPref, lang);
        editor.commit();
    }

    public static String getLocale(Context context) {
        SharedPreferences prefs = context.getSharedPreferences("CommonPrefs", Activity.MODE_PRIVATE);
        return prefs.getString("Language", "");

    }


    public static Address getLocationFromAddress(String strAddress, Activity activity) {

        Geocoder coder = new Geocoder(activity);
        List<Address> address;
        try {
            address = coder.getFromLocationName(strAddress, 1);
            if (address == null) {
                return null;
            }
            if (address != null && address.size() > 0) {
                Address location = address.get(0);
                location.getCountryCode();
                Log.d("the location", location.getCountryCode() + "");
                location.getLatitude();
                location.getLongitude();
                return location;

            }
            return null;

        } catch (IOException e) {
            e.printStackTrace();
            return null;

        }
    }


    public static String[] splitStrings(String string) {
        String[] parts = string.split("\\$");

        Log.d("the string", parts[0] + parts[1]);

        return parts;
    }

    public static boolean checkLocationIsOn(final Context context) {
        LocationManager lm = (LocationManager) context.getSystemService(Context.LOCATION_SERVICE);
        boolean gps_enabled = false;
        boolean network_enabled = false;
        try {
            gps_enabled = lm.isProviderEnabled(LocationManager.GPS_PROVIDER);
        } catch (Exception ex) {
        }

        try {
            network_enabled = lm.isProviderEnabled(LocationManager.NETWORK_PROVIDER);
        } catch (Exception ex) {
        }

        if (!gps_enabled && !network_enabled) {
            // notify user
            AlertDialog.Builder dialog = new AlertDialog.Builder(context);
            dialog.setMessage(context.getResources().getString(R.string.gps_network_not_enabled));
            dialog.setPositiveButton(context.getResources().getString(R.string.open_location_settings), new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface paramDialogInterface, int paramInt) {
                    // TODO Auto-generated method stub
                    Intent myIntent = new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS);
                    context.startActivity(myIntent);
                    //get gps
                }
            });
            dialog.setNegativeButton(context.getString(R.string.cancel), new DialogInterface.OnClickListener() {

                @Override
                public void onClick(DialogInterface paramDialogInterface, int paramInt) {
                    // TODO Auto-generated method stub

                }
            });
            dialog.show();
            return false;
        } else {
            return true;
        }
    }


    public static boolean isOnline(Context context) {
        ConnectivityManager cm =
                (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo netInfo = cm.getActiveNetworkInfo();
        return netInfo != null && netInfo.isConnectedOrConnecting();
    }


    public static void redirectPlaystore(Context context) {
        final String appPackageName = context.getPackageName(); // getPackageName() from Context or Activity object
        try {
            context.startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse("market://details?id=" + appPackageName)));
        } catch (android.content.ActivityNotFoundException anfe) {
            context.startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse("https://play.google.com/store/apps/details?id=" + appPackageName)));
        }
    }


    public static boolean getIsWeekend(Date date) {
        Calendar calendar = Calendar.getInstance();
        int day = calendar.get(Calendar.DAY_OF_WEEK);
        boolean isWeekEnd = false;
        switch (day) {
            case Calendar.SUNDAY:
                isWeekEnd = true;
                break;
            case Calendar.MONDAY:
                break;
            case Calendar.TUESDAY:
                break;
            case Calendar.WEDNESDAY:
                break;
            case Calendar.THURSDAY:
                break;
            case Calendar.FRIDAY:
                isWeekEnd = true;
                break;
            case Calendar.SATURDAY:
                isWeekEnd = true;
                break;

        }
        return isWeekEnd;

    }

    public static Date getTime(String time
    ) {
        SimpleDateFormat dateFormat = new SimpleDateFormat("hh:mm aa");
        try {
            Log.d("Time", time);

            Date date = dateFormat.parse(time);
            Log.d("Time", date.toString());
            return date;
        } catch (ParseException e) {
            Log.d("Error", e.toString());

        }
        return null;
    }


    public static void cancelOrder(final Order order, final Fragment context) {
        final String agentType = ParseUser.getCurrentUser().get("name").toString();
        new AlertDialog.Builder(context.getActivity())
                .setTitle("Cancel Order")
                .setMessage("Are you sure you want to cancel this order?")
                .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        CancelOrder cancelOrder = new CancelOrder(context, agentType, order);
                        cancelOrder.execute();
                    }
                })
                .setNegativeButton(android.R.string.no, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                    }
                })
                .setIcon(android.R.drawable.ic_dialog_alert)
                .show();

    }


    public static boolean acceptPickupForOrder(List<ParseObject> parseObjects, Fragment fragment) {
        if (parseObjects != null) {
            Order order = getOrder(parseObjects);
            if (order.getStatus().equals(Constants.ORDER_PENDING)) {
                order.setStatus(Constants.ORDER_ACCEPT_PICKUP);
                order.setAgentId(ParseUser.getCurrentUser().getObjectId());
                UpdateOrder updateOrder = new UpdateOrder(fragment.getActivity(), order, fragment, order.getStatus());
                updateOrder.execute();
                return false;
            } else {
                DeliveryTrackUtils.showToast(fragment.getActivity(), "Cannot pick up the order");
                return true;
            }
        }

        return false;

    }


    public static boolean pickupForOrder(List<ParseObject> parseObjects, Fragment fragment) {
        if (parseObjects != null) {
            Order order = getOrder(parseObjects);
            if (order.getStatus().equals(Constants.ORDER_ARRIVED_LOCATION)) {
                order.setStatus(Constants.ORDER_PICKEDUP);
                order.setAgentId(ParseUser.getCurrentUser().getObjectId());
                UpdateOrder updateOrder = new UpdateOrder(fragment.getActivity(), order, fragment, order.getStatus());
                updateOrder.execute();
                return false;
            } else {
                DeliveryTrackUtils.showToast(fragment.getActivity(), "Cannot pick up the order");
                return true;
            }
        }

        return false;

    }


    public static boolean arrivedAtLocation(List<ParseObject> parseObjects, Fragment fragment) {
        if (parseObjects != null) {
            Order order = getOrder(parseObjects);
            if (order.getStatus().equals(Constants.ORDER_ACCEPT_PICKUP)) {
                order.setStatus(Constants.ORDER_ARRIVED_LOCATION);
                order.setAgentId(ParseUser.getCurrentUser().getObjectId());
                UpdateOrder updateOrder = new UpdateOrder(fragment.getActivity(), order, fragment, order.getStatus());
                updateOrder.execute();
                return false;
            } else {
                DeliveryTrackUtils.showToast(fragment.getActivity(), "Cannot pick up the order");
                return true;
            }
        }

        return false;

    }

    private static Order getOrder(List<ParseObject> parseObjects) {
        ParseHelper helper = new ParseHelper();
        Order order = helper.getOrdersVisibleDelivered(parseObjects).get(0);
        return order;

    }


    public static String convertDateToString(Date createdAt, String format) {

        DateFormat df = new SimpleDateFormat(format);
        String reportDate = df.format(createdAt);
        Log.d("Report Date: ", " " + reportDate);
        return reportDate;
    }
}




