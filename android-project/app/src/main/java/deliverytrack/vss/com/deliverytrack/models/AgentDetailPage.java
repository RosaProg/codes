package deliverytrack.vss.com.deliverytrack.models;

import android.content.Context;
import android.support.v4.app.Fragment;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.fragments.registration.AgentDetailsFragment;
import deliverytrack.vss.com.deliverytrack.interfaces.ModelCallbacks;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * Created by Adi-Loch on 9/12/2015.
 */
public class AgentDetailPage extends Page {


    /*Agent*/
    public static final String FULL_NAME = "fullname";
    public static final String PASSWORD = "password";
    public static final String CP_PASSWORD = "cppassword";
    public static final String MOBILE_NUMBER = "mobile_number";
    public static final String AREACUR = "areacur";
    public static final String AREAPER = "areaper";
    public static final String CURADDRESS = "curaddress";
    public static final String PERADDRESS = "peraddress";
    public static final String EMAIL = "email";
    public static final String AGENTPINCODECUR = "agentPincodecur";
    public static final String AGENTPINCODEPER = "agentPincodeper";


    /*Restauranr*/

    public static final String REST_NAME = "restName";
    public static final String ADDRESS_REST = "address_rest";
    public static final String REST_EMAIL = "email_rest";
    public static final String PINCODE = "pincode";
    public static final String RESTAREA = "restarea";


    protected AgentDetailPage(ModelCallbacks callbacks, String title) {
        super(callbacks, title);
    }

    @Override
    public Fragment createFragment() {
        return AgentDetailsFragment.create(getKey());
    }

    @Override
    public void getReviewItems(ArrayList<ReviewItem> dest, Context activity) {

        if (DeliveryTrackUtils.getUserType().equals("Agent")) {
            dest.add(new ReviewItem("Your Full Name", mData.getString(FULL_NAME),
                    getKey(), -1));
            dest.add(new ReviewItem("Your Mobile Number", mData.getString(MOBILE_NUMBER),
                    getKey(), -1));
            dest.add(new ReviewItem("Your Current Area", mData.getString(AREACUR),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Permanent Area", mData.getString(AREACUR),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Current Address", mData.getString(CURADDRESS),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Permanent Address", mData.getString(PERADDRESS),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Current Pincode", mData.getString(AGENTPINCODECUR),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Permanent Pincode", mData.getString(AGENTPINCODEPER),
                    getKey(), -1));


            dest.add(new ReviewItem("Your Email", mData.getString(EMAIL), getKey(), -1));

        } else {

            dest.add(new ReviewItem("Your Restaurant Name", mData.getString(REST_NAME),
                    getKey(), -1));
            dest.add(new ReviewItem("Your Restaurant Address", mData.getString(ADDRESS_REST),
                    getKey(), -1));
            dest.add(new ReviewItem("Your Restaurant EmailId", mData.getString(REST_EMAIL),
                    getKey(), -1));

            dest.add(new ReviewItem("Your Area", mData.getString(RESTAREA),
                    getKey(), -1));


            dest.add(new ReviewItem("Your Pincode", mData.getString(PINCODE),
                    getKey(), -1));


        }
    }

}
