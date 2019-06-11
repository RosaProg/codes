package deliverytrack.vss.com.deliverytrack.fragments.orders;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.location.Address;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import com.parse.ParseGeoPoint;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.util.List;

import deliverytrack.vss.com.deliverytrack.MapActivity;
import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.SignatureActivity;
import deliverytrack.vss.com.deliverytrack.asynch.FindOrder;
import deliverytrack.vss.com.deliverytrack.asynch.FindUser;
import deliverytrack.vss.com.deliverytrack.asynch.UpdateOrder;
import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.interfaces.ReceiveUserCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.ParseHelper;
import deliverytrack.vss.com.deliverytrack.utility.lazyloader.ImageLoader;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link OrderdetailedFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link OrderdetailedFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class OrderdetailedFragment extends Fragment implements View.OnClickListener, OrderCallbacks, ReceiveUserCallbacks {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private Order order;
    private double total;
    private TextView restAddress;
    private TextView restaName;
    private ImageLoader imageLoader;

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment OrderdetailedFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static OrderdetailedFragment newInstance(String param1, String param2) {
        OrderdetailedFragment fragment = new OrderdetailedFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public void setOrder(Order order) {
        this.order = order;
    }

    public OrderdetailedFragment() {
        // Required empty public constructor
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment

        View view = inflater.inflate(R.layout.fragment_orderdetailed, container, false);
        TextView orderName = (TextView) view.findViewById(R.id.txt_order_name);
        TextView amount = (TextView) view.findViewById(R.id.txt_amount);
        TextView location = (TextView) view.findViewById(R.id.txt_location);
        TextView agentId = (TextView) view.findViewById(R.id.txt_agentId);
        TextView agentName = (TextView) view.findViewById(R.id.txt_agentname);
        TextView status = (TextView) view.findViewById(R.id.txt_status);
        TextView restId = (TextView) view.findViewById(R.id.txt_rest_id);
        TextView agentPhone = (TextView) view.findViewById(R.id.txt_agent_phone);
        TextView restPhone = (TextView) view.findViewById(R.id.txt_rest_phone);
        TextView customerPhone = (TextView) view.findViewById(R.id.txt_cust_phone);
        TextView agentLocation = (TextView) view.findViewById(R.id.agent_location);
        TextView paymentMode = (TextView) view.findViewById(R.id.payment_mode);
        restAddress = (TextView) view.findViewById(R.id.txt_rest_address);
        restaName = (TextView) view.findViewById(R.id.txt_rest_name);
        ImageView thumb = (ImageView) view.findViewById(R.id.thumbnail);

        Button deliver = (Button) view.findViewById(R.id.deliver);
        Button pickup = (Button) view.findViewById(R.id.pickup);

        location.setOnClickListener(this);
        agentLocation.setOnClickListener(this);

        deliver.setOnClickListener(this);
        pickup.setOnClickListener(this);


        if (this.order.getSelfieURL() != null && !this.order.getSelfieURL().equals("")) {
            imageLoader = new ImageLoader(this.getActivity());
            imageLoader.DisplayImage(this.order.getSelfieURL(), thumb);
        }


        orderName.setText("OrderName: " + this.order.getName());
        amount.setText("Amount: " + String.valueOf(this.order.getAmount()));
        location.setText("Location: " + this.order.getAddress());
        restId.setText(getResources().getString(R.string.rest_id) + " : " + this.order.getRestId());
        status.setText(getResources().getString(R.string.status) + " : " + this.order.getStatus());
        agentName.setText(getResources().getString(R.string.agent_name) + " : " + this.order.getAgentName());
        agentPhone.setText(getResources().getString(R.string.agent_contact) + " : " + this.order.getAgentContactNo());
        restPhone.setText(getResources().getString(R.string.rest_contact) + " : " + this.order.getRestContactNo());
        customerPhone.setText(getResources().getString(R.string.customer_contact_no) + " : " + this.order.getCustomerContactNo());
        paymentMode.setText(getResources().getString(R.string.payment_mode) + " : " + this.order.getPaymentMode());

        agentPhone.setOnClickListener(this);
        restPhone.setOnClickListener(this);
        customerPhone.setOnClickListener(this);


        if (this.order.getStatus().equals(Constants.ORDER_PENDING)) {
            agentId.setText("No pickup has been done");
        } else if (!this.order.getStatus().equals(Constants.ORDER_PENDING)) {
            agentId.setText("AgentId :" + this.order.getAgentId());

        }

        if (DeliveryTrackUtils.isAgent()) {
            if (this.order.getStatus().equals(Constants.ORDER_PICKEDUP)) {
                pickup.setVisibility(View.GONE);
                deliver.setVisibility(View.VISIBLE);
            } else {
                pickup.setVisibility(View.GONE);
                deliver.setVisibility(View.GONE);

            }
        } else if (DeliveryTrackUtils.isRestaurentUser()) {
            pickup.setVisibility(View.GONE);
            deliver.setVisibility(View.GONE);
        }

        if (DeliveryTrackUtils.getType().equals("agentId")) {
            agentLocation.setVisibility(View.GONE);
        } else {
            agentLocation.setVisibility(View.VISIBLE);

        }

        ParseQuery<ParseUser> query = ParseUser.getQuery();
        query.whereEqualTo("objectId", order.getRestId());
        FindUser user = new FindUser(query, getActivity(), this, 333);
        user.execute();


        return view;

    }

    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }


    @Override
    public void onResume() {
        super.onResume();
        // Set title
        getActivity()
                .setTitle(getResources().getString(R.string.order_detailed_title));
    }


    @Override
    public void onAttach(Activity activity) {
        super.onAttach(activity);
        try {
            mListener = (OnFragmentInteractionListener) activity;


        } catch (ClassCastException e) {
            throw new ClassCastException(activity.toString()
                    + " must implement OnFragmentInteractionListener");
        }

    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;

    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);

        getActivity()
                .setTitle(getResources().getString(R.string.order_detailed_title));

    }

    @Override
    public void onClick(View v) {
        String phoneNum = null;
        switch (v.getId()) {
            case R.id.txt_cust_phone:
                phoneNum = order.getCustomerContactNo();
                break;

            case R.id.txt_agent_phone:
                phoneNum = order.getAgentContactNo();
                break;


            case R.id.deliver:
                order.setStatus("Delivered");
                order.setAgentId(ParseUser.getCurrentUser().getObjectId());
                UpdateOrder updateOrder = new UpdateOrder(this.getActivity(), order, this, order.getStatus());
                updateOrder.execute();
                break;

            case R.id.pickup:
                ParseQuery query2 = new ParseQuery("order");
                query2.whereEqualTo("objectId", order.getObjectId());
                FindOrder findOrder = new FindOrder(query2, this.getActivity(), this, 000);
                findOrder.execute();
                break;

            case R.id.txt_location:
                if (this.order.getAgentId() != null && this.order.getStatus().equals(Constants.ORDER_ACCEPT_PICKUP)) {
                    ParseQuery<ParseUser> query = ParseUser.getQuery();
                    query.whereEqualTo("objectId", order.getAgentId());
                    FindUser user = new FindUser(query, getActivity(), this, 888);
                    user.execute();
                } else if (this.order.getStatus().equals("Delivered")) {
                    DeliveryTrackUtils.showToast(getActivity(), "Order Delivered to customer");

                } else {
                    DeliveryTrackUtils.showToast(getActivity(), "No agent has picked up");

                }

                break;

            case R.id.agent_location:
                if (this.order.getAgentId() != null && this.order.getStatus().equals(Constants.ORDER_ACCEPT_PICKUP)) {
                    ParseQuery<ParseUser> query1 = ParseUser.getQuery();
                    query1.whereEqualTo("objectId", order.getAgentId());
                    FindUser user1 = new FindUser(query1, getActivity(), this, 777);
                    user1.execute();
                } else if (this.order.getStatus().equals("Delivered")) {
                    DeliveryTrackUtils.showToast(getActivity(), "Order Delivered to customer");

                } else {
                    DeliveryTrackUtils.showToast(getActivity(), "No agent has picked up");

                }
                break;


        }

        if (phoneNum != null && phoneNum.length() > 0) {
            Intent callIntent = new Intent(Intent.ACTION_CALL);
            callIntent.setData(Uri.parse("tel:" + phoneNum));

        }

    }

    @Override
    public void getParseObjects(List<ParseObject> parseObjects, int tableId) {
        if (tableId == 000) {
            /*during pickup... Check whether the order is already pickedup? if not pickup else refresh*/
            DeliveryTrackUtils.acceptPickupForOrder(parseObjects, this);

        }
    }


    @Override
    public void restParseObjects(Order order) {
        if (order.getStatus().equals(Constants.ORDER_DELIVERED)) {
            DeliveryTrackUtils.showToast(getActivity(), getResources().getString(R.string.order_delivered));
            Intent intent = new Intent(getActivity(), SignatureActivity.class);
            intent.putExtra("orderNumber", order.getObjectId());
            startActivity(intent);

        } else if (order.getStatus().equals(Constants.ORDER_PICKEDUP)) {
            DeliveryTrackUtils.showToast(getActivity(), getResources().getString(R.string.order_pickedup));
            getFragmentManager().popBackStack();

        }
    }

    @Override
    public void onOrderSaveSuccess() {

    }

    @Override
    public void onOrderSaveFailure() {

    }

    @Override
    public void onOrderCanceled() {

    }

    @Override
    public void getUser(List<ParseUser> parseUsers, int tableId) {

        if (parseUsers != null && parseUsers.size() > 0) {
            ParseUser user = parseUsers.get(0);
            ParseGeoPoint geoPoint = (ParseGeoPoint) user.get("location");
            Intent agentLocation = new Intent(getActivity(), MapActivity.class);
            Address location = DeliveryTrackUtils.getLocationFromAddress(order.getAddress(), getActivity());
            if (tableId == 777) {
                agentLocation.putExtra("single", true);
                agentLocation.putExtra("Lat", geoPoint.getLatitude());
                agentLocation.putExtra("Log", geoPoint.getLongitude());
                startActivity(agentLocation);

            } else if (tableId == 333) {
                restaName.setText(getResources().getString(R.string.rest_name) + ": " + user.get("userFullName").toString());
                restAddress.setText(getResources().getString(R.string.rest_address) + ": " + user.get("address").toString());

            } else if (tableId == 888) {
                if (location != null) {
                    agentLocation.putExtra("single", false);
                    agentLocation.putExtra("tolat", location.getLatitude());
                    agentLocation.putExtra("tolon", location.getLongitude());
                    agentLocation.putExtra("Lat", geoPoint.getLatitude());
                    agentLocation.putExtra("Log", geoPoint.getLongitude());
                    startActivity(agentLocation);
                }

            }
        }


    }


    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p/>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */

    public interface OnFragmentInteractionListener {
        // TODO: Update argument type and name
        public void onFragmentInteraction(Uri uri);
    }


}
