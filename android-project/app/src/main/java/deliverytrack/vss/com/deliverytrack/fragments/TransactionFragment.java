package deliverytrack.vss.com.deliverytrack.fragments;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;

import com.itextpdf.text.BaseColor;
import com.itextpdf.text.Document;
import com.itextpdf.text.DocumentException;
import com.itextpdf.text.Element;
import com.itextpdf.text.Font;
import com.itextpdf.text.Paragraph;
import com.itextpdf.text.Phrase;
import com.itextpdf.text.Section;
import com.itextpdf.text.pdf.PdfPCell;
import com.itextpdf.text.pdf.PdfPTable;
import com.itextpdf.text.pdf.PdfWriter;
import com.parse.GetCallback;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.adapters.TransactionAdapter;
import deliverytrack.vss.com.deliverytrack.asynch.FindForQuery;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.models.OrderTransaction;
import deliverytrack.vss.com.deliverytrack.models.TransactionModel;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.ParseHelper;
import deliverytrack.vss.com.deliverytrack.utility.pdfwriter.PDFUtils;

/**
 * Created by Adi-Loch on 6/24/2015.
 */

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link TransactionFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link TransactionFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class TransactionFragment extends Fragment implements OrderCallbacks, View.OnClickListener {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private ArrayList<TransactionModel> tms = new ArrayList<>();
    private TransactionAdapter transactionAdapter;
    private double sum;
    private TextView tv;
    private TextView tvMonthSum;
    private TextView balance;
    private ArrayList<OrderTransaction> orderTransactions = new ArrayList<>();
    private File myFile;

    private String daname = "Delivery Agent Name";
    private String daid = "Delivery Agent Id";

    private String raname = "Restaurant Name";
    private String raid = "Restaurant Id";


    private static Font catFont = new Font(Font.FontFamily.TIMES_ROMAN, 18,
            Font.BOLD);
    private static Font redFont = new Font(Font.FontFamily.TIMES_ROMAN, 12,
            Font.NORMAL, BaseColor.RED);
    private static Font subFont = new Font(Font.FontFamily.TIMES_ROMAN, 16,
            Font.BOLD);
    private static Font smallBold = new Font(Font.FontFamily.TIMES_ROMAN, 12,
            Font.BOLD);


    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment TransactionFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static TransactionFragment newInstance(String param1, String param2) {
        TransactionFragment fragment = new TransactionFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public TransactionFragment() {
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


    private void fecthTransactionQueries() {
        //TODO to get the sum of today's credit make use of today's credit query
        getTodayEarning();

    }


    private void fecthOrderTransactionQueries() {
        //TODO to get the sum of today's credit make use of today's credit query
        ParseQuery query = DeliveryTrackUtils.getQueryForTodayEarning();
        FindForQuery findQuery = new FindForQuery(query, getActivity(), this, 100);
        findQuery.execute();


    }


    private void getTodayEarning() {
        ParseQuery query = DeliveryTrackUtils.getQueryForTodayEarning();
        FindForQuery findForQuery = new FindForQuery(query, getActivity(), this, 666);
        findForQuery.execute();
    }


    private void viewPdf(File myFile) {
        Intent intent = new Intent(Intent.ACTION_VIEW);
        intent.setDataAndType(Uri.fromFile(myFile), "application/pdf");
        intent.setFlags(Intent.FLAG_ACTIVITY_NO_HISTORY);
        startActivity(intent);
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_transaction, container, false);
        ListView lv = (ListView) view.findViewById(R.id.transaction_list);
        transactionAdapter = new TransactionAdapter(getActivity(), tms);
        lv.setAdapter(transactionAdapter);
        Button btn = (Button) view.findViewById(R.id.view_file);
        btn.setOnClickListener(this);

        tv = (TextView) view.findViewById(R.id.sum);
        tvMonthSum = (TextView) view.findViewById(R.id.monthSum);
        balance = (TextView) view.findViewById(R.id.user_total);
        fecthTransactionQueries();
        fecthOrderTransactionQueries();

        getUserData();
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

    public void setTransactions(ArrayList<TransactionModel> tms) {
        this.tms = tms;
        transactionAdapter.setItems(tms);
        transactionAdapter.notifyDataSetChanged();
    }

    public void setSum(double sum) {
        this.sum = sum;
        tv.setText("Today's Earning : " + String.valueOf(DeliveryTrackUtils.round(this.sum, 2)));
    }

    public void setMonthSum(double monthSum) {
        tvMonthSum.setText("Month's Earning :" + String.valueOf(DeliveryTrackUtils.round(monthSum, 2)));
    }

    @Override
    public void getParseObjects(List<ParseObject> parseObjects, int tableId) {

        Date date = new Date();
        double sum = 0;
        if (tableId == 666) {
            if (tms.size() > 0) {
                tms.clear();
            }
            //    double monthSum = 0;
            for (ParseObject transactionModels : parseObjects) {
                TransactionModel tm = new TransactionModel();
                Object userCredit = transactionModels.get("credit");
                if (userCredit instanceof Double) {
                    tm.setUserCredit((Double) transactionModels.get("credit"));
                } else if (userCredit instanceof Integer) {
                    tm.setUserCredit(((Integer) userCredit).doubleValue());
                }

                Object userDebit = transactionModels.get("debit");
                if (userDebit instanceof Double) {
                    tm.setUserDebit((Double) transactionModels.get("debit"));
                } else if (userDebit instanceof Integer) {
                    tm.setUserDebit(((Integer) userDebit).doubleValue());
                }
                tm.setDateCreated(transactionModels.getCreatedAt());


                if (tm.getDateCreated() != null && DeliveryTrackUtils.removeTime(date).compareTo(DeliveryTrackUtils.removeTime(tm.getDateCreated())) == 0) {
                    sum = sum + tm.getUserCredit();
                }
                tms.add(tm);
                setSum(sum);
            }
            //    setMonthSum(monthSum);
            setTransactions(tms);

        } else if (tableId == 100) {
            orderTransactions.clear();
            for (ParseObject transactionModels : parseObjects) {
                OrderTransaction orderTransaction = ParseHelper.getOrderTransaction(transactionModels);
                orderTransactions.add(orderTransaction);
            }
            myFile = PDFUtils.createPDF(orderTransactions, DeliveryTrackUtils.round(this.sum, 2));
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

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        getActivity()
                .setTitle(getString(R.string.my_transactions_title));
    }

    @Override
    public void onResume() {
        super.onResume();
        // Set title
        getActivity()
                .setTitle(getString(R.string.my_transactions_title));
    }

    private void getUserData() {
        ParseUser currentUser = ParseUser.getCurrentUser();
        currentUser.fetchInBackground(new GetCallback<ParseObject>() {
            public void done(ParseObject object, ParseException e) {
                if (e == null) {
                    ParseUser currUser = (ParseUser) object;

                    if (currUser.get("total") instanceof Integer) {
                        balance.setText("Your Balance : " + DeliveryTrackUtils.round((Integer) currUser.get("total"), 2));

                    } else if (currUser.get("total") instanceof Double) {
                        balance.setText("Your Balance : " + DeliveryTrackUtils.round((Double) currUser.get("total"), 2));

                    }
                } else {
                }
            }
        });
    }


    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.view_file:
                if (myFile != null) {
                    viewPdf(myFile);
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

}
