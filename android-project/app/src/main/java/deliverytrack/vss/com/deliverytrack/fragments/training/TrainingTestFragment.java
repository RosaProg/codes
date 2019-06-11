package deliverytrack.vss.com.deliverytrack.fragments.training;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.RadioButton;
import android.widget.TextView;

import com.parse.ParseObject;

import java.util.ArrayList;
import java.util.List;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.TrainingActivity;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.models.Questions;
import deliverytrack.vss.com.deliverytrack.models.TrainingModel;
import deliverytrack.vss.com.deliverytrack.utility.LocationUtils;

/**
 * Created by anna on 02.07.15.
 */
public class TrainingTestFragment extends Fragment implements OrderCallbacks {
    private String UserId;

    private static final String USER_ID = "userID";

    private TextView question_number;
    private TextView question;
    private Button button;
    private RadioButton radio[];

    private TrainingModel training;
    private ArrayList<Questions> questionsList = new ArrayList<>();
    private String[] optionsArrays;
    private boolean isQuestions;

    @Override
    public void onResume() {
        super.onResume();
        getActivity()
                .setTitle("My Training");
    }

    private int question_num = 1;
    private int score;
    private String current_answer;
    private LocationUtils mLocationUtils;

    public TrainingTestFragment() {
        // Required empty public constructor
    }


    public void setTraining(TrainingModel training) {
        this.training = training;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            UserId = getArguments().getString(USER_ID);
        }

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_training_test, container, false);

        question_number = (TextView) view.findViewById(R.id.question_number);
        question = (TextView) view.findViewById(R.id.question);
        button = (Button) view.findViewById(R.id.button);
        radio = new RadioButton[4];
        radio[0] = (RadioButton) view.findViewById(R.id.radio1);
        radio[1] = (RadioButton) view.findViewById(R.id.radio2);
        radio[2] = (RadioButton) view.findViewById(R.id.radio3);
        radio[3] = (RadioButton) view.findViewById(R.id.radio4);


        score = 0;
        current_answer = "";

        update();


        for (int i = 0; i < optionsArrays.length; i++) {
            radio[i].setChecked(false);
        }
        button.setEnabled(false);

        button.setText("NEXT");
        question_num = 1;

        question_number.setText("QUESTION " + question_num + "  OF 10");


        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                Questions ques = questionsList.get(question_num - 1);

                if (ques.getCorrectAnswer().equals(current_answer)) {
                    score++;
                }
                if (question_num == questionsList.size()) {
                    if (score > 10) {
                        score = 10;
                    }
                    Log.d("the value", "Inside training test frgament");
                    ((TrainingActivity ) getActivity()).saveCompleteTraining(training, score, false, 0);

                } else {
                    question_num++;
                    question_number.setText("QUESTION " + question_num + "  OF 10");
                    if (question_num == questionsList.size())
                        button.setText("SUBMIT");

                    for (int i = 0; i < optionsArrays.length; i++) {
                        radio[i].setChecked(false);
                    }
                    button.setEnabled(false);
                    update();
                }
            }
        });

        for (int x = 0; x < optionsArrays.length; x++) {
            final String ans = (String) radio[x].getTag(x);
            radio[x].setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    button.setEnabled(true);
                    for (int i = 0; i < optionsArrays.length; i++) {
                        radio[i].setChecked(false);
                        ((RadioButton) v).setChecked(true);
                        current_answer = (String) v.getTag();
                        Log.d("current_answer", current_answer + "");
                    }
                }
            });
        }
        return view;
    }

    private void update() {

        Questions ques = questionsList.get(question_num - 1);
        String options = ques.getOptions();
        optionsArrays = new String[2];

        if (options != null) {
            optionsArrays = options.split(",");
        }
        question.setText(ques.getQuestion());
        radio[0].setText("a. " + optionsArrays[0]);
        radio[0].setTag("a");
        if (optionsArrays.length > 1) {
            radio[1].setText("b. " + optionsArrays[1]);
            radio[1].setTag("b");
        }

        if (optionsArrays.length > 2) {
            radio[2].setText("c. " + optionsArrays[2]);
            radio[2].setVisibility(View.VISIBLE);
            radio[2].setTag("c");

        } else {
            radio[2].setVisibility(View.GONE);
        }
        if (optionsArrays.length > 3) {
            radio[3].setVisibility(View.VISIBLE);
            radio[3].setText("d. " + optionsArrays[3]);
            radio[3].setTag("d");

        } else {
            radio[3].setVisibility(View.GONE);
        }
    }


    @Override
    public void getParseObjects(List<ParseObject> parseObjects, int tableId) {
        Log.d("the questions", parseObjects.size() + "");
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

    public void setQuestions(ArrayList<Questions> questionList) {

        this.questionsList = questionList;
    }
}
