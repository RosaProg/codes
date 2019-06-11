package deliverytrack.vss.com.deliverytrack.fragments.payone;

import android.app.Activity;
import android.app.ProgressDialog;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.google.gson.Gson;
import com.parse.FindCallback;
import com.parse.Parse;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;
import com.parse.SaveCallback;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;

import java.util.ArrayList;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.asynch.DTHTTPURLConnection;
import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.interfaces.PayOneCallbacks;
import deliverytrack.vss.com.deliverytrack.models.payone.StatusCheckResponse;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link StatusCheckFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link StatusCheckFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class StatusCheckFragment extends Fragment implements PayOneCallbacks, View.OnClickListener {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private ProgressDialog pDialog;

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment StatusCheckFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static StatusCheckFragment newInstance(String param1, String param2) {
        StatusCheckFragment fragment = new StatusCheckFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public StatusCheckFragment() {
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
        View view = inflater.inflate(R.layout.check_status, container, false);
        Button bt_check_status = (Button) view.findViewById(R.id.button_check_status_btn);

        bt_check_status.setOnClickListener(this);

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
    public void onReceiveJSONString(Object json, int reqType) {

        if (json != null) {
            Gson gson = new Gson();
            StatusCheckResponse res = gson.fromJson(json.toString(), StatusCheckResponse.class);
            Log.d("the res", res.getDescription().getStatus());

            String status = res.getDescription().getStatus();
            String toast = " ";

            if (status.equals("0")) {
                toast = "Your Transaction is pending";
            } else if (status.equals("1")) {
                toast = "Your Payment is collected";
            }

            DeliveryTrackUtils.showToast(this.getActivity(), toast);
        }

    }

    @Override
    public void onClick(View v) {

        switch (v.getId()) {

            case R.id.button_check_status_btn:
                new CheckStatus().execute();
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

    class CheckStatus extends AsyncTask<String, String, ParseObject> {

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            pDialog = new ProgressDialog(StatusCheckFragment.this.getActivity());
            pDialog.setMessage("Please Wait...");
            pDialog.setIndeterminate(false);
            pDialog.setCancelable(true);
            pDialog.show();
        }

        /**
         * Creating product
         */
        protected ParseObject doInBackground(String... args) {

            ParseQuery payonerequest = new ParseQuery("PayOneRequest");

            payonerequest.whereEqualTo("mobileNumber", ParseUser.getCurrentUser().getUsername());
            try {
                List<ParseObject> list = payonerequest.find();

                if (list != null && list.size() > 0) {
                    ParseObject object = (ParseObject) list.get(0);
                    return object;

                } else {
                }


            } catch (
                    ParseException e
                    )

            {
                e.printStackTrace();
            }
            return null;

        }

        /**
         * After completing background task Dismiss the progress dialog
         **/
        protected void onPostExecute(ParseObject message) {
            // dismiss the dialog once done
            pDialog.dismiss();
            if (message != null && message.get("requestId") != null) {
                String ar[] = new String[4];
                ar[0] = Constants.payOneKey;
                ar[1] = message.getObjectId();
                ar[2] = message.get("requestId").toString();
                ar[3] = Constants.payOneSalt;

                List<NameValuePair> params = new ArrayList<NameValuePair>();
                params.add(new BasicNameValuePair("key", Constants.payOneKey));
                params.add(new BasicNameValuePair("ref_id", message.getObjectId()));
                params.add(new BasicNameValuePair("request_id", message.get("requestId").toString()));
                params.add(new BasicNameValuePair("signature", DeliveryTrackUtils.getPayOneSignature(ar, "|")));

                DTHTTPURLConnection dthttpurlConnection = new DTHTTPURLConnection(Constants.CHECK_STATUS, params, StatusCheckFragment.this, Constants.COLLECTION_REQ);
                dthttpurlConnection.execute();
            }
        }
    }


}
