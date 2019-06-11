package deliverytrack.vss.com.deliverytrack.utility;

/**
 * Created by Adi-Loch on 7/16/2015.
 */

import android.app.ActivityManager;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.os.Build;
import android.support.v4.app.NotificationCompat;
import android.text.TextUtils;
import java.util.List;
import deliverytrack.vss.com.deliverytrack.R;


/**
 * Created by Ravi on 01/06/15.
 */
public class NotificationUtils {

    private String TAG = NotificationUtils.class.getSimpleName();

    private Context mContext;

    public NotificationUtils() {
    }

    public NotificationUtils(Context mContext) {
        this.mContext = mContext;
    }

    public void showNotificationMessage(String title, String message, Intent intent, Class<?> cls) {

        String action = intent.getAction();
        Intent pupInt = new Intent(mContext, cls);


        if (title.equals("Order")) {
            pupInt.putExtra("order", "order");
        } else {
            pupInt.putExtra("account", "generated");
        }
        pupInt.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_SINGLE_TOP);

        // Check for empty push message
        if (TextUtils.isEmpty(message))
            return;

        int icon = R.drawable.ic_launcher;
        int mNotificationId = 0;

        PendingIntent resultPendingIntent =
                PendingIntent.getActivity(
                        mContext,
                        0,
                        pupInt,
                        PendingIntent.FLAG_CANCEL_CURRENT
                );


        NotificationCompat.Builder builder =
                new NotificationCompat.Builder(mContext)
                        .setSmallIcon(icon)
                        .setContentTitle(title)
                        .setContentText(message)
                        .setDefaults(Notification.DEFAULT_ALL)
                        .setAutoCancel(true)
                        // requires VIBRATE permission
        /*
         * Sets the big view "big text" style and supplies the
         * text (the user's reminder message) that will be displayed
         * in the detail area of the expanded notification.
         * These calls are ignored by the support library for
         * pre-4.1 devices.
         */
                        .setStyle(new NotificationCompat.BigTextStyle()
                                .bigText(message));

        builder.setContentIntent(resultPendingIntent);
        NotificationManager notificationManager = (NotificationManager) mContext.getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.notify(mNotificationId, builder.build());

    }

    /**
     * Method checks if the app is in background or not
     *
     * @param context
     * @return
     */
    public static boolean isAppIsInBackground(Context context) {
        boolean isInBackground = true;
        ActivityManager am = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        if (Build.VERSION.SDK_INT > Build.VERSION_CODES.KITKAT_WATCH) {
            List<ActivityManager.RunningAppProcessInfo> runningProcesses = am.getRunningAppProcesses();
            for (ActivityManager.RunningAppProcessInfo processInfo : runningProcesses) {
                if (processInfo.importance == ActivityManager.RunningAppProcessInfo.IMPORTANCE_FOREGROUND) {
                    for (String activeProcess : processInfo.pkgList) {
                        if (activeProcess.equals(context.getPackageName())) {
                            isInBackground = false;
                        }
                    }
                }
            }
        } else {
            DeliveryTrackUtils.showToast(context, "message");
        }

        return isInBackground;
    }
}