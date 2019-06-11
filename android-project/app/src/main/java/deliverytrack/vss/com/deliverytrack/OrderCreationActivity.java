package deliverytrack.vss.com.deliverytrack;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.location.Address;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.text.InputFilter;
import android.text.Spanned;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.support.v7.app.AlertDialog.Builder;


import com.parse.ParseGeoPoint;
import com.parse.ParseObject;
import com.parse.ParseUser;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import deliverytrack.vss.com.deliverytrack.adapters.PlacesAdapter;
import deliverytrack.vss.com.deliverytrack.asynch.SaveObject;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.ParseHelper;
import deliverytrack.vss.com.deliverytrack.utility.PostalCodeAPI;

/*used to create an order*/
public class
        OrderCreationActivity extends AppCompatActivity implements View.OnClickListener,
        OrderCallbacks, AdapterView.OnItemSelectedListener, DialogInterface.OnClickListener {


    private EditText edtOrderName;
    private EditText edtOrder;
    private EditText edtAddress;
    private EditText edtAmount;
    private EditText edtCustomerContactNo;
    private Spinner edtPaymentMode;
    private ArrayList<String> categories;
    private ArrayAdapter dataAdapter;
    private String selectedPayment;
    private LocationManager locManager;
    private LocationListener locListener = new MyLocationListener();
    private double longitude;
    private double latitude;
    private Location location;
    private boolean gps_enabled;
    private boolean network_enabled;
    private String area;
    private AutoCompleteTextView autocompleteView;
    private ProgressDialog mProgressDialog;
    private EditText edtPinCode;
    private String city = " ";
    private EditText edtCity;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_order_creation);

        edtOrderName = (EditText) findViewById(R.id.edt_order_name);
        edtOrder = (EditText) findViewById(R.id.edt_order);
        edtAddress = (EditText) findViewById(R.id.edt_order_address);
        edtAmount = (EditText) findViewById(R.id.edt_order_amount);
        edtPinCode = (EditText) findViewById(R.id.edt_pincode);
        edtCity = (EditText) findViewById(R.id.edt_city);
        edtAmount.setFilters(new InputFilter[]{new DecimalDigitsInputFilter(6, 2)});
        autocompleteView = (AutoCompleteTextView) findViewById(R.id.autocomplete);
        autocompleteView.setAdapter(new PlacesAdapter(this, R.layout.autocomplete_list));
        autocompleteView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

                autocompleteView.post(new Runnable() {
                    public void run() {
                        autocompleteView.dismissDropDown();
                    }
                });

                String apiresult = (String) parent.getItemAtPosition(position);
                String[] parts = DeliveryTrackUtils.splitStrings(apiresult);
                area = parts[0];
                new GetPostalCode(parts[1]).execute();
                autocompleteView.setText(DeliveryTrackUtils.splitStrings(apiresult)[0]);

            }
        });

        edtCustomerContactNo = (EditText) findViewById(R.id.edt_customer_number);
        edtCustomerContactNo.setText("+91");
        edtPaymentMode = (Spinner) findViewById(R.id.spinner);

        edtPaymentMode.setOnItemSelectedListener(this);

        // Spinner Drop down elements
        categories = new ArrayList<String>();
        categories.add(getResources().getString(R.string.postpaid));
        categories.add(getResources().getString(R.string.prepaid));

        // Creating adapter for spinner
        dataAdapter = new ArrayAdapter(this, android.R.layout.simple_spinner_item, categories);

        // Drop down layout style - list view with radio button
        dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        // attaching data adapter to spinner
        edtPaymentMode.setAdapter(dataAdapter);

        locManager = (LocationManager) this.getSystemService(Context.LOCATION_SERVICE);

        try {
            gps_enabled = locManager.isProviderEnabled(LocationManager.GPS_PROVIDER);
        } catch (Exception ex) {
        }
        try {
            network_enabled = locManager.isProviderEnabled(LocationManager.NETWORK_PROVIDER);
        } catch (Exception ex) {
        }

        // don't start listeners if no provider is enabled
        if (!gps_enabled && !network_enabled) {
            AlertDialog.Builder builder = new Builder(this);
            builder.setTitle("Attention!");
            builder.setMessage("Sorry, location is not determined. Please enable location providers");
            builder.setPositiveButton("OK", this);
            builder.setNeutralButton("Cancel", this);
            builder.create().show();
        }

        if (gps_enabled) {
            locManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, locListener);
        }
        if (network_enabled) {
            locManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER, 0, 0, locListener);
        }

        Button btn = (Button) findViewById(R.id.btn_submit);
        btn.setOnClickListener(this);
        setTitle(getResources().getString(R.string.create_delivery_order));

    }


    class MyLocationListener implements LocationListener {

        @Override
        public void onLocationChanged(Location location) {
            if (location != null) {
                locManager.removeUpdates(locListener);
                OrderCreationActivity.this.location = location;
                longitude = location.getLongitude();
                latitude = location.getLatitude();

            }
        }

        @Override
        public void onProviderDisabled(String provider) {
            // TODO Auto-generated method stub

        }

        @Override
        public void onProviderEnabled(String provider) {

        }

        @Override
        public void onStatusChanged(String provider, int status, Bundle extras) {

        }
    }


    @Override
    public void onClick(View v) {
        Address location = null;
      /*saving the order to the server*/
        ParseGeoPoint geoPoint = null;
        if (OrderCreationActivity.this.location != null) {
            geoPoint = new ParseGeoPoint(OrderCreationActivity.this.location.getLatitude(),
                    OrderCreationActivity.this.location.getLongitude());
        } else {
            DeliveryTrackUtils.showToast(this, "Kindly fill all the details");
        }

        if (autocompleteView.getText().toString() != null && !autocompleteView.getText().toString().equals("")) {
            DeliveryTrackUtils.getLocationFromAddress(autocompleteView.getText().toString(), this);
        }

        if (geoPoint != null) {
            if (edtOrderName != null && !edtOrderName.getText().toString().equals("") &&
                    edtOrder != null && !edtOrder.getText().toString().equals("") &&
                    edtAddress != null && !edtAddress.getText().toString().equals("") &&
                    edtAmount != null && !edtAmount.getText().toString().equals("")
                    && edtCustomerContactNo != null && !edtCustomerContactNo.getText().toString().equals("")
                    && selectedPayment != null && !selectedPayment.equals("") &&
                    autocompleteView.getText().toString() != null &&
                    !autocompleteView.getText().toString().equals("") &&
                    !edtPinCode.getText().toString().equals("") && !edtCity.getText().toString().equals("")) {
                ParseUser user = ParseUser.getCurrentUser();
                Order order = new Order();
                order.setName(edtOrderName.getText().toString());
                order.setOrder(edtOrder.getText().toString());
                order.setAddress(edtAddress.getText().toString());
                order.setAmount(Double.parseDouble(edtAmount.getText().toString()));
                order.setLocation(geoPoint);
                order.setPinCode(edtPinCode.getText().toString());
                order.setArea(autocompleteView.getText().toString());
                order.setCity(edtCity.getText().toString());
                order.setRestId(user.getObjectId());
                order.setRestName(user.get("userFullName") == null ? "" : ParseUser.getCurrentUser().get("userFullName").toString());
                order.setCustomerContactNo(edtCustomerContactNo.getText().toString());
                order.setRestContactNo(user.get("phone" +
                        "") != null ? user.get("phone").toString() : "");
                order.setPaymentMode(selectedPayment);

                ParseHelper helper = new ParseHelper();
                ParseObject object = helper.saveAsOrder(order);

                SaveObject saveObject = new SaveObject(this, object);
                saveObject.execute();
            } else {
                DeliveryTrackUtils.showToast(this, "mandtory fields are missing");

                if (edtOrderName.getText().toString().length() == 0) {
                    edtOrderName.setError("Order name is required");
                }
                if (edtOrder.getText().toString().length() == 0) {
                    edtOrder.setError("Order is required");
                }
                if (edtAddress.getText().toString().length() == 0) {
                    edtAddress.setError("Address is required");
                }
                if (edtAmount.getText().toString().length() == 0) {
                    edtAmount.setError("Amount is required");
                }
                if (edtCustomerContactNo.getText().toString().length() == 0) {
                    edtCustomerContactNo.setError("Customer Contact number is required");
                }
                if (autocompleteView.getText().toString().length() == 0) {
                    autocompleteView.setError("Area is required");
                }

            }
        } else {
            DeliveryTrackUtils.showToast(this, "Error Please try again");

        }


    }

    @Override
    public void getParseObjects(List<ParseObject> parseObjects, int tableId) {

    }

    @Override
    public void restParseObjects(Order order) {

    }

    @Override
    public void onOrderSaveSuccess() {
        this.finish();
        DeliveryTrackUtils.showToast(this, "The order was successfully saved");

    }

    @Override
    public void onOrderSaveFailure() {
        DeliveryTrackUtils.showToast(this, "The order was not saved!!! Please Try Again");

    }

    @Override
    public void onOrderCanceled() {

    }

    @Override
    public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {

        selectedPayment = categories.get(position);
    }

    @Override
    public void onNothingSelected(AdapterView<?> parent) {
        selectedPayment = categories.get(0);
    }

    @Override
    public void onClick(DialogInterface dialog, int which) {
        if (which == DialogInterface.BUTTON_NEUTRAL) {
            DeliveryTrackUtils.showToast(this, "Sorry, location is not determined. To fix this please enable location providers");
        } else if (which == DialogInterface.BUTTON_POSITIVE) {
            startActivity(new Intent(android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS));
        }
    }


    public class DecimalDigitsInputFilter implements InputFilter {

        Pattern mPattern;

        public DecimalDigitsInputFilter(int digitsBeforeZero, int digitsAfterZero) {
            mPattern = Pattern.compile("[0-9]{0," + (digitsBeforeZero - 1) + "}+((\\.[0-9]{0," + (digitsAfterZero - 1) + "})?)||(\\.)?");
        }

        @Override
        public CharSequence filter(CharSequence source, int start, int end, Spanned dest, int dstart, int dend) {

            Matcher matcher = mPattern.matcher(dest);
            if (!matcher.matches())
                return "";
            return null;
        }

    }


    public class GetPostalCode extends AsyncTask<Void, Void, String[]> {

        String placeId;

        GetPostalCode(String placeId) {
            this.placeId = placeId;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            mProgressDialog = new ProgressDialog(OrderCreationActivity.this);
            mProgressDialog.setTitle("Retrieving Data");
            mProgressDialog.setMessage("Loading...");
            mProgressDialog.setIndeterminate(false);
            mProgressDialog.setCanceledOnTouchOutside(false);
            mProgressDialog.setCancelable(false);
            mProgressDialog.show();

        }

        @Override
        protected void onPostExecute(String[] s) {
            super.onPostExecute(s);
            mProgressDialog.dismiss();
            if (s != null) {
                if (s[0] != null) {
                    edtPinCode.setText(s[0] + " ");
                }

                if (s[1] != null) {
                    edtCity.setText(s[1]);

                }
            }


        }

        @Override
        protected String[] doInBackground(Void... params) {

            PostalCodeAPI postalCodeAPI = new PostalCodeAPI();
            return postalCodeAPI.details(this.placeId);
        }
    }


}
