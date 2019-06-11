package deliverytrack.vss.com.deliverytrack.Services;

import android.app.IntentService;
import android.content.Intent;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Environment;
import android.util.Log;

import com.parse.ParseException;
import com.parse.ParseFile;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * An {@link IntentService} subclass for handling asynchronous task requests in
 * a service on a separate handler thread.
 * <p/>
 * TODO: Customize class - update intent actions, extra parameters and static
 * helper methods.
 */
public class UploadAdharService extends IntentService {
    // TODO: Rename actions, choose action names that describe tasks that this
    // IntentService can perform, e.g. ACTION_FETCH_NEW_ITEMS
    private static final String ACTION_FOO = "deliverytrack.vss.com.deliverytrack.Services.action.FOO";
    private static final String ACTION_BAZ = "deliverytrack.vss.com.deliverytrack.Services.action.BAZ";

    // TODO: Rename parameters
    private static final String EXTRA_PARAM1 = "deliverytrack.vss.com.deliverytrack.Services.extra.PARAM1";
    private static final String EXTRA_PARAM2 = "deliverytrack.vss.com.deliverytrack.Services.extra.PARAM2";
    private double IMAGE_MAX_SIZE = 7000000;

    /**
     * Starts this service to perform action Foo with the given parameters. If
     * the service is already performing a task this action will be queued.
     *
     * @see IntentService
     */
    // TODO: Customize helper method
    public static void startActionFoo(Context context, String param1, String param2) {
        Intent intent = new Intent(context, UploadAdharService.class);
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
        Intent intent = new Intent(context, UploadAdharService.class);
        intent.setAction(ACTION_BAZ);
        intent.putExtra(EXTRA_PARAM1, param1);
        intent.putExtra(EXTRA_PARAM2, param2);
        context.startService(intent);
    }

    public UploadAdharService() {
        super("UploadAdharService");
    }

    @Override
    protected void onHandleIntent(Intent intent) {

        ParseFile adharParseFile = null;
        ParseFile selfieParseFile = null;
        ParseFile otherParseFile = null;
        ParseFile docParseFile = null;

        String type = DeliveryTrackUtils.getUserType();
        File adharFile = null;
        File selfieFile = null;
        File otherFile = null;
        File doc = null;
        if (type.equals("Agent")) {
            adharFile = new File(Environment.getExternalStorageDirectory() + "/Documents" + "/" + "AdharCard.jpeg");
            adharParseFile = saveFile(adharFile);
            selfieFile = new File(Environment.getExternalStorageDirectory() + "/Documents" + "/" + "OtherCard.jpeg");
            selfieParseFile = saveFile(selfieFile);
            otherFile = new File(Environment.getExternalStorageDirectory() + "/Documents" + "/" + "Selfie.jpeg");
            otherParseFile = saveFile(otherFile);
        } else {
            doc = new File(Environment.getExternalStorageDirectory() + "/Documents" + "/" + "Document.jpeg");
            docParseFile = saveFile(doc);
        }

        ParseQuery query = new ParseQuery("AdhardCard");
        query.whereEqualTo("userId", ParseUser.getCurrentUser().getObjectId());
        try {
            ParseObject adhardCard;
            List<ParseObject> parseObjects = query.find();
            if (parseObjects != null && parseObjects.size() > 0) {
                adhardCard = parseObjects.get(0);

            } else {
                adhardCard = new ParseObject("AdhardCard");
                adhardCard.put("username", ParseUser.getCurrentUser().getUsername());
                adhardCard.put("userId", ParseUser.getCurrentUser().getObjectId());

            }
            if (type.equals("Agent")) {
                if (adharParseFile != null) {
                    adhardCard.put("adhardcard", adharParseFile);
                    adharFile.delete();
                }
                if (otherParseFile != null) {
                    adhardCard.put("otherDoc", otherParseFile);
                    otherFile.delete();
                }
                if (selfieParseFile != null) {
                    adhardCard.put("selfie", selfieParseFile);
                    selfieFile.delete();
                }

            } else {
                if (docParseFile != null) {
                    adhardCard.put("restDoc", docParseFile);
                    doc.delete();
                }
            }
            try {
                adhardCard.save();
                if (type.equals("Agent") && adharParseFile != null && selfieParseFile != null && otherParseFile != null) {
                    SharedPreferences.Editor editor = DeliveryTrackUtils.getSharedPreferences(this).edit();
                    editor.putBoolean("isAdharSaved", true);
                    editor.commit();
                } else if (docParseFile != null) {
                    SharedPreferences.Editor editor = DeliveryTrackUtils.getSharedPreferences(this).edit();
                    editor.putBoolean("isAdharSaved", true);
                    editor.commit();
                }

            } catch (ParseException e) {
                e.printStackTrace();
            }

        } catch (ParseException e) {
            e.printStackTrace();
        }


    }

    private ParseFile saveFile(File file) {

        //    if (file.length() < IMAGE_MAX_SIZE) {
        ParseFile parseFile = null;
        Log.d("the exception is", "" + file.getAbsolutePath());
        Bitmap bmp = null;
        try {
            bmp = decodeFile(file);
            if (bmp != null) {
                byte[] bitmapBytes = bitmapToByteArray(bmp);
                parseFile = new ParseFile(file.getName(), bitmapBytes);
                try {
                    parseFile.save();
                } catch (ParseException e) {
                    e.printStackTrace();
                }
            }
        } catch (IOException e) {
            Log.d("the exception is", "" + e.toString());
            e.printStackTrace();
        }


        return parseFile;
        //  }

        //return null;

    }


    private Bitmap decodeFile(File f) throws IOException {
        Bitmap b = null;

        //Decode image size
        BitmapFactory.Options o = new BitmapFactory.Options();
        o.inJustDecodeBounds = true;

        FileInputStream fis = new FileInputStream(f);
        BitmapFactory.decodeStream(fis, null, o);
        fis.close();

        int scale = 1;
        if (o.outHeight > IMAGE_MAX_SIZE || o.outWidth > IMAGE_MAX_SIZE) {
            scale = (int) Math.pow(2, (int) Math.ceil(Math.log(IMAGE_MAX_SIZE /
                    (double) Math.max(o.outHeight, o.outWidth)) / Math.log(0.5)));
        }

        //Decode with inSampleSize
        BitmapFactory.Options o2 = new BitmapFactory.Options();
        o2.inSampleSize = scale;
        fis = new FileInputStream(f);
        b = BitmapFactory.decodeStream(fis, null, o2);
        fis.close();

        return b;
    }

    public byte[] bitmapToByteArray(Bitmap b) {

        try {
            ByteArrayOutputStream stream = new ByteArrayOutputStream();
            b.compress(Bitmap.CompressFormat.PNG, 100, stream);
            byte[] byteArray = stream.toByteArray();
            b.recycle();
            return byteArray;
        } catch (OutOfMemoryError e) {

            return null;
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
