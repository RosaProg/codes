package deliverytrack.vss.com.deliverytrack.asynch;

import android.app.Activity;
import android.app.ProgressDialog;
import android.os.AsyncTask;

import com.parse.ParseACL;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.util.List;

import deliverytrack.vss.com.deliverytrack.interfaces.CommissionCallBacks;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * Created by Adi-Loch on 1/7/2016.
 */
public class AddDefaultCommission extends AsyncTask<Void, Void, Boolean> {

    private String userType;
    private String userId;
    private Activity mActivity;
    private ProgressDialog mProgressDialog;

    public AddDefaultCommission(String userId, String userType, Activity activiy) {

        this.userId = userId;
        this.userType = userType;
        this.mActivity = activiy;

    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();
        mProgressDialog = new ProgressDialog(this.mActivity);
        mProgressDialog.setTitle("Saving Data");
        mProgressDialog.setMessage("Loading...");
        mProgressDialog.setIndeterminate(false);
        mProgressDialog.setCanceledOnTouchOutside(false);
        mProgressDialog.setCancelable(false);
        mProgressDialog.show();

    }

    @Override
    protected void onPostExecute(Boolean isSaved) {
        super.onPostExecute(isSaved);
        if (mProgressDialog.isShowing()) {
            mProgressDialog.dismiss();
            ((CommissionCallBacks) this.mActivity).isCommissionSaved(isSaved);
        }

    }

    @Override
    protected Boolean doInBackground(Void... params) {

        ParseQuery query = new ParseQuery("Commission");
        query.whereEqualTo("userObjectId", this.userId);

        try {
            List<ParseObject> parseObjects = query.find();
            if (parseObjects != null && parseObjects.size() > 0) {

                ParseObject object = parseObjects.get(0);
                if (this.userType.equals(DeliveryTrackUtils.AGENT)) {
                    object.put("commision", 0);
                } else if (this.userType.equals(DeliveryTrackUtils.RESTAURANT)) {
                    object.put("commision", 0);

                }

                object.put("userId", ParseUser.getCurrentUser().getUsername());

                ParseACL acl = new ParseACL();
                acl.setPublicReadAccess(true);
                acl.setPublicWriteAccess(true);
                object.setACL(acl);
                object.save();
            } else {

                ParseObject commissionObject = new ParseObject("Commission");
                if (this.userType.equals(DeliveryTrackUtils.RESTAURANT)) {
                    commissionObject.put("commision", 0);
                } else if (this.userType.equals(DeliveryTrackUtils.AGENT)) {
                    commissionObject.put("commision", 0);

                }
                commissionObject.put("userObjectId", this.userId);
                commissionObject.put("usertype", this.userType);
                commissionObject.put("userId", ParseUser.getCurrentUser().getUsername());

                ParseACL acl = new ParseACL();
                acl.setPublicReadAccess(true);
                acl.setPublicWriteAccess(true);
                commissionObject.setACL(acl);
                commissionObject.save();
            }
            return true;
        } catch (ParseException e) {
            e.printStackTrace();
        }
        return false;
    }
}
