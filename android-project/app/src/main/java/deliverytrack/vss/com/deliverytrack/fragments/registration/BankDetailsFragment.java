package deliverytrack.vss.com.deliverytrack.fragments.registration;

import android.app.Activity;
import android.content.Context;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.text.Editable;
import android.text.InputFilter;
import android.text.Spanned;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.inputmethod.InputMethodManager;
import android.widget.EditText;

import com.parse.ParseUser;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.interfaces.PageFragmentCallbacks;
import deliverytrack.vss.com.deliverytrack.models.BankDetailsPage;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link BankDetailsFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link BankDetailsFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class BankDetailsFragment extends Fragment {
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
    private BankDetailsPage mPage;
    private PageFragmentCallbacks mCallbacks;
    private EditText edtBankAcc;
    private EditText edtBankName;
    private EditText edtExactName;
    private EditText edtIfscCode;

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
        // Required empty public constructor
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Bundle args = getArguments();
        mKey = args.getString(ARG_KEY);
        mPage = (BankDetailsPage) mCallbacks.onGetPage(mKey);
        setRetainInstance(true);


    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_bank_details, container, false);
        ParseUser user = ParseUser.getCurrentUser();
        edtBankAcc = (EditText) view.findViewById(R.id.account_number);
        edtBankName = (EditText) view.findViewById(R.id.bank_name);
        edtExactName = (EditText) view.findViewById(R.id.exact_name);
        edtIfscCode = (EditText) view.findViewById(R.id.ifsc_code);

        edtBankName.setFilters(new InputFilter[]{filtertxt});
        edtExactName.setFilters(new InputFilter[]{filtertxt});

        edtBankAcc.setText(user.get("bankaccount") != null ? user.get("bankaccount").toString() : "");
        edtBankName.setText(user.get("bankname") != null ? user.get("bankname").toString() : "");
        edtExactName.setText(user.get("exactname") != null ? user.get("exactname").toString() : "");
        edtIfscCode.setText(user.get("bankifsc") != null ? user.get("bankifsc").toString() : "");


        if (edtBankAcc.getText().toString().length() == 0) {
            edtBankAcc.setError("Bank Account number is required");
        }
        if (edtBankName.getText().toString().length() == 0) {
            edtBankName.setError("Bank name is required");
        }
        if (edtExactName.getText().toString().length() == 0) {
            edtExactName.setError("Exact name is required");
        }
        if (edtIfscCode.getText().toString().length() == 0) {
            edtIfscCode.setError("Ifscode is required");
        }

        mPage.getData().putString(BankDetailsPage.BANK_NAME, edtBankName.getText().toString());
        mPage.getData().putString(BankDetailsPage.BANK_ACCOUNT, edtBankAcc.getText().toString());
        mPage.getData().putString(BankDetailsPage.EXACT_NAME, edtExactName.getText().toString());
        mPage.getData().putString(BankDetailsPage.IFSCCODE, edtIfscCode.getText().toString());

        mPage.notifyDataChanged();
        return view;
    }

    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        edtBankAcc.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable editable) {
                mPage.getData().putString(BankDetailsPage.BANK_ACCOUNT,
                        (editable != null) ? editable.toString() : null);
                mPage.notifyDataChanged();


            }
        });

        edtBankName.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable editable) {
                mPage.getData().putString(BankDetailsPage.BANK_NAME,
                        (editable != null) ? editable.toString() : null);
                mPage.notifyDataChanged();

            }
        });

        edtExactName.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable editable) {
                mPage.getData().putString(BankDetailsPage.EXACT_NAME,
                        (editable != null) ? editable.toString() : null);
                mPage.notifyDataChanged();

            }
        });

        edtIfscCode.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable editable) {
                mPage.getData().putString(BankDetailsPage.IFSCCODE,
                        (editable != null) ? editable.toString() : null);
                mPage.notifyDataChanged();

            }
        });


    }

    @Override
    public void onAttach(Activity activity) {
        super.onAttach(activity);
        if (!(activity instanceof PageFragmentCallbacks)) {
            throw new ClassCastException("Activity must implement PageFragmentCallbacks");
        }
        mCallbacks = (PageFragmentCallbacks) activity;

    }

    public static BankDetailsFragment create(String key) {
        Bundle args = new Bundle();
        args.putString(ARG_KEY, key);
        BankDetailsFragment fragment = new BankDetailsFragment();
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
        mCallbacks = null;
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

        if (edtBankName != null && edtBankAcc != null && edtExactName != null &&
                edtIfscCode != null) {
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
