package deliverytrack.vss.com.deliverytrack.fragments.training;

import android.content.res.Configuration;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.astuetz.PagerSlidingTabStrip;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.models.TrainingModel;

/**
 * Created by anna on 02.07.15.
 */
public class TraningsFragment extends Fragment {
    private String UserId;

    private static final String USER_ID = "userID";
    private static final int PAGE_COUNT = 2;

    private ViewPager pager;
    private PagerAdapter pagerAdapter;
    private Boolean update;
    private ArrayList<TrainingModel> trainingsList;


    public TraningsFragment() {
        // Required empty public constructor
    }

    public void setAssigned(ArrayList<TrainingModel> tms) {
        AssignedFragment.setTrainings(tms);
    }


    public void setComplete(ArrayList<TrainingModel> tms) {
        CompleteFragment.setTrainings(tms);
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            UserId = getArguments().getString(USER_ID);
        }
    }


    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        getActivity()
                .setTitle(getString(R.string.my_training_title));

    }

    @Override
    public void onResume() {
        super.onResume();
        // Set title
        getActivity()
                .setTitle(getString(R.string.my_training_title));

       // ((BaseActivity)getActivity()).getTrainigs(true);
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_trainings, container, false);

        //init views
        pager = (ViewPager) view.findViewById(R.id.viewpager);

        pagerAdapter = new MyFragmentPagerAdapter(getChildFragmentManager());
        pager.setAdapter(pagerAdapter);

        PagerSlidingTabStrip tabsStrip = (PagerSlidingTabStrip) view.findViewById(R.id.tabs);
        // Attach the view pager to the tab strip
        tabsStrip.setViewPager(pager);


        tabsStrip.setOnPageChangeListener(new ViewPager.OnPageChangeListener() {
            @Override
            public void onPageSelected(int position) {
            }

            @Override
            public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {
            }

            @Override
            public void onPageScrollStateChanged(int state) {
            }
        });


        return view;

    }

    public TrainingsListAssignedFragment getAssignedFragment() {
        return AssignedFragment;
    }

    public TrainingsListAssignedFragment AssignedFragment;
    public TrainingsListCompleteFragment CompleteFragment;

    public void setIsUpdate(Boolean update) {
        this.update = update;
    }

    private class MyFragmentPagerAdapter extends FragmentPagerAdapter {

        public MyFragmentPagerAdapter(android.support.v4.app.FragmentManager fm) {
            super(fm);
        }

        @Override
        public Fragment getItem(int position) {
            if (position == 0) {
                AssignedFragment = TrainingsListAssignedFragment.newInstance();
                return AssignedFragment;
            } else {
                CompleteFragment = TrainingsListCompleteFragment.newInstance();
                return CompleteFragment;
            }
        }

        @Override
        public int getCount() {
            return PAGE_COUNT;
        }

        @Override
        public CharSequence getPageTitle(int position) {
            return (position == 0) ? getString(R.string.assigned_trainings) :
                    getString(R.string.complete_trainings);
        }


    }


}
