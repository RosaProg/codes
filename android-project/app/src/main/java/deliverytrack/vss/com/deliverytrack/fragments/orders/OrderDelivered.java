package deliverytrack.vss.com.deliverytrack.fragments.orders;

import android.app.Activity;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.util.ArrayList;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.adapters.OrderAdapter;
import deliverytrack.vss.com.deliverytrack.asynch.FindForQuery;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.ParseHelper;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link OrderDelivered.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link OrderDelivered#newInstance} factory method to
 * create an instance of this fragment.
 * /*orders pending pickup fragment contains the data about the orders which are delivered
 */


public class OrderDelivered extends Fragment implements OrderCallbacks {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private OrderAdapter orderAdapter;

    public ArrayList<Order> getOrderDelivered() {
        return orderDelivered;
    }

    public void setOrderDelivered(ArrayList<Order> orderDelivered) {
        this.orderDelivered = orderDelivered;
        orderAdapter.setItems(orderDelivered);
        orderAdapter.notifyDataSetChanged();

    }

    private ArrayList<Order> orderDelivered = new ArrayList<>();

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        getActivity()
                .setTitle(getString(R.string.orders_delivered_title));
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment OrderDelivered.
     */
    // TODO: Rename and change types and number of parameters
    public static OrderDelivered newInstance(String param1, String param2) {
        OrderDelivered fragment = new OrderDelivered();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public OrderDelivered() {
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

    private void getOrdersDelivered() {
        ParseUser user = ParseUser.getCurrentUser();
        String type = DeliveryTrackUtils.getType();
        String queryCol = "";
        if (type.equals("agentId")) {
            queryCol = "agentid";
        } else {
            queryCol = "resturantid";
        }
        ParseQuery query = new ParseQuery("order");
        query.whereEqualTo(queryCol, user.getObjectId());
        query.whereEqualTo("status", "Delivered");
        FindForQuery findQuery = new FindForQuery(query, getActivity(), this, 222);
        findQuery.execute();

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        View view = inflater.inflate(R.layout.fragment_ordered_delivered, container, false);
        ListView lv = (ListView) view.findViewById(R.id.pickedup_list);
        orderAdapter = new OrderAdapter(getActivity(), orderDelivered, this);
        lv.setAdapter(orderAdapter);
        getOrdersDelivered();
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
        orderDelivered.clear();
        ParseHelper helper = new ParseHelper();
        orderDelivered = helper.getOrdersVisibleDelivered(parseObjects);
        setOrderDelivered(orderDelivered);

    }

    @Override
    public void restParseObjects(Order order) {

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
    public void onResume() {
        super.onResume();
        // Set title
        getActivity()
                .setTitle(getString(R.string.orders_delivered_title));
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
