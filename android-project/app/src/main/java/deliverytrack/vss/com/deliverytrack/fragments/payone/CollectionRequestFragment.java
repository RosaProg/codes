package deliverytrack.vss.com.deliverytrack.fragments.payone;

import android.app.Activity;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.google.gson.Gson;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseUser;
import com.parse.SaveCallback;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import java.util.ArrayList;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.asynch.DTHTTPURLConnection;
import deliverytrack.vss.com.deliverytrack.asynch.payone.PayOneRequestId;
import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.interfaces.PayOneCallbacks;
import deliverytrack.vss.com.deliverytrack.models.payone.CollectionResponse;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link CollectionRequestFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link CollectionRequestFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class CollectionRequestFragment extends Fragment implements View.OnClickListener, PayOneCallbacks {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private Button bt_submit;
    private EditText et_amount;

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment CollectionRequestFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static CollectionRequestFragment newInstance(String param1, String param2) {
        CollectionRequestFragment fragment = new CollectionRequestFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public CollectionRequestFragment() {
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
        View view = inflater.inflate(R.layout.payment_req, container, false);
        bt_submit = (Button) view.findViewById(R.id.button_submit_mobile_number);
        et_amount = (EditText) view.findViewById(R.id.editText_amount);

        bt_submit.setOnClickListener(this);
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
        if (et_amount.getText().toString().equals("") || et_amount.getText().toString().equals(null)) {
            Toast.makeText(getActivity(), "Please enter amount", Toast.LENGTH_LONG).show();
        } else {
            sendCollectionReq(ParseUser.getCurrentUser().getUsername(), et_amount.getText().toString());
        }
    }


    private void sendCollectionReq(String mobile, String amount) {
        final ParseObject payonerequest = new ParseObject("PayOneRequest");
        payonerequest.put("mobileNumber", mobile);
        payonerequest.put("amount", amount);
        payonerequest.put("expiryTime", DeliveryTrackUtils.getPayOneExpTime());
        payonerequest.saveInBackground(new SaveCallback() {
            public void done(ParseException e) {
                if (e == null) {
                    String objectId = payonerequest.getObjectId();
                    String ar[] = new String[6];
                    ar[0] = Constants.payOneKey;
                    ar[1] = ParseUser.getCurrentUser().getUsername();
                    ar[2] = et_amount.getText().toString();
                    ar[3] = DeliveryTrackUtils.getPayOneExpTime();
                    ar[4] = objectId;
                    ar[5] = Constants.payOneSalt;

                    List<NameValuePair> params = new ArrayList<NameValuePair>();
                    params.add(new BasicNameValuePair("key", Constants.payOneKey));
                    params.add(new BasicNameValuePair("mobile", ParseUser.getCurrentUser().getUsername()));
                    params.add(new BasicNameValuePair("amount", et_amount.getText().toString()));
                    params.add(new BasicNameValuePair("expiry_time", DeliveryTrackUtils.getPayOneExpTime()));
                    params.add(new BasicNameValuePair("ref_id", objectId));
                    params.add(new BasicNameValuePair("signature", DeliveryTrackUtils.getPayOneSignature(ar, "|")));

                    DTHTTPURLConnection dthttpurlConnection = new DTHTTPURLConnection(Constants.COLLECTIONAPI, params, CollectionRequestFragment.this, Constants.COLLECTION_REQ);
                    dthttpurlConnection.execute();

                } else {
                    // Failure!
                }
            }
        });
    }

    @Override
    public void onReceiveJSONString(Object json, int reqType) {

        if (json != null) {
            Gson gson = new Gson();
            CollectionResponse res = gson.fromJson(json.toString(), CollectionResponse.class);
            if (res.getDescription() != null && res.getDescription().getReq_id() != null) {
                DeliveryTrackUtils.showToast(this.getActivity(), "The transaction id" + " " + res.getDescription().getReq_id());
                PayOneRequestId req = new PayOneRequestId(this, res.getDescription().getReq_id());
                req.execute();
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
