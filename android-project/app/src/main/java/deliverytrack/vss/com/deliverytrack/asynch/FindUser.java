package deliverytrack.vss.com.deliverytrack.asynch;

import android.app.Activity;
import android.app.ProgressDialog;
import android.os.AsyncTask;
import android.support.v4.app.Fragment;
import android.util.Log;

import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.util.List;

import deliverytrack.vss.com.deliverytrack.interfaces.ReceiveUserCallbacks;

/**
 * Created by Adi-Loch on 8/1/2015.
 */
public class FindUser extends AsyncTask<List<ParseUser>, Void, List<ParseUser>> {

    private final ParseQuery<ParseUser> mQuery;
    private final int tableId;
    private final Fragment mFrgament;
    private Activity mActivity;
    List<ParseUser> ob;
    ProgressDialog mProgressDialog;


    @Override
    protected List<ParseUser> doInBackground(List<ParseUser>... params) {
        try {
            ob = this.mQuery.find();
        } catch (com.parse.ParseException e) {
            e.printStackTrace();
            Log.d("excep", e.toString() + " ");
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
        mProgressDialog.setCancelable(false);
        mProgressDialog.setCanceledOnTouchOutside(false);
        mProgressDialog.show();
    }

    @Override
    protected void onPostExecute(List<ParseUser> ob) {
        super.onPostExecute(ob);
        mProgressDialog.dismiss();
        if (ob != null) {
            if (this.mFrgament == null) {
                ((ReceiveUserCallbacks) this.mActivity).getUser(ob, tableId);
            } else {
                ((ReceiveUserCallbacks) this.mFrgament).getUser(ob, tableId);

            }
        }


    }


    public FindUser(ParseQuery<ParseUser> query, Activity activity, Fragment fragment, int tableId) {
        this.mQuery = query;
        this.mActivity = activity;
        this.tableId = tableId;
        this.mFrgament = fragment;
    }

}
