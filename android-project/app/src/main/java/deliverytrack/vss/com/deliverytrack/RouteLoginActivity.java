package deliverytrack.vss.com.deliverytrack;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;

import com.parse.ParseUser;

import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

public class RouteLoginActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_route_login);

        boolean isRegistered = DeliveryTrackUtils.getSharedPreferences(RouteLoginActivity.this).
                getBoolean("isRegistered", false);

        Log.d("The registration done", String.valueOf(isRegistered));

        if (isRegistered && ParseUser.getCurrentUser() == null) {
            startActivity(new Intent(this, DeliveryTrackLogin.class));
        } else if (isRegistered && ParseUser.getCurrentUser() != null) {
            startActivity(new Intent(this, BaseActivity.class));

        } else if (!isRegistered && ParseUser.getCurrentUser() == null) {
            startActivity(new Intent(this, LandingScreenActivity.class));
        } else if (!isRegistered && ParseUser.getCurrentUser() != null) {
            startActivity(new Intent(this, BaseActivity.class));

        }

    }

}
