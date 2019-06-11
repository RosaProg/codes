package deliverytrack.vss.com.deliverytrack.Services;

import android.app.IntentService;
import android.content.Intent;
import android.content.Context;

import com.parse.GetCallback;
import com.parse.ParseException;
import com.parse.ParseFile;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.util.List;

/**
 * An {@link IntentService} subclass for handling asynchronous task requests in
 * a service on a separate handler thread.
 * <p/>
 * TODO: Customize class - update intent actions, extra parameters and static
 * helper methods.
 */
public class SignatureUploadService extends IntentService {
    // TODO: Rename actions, choose action names that describe tasks that this
    // IntentService can perform, e.g. ACTION_FETCH_NEW_ITEMS
    private static final String ACTION_FOO = "deliverytrack.vss.com.deliverytrack.Services.action.FOO";
    private static final String ACTION_BAZ = "deliverytrack.vss.com.deliverytrack.Services.action.BAZ";

    // TODO: Rename parameters
    private static final String EXTRA_PARAM1 = "deliverytrack.vss.com.deliverytrack.Services.extra.PARAM1";
    private static final String EXTRA_PARAM2 = "deliverytrack.vss.com.deliverytrack.Services.extra.PARAM2";

    /**
     * Starts this service to perform action Foo with the given parameters. If
     * the service is already performing a task this action will be queued.
     *
     * @see IntentService
     */
    // TODO: Customize helper method
    public static void startActionFoo(Context context, String param1, String param2) {
        Intent intent = new Intent(context, SignatureUploadService.class);
        intent.setAction(ACTION_FOO);
        intent.putExtra(EXTRA_PARAM1, param1);
        intent.putExtra(EXTRA_PARAM2, param2);
        context.startService(intent);
    }

    /**
     * Starts this service to perform action Baz with the given parameters. If
     * the service is already performing a task this action will be queued.
     *
     * @see IntentService
     */
    // TODO: Customize helper method
    public static void startActionBaz(Context context, String param1, String param2) {
        Intent intent = new Intent(context, SignatureUploadService.class);
        intent.setAction(ACTION_BAZ);
        intent.putExtra(EXTRA_PARAM1, param1);
        intent.putExtra(EXTRA_PARAM2, param2);
        context.startService(intent);
    }

    public SignatureUploadService() {
        super("SignatureUploadService");
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        if (intent != null) {
            byte[] bytesdata = intent.getByteArrayExtra("parsfile");
            String ObjectID = intent.getStringExtra("object_id");
            ParseFile parseFile = new ParseFile("Signature.jpg", bytesdata);
            try {
                parseFile.save();
            } catch (ParseException e) {
                e.printStackTrace();
            }

            ParseQuery<ParseObject> qquery = ParseQuery.getQuery("Order_Confirmation_Details");
            qquery.whereEqualTo("Date_Time", ObjectID);
            try {
                List<ParseObject> obj = qquery.find();

                ParseObject odercon = obj.get(0);
                odercon.put("Signature", parseFile);
                odercon.save();

            } catch (ParseException e) {
                e.printStackTrace();
            }



        }
    }

    /**
     * Handle action Foo in the provided background thread with the provided
     * parameters.
     */
    private void handleActionFoo(String param1, String param2) {
        // TODO: Handle action Foo
        throw new UnsupportedOperationException("Not yet implemented");
    }

    /**
     * Handle action Baz in the provided background thread with the provided
     * parameters.
     */
    private void handleActionBaz(String param1, String param2) {
        // TODO: Handle action Baz
        throw new UnsupportedOperationException("Not yet implemented");
    }
}
