package deliverytrack.vss.com.deliverytrack.receivers;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import deliverytrack.vss.com.deliverytrack.Services.TrackLocationService;
/**
 * Created by Adi-Loch on 11/17/2015.
 */

public class AlarmReceiver extends BroadcastReceiver {


    @Override
    public void onReceive(Context context, Intent intent) {
        Intent service = new Intent(context, TrackLocationService.class);
        context.startService(service);

    }
}
