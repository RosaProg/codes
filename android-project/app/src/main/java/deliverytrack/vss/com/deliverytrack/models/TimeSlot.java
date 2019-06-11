package deliverytrack.vss.com.deliverytrack.models;

import java.util.ArrayList;

/**
 * Created by Adi-Loch on 1/9/2016.
 */
public class TimeSlot {


    public ArrayList<String> getTimeSlotStart() {
        return timeSlotStart;
    }

    public void setTimeSlotStart(ArrayList<String> timeSlotStart) {
        this.timeSlotStart = timeSlotStart;
    }

    public ArrayList<String> getTimeSlotEnd() {
        return timeSlotEnd;
    }

    public void setTimeSlotEnd(ArrayList<String> timeSlotEnd) {
        this.timeSlotEnd = timeSlotEnd;
    }

    ArrayList<String> timeSlotStart;
    ArrayList<String> timeSlotEnd;

    public ArrayList<String> getTimeSlotAmt() {
        return timeSlotAmt;
    }

    public void setTimeSlotAmt(ArrayList<String> timeSlotAmt) {
        this.timeSlotAmt = timeSlotAmt;
    }

    ArrayList<String> timeSlotAmt;
}
