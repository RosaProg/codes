package deliverytrack.vss.com.deliverytrack.interfaces;

import com.parse.ParseObject;
import deliverytrack.vss.com.deliverytrack.models.Area;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.models.Order;

/**
 * Created by Johnn Charles on 3/25/2016.
 */
public interface AreasCallbacks {

    public void getAreas(List<ParseObject> parseObjects, int tableId);
    public void onAreaAccepted(Area area);

}
