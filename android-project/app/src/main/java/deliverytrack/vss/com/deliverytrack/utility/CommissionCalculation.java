package deliverytrack.vss.com.deliverytrack.utility;

import android.util.Log;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;


import deliverytrack.vss.com.deliverytrack.models.Comission;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.models.OrderTransaction;
import deliverytrack.vss.com.deliverytrack.models.Tax;
import deliverytrack.vss.com.deliverytrack.models.TimeSlot;

/**
 * Created by Adi-Loch on 12/25/2015.
 */
public class CommissionCalculation {


    private final String userId;
    private final Order order;

    public ArrayList<String> getSurs() {
        return surs;
    }

    public void setSurs(ArrayList<String> surs) {
        this.surs = surs;
    }

    private ArrayList<String> surs;

    public TimeSlot getTimeSlot() {
        return timeSlot;
    }

    public void setTimeSlot(TimeSlot timeSlot) {
        Log.d("Setting timeslot", timeSlot + "timeslot");
        this.timeSlot = timeSlot;
    }

    private TimeSlot timeSlot;

    public Comission getComission() {
        return comission;
    }

    public void setComission(Comission comission) {
        this.comission = comission;
    }

    private Comission comission;

    public Tax getTax() {
        return tax;
    }

    public void setTax(Tax tax) {
        this.tax = tax;
    }

    private Tax tax;

    public double getTotalAmount() {
        return totalAmount;
    }

    private double totalAmount;

    public CommissionCalculation(String userId, Order order) {
        this.userId = userId;
        this.order = order;
    }


    private double getBaseCommissionCalculation() {
        double amount = this.order.getAmount();
        double comission = this.comission.getCommission();
        return (amount * (comission / 100));
    }


    public OrderTransaction getCalculatedCommissionsForUser() {

        double baseCommission = getBaseCommissionCalculation();
     //   Log.d("base commission", baseCommission + " ");
        double surCharges = getCalculatedSurcharges();
        //Log.d("calculated surcharges", surCharges + " ");
        totalAmount = baseCommission + surCharges;
       // Log.d("total credit", totalAmount + " ");

        if (totalAmount > baseCommission * 2) {
            totalAmount = baseCommission * 2;
        }

        Log.d("Actual credit", totalAmount + "");

        OrderTransaction orderTransaction = new OrderTransaction();
        orderTransaction.setOrderId(this.order.getObjectId());
        orderTransaction.setOrderAmount(this.order.getAmount());
        orderTransaction.setUserId(this.userId);
        orderTransaction.setOrderCommission(this.comission.getCommission());
        orderTransaction.setSurcharges(round(surCharges, 2));
        orderTransaction.setOrderCommission(round(baseCommission, 2));
        orderTransaction.setUserId(this.userId);
        orderTransaction.setRestName(this.order.getRestName());
        orderTransaction.setLocation(this.order.getArea());
        orderTransaction.setPaymentMode(this.order.getPaymentMode());
        if (this.comission.getUserType().equals("Agent")) {
            double taxDeducted = getTax(tax.getTds(), totalAmount);
            taxDeducted = round(taxDeducted, 2);
           // Log.d("tax deducted for agent", taxDeducted + " ");
            double credit = totalAmount - taxDeducted;
            credit = round(credit, 2);
            //Log.d("credited for agent", credit + " ");
            orderTransaction.setCredit(credit);
            orderTransaction.setDebit(this.order.getAmount());
            orderTransaction.setTaxDeducted(taxDeducted);
            orderTransaction.setTypeofTax("Tds");
        } else if (this.comission.getUserType().equals("Restaurant")) {
            double taxDeducted = getTax(tax.getServiceTax(), totalAmount);
            taxDeducted = round(taxDeducted, 2);
            //Log.d("tax deducted for rest", taxDeducted + " ");
            double credit = totalAmount + taxDeducted;
            credit = round(credit, 2);
            //Log.d("credited for rest", credit + " ");
            orderTransaction.setCredit(this.order.getAmount());
            orderTransaction.setDebit(round(credit, 2));
            orderTransaction.setTaxDeducted(taxDeducted);
            orderTransaction.setTypeofTax("ServiceTax");

        }
        return orderTransaction;
    }

    private double getTax(double serviceTax, double totalAmount) {
        double taxDeducted = (totalAmount * (serviceTax / 100));
        return round(taxDeducted, 2);

    }


    public double getCalculatedSurcharges() {

        double baseComAmount = getBaseCommissionCalculation();

        double totalSurcharge = 0;
        double totalCharges = 0;
        if (this.getSurs() != null && this.getSurs().size() > 0) {
            for (String sur : this.getSurs()) {
                double commsur = Double.parseDouble(sur);
              //  Log.d("the sur1 commission", commsur + " ");
                totalSurcharge = ((commsur / 100) * baseComAmount);
            }

        }


        if (this.getTimeSlot() != null) {
            ArrayList<String> timestart = this.getTimeSlot().getTimeSlotStart();
            ArrayList<String> timestop = this.getTimeSlot().getTimeSlotEnd();
            ArrayList<String> timeamt = this.getTimeSlot().getTimeSlotAmt();

            //  Log.d("the time", timestart.size() + " ");

            String amt = null;
            for (int i = 0; i < timestart.size(); i++) {
                String timeSlot1 = timestart.get(i);
                Date time1 = DeliveryTrackUtils.getTime(getTimeDate(timeSlot1));

                String timeSlot2 = timestop.get(i);
                Date time2 = DeliveryTrackUtils.getTime(getTimeDate(timeSlot2));

                String time = this.order.getDeliveryTime();
                Date orderDate = DeliveryTrackUtils.getTime(time);
                Log.d("the time interval", timeSlot1 + " " + timeSlot2 + " " + time + "" + time1.toString() + " " + time2.toString());
                Log.d("The date" ,"orderDate: " + orderDate.toString() + "time: " +time1.toString() + "time2 :" + time2.toString() );
                if (orderDate.after(time1) && orderDate.before(time2)) {
                    amt = timeamt.get(i);
                    Log.d("time commission", amt + " ");
                }

            }
            if (amt != null) {
                double timecharg = Double.parseDouble(amt);
                totalCharges = ((timecharg / 100) * baseComAmount);
            }


        }

        Log.d("the timeSlotCharges", totalCharges + " ");
        Log.d("the totalSurcharge", totalSurcharge + " ");

        return totalCharges + totalSurcharge;
    }

    public double round(double value, int places) {
        if (places < 0) throw new IllegalArgumentException();

        long factor = (long) Math.pow(10, places);
        value = value * factor;
        long tmp = Math.round(value);
        return (double) tmp / factor;
    }

    private String getMeridian(String str) {

        int hours;
        int minutes;
        String meridian;

        String[] ar = new String[2];
        ar = str.split(":");
        hours = Integer.parseInt(ar[0]);
        minutes = Integer.parseInt(ar[1].substring(0, 2));
        meridian = ar[1].substring(2);

        return meridian;
    }

    private String getTimeDate(String str) {

        int hours;
        int minutes;
        String meridian;

        String[] ar = new String[2];
        ar = str.split(":");
        hours = Integer.parseInt(ar[0]);
        minutes = Integer.parseInt(ar[1].substring(0, 2));
        meridian = ar[1].substring(2);

        Log.d("ar", " " + ar + " " + hours + " " + minutes + " " + meridian + " " + str);



        Calendar cal = Calendar.getInstance();
        cal.set(Calendar.HOUR_OF_DAY, hours);
        cal.set(Calendar.MINUTE, minutes);
        if (meridian.equalsIgnoreCase("am")) {
            cal.set(Calendar.AM_PM, Calendar.AM);
        } else if (meridian.equalsIgnoreCase("pm")) {
            cal.set(Calendar.AM_PM, Calendar.PM);
        }

       // Log.d("test", cal.getTime().toString() + cal.get(Calendar.HOUR) + cal.get(Calendar.MINUTE));

        //DateFormat readFormat = new SimpleDateFormat("hh:mm aa");
        return hours + ":" + minutes + " " + meridian;
//        DateFormat readFormat = new SimpleDateFormat("hh:mm aa");
//        return readFormat.format(cal.getTime());


    }


}
