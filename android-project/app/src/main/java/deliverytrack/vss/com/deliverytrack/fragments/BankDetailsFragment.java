package deliverytrack.vss.com.deliverytrack.fragments;

import android.app.Activity;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.util.List;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.asynch.FindForQuery;

import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link BankDetailsFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link BankDetailsFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class BankDetailsFragment extends Fragment implements OrderCallbacks {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private TextView txtbankName;
    private TextView txtifscCode;
    private TextView txtaccountName;
    private TextView txtaccountNumber;

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment BankDetailsFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static BankDetailsFragment newInstance(String param1, String param2) {
        BankDetailsFragment fragment = new BankDetailsFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public BankDetailsFragment() {
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
    public void onResume() {
        super.onResume();
        getActivity()
                .setTitle(getString(R.string.bank_details_frag));

    }

    private void getAgentAccountNumber() {
        ParseUser user = ParseUser.getCurrentUser();
        ParseQuery query = new ParseQuery("Agents");
        query.whereEqualTo("userName", user.getUsername());
        FindForQuery findQuery = new FindForQuery(query, getActivity(), this, 878);
        findQuery.execute();

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_bank_details2, container, false);
        txtbankName = (TextView) view.findViewById(R.id.txt_bank_name);
        txtifscCode = (TextView) view.findViewById(R.id.txt_ifsc_code);
        txtaccountName = (TextView) view.findViewById(R.id.txt_account_name);
        txtaccountNumber = (TextView) view.findViewById(R.id.txt_account_number);
        txtbankName.setText("Bank Name :" + "YES Bank");
        txtifscCode.setText("IFSCCode : " + "YESB0000203");
        txtaccountName.setText("Account Name :" + "VSS Tech Solution P Limited A/C Food Credits");
        getAgentAccountNumber();
        return view;
    }

    public void setTextViews(String accountNumber) {
        txtaccountNumber.setText("Account Number :" + accountNumber);
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
        if (parseObjects != null && parseObjects.size() > 0) {
            ParseObject object = parseObjects.get(0);
            setTextViews(object.get("accountNumber") != null ? object.get("accountNumber").toString() : "");
        } else {
            DeliveryTrackUtils.showToast(getActivity(), "Kindly save the Profile Settings to generate Account Number");
        }

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
        public void onFragmentInteraction(Uri uri);
    }

}
