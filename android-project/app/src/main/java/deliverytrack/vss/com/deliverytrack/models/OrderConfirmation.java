package deliverytrack.vss.com.deliverytrack.models;

import com.parse.ParseClassName;
import com.parse.ParseFile;
import com.parse.ParseObject;

@ParseClassName("OrderConfirmation")
public class OrderConfirmation extends ParseObject {
    public OrderConfirmation() {

    }

    public String getName() {
        return getString("name");
    }

    public void setName(String name) {
        put("name", name);
    }

    public ParseFile getSignatureFile() {
        return getParseFile("signature");
    }

    public void setSignatureFile(ParseFile file) {
        put("signature", file);
    }
}
