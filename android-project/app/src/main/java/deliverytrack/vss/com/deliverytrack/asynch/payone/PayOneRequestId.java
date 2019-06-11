package deliverytrack.vss.com.deliverytrack.asynch.payone;

import android.app.ProgressDialog;
import android.os.AsyncTask;
import android.support.v4.app.Fragment;
import android.util.Log;

import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.util.List;

/**
 * Created by Adi-Loch on 1/9/2016.
 */
public class PayOneRequestId extends AsyncTask<Void, Void, String> {
    private ProgressDialog mProgressDialog;
    private Fragment fragment;
    private String requestId;

    public PayOneRequestId(Fragment fragment, String requestId) {
        this.fragment = fragment;
        this.requestId = requestId;
    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();
        mProgressDialog = new ProgressDialog(this.fragment.getActivity());
        mProgressDialog.setTitle("Retrieving Data");
        mProgressDialog.setMessage("Loading...");
        mProgressDialog.setIndeterminate(false);
        mProgressDialog.setCanceledOnTouchOutside(false);
        mProgressDialog.setCancelable(false);
        mProgressDialog.show();

    }

    @Override
    protected void onPostExecute(String s) {
        super.onPostExecute(s);
        mProgressDialog.dismiss();

    }

    @Override
    protected String doInBackground(Void... params) {

        Log.d("The user", ParseUser.getCurrentUser().getUsername());
        ParseQuery payonerequest = new ParseQuery("PayOneRequest");
        payonerequest.whereEqualTo("mobileNumber", ParseUser.getCurrentUser().getUsername());
        try {
            List<ParseObject> parseObjects = payonerequest.find();
            ParseObject object = parseObjects.get(0);
            object.put("requestId", this.requestId);
            object.save();
        } catch (ParseException e) {
            e.printStackTrace();
        }

        return null;
    }
}
