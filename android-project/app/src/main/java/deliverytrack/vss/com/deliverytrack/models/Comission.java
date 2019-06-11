package deliverytrack.vss.com.deliverytrack.models;

/**
 * Created by Adi-Loch on 12/25/2015.
 */
public class Comission {

    public double getCommission() {
        return commission;
    }

    public void setCommission(double commission) {
        this.commission = commission;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getUserType() {
        return userType;
    }

    public void setUserType(String userType) {
        this.userType = userType;
    }

    double commission;
    String userId;
    String userType;
}
