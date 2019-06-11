package deliverytrack.vss.com.deliverytrack.asynch;

import android.app.Activity;
import android.app.ProgressDialog;
import android.os.AsyncTask;
import android.support.v4.app.Fragment;
import android.util.Log;

import com.parse.ParseACL;
import com.parse.ParseCloud;
import com.parse.ParseException;
import com.parse.ParseFile;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.List;


import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Comission;
import deliverytrack.vss.com.deliverytrack.models.CommissionData;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.models.OrderTransaction;
import deliverytrack.vss.com.deliverytrack.models.SurchargeCloud;
import deliverytrack.vss.com.deliverytrack.models.Tax;
import deliverytrack.vss.com.deliverytrack.models.TimeSlot;
import deliverytrack.vss.com.deliverytrack.utility.CommissionCalculation;
import deliverytrack.vss.com.deliverytrack.utility.ParseHelper;

/**
 * Created by Adi-Loch on 7/22/2015.
 */

public class UpdateOrder extends AsyncTask<Void, Void, Void> {

    private final Activity mActivity;
    private final Order order;
    private final Fragment mfragment;
    private String status;
    private ProgressDialog mProgressDialog;

    public UpdateOrder(Activity activity, Order order, Fragment fragment, String status) {
        this.mActivity = activity;
        this.order = order;
        this.mfragment = fragment;
        this.status = status;
    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();
        mProgressDialog = new ProgressDialog(this.mActivity);
        mProgressDialog.setTitle("Saving Data");
        mProgressDialog.setMessage("Loading...");
        mProgressDialog.setIndeterminate(false);
        mProgressDialog.setCanceledOnTouchOutside(false);
        mProgressDialog.setCancelable(false);
        mProgressDialog.show();

    }

    @Override
    protected void onPostExecute(Void aVoid) {
        super.onPostExecute(aVoid);
        mProgressDialog.dismiss();
        if (this.mfragment == null) {
            ((OrderCallbacks) this.mActivity).restParseObjects(this.order);
        } else {
            ((OrderCallbacks) this.mfragment).restParseObjects(this.order);

        }
    }

    @Override
    protected Void doInBackground(Void... params) {
        if (this.order != null) {

            try {
                String objectId = this.order.getObjectId();
                ParseUser user = ParseUser.getCurrentUser();

                if (this.status.equals(Constants.ORDER_ACCEPT_PICKUP)) {
                    updateOrderPickup(objectId, user, this.status, this.order.getAgentId());
                } else if (this.status.equals(Constants.ORDER_ARRIVED_LOCATION)) {
                    ParseQuery query = new ParseQuery("order");
                    query.whereEqualTo("objectId", objectId);
                    List<ParseObject> parseObjects = null;
                    try {
                        parseObjects = query.find();
                        ParseObject updateOrder = parseObjects.get(0);
                        updateOrder.put("status", this.status);
                        updateOrder.save();
                    } catch (ParseException e) {
                        e.printStackTrace();
                    }

                } else if (this.status.equals(Constants.ORDER_PICKEDUP)) {

                    updateOrderPickup(objectId, user, this.status, this.order.getAgentId());

                } else if (this.status.equals(Constants.ORDER_DELIVERED)) {

                    DateFormat readFormat = new SimpleDateFormat("hh:mm aa");
                    ParseQuery orderQuery = new ParseQuery("order");
                    orderQuery.whereEqualTo("objectId", order.getObjectId());

                    List<ParseObject> orderList = orderQuery.find();
                    ParseObject deliveredOrder = orderList.get(0);
                    deliveredOrder.put("status", Constants.ORDER_DELIVERED);
                    deliveredOrder.put("agentid", order.getAgentId());
                    deliveredOrder.put("deliverytime", readFormat.format(new Date()));
                    deliveredOrder.save();


                    CommissionData restComm = getCommissionData(this.order.getRestId(), this.order.getCity(), this.order, "Restaurant");
                    CommissionCalculation restCommCal = new CommissionCalculation(this.order.getRestId(), this.order);
                    restCommCal.setComission(restComm.getCommission());
                    restCommCal.setTimeSlot(restComm.getTimeSlot());
                    restCommCal.setTax(restComm.getTax());
                    restCommCal.setSurs(restComm.getSurComm());
                    OrderTransaction orderTransactionRest = restCommCal.getCalculatedCommissionsForUser();


                    CommissionData agentComm = getCommissionData(this.order.getAgentId(), this.order.getCity(), this.order, "Agent");
                    CommissionCalculation agentCommCal = new CommissionCalculation(this.order.getAgentId(), this.order);
                    agentCommCal.setComission(agentComm.getCommission());
                    agentCommCal.setTimeSlot(agentComm.getTimeSlot());
                    agentCommCal.setTax(agentComm.getTax());
                    agentCommCal.setSurs(agentComm.getSurComm());

                    OrderTransaction orderTransactionAgent = agentCommCal.getCalculatedCommissionsForUser();

                    ParseACL acl = new ParseACL();
                    acl.setPublicReadAccess(true);
                    acl.setPublicWriteAccess(false);


                    ParseHelper helper1 = new ParseHelper();
                    ParseObject rest = helper1.saveOrderTransaction(orderTransactionRest);


                    ParseHelper helper = new ParseHelper();
                    ParseObject agent = helper.saveOrderTransaction(orderTransactionAgent);

                    rest.setACL(acl);
                    rest.save();

                    agent.setACL(acl);
                    agent.save();


                }

            } catch (ParseException e) {
                e.printStackTrace();
            }
        }
        return null;
    }


    private String getSelfieURL(String agentId) {
        ParseQuery query = new ParseQuery("AdhardCard");
        query.whereEqualTo("userId", agentId);
        List<ParseObject> parseObjects = null;
        try {
            parseObjects = query.find();
            if (parseObjects != null && parseObjects.size() > 0) {
                ParseObject object = parseObjects.get(0);
                ParseFile image = (ParseFile) object.get("otherDoc");
                if (image != null) {
                    return image.getUrl();
                }
            }

        } catch (ParseException e) {
            return null;
        }

        return null;
    }

    private void updateOrderPickup(String objectId, ParseUser user, String status, String agentId) {

        String selfieURL = getSelfieURL(agentId);
        ParseQuery query = new ParseQuery("order");
        query.whereEqualTo("objectId", objectId);
        List<ParseObject> parseObjects = null;
        try {
            parseObjects = query.find();
            ParseObject updateOrder = parseObjects.get(0);
            updateOrder.put("status", status);
            updateOrder.put("agentid", agentId);
            updateOrder.put("AgentContactNo", user.get("phone").toString());
            updateOrder.put("agentName", user.get("userFullName"));
            if (selfieURL != null) {
                updateOrder.put("selfieUrl", selfieURL);
            }
            updateOrder.save();
        } catch (ParseException e) {
            e.printStackTrace();
        }

    }

    private CommissionData getCommissionData(String userId, String city, Order order, String userType) {

        Date date = order.getUpdatedAt();

        Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);
        calendar.add(Calendar.DATE, 1);

        Calendar cal = Calendar.getInstance();
        cal.setTime(date);
        cal.add(Calendar.DATE, -1);

        CommissionData commissionData = new CommissionData();
        ParseQuery queryComm = new ParseQuery("Commission");
        queryComm.whereEqualTo("userObjectId", userId);

        Log.d("the ordeid", userId + "");
        try {
            List<ParseObject> parseObjects = queryComm.find();
            ParseObject objectComm = parseObjects.get(0);

            Comission comission = new Comission();
            Object comm = objectComm.get("commision");
            if (comm instanceof Double) {
                comission.setCommission((Double) objectComm.get("commision"));
            } else if (comm instanceof Integer) {
                comission.setCommission(((Integer) comm).doubleValue());
            }

            comission.setUserId(objectComm.get("userId").toString());
            comission.setUserType(objectComm.get("usertype").toString());

            Log.d("the ordeid", comission.getCommission() + " " + comission.getUserId());
            commissionData.setCommission(comission);

            ///////////////////////////////////////////////////////////////////////////////////////////////

            ParseQuery querySur = new ParseQuery("Surcharge");
            querySur.whereEqualTo("userType", userType);

            List<ParseObject> parseObject = null;
            try {
                parseObject = querySur.find();

                if (parseObject != null && parseObject.size() > 0) {
                    ParseObject object = parseObject.get(0);

                    SurchargeCloud surcharges = new SurchargeCloud();

                    ParseObject surchrageCloud = ParseObject.create("SurchageCloud");
                    surchrageCloud = object.getParseObject("surchargeCloud");

                    ParseObject timeslot = ParseObject.create("Timeslot");
                    timeslot = object.getParseObject("timeslot");

                    TimeSlot timeSlots = new TimeSlot();

                    timeSlots.setTimeSlotStart((ArrayList) timeslot.fetchIfNeeded().get("timeslotStart"));
                    timeSlots.setTimeSlotEnd((ArrayList) timeslot.fetchIfNeeded().get("timeslotEnd"));
                    timeSlots.setTimeSlotAmt((ArrayList) timeslot.fetchIfNeeded().get("timeslotamt"));

                    Log.d("the time slot", timeSlots.getTimeSlotStart().size() + " ");
                    Log.d("the time slot", timeSlots.getTimeSlotEnd().size() + " ");
                    Log.d("the time slot", timeSlots.getTimeSlotAmt().size() + " ");
                    commissionData.setTimeSlot(timeSlots);
                    surcharges.setCloudCodeComm((ArrayList) surchrageCloud.fetchIfNeeded().get("cloudCodeName"));

                    ArrayList<String> strings = new ArrayList<>();

                    if (surcharges.getCloudCodeComm() != null && surcharges.getCloudCodeComm().size() > 0) {
                        HashMap<String, Object> hashMaps = new HashMap<>();
                        hashMaps.put("date", new Date());
                        hashMaps.put("orderid", this.order.getObjectId());
                        for (String surchargeCloud : surcharges.getCloudCodeComm()) {
                            String[] array = splitString(surchargeCloud);
                            int result = ParseCloud.callFunction(array[0], hashMaps);
                            if (result > 0) {
                                strings.add(array[1]);
                            }
                        }
                    }
                    commissionData.setSurComm(strings);
                }

            } catch (ParseException e1) {

                e1.printStackTrace();
            }

            /////////////////////////////////////////////////////////////////////////////

            commissionData.setOrder(order);
            commissionData.setUserId(userId);

            ParseQuery taxquery = new ParseQuery("Tax");
            List<ParseObject> taxes = null;
            try {
                taxes = taxquery.find();

                if (taxes != null && taxes.size() > 0) {
                    ParseObject taxobject = taxes.get(0);
                    Tax tax = new Tax();
                    Object serviceTax = taxobject.get("serviceTax");
                    if (serviceTax instanceof Double) {
                        tax.setServiceTax((Double) taxobject.get("serviceTax"));

                    } else if (serviceTax instanceof Integer) {
                        tax.setServiceTax(((Integer) serviceTax).doubleValue());
                    }

                    Object tds = taxobject.get("tds");
                    if (tds instanceof Double) {
                        tax.setTds((Double) taxobject.get("tds"));

                    } else if (tds instanceof Integer) {
                        tax.setTds(((Integer) tds).doubleValue());
                    }
                    commissionData.setTax(tax);

                }
            } catch (ParseException e1) {
                e1.printStackTrace();
            }


            Log.d("the exception is", commissionData.getCommission() + " ");

            return commissionData;

        } catch (
                ParseException e
                )

        {
            Log.d("the exception is", e.toString() + " ");
        }

        return null;


    }


    private String[] splitString(String e) {

        Log.d("the split ", e + " " + e.split("\\$"));
        return e.split("\\$");
    }
}


