package deliverytrack.vss.com.deliverytrack.adapters;

import android.app.Activity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;
import java.util.ArrayList;
import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.models.TransactionModel;

/**
 * Created by Adi-Loch on 6/27/2015.
 */
public class TransactionAdapter extends BaseAdapter {
    private ArrayList<TransactionModel> transactionModels;
    private Activity context;


    public TransactionAdapter(Activity context, ArrayList<TransactionModel> transactions) {
        this.context = context;
        this.transactionModels = transactions;
    }

    public void setItems(ArrayList<TransactionModel> tms) {
        this.transactionModels = tms;
    }


    class ViewHolder {
        TextView credit;
        TextView debit;
        TextView restId;
    }


    @Override
    public int getCount() {
        return transactionModels.size();
    }

    @Override
    public TransactionModel getItem(int position) {
        return transactionModels.get(position);
    }

    @Override
    public long getItemId(int position) {
        return 0;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        final TransactionModel tm = getItem(position);
        LayoutInflater inflater = context.getLayoutInflater();
        ViewHolder holder = null;
        if (convertView == null) {
            holder = new ViewHolder();
            convertView = inflater.inflate(R.layout.transaction_row, null);
            holder.debit = (TextView) convertView.findViewById(R.id.txt_debit);
            holder.credit = (TextView) convertView.findViewById(R.id.txt_credit);
            holder.restId = (TextView) convertView.findViewById(R.id.txt_rest_id);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        holder.credit.setText("Credit:" + String.valueOf(tm.getUserCredit()));
        holder.debit.setText("Debit:" + String.valueOf(tm.getUserDebit()));
        holder.restId.setText(tm.getRestId());
        convertView.setTag(holder);
        return convertView;
    }
}
