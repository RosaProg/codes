package deliverytrack.vss.com.deliverytrack.models.payone;

import com.google.gson.annotations.SerializedName;

/**
 * Created by Adi-Loch on 1/9/2016.
 */
public class CollectionResponse {

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



   public class Description{
        public String getReq_id() {
            return req_id;
        }

        public void setReq_id(String req_id) {
            this.req_id = req_id;
        }

        public String getCust_vpin() {
            return cust_vpin;
        }

        public void setCust_vpin(String cust_vpin) {
            this.cust_vpin = cust_vpin;
        }

        @SerializedName("request_id")
        String req_id;
        @SerializedName("cust_vpin")
        String cust_vpin;
    }





}
