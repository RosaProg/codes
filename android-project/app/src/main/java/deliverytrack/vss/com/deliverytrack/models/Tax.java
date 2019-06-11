package deliverytrack.vss.com.deliverytrack.models;

/**
 * Created by Adi-Loch on 1/3/2016.
 */
public class Tax {

    public double getServiceTax() {
        return serviceTax;
    }

    public void setServiceTax(double serviceTax) {
        this.serviceTax = serviceTax;
    }

    public double getTds() {
        return tds;
    }

    public void setTds(double tds) {
        this.tds = tds;
    }

    double serviceTax;
    double tds;
}
