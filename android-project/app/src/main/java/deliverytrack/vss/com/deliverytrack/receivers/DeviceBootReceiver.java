package deliverytrack.vss.com.deliverytrack.receivers;

import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

/**
 * Created by Adi-Loch on 11/17/2015.
 */
public class DeviceBootReceiver extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        if (intent.getAction().equals("android.intent.action.BOOT_COMPLETED")) {
            int interval = 1000 * 60 * 5;
            Intent alarmIntent = new Intent(context, AlarmReceiver.class);
            PendingIntent pendingIntent = PendingIntent.getBroadcast(context, 0, alarmIntent, 0);

            Intent billalarmIntent = new Intent(context, BillGenerationAlarm.class);
            PendingIntent billpendingIntent = PendingIntent.getBroadcast(context, 0, billalarmIntent, 0);


            AlarmManager manager = (AlarmManager) context.getSystemService(Context.ALARM_SERVICE);
            manager.setInexactRepeating(AlarmManager.RTC_WAKEUP, System.currentTimeMillis(), interval, pendingIntent);

            AlarmManager billmanager = (AlarmManager) context.getSystemService(Context.ALARM_SERVICE);
            billmanager.setInexactRepeating(AlarmManager.RTC_WAKEUP, System.currentTimeMillis(), interval, billpendingIntent);


        }
    }
}
