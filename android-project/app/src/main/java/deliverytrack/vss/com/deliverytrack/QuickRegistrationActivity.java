package deliverytrack.vss.com.deliverytrack;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.speech.RecognizerIntent;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.Spinner;
import android.widget.Toast;

import com.parse.ParseException;
import com.parse.ParseUser;
import com.parse.SignUpCallback;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;


public class QuickRegistrationActivity extends AppCompatActivity implements View.OnFocusChangeListener, View.OnClickListener, AdapterView.OnItemSelectedListener {

    private EditText edtFullname;
    private EditText edtPassword;
    private EditText edtConPassword;
    private EditText edtMobileNumber;
    private ImageButton fullNameMic;
    private int NAME_TEXT = 10;
    private String[] users;
    private String userType = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_quick_registration);
        edtFullname = (EditText) findViewById(R.id.edt_full_name);
        edtPassword = (EditText) findViewById(R.id.edt_password);
        edtConPassword = (EditText) findViewById(R.id.edt_con_password);
        edtMobileNumber = (EditText) findViewById(R.id.edt_mobile_number);
        edtFullname.setOnFocusChangeListener(this);

        fullNameMic = (ImageButton) findViewById(R.id.id_your_name_mic);
        fullNameMic.setOnClickListener(this);

        Button btn = (Button) findViewById(R.id.registration);
        btn.setOnClickListener(this);

        setTitle(getResources().getString(R.string.signup));


        users = getResources().getStringArray(R.array.user_type);
        // Creating adapter for spinner
        ArrayAdapter dataAdapter = new ArrayAdapter(this, android.R.layout.simple_spinner_item, users);
        // Drop down layout style - list view with radio button
        dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        // attaching data adapter to spinner
        Spinner spinneruser = (Spinner) findViewById(R.id.spinneruser);
        spinneruser.setOnItemSelectedListener(this);
        spinneruser.setAdapter(dataAdapter);


    }


    @Override
    public void onFocusChange(View v, boolean hasFocus) {

    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.id_your_name_mic:
                startVoiceRecognitionActivity(NAME_TEXT);

                break;

            case R.id.registration:
                processRegistration();
                break;
        }

    }

    private void processRegistration() {

        if (edtFullname.getText().toString() != null &&
                !edtFullname.getText().toString().equals("") &&
                edtPassword.getText().toString() != null &&
                !edtPassword.getText().toString().equals("") &&
                edtConPassword.getText().toString() != null &&
                !edtConPassword.getText().toString().equals("") &&
                edtMobileNumber.getText().toString() != null &&
                !edtMobileNumber.getText().toString().equals("")
                && userType != null) {

            if (!edtPassword.getText().toString().equals(edtConPassword.getText().toString())) {
                Toast.makeText(QuickRegistrationActivity.this, getResources().getString(R.string.password_error),
                        Toast.LENGTH_LONG).show();
                return;
            }

            if (!edtMobileNumber.getText().toString().equals("") && edtMobileNumber.getText().toString().length() != 10) {
                Toast.makeText(QuickRegistrationActivity.this, "Please type the mobile number properly",
                        Toast.LENGTH_LONG).show();
                return;
            }

            final ParseUser user = new ParseUser();
            user.setUsername(edtMobileNumber.getText().toString());
            user.setPassword(edtPassword.getText().toString());
            user.put("name", userType);
            user.put("phone", edtMobileNumber.getText().toString());
            user.put("userStatus", "InActive");
            user.put("userFullName", edtFullname.getText().toString());

            user.signUpInBackground(new SignUpCallback() {
                @Override
                public void done(ParseException e) {
                    if (e != null) {
                        String[] stry = splitString(e.toString());
                        if (stry != null && stry.length > 1) {

                            DeliveryTrackUtils.showToast(QuickRegistrationActivity.this, stry[1]);
                        }
                    } else {
                        if (userType.equals("Agent")) {
                            DeliveryTrackUtils.showToast(QuickRegistrationActivity.this, getResources().getString(R.string.agent_registered));
                        } else {
                            DeliveryTrackUtils.showToast(QuickRegistrationActivity.this, getResources().getString(R.string.supervisor_registered));
                        }
                        SharedPreferences.Editor editor = DeliveryTrackUtils.getSharedPreferences(QuickRegistrationActivity.this).edit();
                        editor.putBoolean("isRegistered", true);
                        editor.commit();

                        Intent i = new Intent(QuickRegistrationActivity.this,
                                DeliveryTrackLogin.class);
                        startActivity(i);


                    }
                }

            });
        } else {
            DeliveryTrackUtils.showToast(QuickRegistrationActivity.this, getResources().getString(R.string.mandatory_fields));
        }
    }


    private void startVoiceRecognitionActivity(int reqCode) {
        try {
            Intent intent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
            intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL,
                    RecognizerIntent.LANGUAGE_MODEL_FREE_FORM);
            intent.putExtra(RecognizerIntent.EXTRA_PROMPT, "Please Speak to input...");
            startActivityForResult(intent, reqCode);
        } catch (Exception e) {

        }
    }

    private String[] splitString(String e) {

        return e.split(":");
    }


    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (data != null) {
            ArrayList<String> matches = data.getStringArrayListExtra(
                    RecognizerIntent.EXTRA_RESULTS);

            if (requestCode == NAME_TEXT) {
                edtFullname.setText(matches.get(0));
            }
        }
        super.onActivityResult(requestCode, resultCode, data);

    }

    @Override
    public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {

        if (position == 0) {
            userType = "Agent";
        } else if (position == 1) {
            userType = "Restaurant";
        }

    }

    @Override
    public void onNothingSelected(AdapterView<?> parent) {

    }
}
