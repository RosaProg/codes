package deliverytrack.vss.com.deliverytrack.fragments.registration;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.net.Uri;
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
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;

import com.parse.ParseUser;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.interfaces.PageFragmentCallbacks;
import deliverytrack.vss.com.deliverytrack.models.AgentReferencePage;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link AgentRefDetailsFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link AgentRefDetailsFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class AgentRefDetailsFragment extends Fragment implements View.OnClickListener, View.OnFocusChangeListener {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private static final String ARG_KEY = "key";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private String mKey;
    private AgentReferencePage mPage;
    private PageFragmentCallbacks mCallbacks;
    private EditText edtReference1;
    private EditText edtReference2;
    private EditText edtFullnameReference1;
    private EditText edtFullnameReference2;
    private EditText edtRefRel;
    private EditText edtRefRe2;
    private ImageButton fullnameMic;
    private ImageButton fullnameMic1;
    private ImageButton relationshipMic;
    private ImageButton relationshipMic1;
    private EditText edtMobileNumber;
    private EditText edtMobileNumber2;
    private ImageButton mobileNumber1Mic;
    private ImageButton mobileNumber2Mic;
    private int REFERNAME1 = 1;
    private int REFERNAME2 = 2;
    private int REFERREL1 = 3;
    private int REFERREL2 = 4;
    private int REFERPHONE1 = 5;
    private int REFERPHONE2 = 6;
    private EditText managerFullName;
    private TextView managePhone;
    private ImageButton managerMic;
    private int MANAGERNAME = 111;

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment AgentRefDetailsFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static AgentRefDetailsFragment newInstance(String param1, String param2) {
        AgentRefDetailsFragment fragment = new AgentRefDetailsFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public AgentRefDetailsFragment() {
        // Required empty public constructor
    }


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Bundle args = getArguments();
        mKey = args.getString(ARG_KEY);
        mPage = (AgentReferencePage) mCallbacks.onGetPage(mKey);
        setRetainInstance(true);

    }

    public static AgentRefDetailsFragment create(String key) {
        Bundle args = new Bundle();
        args.putString(ARG_KEY, key);
        AgentRefDetailsFragment fragment = new AgentRefDetailsFragment();
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = null;
        // Inflate the layout for this fragment
        ParseUser user = ParseUser.getCurrentUser();
        if (DeliveryTrackUtils.getUserType().equals("Agent")) {

            view = inflater.inflate(R.layout.fragment_reference__agent_details, container, false);

            edtFullnameReference1 = (EditText) view.findViewById(R.id.edt_full_name_reference);
            edtFullnameReference2 = (EditText) view.findViewById(R.id.edt_full_name_reference1);
            edtRefRel = (EditText) view.findViewById(R.id.edt_relationship);
            edtRefRe2 = (EditText) view.findViewById(R.id.edt_relationship2);
            edtMobileNumber = (EditText) view.findViewById(R.id.mobile_number1);
            edtMobileNumber2 = (EditText) view.findViewById(R.id.mobile_number2);


            fullnameMic = (ImageButton) view.findViewById(R.id.id_full_name_ref_mic);
            fullnameMic1 = (ImageButton) view.findViewById(R.id.id_full_name_ref_mic1);
            relationshipMic = (ImageButton) view.findViewById(R.id.edt_relationship_mic);
            relationshipMic1 = (ImageButton) view.findViewById(R.id.id_relationship2_mic);
            mobileNumber1Mic = (ImageButton) view.findViewById(R.id.mobile_number1_mic);
            mobileNumber2Mic = (ImageButton) view.findViewById(R.id.mobile_number2_mic);


            edtFullnameReference1.setOnFocusChangeListener(this);
            edtFullnameReference2.setOnFocusChangeListener(this);

            edtRefRel.setOnFocusChangeListener(this);
            edtRefRe2.setOnFocusChangeListener(this);

            fullnameMic.setOnClickListener(this);
            fullnameMic1.setOnClickListener(this);
            relationshipMic.setOnClickListener(this);
            relationshipMic1.setOnClickListener(this);
            mobileNumber1Mic.setOnClickListener(this);
            mobileNumber2Mic.setOnClickListener(this);

            fullnameMic.setVisibility(View.GONE);
            fullnameMic1.setVisibility(View.GONE);
            relationshipMic.setVisibility(View.GONE);
            relationshipMic1.setVisibility(View.GONE);
            mobileNumber1Mic.setVisibility(View.GONE);
            mobileNumber2Mic.setVisibility(View.GONE);

            edtFullnameReference1.setFilters(new InputFilter[]{filtertxt});
            edtFullnameReference2.setFilters(new InputFilter[]{filtertxt});

            edtRefRel.setFilters(new InputFilter[]{filtertxt});
            edtRefRe2.setFilters(new InputFilter[]{filtertxt});

            edtFullnameReference1.setText(user.get("reference1name") != null ? user.get("reference1name").toString() : "");
            edtFullnameReference2.setText(user.get("reference2name") != null ? user.get("reference2name").toString() : "");
            edtRefRel.setText(user.get("reference1rel") != null ? user.get("reference1rel").toString() : "");
            edtRefRe2.setText(user.get("reference2rel") != null ? user.get("reference2rel").toString() : "");
            edtMobileNumber.setText(user.get("reference1phone") != null ? user.get("reference1phone").toString() : "");
            edtMobileNumber2.setText(user.get("reference2phone") != null ? user.get("reference2phone").toString() : "");

            if (edtFullnameReference1.getText().toString().length() == 0) {
                edtFullnameReference1.setError("Reference field is required");
            }

            if (edtFullnameReference2.getText().toString().length() == 0) {
                edtFullnameReference2.setError("Reference field is required");
            }

            if (edtMobileNumber.getText().toString().length() == 0) {
                edtMobileNumber.setError("MobileNumber is required");
            }

            if (edtMobileNumber2.getText().toString().length() == 0) {
                edtMobileNumber2.setError("MobileNumber is required");
            }

            mPage.getData().putString(AgentReferencePage.REFERENCE1, edtFullnameReference1.getText().toString());
            mPage.getData().putString(AgentReferencePage.REFERENCE2, edtFullnameReference2.getText().toString());
            mPage.getData().putString(AgentReferencePage.RELATIONSHIP1, edtRefRel.getText().toString());
            mPage.getData().putString(AgentReferencePage.RELATIONSHIP2, edtRefRe2.getText().toString());
            mPage.getData().putString(AgentReferencePage.MOB1, edtMobileNumber.getText().toString());
            mPage.getData().putString(AgentReferencePage.MOB2, edtMobileNumber2.getText().toString());
            mPage.notifyDataChanged();

        } else {
            view = inflater.inflate(R.layout.rest_manager_details, container, false);
            managerFullName = (EditText) view.findViewById(R.id.edt_full_name_manager);
            managePhone = (TextView) view.findViewById(R.id.edt_mobile_number);
            managerMic = (ImageButton) view.findViewById(R.id.id_full_name_manager_mic);


            managerFullName.setOnFocusChangeListener(this);

            managerMic.setVisibility(View.GONE);
            managerMic.setOnClickListener(this);

            managerFullName.setText(user.get("managerName") != null ? user.get("managerName").toString() : "");
            managePhone.setText(user.get("phone") != null ? user.get("phone").toString() : "");

            if (managerFullName.getText().toString().length() == 0) {
                managerFullName.setError("Manager Name is required");
            }

            if (managePhone.getText().toString().length() == 0) {
                managePhone.setError("Manager Phone number is Required");
            }

            mPage.getData().putString(AgentReferencePage.MANAGER_NAME, managerFullName.getText().toString());
            mPage.getData().putString(AgentReferencePage.MOBILE_NUMBERREST, managePhone.getText().toString());
            mPage.notifyDataChanged();


        }


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
    public void onClick(View v) {

        switch (v.getId()) {

            case R.id.id_full_name_ref_mic1:
                startVoiceRecognitionActivity(REFERNAME2);
                break;

            case R.id.id_relationship2_mic:
                startVoiceRecognitionActivity(REFERREL2);
                break;

            case R.id.id_full_name_ref_mic:
                startVoiceRecognitionActivity(REFERNAME1);
                break;

            case R.id.edt_relationship_mic:
                startVoiceRecognitionActivity(REFERREL1);
                break;

            case R.id.mobile_number1_mic:
                startVoiceRecognitionActivity(REFERPHONE1);
                break;

            case R.id.mobile_number2_mic:
                startVoiceRecognitionActivity(REFERPHONE2);

                break;

            case R.id.id_full_name_manager_mic:
                startVoiceRecognitionActivity(MANAGERNAME);
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
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        if (DeliveryTrackUtils.getUserType().equals("Agent")) {

            edtFullnameReference1.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentReferencePage.REFERENCE1,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();


                }
            });

            edtFullnameReference2.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentReferencePage.REFERENCE2,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();


                }
            });


            edtRefRel.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentReferencePage.RELATIONSHIP1,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();


                }
            });

            edtRefRe2.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentReferencePage.RELATIONSHIP2,
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
                    mPage.getData().putString(AgentReferencePage.MOB1,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();


                }
            });

            edtMobileNumber2.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentReferencePage.MOB2,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();


                }
            });
        } else {

            managerFullName.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {

                    mPage.getData().putString(AgentReferencePage.MANAGER_NAME,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();

                }
            });

            managePhone.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable editable) {
                    mPage.getData().putString(AgentReferencePage.MOBILE_NUMBERREST,
                            (editable != null) ? editable.toString() : null);
                    mPage.notifyDataChanged();

                }

            });
        }
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (data != null) {
            ArrayList<String> matches = data.getStringArrayListExtra(
                    RecognizerIntent.EXTRA_RESULTS);

            if (requestCode == REFERNAME1) {
                edtFullnameReference1.setText(matches.get(0));
            } else if (requestCode == REFERNAME2) {
                edtFullnameReference2.setText(matches.get(0));
            } else if (requestCode == REFERREL1) {
                edtRefRel.setText(matches.get(0));
            } else if (requestCode == REFERREL2) {
                edtRefRe2.setText(matches.get(0));
            } else if (requestCode == REFERPHONE1) {
                edtMobileNumber.setText(matches.get(0));
            } else if (requestCode == REFERPHONE2) {
                edtMobileNumber2.setText(matches.get(0));
            } else if (requestCode == MANAGERNAME) {
                managerFullName.setText(matches.get(0));
            }
        }
        super.onActivityResult(requestCode, resultCode, data);

    }

    @Override
    public void onFocusChange(View v, boolean hasFocus) {

        switch (v.getId()) {

            case R.id.edt_relationship:
                if (hasFocus) {
                    relationshipMic.setVisibility(View.VISIBLE);
                } else {
                    relationshipMic.setVisibility(View.GONE);

                }
                break;

            case R.id.edt_relationship2:

                if (hasFocus) {
                    relationshipMic1.setVisibility(View.VISIBLE);
                } else {
                    relationshipMic1.setVisibility(View.GONE);

                }

                break;

            case R.id.edt_full_name_reference:

                if (hasFocus) {
                    fullnameMic.setVisibility(View.VISIBLE);
                } else {
                    fullnameMic.setVisibility(View.GONE);

                }

                break;


            case R.id.edt_full_name_manager:

                if (hasFocus) {
                    managerMic.setVisibility(View.VISIBLE);
                } else {
                    managerMic.setVisibility(View.GONE);

                }

                break;


            case R.id.edt_full_name_reference1:

                if (hasFocus) {
                    fullnameMic1.setVisibility(View.VISIBLE);
                } else {
                    fullnameMic1.setVisibility(View.GONE);

                }

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

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
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
