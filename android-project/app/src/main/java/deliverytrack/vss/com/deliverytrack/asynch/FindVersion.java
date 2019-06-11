package deliverytrack.vss.com.deliverytrack.asynch;

import android.app.Activity;
import android.app.ProgressDialog;
import android.os.AsyncTask;
import android.os.Build;
import android.support.v4.app.Fragment;

import com.parse.ParseObject;
import com.parse.ParseQuery;

import java.util.List;

import deliverytrack.vss.com.deliverytrack.interfaces.VersionCodeCallback;


/**
 * Created by Adi-Loch on 12/14/2015.
 */
public class FindVersion extends AsyncTask<List<ParseObject>, Void, List<ParseObject>> {


    private final ParseQuery<ParseObject> mQuery;
    private final int tableId;
    private final Fragment mFrgament;
    private Activity mActivity;
    List<ParseObject> ob;
    ProgressDialog mProgressDialog;


    @Override
    protected List<ParseObject> doInBackground(List<ParseObject>... params) {
        try {
            ob = this.mQuery.find();
        } catch (com.parse.ParseException e) {
            e.printStackTrace();
        }
        return ob;
    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();
        mProgressDialog = new ProgressDialog(this.mActivity);
        mProgressDialog.setTitle("Retrieving Data");
        mProgressDialog.setMessage("Loading...");
        mProgressDialog.setIndeterminate(false);
        mProgressDialog.setCanceledOnTouchOutside(false);
        mProgressDialog.setCancelable(false);
        mProgressDialog.show();
    }

    @Override
    protected void onPostExecute(List<ParseObject> ob) {
        super.onPostExecute(ob);

        /*Added exception listeners for postexecute dialogs*/
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1) {
            if (this.mActivity != null && !this.mActivity.isDestroyed() && this.mProgressDialog != null && this.mProgressDialog.isShowing()) {
                mProgressDialog.dismiss();
            }
        } else {
            if (this.mActivity != null && this.mProgressDialog != null && this.mProgressDialog.isShowing()) {
                mProgressDialog.dismiss();
            }

        }
        if (ob != null) {

            ParseObject object = ob.get(0);
            int versionCode = (Integer) object.get("VersionCode");
            if (this.mFrgament == null) {

                ((VersionCodeCallback) this.mActivity).getVersionCode(versionCode);
            } else {
                ((VersionCodeCallback) this.mFrgament).getVersionCode(versionCode);

            }
        }

    }

    public FindVersion(ParseQuery<ParseObject> query, Activity activity, Fragment fragment, int tableId) {
        this.mQuery = query;
        this.mActivity = activity;
        this.tableId = tableId;
        this.mFrgament = fragment;
    }

}
