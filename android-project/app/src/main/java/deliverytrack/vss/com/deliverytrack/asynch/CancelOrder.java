package deliverytrack.vss.com.deliverytrack.asynch;

import android.app.Activity;
import android.app.ProgressDialog;
import android.os.AsyncTask;
import android.support.v4.app.Fragment;
import android.util.Log;

import com.parse.GetCallback;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;

import java.util.List;

import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Order;

/**
 * Created by Adi-Loch on 1/2/2016.
 */
public class CancelOrder extends AsyncTask<Void, Void, Boolean> {

    private Fragment fragment;
    private Order order;
    private String agentType;
    ProgressDialog mProgressDialog;


    public CancelOrder(Fragment fragment, String agentType, Order order) {
        this.agentType = agentType;
        this.order = order;
        this.fragment = fragment;
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
    protected void onPostExecute(Boolean aBoolean) {
        super.onPostExecute(aBoolean);
        mProgressDialog.dismiss();

        ((OrderCallbacks)this.fragment).onOrderCanceled();


    }

    @Override
    protected Boolean doInBackground(Void... params) {

        ParseQuery<ParseObject> query = ParseQuery.getQuery("order");
        query.whereEqualTo("objectId", this.order.getObjectId());
        if ((!this.order.getStatus().equals(Constants.ORDER_PICKEDUP)) && this.agentType.equals("Agent")) {
            try {
                List<ParseObject> objects = query.find();
                if (objects != null && objects.size() > 0) {
                    ParseQuery queryVisible = ParseQuery.getQuery("OrderVisible");
                    queryVisible.whereEqualTo("orderId", this.order.getObjectId());

                    List<ParseObject> visibleObjects = queryVisible.find();
                    if (visibleObjects != null && visibleObjects.size() > 0) {
                        visibleObjects.get(0).delete();
                    }

                    ParseObject object = objects.get(0);
                    object.put("status", Constants.ORDER_PENDING);
                    object.put("agentid", " ");
                    object.put("agentName", " ");
                    object.save();
                    return true;

                }
            } catch (ParseException e) {
                e.printStackTrace();
            }

        } else if ((!this.order.getStatus().equals(Constants.ORDER_PICKEDUP)) && this.agentType.equals("Restaurant")) {
            try {
                List<ParseObject> objects = query.find();
                if (objects != null && objects.size() > 0) {
                    ParseObject object = objects.get(0);
                    object.put("status", Constants.ORDER_CANCELLED);
                    object.save();
                    return true;
                }
            } catch (ParseException e) {
                e.printStackTrace();
            }

        }
        return false;

    }
}
