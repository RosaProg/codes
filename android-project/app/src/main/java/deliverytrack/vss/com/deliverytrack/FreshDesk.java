package deliverytrack.vss.com.deliverytrack;

import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;

import com.freshdesk.mobihelp.Mobihelp;
import com.freshdesk.mobihelp.MobihelpConfig;

import deliverytrack.vss.com.deliverytrack.constants.Constants;


public class FreshDesk extends AppCompatActivity implements View.OnClickListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_fresh_desk);
        Mobihelp.init(this, new MobihelpConfig(Constants.FRESH_DESK_URL,
                Constants.FRESH_DESK_APP_KEY, Constants.FRESH_DESK_APP_SECRET));

        Button support = (Button) findViewById(R.id.support);
        Button feedback = (Button) findViewById(R.id.feedback);

        support.setOnClickListener(this);
        feedback.setOnClickListener(this);

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_fresh_desk, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {

            case R.id.support:
                Mobihelp.showSupport(this);
                break;

            case R.id.feedback:
                Mobihelp.showFeedback(this);
                break;
        }
    }
}
