package deliverytrack.vss.com.deliverytrack.models;

import android.util.Log;

import java.util.ArrayList;

/**
 * Created by Adi-Loch on 12/25/2015.
 */
public class CommissionData {

    private Order order;

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    private String userId;

    public Comission getCommission() {
        return commission;
    }

    public void setCommission(Comission commission) {
        this.commission = commission;
    }



    public boolean isHoliday() {
        return isHoliday;
    }

    public void setIsHoliday(boolean isHoliday) {
        this.isHoliday = isHoliday;
    }

    Comission commission;

    public ArrayList<String> getSurComm() {
        return surComm;
    }

    public void setSurComm(ArrayList<String> surComm) {
        this.surComm = surComm;
    }

    ArrayList<String> surComm;
    boolean isHoliday;

    public Tax getTax() {
        return tax;
    }

    public void setTax(Tax tax) {
        this.tax = tax;
    }

    Tax tax;

    public TimeSlot getTimeSlot() {
        return timeSlot;
    }

    public void setTimeSlot(TimeSlot timeSlot) {

        Log.d("Commission Data", timeSlot + "time");
        this.timeSlot = timeSlot;
    }

    TimeSlot timeSlot;

    public void setOrder(Order order) {
        this.order = order;
    }

    public Order getOrder() {
        return order;
    }
}
