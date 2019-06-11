package deliverytrack.vss.com.deliverytrack.models;

import java.util.Date;

/**
 * Created by Adi-Loch on 12/25/2015.
 */
public class OrderTransaction {


    public String getRestName() {
        return restName;
    }

    public void setRestName(String restName) {
        this.restName = restName;
    }

    public String getPaymentMode() {
        return paymentMode;
    }

    public void setPaymentMode(String paymentMode) {
        this.paymentMode = paymentMode;
    }

    public String getLocation() {
        return location;
    }

    public void setLocation(String location) {
        this.location = location;
    }

    String restName;

    String paymentMode;

    String location;

    public Date getCreatedAt() {
        return createdAt;
    }

    public void setCreatedAt(Date createdAt) {
        this.createdAt = createdAt;
    }

    Date createdAt;

    public String getOrderId() {
        return orderId;
    }

    public void setOrderId(String orderId) {
        this.orderId = orderId;
    }

    public double getOrderAmount() {
        return orderAmount;
    }

    public void setOrderAmount(double orderAmount) {
        this.orderAmount = orderAmount;
    }

    public double getOrderCommission() {
        return orderCommission;
    }

    public void setOrderCommission(double orderCommission) {
        this.orderCommission = orderCommission;
    }

    public double getSurcharges() {
        return surcharges;
    }

    public void setSurcharges(double surcharges) {
        this.surcharges = surcharges;
    }

    public double getCredit() {
        return credit;
    }

    public void setCredit(double credit) {
        this.credit = credit;
    }

    public double getDebit() {
        return debit;
    }

    public void setDebit(double debit) {
        this.debit = debit;
    }

    public double getDeductionfromTotal() {
        return deductionfromTotal;
    }

    public void setDeductionfromTotal(float deductionfromTotal) {
        this.deductionfromTotal = deductionfromTotal;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    String orderId;
    double orderAmount;
    double orderCommission;
    double surcharges;
    double credit;
    double debit;
    double deductionfromTotal;
    String userId;

    public double getTaxDeducted() {
        return taxDeducted;
    }

    public void setTaxDeducted(double taxDeducted) {
        this.taxDeducted = taxDeducted;
    }

    public String getTypeofTax() {
        return typeofTax;
    }

    public void setTypeofTax(String typeofTax) {
        this.typeofTax = typeofTax;
    }

    double taxDeducted;
    String typeofTax;

}
