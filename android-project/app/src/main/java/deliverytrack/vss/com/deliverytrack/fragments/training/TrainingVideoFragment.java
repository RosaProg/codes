package deliverytrack.vss.com.deliverytrack.fragments.training;

import com.google.android.youtube.player.YouTubeInitializationResult;
import com.google.android.youtube.player.YouTubePlayer;
import com.google.android.youtube.player.YouTubePlayerSupportFragment;

import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.Toast;

import java.util.ArrayList;

import deliverytrack.vss.com.deliverytrack.BaseActivity;
import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.TrainingActivity;
import deliverytrack.vss.com.deliverytrack.VideoFullScreenActivity;
import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.models.Questions;
import deliverytrack.vss.com.deliverytrack.models.TrainingModel;

/**
 * Created by anna on 06.07.15.
 */

public class TrainingVideoFragment extends Fragment implements
        YouTubePlayer.OnInitializedListener,
        YouTubePlayer.PlaybackEventListener, YouTubePlayer.PlayerStateChangeListener {

    private Button take_test;
    private TrainingModel mTrainingModel;
    private Boolean fullScreen = false;
    private YouTubePlayer videoPlayer;
    private YouTubePlayerSupportFragment youTubePlayerFragment;
    private ArrayList<Questions> questionList;
    private Button nextVideo;
    private TrainingModel nextTraining;
    private int position;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater layoutInflater, ViewGroup viewGroup, Bundle bundle) {


        View view = layoutInflater.inflate(R.layout.fragment_video, null);
        youTubePlayerFragment =
                YouTubePlayerSupportFragment.newInstance();
        youTubePlayerFragment.initialize(Constants.YOUTUBE_DEVELOPER_KEY, this);


        FragmentTransaction transaction = getChildFragmentManager().beginTransaction();
        transaction.replace(R.id.youtube_container, youTubePlayerFragment, "TrainingTestFragment");
        transaction.addToBackStack(null);
        transaction.commit();

        take_test = (Button) view.findViewById(R.id.take_test);
        nextVideo = (Button) view.findViewById(R.id.next_video);
        nextVideo.setEnabled(false);
        nextVideo.setClickable(false);
        nextVideo.setVisibility(View.GONE);

        nextVideo.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {

                ((TrainingActivity) getActivity()).saveCompleteTraining(mTrainingModel, 10, false, position);


            }
        });

        if (mTrainingModel != null && mTrainingModel.isQuestion()) {
            take_test.setVisibility(View.VISIBLE);
            if (mTrainingModel.isComplete()) {
                take_test.setEnabled(false);
            }
        } else {
            take_test.setVisibility(View.GONE);
        }
        take_test.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                ((TrainingActivity) getActivity()).getTrainigTest(mTrainingModel);
            }
        });


        return view;
    }


    @Override
    public void onInitializationSuccess(YouTubePlayer.Provider provider, YouTubePlayer player,
                                        boolean wasRestored) {
        if (!wasRestored) {

            player.cueVideo(mTrainingModel.getYoutube_link().trim());
            videoPlayer = player;
            videoPlayer.setPlaybackEventListener(this);
            videoPlayer.setFullscreen(false);
            videoPlayer.setFullscreenControlFlags(YouTubePlayer.FULLSCREEN_FLAG_CUSTOM_LAYOUT);
            videoPlayer.setPlaybackEventListener(this);
            videoPlayer.setOnFullscreenListener(new YouTubePlayer.OnFullscreenListener() {
                @Override
                public void onFullscreen(boolean _isFullScreen) {
                    fullScreen = _isFullScreen;

                    if (fullScreen) {
                        nextVideo.setEnabled(false);
                        nextVideo.setClickable(false);
                        videoPlayer.setFullscreen(true);
                        int ms = videoPlayer.getCurrentTimeMillis();
                        Intent intent = new Intent(getActivity(), VideoFullScreenActivity.class);
                        intent.putExtra("link", mTrainingModel.getYoutube_link().trim());
                        intent.putExtra("ms", ms);
                        startActivity(intent);

                    }
                }
            });
        }
    }

    @Override
    public void onInitializationFailure(YouTubePlayer.Provider provider,
                                        YouTubeInitializationResult errorReason) {
        if (errorReason.isUserRecoverableError()) {

        } else {
            String errorMessage = String.format("error loading video", errorReason.toString());
            Toast.makeText(getActivity(), errorMessage, Toast.LENGTH_LONG).show();
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        getActivity().setTitle("My Training");
    }

    @Override
    public void onSaveInstanceState(Bundle outState) {
        super.onSaveInstanceState(outState);
    }

    public void setTraining(TrainingModel model) {
        mTrainingModel = model;
    }

    @Override
    public void onPlaying() {

    }

    @Override
    public void onPaused() {
    }

    @Override
    public void onStopped() {

        Log.d("On Video", "Stopped");
       // if (videoPlayer.getCurrentTimeMillis() == videoPlayer.getDurationMillis()) {
            nextVideo.setEnabled(true);
            nextVideo.setClickable(true);
            nextVideo.setVisibility(View.VISIBLE);
            Log.d("On Video", "Endeded");
       // }
    }

    @Override
    public void onBuffering(boolean b) {

    }

    @Override
    public void onSeekTo(int i) {
    }

    @Override
    public void onLoading() {

    }

    @Override
    public void onLoaded(String s) {

    }

    @Override
    public void onAdStarted() {

    }

    @Override
    public void onVideoStarted() {

    }

    @Override
    public void onVideoEnded() {


    }

    @Override
    public void onError(YouTubePlayer.ErrorReason errorReason) {

    }

    public void setQuestions(ArrayList<Questions> questionList) {
        this.questionList = questionList;
    }


    public void setPosition(int position) {
        this.position = position;
    }
}
