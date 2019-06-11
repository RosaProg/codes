package deliverytrack.vss.com.deliverytrack.asynch;

import android.os.AsyncTask;

/**
 * Created by Adi-Loch on 11/29/2015.
 */

import android.app.Activity;
import android.app.ProgressDialog;
import android.support.v4.app.Fragment;

import com.parse.ParseObject;
import com.parse.ParseQuery;

import java.util.List;

import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;


/**
 * Created by Adi-Loch on 6/26/2015.
 */
public class FindOrder extends AsyncTask<List<ParseObject>, Void, List<ParseObject>> {

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
        mProgressDialog.dismiss();
        if (ob != null) {
            if (this.mFrgament == null) {
                ((OrderCallbacks) this.mActivity).getParseObjects(ob, this.tableId);
            } else {
                ((OrderCallbacks) this.mFrgament).getParseObjects(ob, this.tableId);

            }
        }


    }


    public FindOrder(ParseQuery<ParseObject> query, Activity activity, Fragment fragment, int tableId) {
        this.mQuery = query;
        this.mActivity = activity;
        this.tableId = tableId;
        this.mFrgament = fragment;
    }

}


