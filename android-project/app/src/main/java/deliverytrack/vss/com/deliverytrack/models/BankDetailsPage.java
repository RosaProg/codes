package deliverytrack.vss.com.deliverytrack.models;

import android.content.Context;
import android.support.v4.app.Fragment;
import android.text.TextUtils;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.fragments.registration.BankDetailsFragment;
import deliverytrack.vss.com.deliverytrack.interfaces.ModelCallbacks;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * Created by Adi-Loch on 9/12/2015.
 */
public class BankDetailsPage extends Page {

    public static final String EXACT_NAME = "exact_name";
    public static final String BANK_NAME = "bank_name";
    public static final String BANK_ACCOUNT = "bank_account";
    public static final String IFSCCODE = "ifscode";


    protected BankDetailsPage(ModelCallbacks callbacks, String title) {
        super(callbacks, title);
    }

    @Override
    public Fragment createFragment() {
        return BankDetailsFragment.create(getKey());
    }

    @Override
    public void getReviewItems(ArrayList<ReviewItem> dest, Context activity) {

        dest.add(new ReviewItem("Your Exact Name", mData.getString(EXACT_NAME),
                getKey(), -1));

        dest.add(new ReviewItem("Your Bank Name", mData.getString(BANK_NAME),
                getKey(), -1));

        dest.add(new ReviewItem("Your Bank Account", mData.getString(BANK_ACCOUNT),
                getKey(), -1));


        dest.add(new ReviewItem("Your IFSCCode", mData.getString(IFSCCODE),
                getKey(), -1));


    }

}
