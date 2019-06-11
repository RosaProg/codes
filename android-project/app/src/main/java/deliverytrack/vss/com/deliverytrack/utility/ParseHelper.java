package deliverytrack.vss.com.deliverytrack.utility;


import com.parse.ParseGeoPoint;
import com.parse.ParseObject;

import java.util.ArrayList;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.models.OrderTransaction;

/**
 * Created by Adi-Loch on 6/26/2015.
 */
public class ParseHelper {

    public ParseHelper() {

    }


    public ArrayList<Order> getOrdersVisible(List<ParseObject> parseObjects) {

        ArrayList<Order> orders = new ArrayList<>();
        for (ParseObject parseObject : parseObjects) {
            ParseObject parseOrder = ParseObject.create("Order");
            parseOrder = parseObject.getParseObject("Order");

            Order order = new Order();

            order.setObjectId(parseOrder.getObjectId());

            order.setName(parseOrder.get("name").toString());
            order.setOrder(parseOrder.get("order").toString());
            order.setSelfieURL(parseOrder.get("selfieUrl") != null ? parseOrder.get("selfieUrl").toString() : "");

            Object amount = parseOrder.get("amount");
            if (amount instanceof Double) {
                order.setAmount((Double) parseOrder.get("amount"));

            } else if (amount instanceof Integer) {
                order.setAmount(((Integer) amount).doubleValue());
            }

            order.setAddress(parseOrder.get("address").toString());
            order.setStatus(parseOrder.get("status").toString());
            order.setRestId(parseOrder.get("resturantid").toString());
            order.setAgentId(parseOrder.get("agentid") != null ? parseOrder.get("agentid").toString() : "");
            order.setLocation((ParseGeoPoint) parseOrder.get("location"));
            order.setCreatedAt(parseOrder.getCreatedAt());
            order.setArea(parseOrder.get("area") != null ? parseOrder.get("area").toString() : "");
            order.setUpdatedAt(parseOrder.getUpdatedAt());
            order.setRestName(parseOrder.get("restName") != null ? parseOrder.get("restName").toString() : "");
            order.setPinCode(parseOrder.get("pincode") != null ? parseOrder.get("pincode").toString() : "");
            order.setCity(parseOrder.get("city") != null ? parseOrder.get("city").toString() : "");
            order.setCustomerContactNo(parseOrder.get("customerNumber").toString());
            order.setAgentContactNo(parseOrder.get("AgentContactNo") != null ? parseOrder.get("AgentContactNo").toString() : "");
            order.setRestContactNo(parseOrder.get("restContactNo") != null ? parseOrder.get("restContactNo").toString() : "");
            order.setAgentName(parseOrder.get("agentName") != null ? parseOrder.get("agentName").toString() : "");
            order.setPaymentMode(parseOrder.get("PaymentMode") != null ? parseOrder.get("PaymentMode").toString() : "");
            orders.add(order);

        }

        return orders;
    }

    public ArrayList<Order> getOrdersVisibleDelivered(List<ParseObject> parseObjects) {

        ArrayList<Order> orders = new ArrayList<>();
        for (ParseObject parseOrder : parseObjects) {
            Order order = new Order();
            order.setObjectId(parseOrder.getObjectId());
            order.setName(parseOrder.get("name").toString());
            order.setOrder(parseOrder.get("order").toString());
            order.setArea(parseOrder.get("area") != null ? parseOrder.get("area").toString() : "");
            Object amount = parseOrder.get("amount");
            if (amount instanceof Double) {
                order.setAmount((Double) parseOrder.get("amount"));

            } else if (amount instanceof Integer) {
                order.setAmount(((Integer) amount).doubleValue());
            }

            order.setSelfieURL(parseOrder.get("selfieUrl") != null ? parseOrder.get("selfieUrl").toString() : "");
            order.setAddress(parseOrder.get("address").toString());
            order.setStatus(parseOrder.get("status").toString());
            order.setRestId(parseOrder.get("resturantid").toString());
            order.setAgentId(parseOrder.get("agentid") != null ? parseOrder.get("agentid").toString() : "");
            order.setLocation((ParseGeoPoint) parseOrder.get("location"));
            order.setCreatedAt(parseOrder.getCreatedAt());
            order.setCity(parseOrder.get("city") != null ? parseOrder.get("city").toString() : "");
            order.setUpdatedAt(parseOrder.getUpdatedAt());
            order.setRestName(parseOrder.get("restName") != null ? parseOrder.get("restName").toString() : "");
            order.setPinCode(parseOrder.get("pincode") != null ? parseOrder.get("pincode").toString() : "");
            order.setCity(parseOrder.get("city") != null ? parseOrder.get("city").toString() : "");
            order.setCustomerContactNo(parseOrder.get("customerNumber").toString());
            order.setAgentContactNo(parseOrder.get("AgentContactNo") != null ? parseOrder.get("AgentContactNo").toString() : "");
            order.setRestContactNo(parseOrder.get("restContactNo") != null ? parseOrder.get("restContactNo").toString() : "");
            order.setAgentName(parseOrder.get("agentName") != null ? parseOrder.get("agentName").toString() : "");
            order.setPaymentMode(parseOrder.get("PaymentMode") != null ? parseOrder.get("PaymentMode").toString() : "");
            orders.add(order);
        }

        return orders;
    }


    public ParseObject saveAsOrder(Order order) {

        final ParseObject orderPending = new ParseObject("order");
        orderPending.put("name", order.getName());
        orderPending.put("order", order.getOrder());
        orderPending.put("address", order.getAddress());
        orderPending.put("city", order.getCity());
        orderPending.put("amount", order.getAmount());
        orderPending.put("area", order.getArea());
        orderPending.put("pincode", order.getPinCode());
        orderPending.put("status", Constants.ORDER_PENDING);
        orderPending.put("location", order.getLocation());
        orderPending.put("resturantid", order.getRestId());
        orderPending.put("restName", order.getRestName());
        orderPending.put("customerNumber", order.getCustomerContactNo());
        orderPending.put("restContactNo", order.getRestContactNo());
        orderPending.put("PaymentMode", order.getPaymentMode());

        return orderPending;

    }

    public ParseObject saveOrderTransaction(OrderTransaction orderTransaction) {
        final ParseObject parseObject = new ParseObject("OrderTransaction");
        parseObject.put("orderId", orderTransaction.getOrderId());
        parseObject.put("orderAmount", orderTransaction.getOrderAmount());
        parseObject.put("orderCommission", orderTransaction.getOrderCommission());
        parseObject.put("surchargesComm", orderTransaction.getSurcharges());
        parseObject.put("baseCommission", orderTransaction.getOrderCommission());
        parseObject.put("credit", orderTransaction.getCredit());
        parseObject.put("debit", orderTransaction.getDebit());
        parseObject.put("userId", orderTransaction.getUserId());
        parseObject.put("typeoftax", orderTransaction.getTypeofTax());
        parseObject.put("taxdeducted", orderTransaction.getTaxDeducted());
        parseObject.put("restName", orderTransaction.getRestName());
        parseObject.put("paymentMode", orderTransaction.getPaymentMode());
        parseObject.put("location", orderTransaction.getLocation());

        return parseObject;
    }


    public static OrderTransaction getOrderTransaction(ParseObject parseObjects) {
        OrderTransaction orderTransaction = new OrderTransaction();
        orderTransaction.setOrderId(parseObjects.get("orderId").toString());

        Object orderAmount = parseObjects.get("orderAmount");
        if (orderAmount instanceof Double) {
            orderTransaction.setOrderAmount((Double) parseObjects.get("orderAmount"));
        } else if (orderAmount instanceof Integer) {
            orderTransaction.setOrderAmount(((Integer) orderAmount).doubleValue());
        }

        Object orderCommission = parseObjects.get("orderCommission");
        if (orderCommission instanceof Double) {
            orderTransaction.setOrderCommission((Double) parseObjects.get("orderCommission"));
        } else if (orderCommission instanceof Integer) {
            orderTransaction.setOrderCommission(((Integer) orderCommission).doubleValue());
        }

        Object surchargesComm = parseObjects.get("surchargesComm");
        if (surchargesComm instanceof Double) {
            orderTransaction.setSurcharges((Double) parseObjects.get("surchargesComm"));
        } else if (surchargesComm instanceof Integer) {
            orderTransaction.setSurcharges(((Integer) surchargesComm).doubleValue());
        }

        Object baseCommission = parseObjects.get("baseCommission");
        if (baseCommission instanceof Double) {
            orderTransaction.setOrderCommission((Double) parseObjects.get("baseCommission"));
        } else if (baseCommission instanceof Integer) {
            orderTransaction.setOrderCommission(((Integer) baseCommission).doubleValue());
        }

        Object credit = parseObjects.get("credit");
        if (credit instanceof Double) {
            orderTransaction.setCredit((Double) parseObjects.get("credit"));
        } else if (credit instanceof Integer) {
            orderTransaction.setCredit(((Integer) credit).doubleValue());
        }

        Object debit = parseObjects.get("debit");
        if (debit instanceof Double) {
            orderTransaction.setDebit((Double) parseObjects.get("debit"));

        } else if (debit instanceof Integer) {
            orderTransaction.setDebit(((Integer) debit).doubleValue());
        }
        orderTransaction.setUserId(parseObjects.get("userId").toString());
        orderTransaction.setCreatedAt(parseObjects.getCreatedAt());
        orderTransaction.setPaymentMode(parseObjects.get("paymentMode") != null ? parseObjects.get("paymentMode").toString() : " ");
        orderTransaction.setLocation(parseObjects.get("location") != null ? parseObjects.get("location").toString() : " ");
        orderTransaction.setRestName(parseObjects.get("restName") != null ? parseObjects.get("restName").toString() : " ");

        return orderTransaction;
    }


    public static ParseObject saveAccountSummary(double amountCredit, double amountDebit, double total, String userId) {
        ParseObject accountSummary = new ParseObject("AccountSummary");
        accountSummary.put("AmountDebit", amountDebit);
        accountSummary.put("AmountCredit", amountCredit);
        accountSummary.put("TotalAmount", total);
        accountSummary.put("userId", userId);
        return accountSummary;
    }


}
