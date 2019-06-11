package deliverytrack.vss.com.deliverytrack;

import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.AppCompatActivity;
import android.view.MenuItem;

import deliverytrack.vss.com.deliverytrack.fragments.payone.CollectionRequestFragment;
import deliverytrack.vss.com.deliverytrack.fragments.payone.PayOneFragment;
import deliverytrack.vss.com.deliverytrack.fragments.payone.StatusCheckFragment;

/**
 * Created by LENOVO on 12/10/2015.
 */
public class PayOneActivityOptions extends AppCompatActivity implements PayOneFragment.OnFragmentInteractionListener, CollectionRequestFragment.OnFragmentInteractionListener, StatusCheckFragment.OnFragmentInteractionListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.payone_option);

        PayOneFragment opf = new PayOneFragment();
        FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        transaction.replace(R.id.container, opf, "PayOneFragment");
        transaction.addToBackStack(null);
        transaction.commit();
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
    public void onFragmentInteraction(Uri uri) {

    }
}
