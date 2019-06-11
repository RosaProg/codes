package deliverytrack.vss.com.deliverytrack.receivers;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

import com.parse.ParseUser;

import org.json.JSONException;
import org.json.JSONObject;
import deliverytrack.vss.com.deliverytrack.BaseActivity;
import deliverytrack.vss.com.deliverytrack.SignUpActivity;
import deliverytrack.vss.com.deliverytrack.utility.NotificationUtils;

/**
 * Created by Adi-Loch on 7/15/2015.
 */
public class DeliveryTrackBroadcast extends BroadcastReceiver {

    private NotificationUtils notificationUtils;


    @Override
    public void onReceive(Context context, Intent intent) {

        ParseUser user = ParseUser.getCurrentUser();
        if(user!=null) {

            String action = intent.getAction();
            try {

                if (action.equals("deliverytrack.vss.com.deliverytrack.receivers.ACTION_REMINDER")) {

                    JSONObject json = new JSONObject(intent.getExtras().getString("com.parse.Data"));
                    String pushNotificationMessage = json.getString("alert");
                    showNotificationMessage(context, "Registration Reminder", pushNotificationMessage, intent, SignUpActivity.class);
                } else if (action.equals("deliverytrack.vss.com.deliverytrack.receivers.ACCOUNT_GENERATION")) {

                    JSONObject json = new JSONObject(intent.getExtras().getString("com.parse.Data"));
                    String message = json.getString("message");
                    showNotificationMessage(context, "Account Generation", message, intent, BaseActivity.class);

                } else if (action.equals("deliverytrack.vss.com.deliverytrack.receivers.ORDER")) {

                    JSONObject json = new JSONObject(intent.getExtras().getString("com.parse.Data"));
                    String message = json.getString("message");
                    showNotificationMessage(context, "Order", message, intent, BaseActivity.class);

                }
                else if (action.equals("deliverytrack.vss.com.deliverytrack.receivers.PAYONE")) {

                    JSONObject json = new JSONObject(intent.getExtras().getString("com.parse.Data"));
                    String message = json.getString("message");
                    showNotificationMessage(context, "Payone", message, intent, BaseActivity.class);

                }
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }


    }


    private void showNotificationMessage(Context context, String title, String message, Intent intent,  Class<?> cls) {
        notificationUtils = new NotificationUtils(context);
        intent.putExtras(intent.getExtras());
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_SINGLE_TOP);
        notificationUtils.showNotificationMessage(title, message, intent,  cls);
    }

}
