package deliverytrack.vss.com.deliverytrack.fragments.registration;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.content.res.Configuration;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Matrix;
import android.graphics.Paint;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.support.v4.app.Fragment;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Spinner;

import com.parse.ParseUser;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.interfaces.PageFragmentCallbacks;
import deliverytrack.vss.com.deliverytrack.models.AgentDocumentsPage;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;

/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link AgentDocumentsFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link AgentDocumentsFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class AgentDocumentsFragment extends Fragment implements AdapterView.OnItemClickListener, View.OnClickListener, AdapterView.OnItemSelectedListener {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private static final String ARG_KEY = "key";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    private PageFragmentCallbacks mCallbacks;
    private String mKey;
    private AgentDocumentsPage mPage;
    private EditText edtAdharNumber;
    private Button adharCardBtn;
    private Button otherDocumentBtn;
    private Button selfieBtn;
    private Spinner spinner;
    private Spinner spinnerRest;
    private Button scanBtn;
    String[] categories;
    private ImageView adhar;
    private ImageView other;
    private ImageView selfie;
    private ImageView restDoc;
    private ProgressDialog progressDialog;
    private String filename;


    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment AgentDocumentsFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static AgentDocumentsFragment newInstance(String param1, String param2) {
        AgentDocumentsFragment fragment = new AgentDocumentsFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    public AgentDocumentsFragment() {
        // Required empty public constructor
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Bundle args = getArguments();
        mKey = args.getString(ARG_KEY);
        mPage = (AgentDocumentsPage) mCallbacks.onGetPage(mKey);
        setRetainInstance(true);


    }

    public static AgentDocumentsFragment create(String key) {
        Bundle args = new Bundle();
        args.putString(ARG_KEY, key);
        AgentDocumentsFragment fragment = new AgentDocumentsFragment();
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

            view = inflater.inflate(R.layout.agent_fragment_documents, container, false);

            edtAdharNumber = (EditText) view.findViewById(R.id.enter_your_adhar_number);
            adharCardBtn = (Button) view.findViewById(R.id.id_adhar_card_upload_btn);
            otherDocumentBtn = (Button) view.findViewById(R.id.id_other_card_upload_btn);
            selfieBtn = (Button) view.findViewById(R.id.upload_selfie_btn);
            spinner = (Spinner) view.findViewById(R.id.spinner);

            adhar = (ImageView) view.findViewById(R.id.adhar_card_upload);
            other = (ImageView) view.findViewById(R.id.other_document_upload);
            selfie = (ImageView) view.findViewById(R.id.upload_selfie);

            categories = getResources().getStringArray(R.array.addtional_docs);

            ArrayAdapter dataAdapter = new ArrayAdapter(getActivity(), android.R.layout.simple_spinner_item, categories);
            dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            spinner.setAdapter(dataAdapter);
            spinner.setOnItemSelectedListener(this);

            adharCardBtn.setOnClickListener(this);
            otherDocumentBtn.setOnClickListener(this);
            selfieBtn.setOnClickListener(this);
            edtAdharNumber.setText(user.get("adharCardNo") != null ? user.get("adharCardNo").toString() : "");
            spinner.setSelection(0);
            mPage.getData().putString(AgentDocumentsPage.ADHAR_NO, edtAdharNumber.getText().toString());
            mPage.notifyDataChanged();

            if (edtAdharNumber.getText().toString().length() == 0) {
                edtAdharNumber.setError("AdharNumber is required");
            }
        } else {
            view = inflater.inflate(R.layout.rest_documents, container, false);
            restDoc = (ImageView) view.findViewById(R.id.doc_upload);

            categories = getResources().getStringArray(R.array.entity);
            // Creating adapter for spinner
            ArrayAdapter dataAdapter = new ArrayAdapter(getActivity(), android.R.layout.simple_spinner_item, categories);
            // Drop down layout style - list view with radio button
            dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

            // attaching data adapter to spinner
            spinnerRest = (Spinner) view.findViewById(R.id.spinnerrest);
            spinnerRest.setOnItemSelectedListener(this);
            spinnerRest.setAdapter(dataAdapter);
            spinnerRest.setSelection(0);


            scanBtn = (Button) view.findViewById(R.id.id_doc_upload_btn);
            scanBtn.setOnClickListener(this);

        }

        return view;
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        if (DeliveryTrackUtils.getUserType().equals("Agent")) {
            edtAdharNumber.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                }

                @Override
                public void onTextChanged(CharSequence s, int start, int before, int count) {

                }

                @Override
                public void afterTextChanged(Editable s) {
                    mPage.getData().putString(AgentDocumentsPage.ADHAR_NO,
                            (s != null) ? s.toString() : null);
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
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

    }

    @Override
    public void onClick(View v) {

        switch (v.getId()) {

            case R.id.id_adhar_card_upload_btn:
                filename = "AdharCard.jpeg";
                startCamera(Constants.ADHAR, filename);
                break;

            case R.id.id_other_card_upload_btn:
                filename = "OtherCard.jpeg";
                startCamera(Constants.OTHER, filename);

                break;

            case R.id.upload_selfie_btn:
                filename = "Selfie.jpeg";
                startCamera(Constants.SELFIE, filename);

                break;

            case R.id.id_doc_upload_btn:
                filename = "Document.jpeg";
                startCamera(Constants.DOC, filename);
                break;
        }

    }

    private void startCamera(int reqCode, String fileName) {
        Intent cameraIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        File sdCard = Environment.getExternalStorageDirectory();
        File dir = new File(sdCard.getAbsolutePath() + "/Documents");
        if (!dir.exists()) {
            dir.mkdirs();
        }
        File output = new File(dir, fileName);
        cameraIntent.putExtra(MediaStore.EXTRA_OUTPUT, Uri.fromFile(output));
        startActivityForResult(cameraIntent, reqCode);

    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (resultCode == Activity.RESULT_OK) {
            File sdCard = Environment.getExternalStorageDirectory();
            File dir = new File(sdCard.getAbsolutePath() + "/Documents");
            File file = new File(dir, filename);

            try {
                if (file.exists()) {
                    Bitmap photo = BitmapFactory.decodeFile(file.getAbsolutePath());
                    switch (requestCode) {
                        case Constants.ADHAR:
                            adhar.setImageBitmap(photo);
                            break;
                        case Constants.OTHER:
                            other.setImageBitmap(photo);
                            break;
                        case Constants.SELFIE:
                            selfie.setImageBitmap(photo);
                            break;
                        case Constants.DOC:
                            restDoc.setImageBitmap(photo);
                            break;
                    }

                }
            } catch (OutOfMemoryError error) {
                System.gc();
                DeliveryTrackUtils.showToast(getActivity(), "Please Try again !!!");

            }
        } else {

            DeliveryTrackUtils.showToast(getActivity(), "Please Try again !!!");
        }


    }

    @Override
    public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {

        String value = categories[position];

        if (DeliveryTrackUtils.getUserType().equals("Agent")) {

            mPage.getData().putString(AgentDocumentsPage.DOCS, value);
            mPage.notifyDataChanged();

        } else {
            mPage.getData().putString(AgentDocumentsPage.ENTITY,
                    value);
            mPage.notifyDataChanged();

        }


    }


    public Bitmap getResizedBitmap(Bitmap bitmap, int newWidth, int newHeight) {
//        int width = image.getWidth();
//        int height = image.getHeight();
//
//        int targetWidth = 800; // your arbitrary fixed limit
//        int targetHeight = (int) (width * targetWidth / (double) width); // casts to avoid truncating
//
//
////        float bitmapRatio = (float)width / (float) height;
////        if (bitmapRatio > 0) {
////            width = maxSize;
////            height = (int) (width / bitmapRatio);
////        } else {
////            height = maxSize;
////            width = (int) (height * bitmapRatio);
////        }
//        return Bitmap.createScaledBitmap(image, targetWidth, targetHeight, true);

        Bitmap scaledBitmap = Bitmap.createBitmap(newWidth, newHeight, Bitmap.Config.ARGB_8888);

        float ratioX = newWidth / (float) bitmap.getWidth();
        float ratioY = newHeight / (float) bitmap.getHeight();
        float middleX = newWidth / 2.0f;
        float middleY = newHeight / 2.0f;

        Matrix scaleMatrix = new Matrix();
        scaleMatrix.setScale(ratioX, ratioY, middleX, middleY);

        Canvas canvas = new Canvas(scaledBitmap);
        canvas.setMatrix(scaleMatrix);
        canvas.drawBitmap(bitmap, middleX - bitmap.getWidth() / 2, middleY - bitmap.getHeight() / 2,
                new Paint(Paint.FILTER_BITMAP_FLAG));

        return scaledBitmap;

    }

    @Override
    public void onNothingSelected(AdapterView<?> parent) {

    }


//    private void getFileFromServer() {
//        progressDialog = ProgressDialog.show(getActivity(), "",
//                "Downloading Image...", true);
//
//        // Locate the class table named "ImageUpload" in Parse.com
//        ParseQuery<ParseObject> query = new ParseQuery<ParseObject>(
//                "ImageUpload");
//
//        // Locate the objectId from the class
//        query.getInBackground(ParseUser.getCurrentUser(),
//                new GetCallback<ParseObject>() {
//
//                    public void done(ParseObject object,
//                                     ParseException e) {
//                        // TODO Auto-generated method stub
//
//                        // Locate the column named "ImageName" and set
//                        // the string
//                        ParseFile fileObject = (ParseFile) object
//                                .get("ImageFile");
//                        fileObject
//                                .getDataInBackground(new GetDataCallback() {
//
//                                    public void done(byte[] data,
//                                                     ParseException e) {
//                                        if (e == null) {
//                                            Log.d("test",
//                                                    "We've got data in data.");
//                                            // Decode the Byte[] into
//                                            // Bitmap
//                                            Bitmap bmp = BitmapFactory
//                                                    .decodeByteArray(
//                                                            data, 0,
//                                                            data.length);
//
//                                            // Get the ImageView from
//                                            // main.xml
//                                            ImageView image = (ImageView) findViewById(R.id.image);
//                                            image.setImageBitmap(bmp);
//
//                                            // Close progress dialog
//                                            progressDialog.dismiss();
//
//                                        } else {
//                                            Log.d("test",
//                                                    "There was a problem downloading the data.");
//                                        }
//                                    }
//                                });
//                    }
//                });
//    }


    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
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
