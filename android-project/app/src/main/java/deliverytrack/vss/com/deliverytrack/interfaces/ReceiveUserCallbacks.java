package deliverytrack.vss.com.deliverytrack.interfaces;


import com.parse.ParseUser;
import java.util.List;

/**
 * Created by Adi-Loch on 8/1/2015.
 */
public interface ReceiveUserCallbacks {

    public void getUser(List<ParseUser> parseUsers, int tableId);


}
