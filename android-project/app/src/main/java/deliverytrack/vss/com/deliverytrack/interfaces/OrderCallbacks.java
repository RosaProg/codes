package deliverytrack.vss.com.deliverytrack.interfaces;

import com.parse.ParseObject;

import java.util.List;

import deliverytrack.vss.com.deliverytrack.models.Order;

/**
 * Created by Adi-Loch on 7/30/2015.
 */
public interface OrderCallbacks {


    public void getParseObjects(List<ParseObject> parseObjects, int tableId);

    public void restParseObjects(Order order);

    public void onOrderSaveSuccess();

    public void onOrderSaveFailure();

    public void onOrderCanceled();

}
