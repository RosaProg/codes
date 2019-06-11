package deliverytrack.vss.com.deliverytrack.fragments.orders;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;

import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.util.ArrayList;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.SignatureActivity;
import deliverytrack.vss.com.deliverytrack.adapters.OrderAdapter;
import deliverytrack.vss.com.deliverytrack.asynch.FindForQuery;
import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.ParseHelper;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link OrderPendingDelivery.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link OrderPendingDelivery#newInstance} factory method to
 * create an instance of this fragment.
 * /*orders pending delivery fragment contains the data about the orders which are pending for delivery
 */


public class OrderPendingDelivery extends Fragment implements OrderCallbacks {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private OrderAdapter orderAdapter;

    /*returns the orders which are waiting for delivery*/
    public ArrayList<Order> getOrdersWaitingForDelivery() {
        return ordersWaitingForDelivery;
    }

    public void setOrdersWaitingForDelivery(ArrayList<Order> ordersWaitingForDelivery) {
        Log.v("OrderPendingDelivery","setOrdersWaitingForDelivery");
        this.ordersWaitingForDelivery = ordersWaitingForDelivery;
        orderAdapter.setItems(ordersWaitingForDelivery);
        orderAdapter.notifyDataSetChanged();

    }

    private ArrayList<Order> ordersWaitingForDelivery = new ArrayList<>();

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment OrderPendingDelivery.
     * <p/>
     * the table picked up is listed in this fragment
     */
    // TODO: Rename and change types and number of parameters
    public static OrderPendingDelivery newInstance(String param1, String param2) {
        OrderPendingDelivery fragment = new OrderPendingDelivery();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public OrderPendingDelivery() {
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

    /*queries for orders with status  picked*/
    private void getPickedUpOrders() {
        Log.v("OrderPendingDelivery", "getPickedUpOrders");
        ParseUser user = ParseUser.getCurrentUser();
        ParseQuery query = new ParseQuery("OrderVisible");
        query.whereEqualTo("AgentId", user.getObjectId());
        query.whereEqualTo("status", Constants.ORDER_PICKEDUP);
        query.include("Order");
        FindForQuery findQuery = new FindForQuery(query, getActivity(), this, 333);
        findQuery.execute();

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_order_pending_delivery, container, false);
        ListView lv = (ListView) view.findViewById(R.id.pickedup_list);
        orderAdapter = new OrderAdapter(getActivity(), ordersWaitingForDelivery, this);
        lv.setAdapter(orderAdapter);
        getPickedUpOrders();


        return view;
    }

    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
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
    public void getParseObjects(List<ParseObject> parseObjects, int tableId) {
        /*sets the pidkedup orders to the adapter*/
        ordersWaitingForDelivery.clear();
        ParseHelper helper = new ParseHelper();
        ordersWaitingForDelivery = helper.getOrdersVisible(parseObjects);
        setOrdersWaitingForDelivery(ordersWaitingForDelivery);


    }

    @Override
    public void restParseObjects(Order order) {
        ordersWaitingForDelivery.clear();
        /*call back after order delivered...*/
        getPickedUpOrders();//Screen refresh
        DeliveryTrackUtils.showToast(getActivity(), getResources().getString(R.string.order_delivered));

        /*take to signature activity for DA sign*/
        Intent intent = new Intent(getActivity(), SignatureActivity.class);
        intent.putExtra("orderNumber", order.getObjectId());
        startActivity(intent);

    }

    @Override
    public void onOrderSaveSuccess() {

    }

    @Override
    public void onOrderSaveFailure() {

    }

    @Override
    public void onOrderCanceled() {
        getPickedUpOrders();
    }


    @Override
    public void onResume() {
        super.onResume();
        getActivity()
                .setTitle("Orders Picked Up");
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        getActivity()
                .setTitle(getResources().getString(R.string.orders_pending_for_delivery_title));

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
