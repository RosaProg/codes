package deliverytrack.vss.com.deliverytrack;

import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import android.os.Bundle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;

import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;

import com.parse.ParseException;
import com.parse.ParseUser;
import com.parse.RequestPasswordResetCallback;

import java.util.Locale;

import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;


public class LandingScreenActivity extends AppCompatActivity implements View.OnClickListener {

    private String selectedLanguage = null;
    private Button signup;
    private Button login;
    private Button forgotpassword;

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.language_settings, menu);

        return super.onCreateOptionsMenu(menu);
    }


    @Override
    public boolean onOptionsItemSelected(MenuItem item) {

        switch (item.getItemId()) {

            case R.id.action_settings:
                showLanguageSettings();
                break;


        }
        return super.onOptionsItemSelected(item);

    }


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_landing_screen);

        signup = (Button) findViewById(R.id.sign_up);
        login = (Button) findViewById(R.id.login);
        forgotpassword = (Button) findViewById(R.id.forgotpwd);

        signup.setOnClickListener(this);
        login.setOnClickListener(this);
        forgotpassword.setOnClickListener(this);
        if (!DeliveryTrackUtils.getSharedPreferences(this).getBoolean("isLang", false)) {
            showLanguageSettings();
        } else {
            selectedLanguage = getResources().getConfiguration().locale.getLanguage();
        }
    }


    private void showLanguageSettings() {
        final String[] listContent = {getResources().getString(R.string.english), getResources().getString(R.string.hindi)};
        AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(this);
        LayoutInflater inflater = this.getLayoutInflater();
        View dialogView = inflater.inflate(R.layout.select_language, null);
        dialogBuilder.setView(dialogView);
        dialogBuilder.setTitle("Select Your Language");
        ListView listView = (ListView) dialogView.findViewById(R.id.listview);
        final AlertDialog alertDialog = dialogBuilder.create();

        ArrayAdapter<String> adapter
                = new ArrayAdapter<String>(this,
                android.R.layout.simple_list_item_1, listContent);
        listView.setAdapter(adapter);
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                selectedLanguage = listContent[position];

                switch (position) {
                    case 1:
                        Locale locale = new Locale("hi");
                        DeliveryTrackUtils.saveLocale("hi", LandingScreenActivity.this);
                        Locale.setDefault(locale);
                        Configuration config = new Configuration();
                        config.locale = locale;
                        getApplicationContext().getResources().updateConfiguration(config, getBaseContext().getResources().getDisplayMetrics());
                        onConfigurationChanged(config);
                        SharedPreferences.Editor editor = DeliveryTrackUtils.getSharedPreferences(LandingScreenActivity.this).edit();
                        editor.putBoolean("isLang", true);
                        editor.commit();

                        break;

                    case 0:
                        Locale locale1 = new Locale("en");
                        DeliveryTrackUtils.saveLocale("en", LandingScreenActivity.this);
                        Locale.setDefault(locale1);
                        Configuration config1 = new Configuration();
                        config1.locale = locale1;
                        getApplicationContext().getResources().updateConfiguration(config1, getBaseContext().getResources().getDisplayMetrics());
                        onConfigurationChanged(config1);
                        SharedPreferences.Editor editor1 = DeliveryTrackUtils.getSharedPreferences(LandingScreenActivity.this).edit();
                        editor1.putBoolean("isLang", true);
                        editor1.commit();

                        break;
                }

                alertDialog.dismiss();


            }
        });

        alertDialog.show();


    }

    private void showInstructionDialog() {
        AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(this);
        LayoutInflater inflater = this.getLayoutInflater();
        View dialogView = inflater.inflate(R.layout.instruction_layout, null);
        dialogBuilder.setView(dialogView);
        View titleView = inflater.inflate(R.layout.custom_title, null);
        dialogBuilder.setCustomTitle(titleView);
        Button nextButton = (Button) dialogView.findViewById(R.id.next_button);
        nextButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent signUp = new Intent(LandingScreenActivity.this, QuickRegistrationActivity.class);
                startActivity(signUp);
            }
        });

        AlertDialog alertDialog = dialogBuilder.create();
        alertDialog.show();
    }


    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        login.setText(getResources().getString(R.string.login));
        signup.setText(getResources().getString(R.string.signup));

    }


    private void showEmailBox() {
        LayoutInflater li = LayoutInflater.from(this);
        View promptsView = li.inflate(R.layout.prompt, null);

        AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(
                this);

        // set prompts.xml to alertdialog builder
        alertDialogBuilder.setView(promptsView);

        final TextView text = (TextView) promptsView.findViewById(R.id.textView1);
        text.setText("Enter email");
        final EditText userInput = (EditText) promptsView
                .findViewById(R.id.editTextDialogUserInput);

        // set dialog message
        alertDialogBuilder
                .setCancelable(false)
                .setPositiveButton("OK",
                        new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                // get user input and set it to result
                                // edit text

                                processForgotPassword(userInput.getText().toString());
                            }
                        })
                .setNegativeButton("Cancel",
                        new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                dialog.cancel();
                            }
                        });

        // create alert dialog
        AlertDialog alertDialog = alertDialogBuilder.create();

        // show it
        alertDialog.show();

    }


    private void processForgotPassword(String email) {
        ParseUser.requestPasswordResetInBackground(email,
                new RequestPasswordResetCallback() {
                    public void done(ParseException e) {
                        if (e == null) {
                            DeliveryTrackUtils.showToast(LandingScreenActivity.this, "An email has been sent with reset instructions");
                        } else {
                            // Something went wrong. Look at the ParseException to see what's up.
                            DeliveryTrackUtils.showToast(LandingScreenActivity.this, e.getMessage() + "");
                        }
                    }
                });
    }

    @Override
    public void onClick(View v) {

        switch (v.getId()) {
            case R.id.sign_up:
                if (selectedLanguage != null) {
                    showInstructionDialog();
                } else {

                    showLanguageSettings();
                }
                break;

            case R.id.login:
                if (selectedLanguage != null) {
                    Intent loginIntent = new Intent(this, DeliveryTrackLogin.class);
                    startActivity(loginIntent);
                } else {
                    showInstructionDialog();
                }
                break;


            case R.id.forgotpwd:
                showEmailBox();
                break;
        }

    }

}
