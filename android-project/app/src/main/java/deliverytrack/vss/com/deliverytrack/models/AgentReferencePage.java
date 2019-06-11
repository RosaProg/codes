package deliverytrack.vss.com.deliverytrack.models;

import android.content.Context;
import android.support.v4.app.Fragment;
import android.text.TextUtils;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.fragments.registration.AgentRefDetailsFragment;
import deliverytrack.vss.com.deliverytrack.interfaces.ModelCallbacks;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * Created by Adi-Loch on 9/12/2015.
 */
public class AgentReferencePage extends Page {
    public static final String REFERENCE1 = "ref1";
    public static final String RELATIONSHIP1 = "rela1";
    public static final String MOB1 = "mob1";
    public static final String REFERENCE2 = "ref2";
    public static final String RELATIONSHIP2 = "rela2";
    public static final String MOB2 = "mob";


    public static final String MANAGER_NAME = "managername";
    public static final String MOBILE_NUMBERREST = "mobilenumberrest";


    protected AgentReferencePage(ModelCallbacks callbacks, String title) {
        super(callbacks, title);
    }

    @Override
    public Fragment createFragment() {
        return AgentRefDetailsFragment.create(getKey());
    }

    @Override
    public void getReviewItems(ArrayList<ReviewItem> dest, Context activity) {

        if (DeliveryTrackUtils.getUserType().equals("Agent")) {

            dest.add(new ReviewItem("Your Reference Name", mData.getString(REFERENCE1),
                    getKey(), -1));


            dest.add(new ReviewItem("Your Relationship with reference", mData.getString(RELATIONSHIP1),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Reference Mobile Number", mData.getString(MOB1),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Reference Name", mData.getString(REFERENCE2),
                    getKey(), -1));


            dest.add(new ReviewItem("Your Relationship with reference", mData.getString(RELATIONSHIP2),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Reference Mobile Number", mData.getString(MOB2),
                    getKey(), -1));
        } else {
            dest.add(new ReviewItem("Your  Manager Name", mData.getString(MANAGER_NAME),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Restaurant Mobile", mData.getString(MOBILE_NUMBERREST),
                    getKey(), -1));
        }

    }


}
