package deliverytrack.vss.com.deliverytrack;

import android.content.Context;
import android.content.res.Configuration;
import android.location.Location;
import android.os.Bundle;


import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.Toast;

import com.astuetz.PagerSlidingTabStrip;
import com.parse.ParseException;
import com.parse.ParseGeoPoint;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;
import com.parse.SaveCallback;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.List;
import java.util.Locale;

import deliverytrack.vss.com.deliverytrack.asynch.FindForQuery;
import deliverytrack.vss.com.deliverytrack.fragments.training.TrainingTestFragment;
import deliverytrack.vss.com.deliverytrack.fragments.training.TrainingVideoFragment;
import deliverytrack.vss.com.deliverytrack.fragments.training.TrainingsListAssignedFragment;
import deliverytrack.vss.com.deliverytrack.fragments.training.TrainingsListCompleteFragment;
import deliverytrack.vss.com.deliverytrack.fragments.training.TraningsFragment;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.models.Questions;
import deliverytrack.vss.com.deliverytrack.models.TrainingModel;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.LocationUtils;

public class TrainingActivity extends AppCompatActivity implements OrderCallbacks {

    private Boolean update;
    private int completedTestNo =0;
    private TrainingModel saveTraining;
    private int saveScore;
    private LocationUtils mLocationUtils;
    private ArrayList<TrainingModel> completedTrainingList = new ArrayList<>();
    private ArrayList<TrainingModel> inCompleteTraining = new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_training);

        getTrainigs(true);


    }


    public void getInCompletedTrainingDetails(int completed) {
        ParseQuery query = new ParseQuery("Training");
        query.whereEqualTo("userType", DeliveryTrackUtils.getUserType());
        query.whereGreaterThan("trainingNum", completed);
        query.whereEqualTo("language", "en");
        FindForQuery findQuery = new FindForQuery(query, this, null, 115);
        findQuery.execute();
    }

    public void getCompletedTrainingDetails(int completed) {
        ParseQuery query = new ParseQuery("Training");
        query.whereEqualTo("userType", DeliveryTrackUtils.getUserType());
        query.whereLessThanOrEqualTo("trainingNum", completed);
        query.whereEqualTo("language", "en");
        FindForQuery findQuery = new FindForQuery(query, this, null, 116);
        findQuery.execute();
    }

    public void getCompletedTrainingForUser() {
        ParseQuery query = new ParseQuery("CompletedTrainings");
        query.whereEqualTo("userId", ParseUser.getCurrentUser().getObjectId());
        FindForQuery findQuery = new FindForQuery(query, this, null, 114);
        findQuery.execute();
    }


    public void saveCompleteTraining(TrainingModel training, int score, boolean playNext, int position) {

        this.saveScore = score;
        this.saveTraining = training;

        if (score > 8) {
            Date date = new Date();
            ParseQuery query = new ParseQuery("CompletedTrainings");
            query.whereEqualTo("userId", ParseUser.getCurrentUser().getObjectId());
            FindForQuery findQuery = new FindForQuery(query, this, null, 444);
            findQuery.execute();

            if (playNext) {
                TraningsFragment tmf = (TraningsFragment) getSupportFragmentManager().findFragmentByTag("TraningsFragment");
                TrainingModel trainingModel = tmf.getAssignedFragment().getmTrainingAsssignedModels().get(position);
                getTrainingVideo(trainingModel, false, position);
            }

        } else {
            getSupportFragmentManager().popBackStack(null, FragmentManager.POP_BACK_STACK_INCLUSIVE);
            Toast.makeText(this, "" +
                    "Test score is "
                    + score + " of 10. Try again!", Toast.LENGTH_LONG).show();
            getTrainigs(true);

        }


    }

    private void getTrainigs(boolean b) {
        TraningsFragment opf = new TraningsFragment();
        FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        transaction.replace(R.id.container, opf, "TraningsFragment");
        transaction.addToBackStack(null);
        transaction.commit();
        getCompletedTrainingForUser();
    }

    public void getTrainigTest(TrainingModel training) {
        this.saveTraining = training;
        getQuestionsForTraining(training);
    }


    private void getQuestionsForTraining(TrainingModel training) {

        Configuration con = getResources().getConfiguration();
        Locale locale = Locale.getDefault();
        ParseQuery query = new ParseQuery("TrainingQuestion");
        query.whereEqualTo("language", "en");
        query.whereEqualTo("userType", DeliveryTrackUtils.getUserType());
        FindForQuery findForQuery = new FindForQuery(query, this, null, 185);
        findForQuery.execute();
    }

    public void getTrainingVideo(TrainingModel training, Boolean complete, int position) {

        if (training.getTest_no() == completedTestNo + 1 || training.getTest_no() <= completedTestNo) {
            // completedTestNo + 1 next test
            // < completedtestno already taken

            if (training == null) {
                Toast.makeText(this, "Can't find this training info", Toast.LENGTH_LONG).show();
                return;
            }

            TrainingVideoFragment opf = new TrainingVideoFragment();
            training.setIsComplete(complete);
            opf.setPosition(position);
            opf.setTraining(training);
            FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
            transaction.replace(R.id.container, opf, "TrainingVideoFragment");
            transaction.addToBackStack(null);
            transaction.commit();

        } else {
            DeliveryTrackUtils.showToast(this, "Kindly complete the previous test");
        }
    }


    public TrainingsListAssignedFragment getAssignedFragment() {
        return AssignedFragment;
    }

    public TrainingsListAssignedFragment AssignedFragment;
    public TrainingsListCompleteFragment CompleteFragment;


    public void setIsUpdate(Boolean update) {
        this.update = update;
    }

    @Override
    public void getParseObjects(List<ParseObject> parseObjects, int tableId) {

        if (tableId == 114) {  //completed number and not completed list
            if (parseObjects != null && parseObjects.size() > 0) {
                ParseObject training = parseObjects.get(0);
                completedTestNo = (Integer) training.get("trainingNum");
                /*To get from the */
            }
            getInCompletedTrainingDetails(completedTestNo);
            getCompletedTrainingDetails(completedTestNo);
        } else if (tableId == 115) {  //trainings
            inCompleteTraining.clear();
            for (ParseObject training : parseObjects) {
                TrainingModel tm = new TrainingModel();

                tm.setYoutube_link(training.get("youtubeLink").toString());
                tm.setTest_no((Integer) training.get("trainingNum"));
                tm.setUserType(training.get("userType").toString());
                tm.setLanguage(training.get("language").toString());
                tm.setIsQuestion((boolean) training.get("isQuestion"));
                tm.setVideoTitle(training.get("videoTitle").toString());

                inCompleteTraining.add(tm);
                Log.d("the completed training", inCompleteTraining.size() + "  ");
            }
            onTrainingsObtained(inCompleteTraining);
        } else if (tableId == 116) {  //trainings
            completedTrainingList.clear();
            for (ParseObject training : parseObjects) {
                TrainingModel tm = new TrainingModel();
                tm.setYoutube_link(training.get("youtubeLink").toString());
                tm.setTest_no((Integer) training.get("trainingNum"));
                tm.setUserType(training.get("userType").toString());
                tm.setLanguage(training.get("language").toString());
                tm.setIsQuestion((boolean) training.get("isQuestion"));
                tm.setVideoTitle(training.get("videoTitle").toString());
                completedTrainingList.add(tm);
            }
            onCompleteTrainingsObtained(completedTrainingList);

        } else if (tableId == 444) {
            if (parseObjects != null && parseObjects.size() > 0) {
                ParseObject training = parseObjects.get(0);
                completedTestNo = (Integer) training.get("trainingNum");
            }

            if (mLocationUtils == null)
                mLocationUtils = new LocationUtils(this);
            Location loc = mLocationUtils.getLocation();
            if (loc == null)
                return;

            ParseUser user = ParseUser.getCurrentUser();
            ParseGeoPoint point = new ParseGeoPoint(loc.getLatitude(), loc.getLongitude());
            final int SCORE = TrainingActivity.this.saveScore;
            final Context context = this;

            ParseObject testScore = null;
            if (completedTestNo == 0) {
                testScore = new ParseObject("CompletedTrainings");
                testScore.put("location", point);
                testScore.put("trainingNum", saveTraining.getTest_no());
                testScore.put("userType", DeliveryTrackUtils.getUserType());
                testScore.put("score", SCORE);
                testScore.put("username", ParseUser.getCurrentUser().getUsername());
                testScore.put("userId", ParseUser.getCurrentUser().getObjectId());
                testScore.put("language","en");
            } else {
                testScore = parseObjects.get(0);
                testScore.put("trainingNum", saveTraining.getTest_no());
                testScore.put("score", SCORE);
                testScore.put("language","en");

            }
            testScore.saveInBackground(new SaveCallback() {
                public void done(ParseException e) {
                    TrainingActivity.this.saveTraining = null;
                    TrainingActivity.this.saveScore = 0;
                    Toast.makeText(context, "Congratulations! You passed test! Test score is "
                            + SCORE + " of 10", Toast.LENGTH_LONG).show();

                    TrainingActivity.this.finish();

                }
            });
        } else if (tableId == 185) {
            if (parseObjects != null && parseObjects.size() > 0) {
                ArrayList<Questions> questionList = new ArrayList<>();
                for (ParseObject question : parseObjects) {
                    Questions questions = new Questions();
                    questions.setTest_no((Integer) question.get("test_id"));
                    questions.setCorrectAnswer(question.get("correctAnswer").toString());
                    questions.setUserType(question.get("userType").toString());
                    questions.setOptions(question.get("Options").toString());
                    questions.setLanguage(question.get("language").toString());
                    questions.setSerialNo(question.get("SerialNo").toString());
                    questions.setQuestion(question.get("Question").toString());
                    questionList.add(questions);
                }
                Collections.reverse(questionList);
                TrainingTestFragment opf = new TrainingTestFragment();
                opf.setTraining(this.saveTraining);
                opf.setQuestions(questionList);
                FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
                transaction.replace(R.id.container, opf, "TrainingTestFragment");
                transaction.addToBackStack(null);
                transaction.commit();
                this.saveTraining = null;

            }

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


    public void onCompleteTrainingsObtained(ArrayList<TrainingModel> tms) {
        TraningsFragment tmf = (TraningsFragment) getSupportFragmentManager().findFragmentByTag("TraningsFragment");
        Collections.reverse(tms);
        tmf.setComplete(tms);
    }


    public void onTrainingsObtained(ArrayList<TrainingModel> tms) {
        TraningsFragment tmf = (TraningsFragment) getSupportFragmentManager().findFragmentByTag("TraningsFragment");
        Collections.reverse(tms);
        tmf.setAssigned(tms);
    }


}
