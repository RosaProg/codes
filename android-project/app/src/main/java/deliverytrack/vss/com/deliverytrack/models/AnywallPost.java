package deliverytrack.vss.com.deliverytrack.models;

import com.parse.ParseClassName;
import com.parse.ParseGeoPoint;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

/**
 * Data model for a post.
 */

@ParseClassName("Posts")
public class AnywallPost extends ParseObject {

    public void setUserId(String userId){
        put("userId",userId);
    }

    public String getUserId(){
        return getString("userId");
    }

    public String getText() {
        return getString("text");
    }

    public void setText(String value) {
        put("text", value);
    }

    public String getType() {
        return getString("type");
    }

    public void setType(String value) {
        put("type", value);
    }

    public String getPhone() {
        return getString("phone");
    }

    public void setPhone(String value) {
        put("phone", value);
    }

    public ParseUser getUser() {
        return getParseUser("user");
    }

    public void setUser(ParseUser value) {
        put("user", value);
    }

    public ParseGeoPoint getLocation() {
        return getParseGeoPoint("location");
    }

    public void setLocation(ParseGeoPoint value) {
        put("location", value);
    }

    public String getName() {
        return getString("name");
    }

    public void setName(String value) {
        put("name", value);
    }

    public String getAmount() {
        return getString("text");
    }

    public void setAmount(String value) {
        put("amount", value);
    }

    public String getAddress() {
        return getString("address");
    }

    public void setAddress(String value) {
        put("address", value);
    }

    public String getOrder() {
        return getString("address");
    }

    public void setOrder(String value) {
        put("order", value);
    }

    public String getStatus() {
        return getString("status");
    }

    public void setStatus(String value) {
        put("status", value);
    }


    public static ParseQuery<AnywallPost> getQuery() {
        return ParseQuery.getQuery(AnywallPost.class);
    }
}
