package deliverytrack.vss.com.deliverytrack;


import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.location.Location;
import android.location.LocationListener;
import android.os.Bundle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.Serializable;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;

import android.app.Activity;
import android.content.Context;
import android.content.ContextWrapper;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Path;
import android.graphics.RectF;
import android.os.Environment;
import android.provider.MediaStore.Images;
import android.util.AttributeSet;
import android.util.Log;
import android.view.Gravity;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup.LayoutParams;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.GooglePlayServicesUtil;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationServices;
import com.parse.ParseException;
import com.parse.ParseFile;
import com.parse.ParseGeoPoint;
import com.parse.ParseObject;
import com.parse.SaveCallback;

import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;


public class SignatureActivity extends AppCompatActivity implements GoogleApiClient.ConnectionCallbacks, GoogleApiClient.OnConnectionFailedListener {

    View mView;
    Button b1;
    Button b2;
    private Paint mPaint;

    private Path path;
    private Bitmap mBitmap;
    private Canvas mCanvas;

    double latitud;
    double longitud;

    String currentDateTime2;
    private GoogleApiClient mGoogleApiClient;
    private LocationRequest mLocationRequest;

    // boolean flag to toggle periodic location updates
    private boolean mRequestingLocationUpdates = false;

    // Location updates intervals in sec
    private static int UPDATE_INTERVAL = 10000; // 10 sec
    private static int FATEST_INTERVAL = 5000; // 5 sec
    private static int DISPLACEMENT = 10;

    private static final int PLAY_SERVICES_RESOLUTION_REQUEST = 0;
    private Location mLastLocation;
    private String uniqueId;
    private EditText yourName;
    private String orderNo;
    private ProgressDialog dialog;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_signature);

        dialog = new ProgressDialog(this);
        dialog.setTitle("DeliveryTrack");
        dialog.setMessage("Loading");
        dialog.setCancelable(false);
        dialog.setCanceledOnTouchOutside(false);
        LinearLayout layout = (LinearLayout) findViewById(R.id.myDrawing);
        mView = new DrawingView(this);
        layout.addView(mView, new LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.MATCH_PARENT));
        init();


        Intent intent = getIntent();
        orderNo = intent.getExtras().getString("orderNumber");
        b1 = (Button) findViewById(R.id.button);
        b1.setOnClickListener(myhandler1);
        b2 = (Button) findViewById(R.id.button1);
        b2.setOnClickListener(myhandler2);

        setTitle(getResources().getString(R.string.please_sign_here));
        if (checkPlayServices()) {
            buildGoogleApiClient();
        }

    }

    private void displayLocation() {

        mLastLocation = LocationServices.FusedLocationApi
                .getLastLocation(mGoogleApiClient);

        if (DeliveryTrackUtils.checkLocationIsOn(this)) {
            if (mLastLocation != null) {
                double latitude = mLastLocation.getLatitude();
                double longitude = mLastLocation.getLongitude();

                latitud = latitude;
                longitud = longitude;

            }
        } else {


        }

    }

    protected synchronized void buildGoogleApiClient() {
        mGoogleApiClient = new GoogleApiClient.Builder(this)
                .addConnectionCallbacks(this)
                .addOnConnectionFailedListener(this)
                .addApi(LocationServices.API).build();
    }

    private boolean checkPlayServices() {
        int resultCode = GooglePlayServicesUtil
                .isGooglePlayServicesAvailable(this);
        if (resultCode != ConnectionResult.SUCCESS) {
            if (GooglePlayServicesUtil.isUserRecoverableError(resultCode)) {
                GooglePlayServicesUtil.getErrorDialog(resultCode, this,
                        PLAY_SERVICES_RESOLUTION_REQUEST).show();
            } else {
                Toast.makeText(getApplicationContext(),
                        "This device is not supported.", Toast.LENGTH_LONG)
                        .show();
                finish();
            }
            return false;
        }
        return true;
    }

    private class mylocationlistener implements LocationListener {
        @Override
        public void onLocationChanged(Location location) {
            System.out.println("Location changed, " + location.getAccuracy() + " , " + location.getLatitude() + "," + location.getLongitude());
            if (location != null) {
                latitud = location.getLatitude();
                longitud = location.getLongitude();
            }
        }

        @Override
        public void onProviderDisabled(String provider) {
        }

        @Override
        public void onProviderEnabled(String provider) {
        }

        @Override
        public void onStatusChanged(String provider, int status, Bundle extras) {
        }
    }

    @Override
    protected void onStart() {
        super.onStart();
        if (mGoogleApiClient != null) {
            mGoogleApiClient.connect();
        }
    }

    @Override
    protected void onResume() {
        super.onResume();

        checkPlayServices();
    }

    @Override
    public void onConnectionFailed(ConnectionResult result) {

        AlertDialog alertDialog = new AlertDialog.Builder(SignatureActivity.this).create();
        alertDialog.setTitle("Error");
        alertDialog.setMessage("Connection failed: ConnectionResult.getErrorCode() = " + result.getErrorCode());
        alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, "OK",
                new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        dialog.dismiss();
                    }
                });
        alertDialog.show();
        System.out.println("Connection failed: ConnectionResult.getErrorCode() = " + result.getErrorCode());
    }

    @Override
    public void onConnected(Bundle arg0) {
        // Once connected with google api, get the location
        displayLocation();
    }

    @Override
    public void onConnectionSuspended(int arg0) {
        mGoogleApiClient.connect();
    }

    View.OnClickListener myhandler1 = new View.OnClickListener() {
        public void onClick(View v) {
            // Clear Button
            finish();
            //init();
            //mBitmap.eraseColor(Color.TRANSPARENT);

            /*AlertDialog alertDialog = new AlertDialog.Builder(SignattureActivity.this).create();
            alertDialog.setTitle("Clear");
            alertDialog.setMessage("Alert message to be shown");
            alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, "OK",
                    new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.dismiss();
                        }
                    });
            alertDialog.show();*/

        }
    };
    private ParseFile mysignature;
    private byte[] scaledData;
    View.OnClickListener myhandler2 = new View.OnClickListener() {
        public void onClick(View v) {

            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            mView.draw(mCanvas);
            mBitmap.compress(Bitmap.CompressFormat.JPEG, 100, bos);
             scaledData = bos.toByteArray();
            mysignature = new ParseFile("mysignature.jpg", scaledData);
            SimpleDateFormat sdf = new SimpleDateFormat("yyyyMMdd_HHmmss");
            final String currentDateTime = sdf.format(new Date());
            currentDateTime2 = currentDateTime;

            ParseObject oImg = new ParseObject("Order_Confirmation_Details");
            oImg.put("Date_Time", currentDateTime);
            oImg.put("Order_ID", orderNo); /* Replace with real order id */
            ParseGeoPoint point = new ParseGeoPoint(latitud, longitud);
            oImg.put("GPS_Location", point);
            oImg.put("OObjectId", "ID_" + currentDateTime);

            oImg.saveInBackground(new SaveCallback() {
                @Override
                public void done(ParseException e) {
                    dialog.dismiss();

                    if (e == null) {
                        System.out.println("img save ....");
                        OpenRatingOrder();
                    } else {
                        DeliveryTrackUtils.showToast(SignatureActivity.this, e.toString());
                        Log.d("The error", e.toString() + " saved image");
                    }
                }
            });
        }


        // Open prettyRatingBar

    };

    public void OpenRatingOrder() {
        AlertDialog alertDialog = new AlertDialog.Builder(SignatureActivity.this).create();
        alertDialog.setTitle("Confirmation Sent");
        alertDialog.setMessage("Your confirmation or the order has been sent.");
        alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, "OK",
                new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        Intent i = new Intent(getApplicationContext(), RatingActivity.class);
                        i.putExtra("object_id", currentDateTime2);
                        i.putExtra("parsefile", scaledData);
                        startActivity(i);
                    }
                });
        alertDialog.show();


    }

    private void init() {
        mPaint = new Paint();
        mPaint.setDither(true);
        mPaint.setColor(Color.BLACK);
        mPaint.setStyle(Paint.Style.STROKE);
        mPaint.setStrokeJoin(Paint.Join.ROUND);
        mPaint.setStrokeCap(Paint.Cap.ROUND);
        mPaint.setStrokeWidth(3);


    }

    class DrawingView extends View {


        public DrawingView(Context context) {
            super(context);
            path = new Path();
            mBitmap = Bitmap.createBitmap(900, 900, Bitmap.Config.ARGB_8888);
            mCanvas = new Canvas(mBitmap);
            this.setBackgroundColor(Color.WHITE);
        }

        private ArrayList<PathWithPaint> _graphics1 = new ArrayList<PathWithPaint>();

        @Override
        public boolean onTouchEvent(MotionEvent event) {
            PathWithPaint pp = new PathWithPaint();
            mCanvas.drawPath(path, mPaint);
            if (event.getAction() == MotionEvent.ACTION_DOWN) {
                path.moveTo(event.getX(), event.getY());
                path.lineTo(event.getX(), event.getY());
            } else if (event.getAction() == MotionEvent.ACTION_MOVE) {
                path.lineTo(event.getX(), event.getY());
                pp.setPath(path);
                pp.setmPaint(mPaint);
                _graphics1.add(pp);
            }
            invalidate();
            return true;
        }

        @Override
        protected void onDraw(Canvas canvas) {
            super.onDraw(canvas);
            if (_graphics1.size() > 0) {
                canvas.drawPath(
                        _graphics1.get(_graphics1.size() - 1).getPath(),
                        _graphics1.get(_graphics1.size() - 1).getmPaint());
            }
        }
    }


    public class PathWithPaint {
        private Path path;

        public Path getPath() {
            return path;
        }

        public void setPath(Path path) {
            this.path = path;
        }

        private Paint mPaint;

        public Paint getmPaint() {
            return mPaint;
        }

        public void setmPaint(Paint mPaint) {
            this.mPaint = mPaint;
        }
    }
}

