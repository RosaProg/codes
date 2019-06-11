package deliverytrack.vss.com.deliverytrack.fragments.registration;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.speech.RecognizerIntent;
import android.support.v4.app.Fragment;
import android.text.Editable;
import android.text.InputFilter;
import android.text.Spanned;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.inputmethod.InputMethodManager;
import android.widget.AdapterView;
import android.widget.AutoCompleteTextView;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;

import com.parse.ParseUser;


import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.adapters.PlacesAdapter;
import deliverytrack.vss.com.deliverytrack.interfaces.PageFragmentCallbacks;
import deliverytrack.vss.com.deliverytrack.models.AgentDetailPage;
import deliverytrack.vss.com.deliverytrack.models.SandwichWizardModel;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.PostalCodeAPI;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link AgentDetailsFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link AgentDetailsFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class AgentDetailsFragment extends Fragment implements View.OnFocusChangeListener, View.OnClickListener {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private static final String ARG_KEY = "key";
    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private AgentDetailPage mPage;
    private String mKey;
    private PageFragmentCallbacks mCallbacks;
    private EditText edtFullname;
    private TextView edtMobileNumber;
    private AutoCompleteTextView edtCurAreaYouWant;
    private AutoCompleteTextView edtPerAreaYouWant;
    private EditText edtPerAddress;
    private EditText edtCurAddress;
    private EditText edtPinCodePer;
    private EditText edtPinCodeCur;

    private EditText edtAddress;
    private EditText edtaddressOfRest;
    private EditText edtNameOfRestaurant;
    private EditText edtemailId;
    private EditText pinCode;
    private ImageButton fullNameMic;
    private int BRANCH_TEXT = 1000;
    private int LOCATION_TEXT = 1001;
    private int AREA_TEXT = 1002;
    private int NAME_TEXT = 1003;
    private int ADDRESS_TEXT = 1004;
    private SandwichWizardModel mWizardModel;
    private EditText edtEmailAgent;
    private AutoCompleteTextView edtRestAreaWant;
    private ProgressDialog mProgressDialog;

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment AgentDetailsFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static AgentDetailsFragment newInstance(String param1, String param2) {
        AgentDetailsFragment fragment = new AgentDetailsFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public AgentDetailsFragment() {
        // Required empty public constructor
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Bundle args = getArguments();
        mKey = args.getString(ARG_KEY);
        mPage = (AgentDetailPage) mCallbacks.onGetPage(mKey);
        setRetainInstance(true);


    }

    public static AgentDetailsFragment create(String key) {
        Bundle args = new Bundle();
        args.putString(ARG_KEY, key);
        AgentDetailsFragment fragment = new AgentDetailsFragment();
        fragment.setArguments(args);
        return fragment;
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = null;
        ParseUser user = ParseUser.getCurrentUser();

        if (DeliveryTrackUtils.getUserType().equals("Agent")) {
            view = inflater.inflate(R.layout.fragment_agent_details, container, false);
            edtFullname = (EditText) view.findViewById(R.id.edt_full_name);
            edtMobileNumber = (TextView) view.findViewById(R.id.edt_mobile_number);

            edtCurAreaYouWant = (AutoCompleteTextView) view.findViewById(R.id.autocomplete_cur);
            edtPerAreaYouWant = (AutoCompleteTextView) view.findViewById(R.id.autocomplete_per);


            edtCurAreaYouWant.setAdapter(new PlacesAdapter(getActivity(), R.layout.autocomplete_list));
            edtPerAreaYouWant.setAdapter(new PlacesAdapter(getActivity(), R.layout.autocomplete_list));


            edtCurAreaYouWant.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                @Override
                public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

                    edtCurAreaYouWant.post(new Runnable() {
                        public void run() {
                            edtCurAreaYouWant.dismissDropDown();
                        }
                    });

                    String apiresult = (String) parent.getItemAtPosition(position);
                    edtCurAreaYouWant.setText(DeliveryTrackUtils.splitStrings(apiresult)[0]);
                    new GetPostalCode(DeliveryTrackUtils.splitStrings(apiresult)[1], edtPinCodeCur).execute();

                }
            });


            edtPerAreaYouWant.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                @Override
                public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                    edtPerAreaYouWant.post(new Runnable() {
                        public void run() {
                            edtPerAreaYouWant.dismissDropDown();
                        }
                    });

                    String apiresult = (String) parent.getItemAtPosition(position);
                    edtPerAreaYouWant.setText(DeliveryTrackUtils.splitStrings(apiresult)[0]);
                    new GetPostalCode(DeliveryTrackUtils.splitStrings(apiresult)[1], edtPinCodePer).execute();

                }
            });


            edtCurAddress = (EditText) view.findViewById(R.id.edt_cur_address);
            edtPerAddress = (EditText) view.findViewById(R.id.edt_per_address);

            edtPinCodeCur = (EditText) view.findViewById(R.id.edt_pincode_cur);
            edtPerAddress = (EditText) view.findViewById(R.id.edt_per_address);

            edtEmailAgent = (EditText) view.findViewById(R.id.edt_email_agent);

            edtCurAddress.setOnFocusChangeListener(this);
            edtPerAddress.setOnFocusChangeListener(this);
            edtPinCodeCur.setOnFocusChangeListener(this);
            edtPinCodePer = (EditText) view.findViewById(R.id.edt_pincode_per);
            edtPinCodePer.setOnFocusChangeListener(this);
            edtPerAddress.setOnFocusChangeListener(this);
            edtCurAreaYouWant.setOnFocusChangeListener(this);
            edtPerAreaYouWant.setOnFocusChangeListener(this);

            fullNameMic = (ImageButton) view.findViewById(R.id.id_your_name_mic);

            fullNameMic.setOnClickListener(this);

            edtFullname.setText(user.get("userFullName") != null ? user.get("userFullName").toString() : "");
            edtMobileNumber.setText(user.get("phone") != null ? user.get("phone").toString() : "");
            edtPerAreaYouWant.setText(user.get("perAreaDA") != null ? user.get("perAreaDA").toString() : "");
            edtCurAreaYouWant.setText(user.get("currentAreaDA") != null ? user.get("currentAreaDA").toString() : "");


            if (edtPerAreaYouWant.getText().toString().length() == 0) {
                edtPerAreaYouWant.setError("Your Permanent Area is required");
            }

            if (edtCurAreaYouWant.getText().toString().length() == 0) {
                edtCurAreaYouWant.setError("Your Current Area is required");
            }
            edtCurAddress.setText(user.get("currentAddressDA") != null ? user.get("currentAddressDA").toString() : "");

            if (edtCurAddress.getText().toString().length() == 0) {
                edtCurAddress.setError("Your Current Address is required");
            }

            edtPerAddress.setText(user.get("perAddressDA") != null ? user.get("perAddressDA").toString() : "");

            if (edtPerAddress.getText().toString().length() == 0) {
                edtPerAddress.setError("Your Permanent Address is required");
            }


            edtCurAreaYouWant.setText(user.get("currentAreaDA") != null ? user.get("currentAreaDA").toString() : "");

            edtPinCodeCur.setText(user.get("currentPinCodeDA") != null ? user.get("currentPinCodeDA").toString() : "");


            edtPerAreaYouWant.setText(user.get("perAreaDA") != null ? user.get("perAreaDA").toString() : "");

            edtPinCodePer.setText(user.get("perPinCodeDA") != null ? user.get("perPinCodeDA").toString() : "");


            edtEmailAgent.setText(user.get("email") != null ? user.get("email").toString() : "");


            edtFullname.setFilters(new InputFilter[]{filtertxt});
            // edtPerAddress.setFilters(new InputFilter[]{filtertxt});
            //  edtCurAddress.setFilters(new InputFilter[]{filtertxt});

            mPage.getData().putString(AgentDetailPage.FULL_NAME, edtFullname.getText().toString());
            mPage.getData().putString(AgentDetailPage.MOBILE_NUMBER, edtMobileNumber.getText().toString());
            mPage.getData().putString(AgentDetailPage.AREACUR, edtCurAreaYouWant.getText().toString());
            mPage.getData().putString(AgentDetailPage.AREAPER, edtPerAreaYouWant.getText().toString());
            mPage.getData().putString(AgentDetailPage.CURADDRESS, edtCurAddress.getText().toString());
            mPage.getData().putString(AgentDetailPage.PERADDRESS, edtPerAddress.getText().toString());
            mPage.getData().putString(AgentDetailPage.AGENTPINCODECUR, edtPinCodeCur.getText().toString());
            mPage.getData().putString(AgentDetailPage.AGENTPINCODEPER, edtPinCodePer.getText().toString());


            mPage.getData().putString(AgentDetailPage.EMAIL, edtEmailAgent.getText().toString());
            mPage.notifyDataChanged();


        } else {

            view = inflater.inflate(R.layout.rest_details, container, false);
            edtNameOfRestaurant = (EditText) view.findViewById(R.id.name_of_rest);
            edtaddressOfRest = (EditText) view.findViewById(R.id.address_of_rest);
            edtRestAreaWant = (AutoCompleteTextView) view.findViewById(R.id.autocomplete);
            edtRestAreaWant.setAdapter(new PlacesAdapter(getActivity(), R.layout.autocomplete_list));

            edtRestAreaWant.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                @Override
                public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                    edtRestAreaWant.post(new Runnable() {
                        public void run() {
                            edtRestAreaWant.dismissDropDown();
                        }
                    });

                    String apiresult = (String) parent.getItemAtPosition(position);
                    edtRestAreaWant.setText(DeliveryTrackUtils.splitStrings(apiresult)[0]);
                    new GetPostalCode(DeliveryTrackUtils.splitStrings(apiresult)[1], pinCode).execute();
                }
            });


            edtemailId = (EditText) view.findViewById(R.id.email_of_rest);
            pinCode = (EditText) view.findViewById(R.id.pincode);

            edtNameOfRestaurant.setText(user.get("restName") != null ? user.get("restName").toString() : "");
            edtaddressOfRest.setText(user.get("address") != null ? user.get("address").toString() : "");
            edtemailId.setText(user.getEmail() != null ? user.getEmail() : "");
            pinCode.setText(user.get("pincode") != null ? user.get("pincode").toString() : "");

            if (edtNameOfRestaurant.getText().toString().length() == 0) {
                edtNameOfRestaurant.setError("Name of the restaurant is required");
            }

            if (edtaddressOfRest.getText().toString().length() == 0) {
                edtaddressOfRest.setError("Name of address is required");
            }

            if (edtemailId.getText().toString().length() == 0) {
                edtemailId.setError("Email id is required");
            }

            mPage.getData().putString(AgentDetailPage.REST_NAME, edtNameOfRestaurant.getText().toString());
            mPage.getData().putString(AgentDetailPage.ADDRESS_REST, edtaddressOfRest.getText().toString());
            mPage.getData().putString(AgentDetailPage.REST_EMAIL, edtemailId.getText().toString());
            mPage.getData().putString(AgentDetailPage.PINCODE, pinCode.getText().toString());
            mPage.getData().putString(AgentDetailPage.RESTAREA, edtRestAreaWant.getText().toString());


            mPage.notifyDataChanged();
        }


        return view;
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        if (DeliveryTrackUtils.getUserType().equals("Agent")) {

            edtFullname.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {

                    mPage.getData().putString(AgentDetailPage.FULL_NAME,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtMobileNumber.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.MOBILE_NUMBER,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtCurAreaYouWant.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.AREACUR,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtPerAreaYouWant.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.AREAPER,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtCurAddress.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.CURADDRESS,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtPerAddress.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.PERADDRESS,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtPinCodePer.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.AGENTPINCODEPER,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtPinCodeCur.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.AGENTPINCODECUR,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtEmailAgent.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.EMAIL,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


        } else {

            edtNameOfRestaurant.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.REST_NAME,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtaddressOfRest.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.ADDRESS_REST,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });

            edtemailId.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.REST_EMAIL,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            pinCode.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.PINCODE,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


            edtRestAreaWant.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentDetailPage.RESTAREA,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();
                }
            });


        }


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
        if (!(activity instanceof PageFragmentCallbacks)) {
            throw new ClassCastException("Activity must implement PageFragmentCallbacks");
        }


        mCallbacks = (PageFragmentCallbacks) activity;

    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;

        mCallbacks = null;

    }

    @Override
    public void onFocusChange(View v, boolean hasFocus) {

        switch (v.getId()) {

            case R.id.edt_full_name:

                if (hasFocus) {
                    fullNameMic.setVisibility(View.VISIBLE);
                } else {
                    fullNameMic.setVisibility(View.GONE);
                }
                break;


            case R.id.edt_area_you_want:


                break;

        }

    }

    @Override
    public void onClick(View v) {

        switch (v.getId()) {


            case R.id.id_your_name_mic:
                startVoiceRecognitionActivity(NAME_TEXT);

                break;

            case R.id.id_your_address_mic:
                startVoiceRecognitionActivity(ADDRESS_TEXT);

                break;

            case R.id.id_your_area_mic:
                startVoiceRecognitionActivity(AREA_TEXT);

                break;

        }

    }


    private void startVoiceRecognitionActivity(int reqCode) {
        Intent intent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL,
                RecognizerIntent.LANGUAGE_MODEL_FREE_FORM);
        intent.putExtra(RecognizerIntent.EXTRA_PROMPT, "Please Speak to input...");
        startActivityForResult(intent, reqCode);
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

//        if (data != null) {
//            ArrayList<String> matches = data.getStringArrayListExtra(
//                    RecognizerIntent.EXTRA_RESULTS);
//
//            if (requestCode == AREA_TEXT) {
//                edtPerAddress.setText(matches.get(0));
//            } else if (requestCode == ADDRESS_TEXT) {
//                edtAddress.setText(matches.get(0));
//            } else if (requestCode == NAME_TEXT) {
//                edtFullname.setText(matches.get(0));
//            }
//        }
        super.onActivityResult(requestCode, resultCode, data);

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


    @Override
    public void setMenuVisibility(boolean menuVisible) {
        super.setMenuVisibility(menuVisible);

        // In a future update to the support library, this should override setUserVisibleHint
        // instead of setMenuVisibility.

        if (edtFullname != null && edtMobileNumber != null &&
                edtPerAddress != null &&
                edtCurAddress != null) {
            InputMethodManager imm = (InputMethodManager) getActivity().getSystemService(
                    Context.INPUT_METHOD_SERVICE);
            if (!menuVisible) {
                imm.hideSoftInputFromWindow(getView().getWindowToken(), 0);
            }
        }
        if (edtaddressOfRest != null && edtNameOfRestaurant != null && pinCode != null &&
                edtemailId != null) {
            InputMethodManager imm = (InputMethodManager) getActivity().getSystemService(
                    Context.INPUT_METHOD_SERVICE);
            if (!menuVisible) {
                imm.hideSoftInputFromWindow(getView().getWindowToken(), 0);
            }
        }

    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
    }


    public class GetPostalCode extends AsyncTask<Void, Void, String[]> {

        String placeId;
        EditText edt;

        GetPostalCode(String placeId, EditText edt) {
            this.placeId = placeId;
            this.edt = edt;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            mProgressDialog = new ProgressDialog(getActivity());
            mProgressDialog.setTitle("Retrieving Data");
            mProgressDialog.setMessage("Loading...");
            mProgressDialog.setIndeterminate(false);
            mProgressDialog.setCanceledOnTouchOutside(false);
            mProgressDialog.setCancelable(false);
            mProgressDialog.show();

        }

        @Override
        protected void onPostExecute(String[] s) {
            super.onPostExecute(s);
            mProgressDialog.dismiss();
            if (s != null) {
                if (s[0] != null) {
                    edt.setText(s[0] + " ");
                }
            }

        }

        @Override
        protected String[] doInBackground(Void... params) {

            PostalCodeAPI postalCodeAPI = new PostalCodeAPI();
            return postalCodeAPI.details(this.placeId);
        }
    }


    InputFilter filtertxt = new InputFilter() {
        public CharSequence filter(CharSequence source, int start, int end,
                                   Spanned dest, int dstart, int dend) {
            for (int i = start; i < end; i++) {
                if (!Character.isLetter(source.charAt(i)) && !Character.isSpaceChar(source.charAt(i))) {
                    return "";
                }
            }
            return null;
        }
    };

}


