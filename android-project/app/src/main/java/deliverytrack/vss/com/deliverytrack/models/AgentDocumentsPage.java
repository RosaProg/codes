package deliverytrack.vss.com.deliverytrack.models;

import android.content.Context;
import android.support.v4.app.Fragment;
import android.text.TextUtils;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.fragments.registration.AgentDocumentsFragment;
import deliverytrack.vss.com.deliverytrack.interfaces.ModelCallbacks;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * Created by Adi-Loch on 9/12/2015.
 */
public class AgentDocumentsPage extends Page {

    public static final String ADHAR_NO = "adhar_no";
    public static final String DOCS = "other_docs";
    public static final String ENTITY = "entity";

    protected AgentDocumentsPage(ModelCallbacks callbacks, String title) {
        super(callbacks, title);
    }

    @Override
    public Fragment createFragment() {
        return AgentDocumentsFragment.create(getKey());
    }

    @Override
    public void getReviewItems(ArrayList<ReviewItem> dest, Context activity) {

        if (DeliveryTrackUtils.getUserType().equals("Agent")) {
            dest.add(new ReviewItem("Your Adhar No", mData.getString(ADHAR_NO),
                    getKey(), -1));
            dest.add(new ReviewItem("Your Other Document", mData.getString(DOCS),
                    getKey(), -1));


        } else {
            dest.add(new ReviewItem("Your Entity", mData.getString(ENTITY),
                    getKey(), -1));

        }

    }

}
