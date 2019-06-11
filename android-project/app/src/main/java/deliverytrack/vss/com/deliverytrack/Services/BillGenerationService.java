package deliverytrack.vss.com.deliverytrack.Services;

import android.app.IntentService;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Intent;
import android.content.Context;
import android.os.Environment;
import android.support.v4.app.NotificationCompat;
import android.text.TextUtils;
import android.util.Log;

import com.itextpdf.text.BadElementException;
import com.itextpdf.text.Document;
import com.itextpdf.text.DocumentException;
import com.itextpdf.text.Element;
import com.itextpdf.text.Font;
import com.itextpdf.text.Paragraph;
import com.itextpdf.text.Phrase;
import com.itextpdf.text.pdf.PdfPCell;
import com.itextpdf.text.pdf.PdfPTable;
import com.itextpdf.text.pdf.PdfWriter;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Date;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.PDFActivity;
import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.models.OrderTransaction;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.ParseHelper;
import deliverytrack.vss.com.deliverytrack.utility.pdfwriter.PDFUtils;

/**
 * An {@link IntentService} subclass for handling asynchronous task requests in
 * a service on a separate handler thread.
 * <p/>
 * TODO: Customize class - update intent actions, extra parameters and static
 * helper methods.
 */
public class BillGenerationService extends IntentService {
    // TODO: Rename actions, choose action names that describe tasks that this
    // IntentService can perform, e.g. ACTION_FETCH_NEW_ITEMS
    private static final String ACTION_FOO = "deliverytrack.vss.com.deliverytrack.Services.action.FOO";
    private static final String ACTION_BAZ = "deliverytrack.vss.com.deliverytrack.Services.action.BAZ";

    // TODO: Rename parameters
    private static final String EXTRA_PARAM1 = "deliverytrack.vss.com.deliverytrack.Services.extra.PARAM1";
    private static final String EXTRA_PARAM2 = "deliverytrack.vss.com.deliverytrack.Services.extra.PARAM2";
    private ArrayList<OrderTransaction> orderTransactions;

    public BillGenerationService() {
        super("BillGenerationService");
    }

    /**
     * Starts this service to perform action Foo with the given parameters. If
     * the service is already performing a task this action will be queued.
     *
     * @see IntentService
     */
    // TODO: Customize helper method
    public static void startActionFoo(Context context, String param1, String param2) {
        Intent intent = new Intent(context, BillGenerationService.class);
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
        Intent intent = new Intent(context, BillGenerationService.class);
        intent.setAction(ACTION_BAZ);
        intent.putExtra(EXTRA_PARAM1, param1);
        intent.putExtra(EXTRA_PARAM2, param2);
        context.startService(intent);
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        if (intent != null) {

            ParseQuery query = DeliveryTrackUtils.getQueryForTodayEarning();
            orderTransactions = new ArrayList<>();
            try {
                List<ParseObject> parseObjects = query.find();
                for (ParseObject transactionModels : parseObjects) {
                    OrderTransaction orderTransaction = ParseHelper.getOrderTransaction(transactionModels);
                    orderTransactions.add(orderTransaction);
                }
                File file = PDFUtils.createPDF(orderTransactions,0);
                if (file != null) {
                    sendNotification(file);
                } else {
                    Log.d("Error", "Error in pdf generation");
                }

            } catch (ParseException e) {
                e.printStackTrace();
            }

        }
    }


    private void sendNotification(File myFile) {
        Intent pupInt = new Intent(this, PDFActivity.class);
        pupInt.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_SINGLE_TOP);
        pupInt.putExtra("file", myFile.getAbsolutePath());

        int icon = R.drawable.ic_launcher;
        int mNotificationId = 0;

        PendingIntent resultPendingIntent =
                PendingIntent.getActivity(
                        this,
                        0,
                        pupInt,
                        PendingIntent.FLAG_CANCEL_CURRENT
                );


        NotificationCompat.Builder builder =
                new NotificationCompat.Builder(this)
                        .setSmallIcon(icon)
                        .setContentTitle("DeliveryTrack Bill")
                        .setContentText("Your bill for the day")
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
                                .bigText("Your bill for the day"));

        builder.setContentIntent(resultPendingIntent);
        NotificationManager notificationManager = (NotificationManager) this.getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.notify(mNotificationId, builder.build());

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
