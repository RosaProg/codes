package deliverytrack.vss.com.deliverytrack.models;

import java.util.ArrayList;

/**
 * Created by anna on 06.07.15.
 */
public class TrainingModel {


    int training_id;
    String youtube_link;
    String language;
    int test_no;
    String userType;
    String videoTitle;
    boolean isQuestion;
    boolean isComplete;


    public boolean isComplete() {
        return isComplete;
    }

    public void setIsComplete(boolean isComplete) {
        this.isComplete = isComplete;
    }



    public boolean isQuestion() {
        return isQuestion;
    }

    public void setIsQuestion(boolean isQuestion) {
        this.isQuestion = isQuestion;
    }


    public int getTraining_id() {
        return training_id;
    }

    public void setTraining_id(int training_id) {
        this.training_id = training_id;
    }

    public String getYoutube_link() {
        return youtube_link;
    }

    public void setYoutube_link(String youtube_link) {
        this.youtube_link = youtube_link;
    }

    public String getLanguage() {
        return language;
    }

    public void setLanguage(String language) {
        this.language = language;
    }

    public int getTest_no() {
        return test_no;
    }

    public void setTest_no(int test_no) {
        this.test_no = test_no;
    }

    public String getUserType() {
        return userType;
    }

    public void setUserType(String userType) {
        this.userType = userType;
    }

    public String getVideoTitle() {
        return videoTitle;
    }

    public void setVideoTitle(String videoTitle) {
        this.videoTitle = videoTitle;
    }




}
