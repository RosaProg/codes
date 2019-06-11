package deliverytrack.vss.com.deliverytrack.models;

import java.util.Date;

/**
 * Created by Adi-Loch on 6/27/2015.
 */
public class TransactionModel {


    public Date getDateCreated() {
        return dateCreated;
    }

    public void setDateCreated(Date dateCreated) {
        this.dateCreated = dateCreated;
    }

    Date dateCreated;

    public double getUserCredit() {
        return userCredit;
    }

    public void setUserCredit(double userCredit) {
        this.userCredit = userCredit;
    }

    public double getUserDebit() {
        return userDebit;
    }

    public void setUserDebit(double userDebit) {
        this.userDebit = userDebit;
    }

    public String getOrderName() {
        return orderName;
    }

    public void setOrderName(String orderName) {
        this.orderName = orderName;
    }

    public String getRestId() {
        return restId;
    }

    public void setRestId(String restId) {
        this.restId = restId;
    }

    double userCredit;
    double userDebit;
    String orderName;
    String restId;
}
