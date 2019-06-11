package deliverytrack.vss.com.deliverytrack;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.location.Location;
import android.location.LocationManager;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import com.parse.FindCallback;
import com.parse.LogInCallback;
import com.parse.ParseException;
import com.parse.ParseQuery;
import com.parse.ParseUser;
import com.parse.SaveCallback;

import java.util.List;

import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;


public class DeliveryTrackLogin extends AppCompatActivity implements View.OnClickListener {


    private EditText edtUsername;
    private EditText edtPassword;
    private ProgressDialog dialog;

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_delivery_track_login);

        edtUsername = (EditText) findViewById(R.id.edt_username);
        edtPassword = (EditText) findViewById(R.id.edt_password);
        Button btnLogin = (Button) findViewById(R.id.btn_login);
        btnLogin.setOnClickListener(this);
        dialog = new ProgressDialog(this);


    }


    @Override
    public void onClick(View v) {

        String username = String.valueOf(edtUsername.getText().toString());
        String password = edtPassword.getText().toString();

        if (username.trim() != null && password.trim() != null &&
                !username.equals("") && !password.trim().equals("")) {

            if (!DeliveryTrackUtils.isOnline(DeliveryTrackLogin.this)) {
                DeliveryTrackUtils.showToast(DeliveryTrackLogin.this, "No network is connected");
                return;
            }
            if (DeliveryTrackUtils.checkLocationIsOn(DeliveryTrackLogin.this)) {
                parseLogin();
            }
        } else {
            DeliveryTrackUtils.showToast(this, "Username,password should not be empty");
        }


    }

    /* IMPORTANT! The system to login in AWS is done. Please migrate this to the new system */
    private void parseLogin() {
         /*orders pending pickup fragment contains the data about the orders which are pending for pickup*/
        dialog.show();
        ParseUser.logInInBackground(edtUsername.getText().toString(), edtPassword.getText().toString(), new LogInCallback() {

            @Override
            public void done(ParseUser user, ParseException e) {
                dialog.dismiss();
                if (e != null) {
                    String[] stry = splitString(e.toString());
                    if (stry != null && stry.length > 1) {
                        DeliveryTrackUtils.showToast(DeliveryTrackLogin.this, stry[1]);
                    }

                } else {
                    user.put("loggedIn", true);
                    user.saveInBackground(new SaveCallback() {
                        @Override
                        public void done(ParseException e) {
                            String userType = DeliveryTrackUtils.getUserType();
                            Log.d("the userType", userType + "");
                            Intent intent = new Intent(DeliveryTrackLogin.this, BaseActivity.class);
                            startActivity(intent);

                        }
                    });

                }

            }
        });

        String url = "http://www.vsstechnology.com/deliverytrack/api/?q=login&user="+edtUsername.getText().toString()+"&latitude=11111111&longitude=11111111";
        Log.v("LOGIN", "url" + url);
        String[] params = {url};
        new LongOperation().execute(params);
    }


    private String[] splitString(String e) {

        return e.split(":");
    }


}
