package deliverytrack.vss.com.deliverytrack.fragments.orders;

import android.app.Activity;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
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
 * {@link OrderPendingPickup.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link OrderPendingPickup#newInstance} factory method to
 * create an instance of this fragment.
 */
public class OrderPendingPickup extends Fragment implements OrderCallbacks {
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
    public ArrayList<Order> getOrdersPendingToPickup() {
        return ordersPendingToPickup;
    }

    public void setOrdersPendingToPickup(ArrayList<Order> ordersPendingToPickup) {
        this.ordersPendingToPickup = ordersPendingToPickup;
        orderAdapter.setItems(ordersPendingToPickup);
        orderAdapter.notifyDataSetChanged();

    }

    private ArrayList<Order> ordersPendingToPickup = new ArrayList<>();


    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment OrderPendingPickup.
     */
    // TODO: Rename and change types and number of parameters
    public static OrderPendingPickup newInstance(String param1, String param2) {
        OrderPendingPickup fragment = new OrderPendingPickup();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public OrderPendingPickup() {
        // Required empty public constructor
    }


    /*queries for orders with status accept pickup*/
    private void getAcceptOrders() {
        ParseUser user = ParseUser.getCurrentUser();
        ParseQuery query = new ParseQuery("OrderVisible");
        query.whereEqualTo("AgentId", user.getObjectId());
        query.whereEqualTo("status", Constants.ORDER_ARRIVED_LOCATION);
        query.include("Order");
        FindForQuery findQuery = new FindForQuery(query, getActivity(), this, 444);
        findQuery.execute();

    }

    @Override
    public void onResume() {
        super.onResume();

        getActivity()
                .setTitle("Orders- Delivery Agent Arrived");

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
        View view = inflater.inflate(R.layout.fragment_order_pending_pickup2, container, false);
        ListView lv = (ListView) view.findViewById(R.id.pickedup_list);
        orderAdapter = new OrderAdapter(getActivity(), ordersPendingToPickup, this);
        lv.setAdapter(orderAdapter);
        getAcceptOrders();
        lv.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> arg0, View arg1,
                                           int pos, long id) {
                final int item = pos;
                final Order order = ordersPendingToPickup.get(item);
                DeliveryTrackUtils.cancelOrder(order, OrderPendingPickup.this);
                return true;
            }
        });


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

        if (tableId == 444) {
            /*retrieves the orders in accept pickup status which are visible to current user*/
            ordersPendingToPickup.clear();
            ParseHelper helper = new ParseHelper();
            ordersPendingToPickup = helper.getOrdersVisible(parseObjects);
            setOrdersPendingToPickup(ordersPendingToPickup);
        } else if (tableId == 000) {
            /*during accept pickup... Check whether the order is already accept picked up? if not refresh
            .....*/
            boolean isRefresh = DeliveryTrackUtils.pickupForOrder(parseObjects, this);
            if (isRefresh) {
                getAcceptOrders();
            }
        }
    }

    @Override
    public void restParseObjects(Order order) {
        ordersPendingToPickup.clear();
        getAcceptOrders();
        DeliveryTrackUtils.showToast(getActivity(), "Marked Pickedup");

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
