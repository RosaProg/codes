package deliverytrack.vss.com.deliverytrack;


import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.view.View;
import android.widget.RatingBar;
import android.widget.TextView;

import com.parse.FindCallback;
import com.parse.GetCallback;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.SaveCallback;

import java.io.Serializable;
import java.util.List;
import java.util.Random;

import deliverytrack.vss.com.deliverytrack.Services.SignatureUploadService;


public class RatingActivity extends AppCompatActivity {

    public String ls = "Loading question...";
    public TextView tx;
    public String id_q = "";
    public int answer = 0;
    public int raiting = 0;
    public ParseObject gameScore;
    String ObjectID;
    private ProgressDialog mdialog;
    private byte[] mySignature;


    public int randomWithRange(int min, int max) {
        int r;
        Random myRandom = new Random();
        r = myRandom.nextInt(6);
        if (r == 0) {
            r = 1;
        }
        return r;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //System.out.println("Uppps");
        setContentView(R.layout.rating);

        setTitle(getResources().getString(R.string.feedback));
        mdialog = new ProgressDialog(this);
        mdialog.setTitle("DeliveryTrack");
        mdialog.setMessage("Loading");
        mdialog.setCancelable(false);
        mdialog.setCanceledOnTouchOutside(false);


        Intent b = getIntent();
        ObjectID = b.getStringExtra("object_id");
        mySignature =  b.getByteArrayExtra("parsefile");
        System.out.println(ObjectID);

        tx = (TextView) findViewById(R.id.TextView01);
        //System.out.println("uiiiii");
        ParseQuery<ParseObject> query = ParseQuery.getQuery("Feedback_Question_Bank");
        final int random_question = randomWithRange(1, 5);
        query.findInBackground(new FindCallback<ParseObject>() {

            public void done(List<ParseObject> questionsList, ParseException e) {
                if (e == null) {
                    for (ParseObject question : questionsList) {
                        ls = question.getString("Question").toString();
                        id_q = question.getNumber("Question_ID").toString();
                        //Log.d("ls ", "ls " + ls);
                        String xf = "" + random_question;
                        if (id_q.equals(xf)) {
                            System.out.println("question_selected");
                            tx.setText(ls);
                        }
                    }
                } else {
                    if (id_q.equals("" + random_question + "")) {
                        tx.setText(ls);
                    }
                    //id_q = "";
                    //  Log.d("score", "Error: " + e.getMessage());
                }
            }
        });
        //Log.d("ls ", "ls " + ls);

    }


    @Override
    public void onBackPressed() {
        super.onBackPressed();
    }

    public void sendRating(View v) {
        Integer q = 0;
        String btn = v.getTag().toString();

        if (btn != "Yes") {
            answer = 0;
        } else {
            answer = 1;
        }

        RatingBar rb = (RatingBar) findViewById(R.id.ratingbar);
        raiting = (int) rb.getRating();
        //gameScore = new ParseObject("results");
        new AlertDialog.Builder(this)
                .setTitle("Rating")
                .setMessage("Your raiting is " + raiting + " . The answer is " + v.getTag().toString())
                .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        mdialog.show();
                        ParseQuery<ParseObject> qq = new ParseQuery("Order_Confirmation_Details");
                        qq.whereEqualTo("Date_Time", ObjectID);
                        qq.findInBackground(new FindCallback<ParseObject>() {
                            public void done(List<ParseObject> parseObject, ParseException e) {
                                System.out.println("sddddddddd");
                                if (e == null) {
                                    for (ParseObject qqq : parseObject) {

                                        System.out.println("soooooooo");
                                        qqq.put("Rating", "" + raiting + "");
                                        qqq.put("Question_ID", "" + id_q + "");
                                        qqq.put("Answer_ID", "" + 1 + "");
                                        qqq.saveInBackground(new SaveCallback() {
                                            @Override
                                            public void done(ParseException e) {
                                                mdialog.dismiss();

                                                Intent signatureUpload = new Intent(RatingActivity.this, SignatureUploadService.class);
                                                signatureUpload.putExtra("parsfile",  mySignature);
                                                signatureUpload.putExtra("object_id", ObjectID);
                                                startService(signatureUpload);

                                                moveToOrdersPendingPickup();

                                            }
                                        });
                                    }
                                }
                            }

                        });



                        /*


                         */
                    }
                })
                .setNegativeButton(android.R.string.no, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        //	Toast.makeText(getApplicationContext(), "NOooooooo", Toast.LENGTH_LONG).show();
                        mdialog.show();

                        ParseQuery<ParseObject> qquery = ParseQuery.getQuery("Order_Confirmation_Details");
                        qquery.getInBackground(ObjectID, new GetCallback<ParseObject>() {
                            @Override
                            public void done(ParseObject parseObject, ParseException e) {

                                if (e == null) {
                                    parseObject.put("Rating", raiting);
                                    parseObject.put("Question_ID", id_q);
                                    parseObject.put("Answer_ID", 0);
                                    parseObject.saveInBackground(new SaveCallback() {
                                        @Override
                                        public void done(ParseException e) {

                                            Intent signatureUpload = new Intent(RatingActivity.this, SignatureUploadService.class);
                                            signatureUpload.putExtra("parsfile", mySignature);
                                            signatureUpload.putExtra("object_id", ObjectID);
                                            startService(signatureUpload);

                                            mdialog.dismiss();
                                            moveToOrdersPendingPickup();

                                        }
                                    });
                                }

                            }
                        });

                    }
                })
                .setIcon(android.R.drawable.ic_dialog_alert)
                .show();
    }

    public void moveToOrdersPendingPickup() {

        Intent intent = new Intent(this, BaseActivity.class);
        startActivity(intent);
    }
}
