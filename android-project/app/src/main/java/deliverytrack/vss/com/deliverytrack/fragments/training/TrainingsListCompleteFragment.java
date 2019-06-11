package deliverytrack.vss.com.deliverytrack.fragments.training;

import android.content.Context;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import java.util.ArrayList;


import deliverytrack.vss.com.deliverytrack.BaseActivity;
import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.TrainingActivity;
import deliverytrack.vss.com.deliverytrack.models.TrainingModel;

/**
 * Created by anna on 07.07.15.
 */
public class TrainingsListCompleteFragment extends Fragment {
    @Override
    public void onResume() {
        super.onResume();
        getActivity()
                .setTitle("My Training");
    }

    private ListView listview;
    private TrainingsListAdapter adapter;
    private ArrayList<TrainingModel> mTrainingCompleteModels;

    public static TrainingsListCompleteFragment newInstance() {
        TrainingsListCompleteFragment pageFragment = new TrainingsListCompleteFragment();
        Bundle arguments = new Bundle();
        pageFragment.setArguments(arguments);
        return pageFragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_trainings_list, null);

        listview = (ListView) view.findViewById(R.id.listview);
        adapter = new TrainingsListAdapter();
        listview.setAdapter(adapter);

        setupAssinged();
        return view;
    }

    private void setupAssinged() {
        if (mTrainingCompleteModels == null)
            return;

        adapter.addItems(mTrainingCompleteModels);
    }

    public void setTrainings(ArrayList<TrainingModel> tms) {
        mTrainingCompleteModels = tms;
        setupAssinged();
    }


    public class TrainingItemView extends LinearLayout {
        private TextView training_name;
        private TextView date;

        public TrainingItemView(Context context) {
            super(context);

            View view;
            LayoutInflater inflater = (LayoutInflater)
                    context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            view = inflater.inflate(R.layout.item_training_list, null);

            training_name = (TextView) view.findViewById(R.id.name);
            date = (TextView) view.findViewById(R.id.date);

            this.addView(view);
        }

        public void update(TrainingModel item) {
            training_name.setText(item.getVideoTitle());
            date.setText("");
        }
    }

    public class TrainingsListAdapter extends BaseAdapter implements View.OnClickListener {
        private ArrayList<TrainingModel> innerSourceList;

        public TrainingsListAdapter() {
            innerSourceList = new ArrayList<TrainingModel>();
        }

        @Override
        public int getCount() {
            return innerSourceList.size();
        }

        @Override
        public TrainingModel getItem(int position) {
            return innerSourceList.get(position);
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            TrainingItemView trainingItemView;

            if (convertView == null) {
                trainingItemView = new TrainingItemView(getActivity());
            } else {
                trainingItemView = (TrainingItemView) convertView;
            }

            final TrainingModel item = innerSourceList.get(position);
            trainingItemView.update(item);


            trainingItemView.setTag(position);
            trainingItemView.setOnClickListener(this);
            return trainingItemView;
        }

        public void setData(ArrayList<TrainingModel> mSourceList) {
            innerSourceList = mSourceList;
            notifyDataSetChanged();
        }

        public void addItems(ArrayList<TrainingModel> mSourceList) {
            innerSourceList.addAll(mSourceList);
            notifyDataSetChanged();
        }

        public ArrayList<TrainingModel> getItems() {
            return innerSourceList;
        }

        @Override
        public long getItemId(int position) {
            return 0;
        }

        @Override
        public void onClick(View v) {
            int position = (Integer) v.getTag();
            TrainingModel training = getItem(position);
            ((TrainingActivity) getActivity()).getTrainingVideo(training, true, position);

        }
    }


}
