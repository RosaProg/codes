package deliverytrack.vss.com.deliverytrack.asynch;

import android.app.Activity;
import android.app.ProgressDialog;
import android.os.AsyncTask;
import com.parse.ParseACL;
import com.parse.ParseException;
import com.parse.ParseObject;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;

/**
 * Created by Adi-Loch on 7/15/2015.
 */
public class SaveObject extends AsyncTask<Void, Void, Boolean> {

    private final Activity mActivity;
    private final ParseObject parseObject;
    private ProgressDialog mProgressDialog;

    public SaveObject(Activity activity, ParseObject parseObject) {
        this.mActivity = activity;
        this.parseObject = parseObject;
    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();
        if (this.mActivity != null) {
            mProgressDialog = new ProgressDialog(this.mActivity);
            mProgressDialog.setTitle("Saving Data");
            mProgressDialog.setMessage("Loading...");
            mProgressDialog.setIndeterminate(false);
            mProgressDialog.setCanceledOnTouchOutside(false);
            mProgressDialog.setCancelable(false);
            mProgressDialog.show();
        }

    }

    @Override
    protected void onPostExecute(Boolean isSuccess) {
        super.onPostExecute(isSuccess);

        if (mProgressDialog != null && mProgressDialog.isShowing()) {
            mProgressDialog.dismiss();
        }
        if (isSuccess) {
            ((OrderCallbacks) this.mActivity).onOrderSaveSuccess();
        } else {
            ((OrderCallbacks) this.mActivity).onOrderSaveFailure();
        }
    }

    @Override
    protected Boolean doInBackground(Void... params) {

        if (this.parseObject != null) {
            try {
                ParseACL acl = new ParseACL();
                acl.setPublicReadAccess(true);
                acl.setPublicWriteAccess(true);
                parseObject.setACL(acl);
                parseObject.save();
                return true;
            } catch (ParseException e) {
                e.printStackTrace();
                return false;

            }
        } else {
            return false;
        }
    }
}
