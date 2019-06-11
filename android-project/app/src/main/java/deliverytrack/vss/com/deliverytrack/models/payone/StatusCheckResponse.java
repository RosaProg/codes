package deliverytrack.vss.com.deliverytrack.models.payone;

import com.google.gson.annotations.SerializedName;

/**
 * Created by Adi-Loch on 1/9/2016.
 */
public class StatusCheckResponse {

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getErrorCode() {
        return errorCode;
    }

    public void setErrorCode(String errorCode) {
        this.errorCode = errorCode;
    }

    public Description getDescription() {
        return description;
    }

    public void setDescription(Description description) {
        this.description = description;
    }

    @SerializedName("status")
    String status;

    @SerializedName("errorCode")
    String errorCode;

    @SerializedName("description")
    Description description;


   public class Description {
        public String getMobile() {
            return mobile;
        }

        public void setMobile(String mobile) {
            this.mobile = mobile;
        }

        public String getRef_id() {
            return ref_id;
        }

        public void setRef_id(String ref_id) {
            this.ref_id = ref_id;
        }

        public String getIntime() {
            return intime;
        }

        public void setIntime(String intime) {
            this.intime = intime;
        }

        public String getSettled_time() {
            return settled_time;
        }

        public void setSettled_time(String settled_time) {
            this.settled_time = settled_time;
        }

        public String getExpiry_time() {
            return expiry_time;
        }

        public void setExpiry_time(String expiry_time) {
            this.expiry_time = expiry_time;
        }

        public String getStatus() {
            return status;
        }

        public void setStatus(String status) {
            this.status = status;
        }

        @SerializedName("mobile")
        String mobile;
        @SerializedName("ref_id")
        String ref_id;
        @SerializedName("intime")
        String intime;
        @SerializedName("settled_time")
        String settled_time;
        @SerializedName("expiry_time")
        String expiry_time;
        @SerializedName("status")
        String status;
    }


}
