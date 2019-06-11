package deliverytrack.vss.com.deliverytrack;

import com.google.android.youtube.player.YouTubeBaseActivity;
import com.google.android.youtube.player.YouTubeInitializationResult;
import com.google.android.youtube.player.YouTubePlayer;
import com.google.android.youtube.player.YouTubePlayerFragment;

import android.os.Bundle;
import android.widget.Toast;

import deliverytrack.vss.com.deliverytrack.constants.Constants;

/**
 * Created by anna on 08.07.15.
 */
public class VideoFullScreenActivity extends YouTubeBaseActivity {

    //private String videoName = "oZ9zA6Gq5ow";
    private int ms = 0;
    YouTubePlayerFragment youTubePlayerFragment;
    YouTubePlayer youTubePlayer;
    private String videoName;

    public VideoFullScreenActivity() {

    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        videoName = getIntent().getStringExtra("link");
        ms = getIntent().getIntExtra("ms", 0);

        setContentView(R.layout.fragment_video_fullscreen);

        youTubePlayerFragment =
                (YouTubePlayerFragment) getFragmentManager().findFragmentById(R.id.youtube_fragment);
        youTubePlayerFragment.initialize(Constants.YOUTUBE_DEVELOPER_KEY,
                new YouTubePlayer.OnInitializedListener() {
                    @Override
                    public void onInitializationSuccess(YouTubePlayer.Provider provider,
                                                        final YouTubePlayer player, boolean b) {
                        if (!b) {
                            youTubePlayer = player;
                            youTubePlayer.cueVideo(videoName, ms);
                            youTubePlayer
                                    .addFullscreenControlFlag(YouTubePlayer.FULLSCREEN_FLAG_ALWAYS_FULLSCREEN_IN_LANDSCAPE);


                        }
                    }

                    @Override
                    public void onInitializationFailure(YouTubePlayer.Provider provider,
                                                        YouTubeInitializationResult youTubeInitializationResult) {

                        if (youTubeInitializationResult.isUserRecoverableError()) {
                            youTubeInitializationResult.getErrorDialog(VideoFullScreenActivity.this, 1).show();
                        } else {
                            String errorMessage = String.format("There was an error initializing the YouTubePlayer (%1$s)", youTubeInitializationResult.toString());
                            Toast.makeText(VideoFullScreenActivity.this, errorMessage, Toast.LENGTH_LONG).show();
                        }

                    }
                });
    }

    @Override
    public void onBackPressed() {
      //  youTubePlayer.setFullscreen(false);
    }
}
