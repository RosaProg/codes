package deliverytrack.vss.com.deliverytrack.fragments.payone;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import deliverytrack.vss.com.deliverytrack.PayOneWebViewActivity;
import deliverytrack.vss.com.deliverytrack.R;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link PayOneFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link PayOneFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class PayOneFragment extends Fragment implements View.OnClickListener {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment PayOneFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static PayOneFragment newInstance(String param1, String param2) {
        PayOneFragment fragment = new PayOneFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public PayOneFragment() {
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
        View view = inflater.inflate(R.layout.fragment_pay_one, container, false);
        Button bt_rechage = (Button) view.findViewById(R.id.button_recharge);
        Button bt_status = (Button) view.findViewById(R.id.button_check_status);

        Button nearby = (Button) view.findViewById(R.id.btn_nearby_stores);

        bt_rechage.setOnClickListener(this);
        bt_status.setOnClickListener(this);
        nearby.setOnClickListener(this);

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
    public void onClick(View v) {

        switch (v.getId()) {
            case R.id.button_recharge:
                CollectionRequestFragment opf = new CollectionRequestFragment();
                FragmentTransaction transaction = getFragmentManager().beginTransaction();
                transaction.replace(R.id.container, opf, "CollectionRequestFragment");
                transaction.addToBackStack(null);
                transaction.commit();

                break;

            case R.id.button_check_status:

                StatusCheckFragment sts = new StatusCheckFragment();
                FragmentTransaction transaction1 = getFragmentManager().beginTransaction();
                transaction1.replace(R.id.container, sts, "StatusCheckFragment");
                transaction1.addToBackStack(null);
                transaction1.commit();


                break;

            case R.id.btn_nearby_stores:
                Intent intent = new Intent(getActivity(), PayOneWebViewActivity.class);
                startActivity(intent);
                break;
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
