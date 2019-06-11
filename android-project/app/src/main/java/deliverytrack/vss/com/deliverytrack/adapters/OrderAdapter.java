package deliverytrack.vss.com.deliverytrack.adapters;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import com.parse.ParseQuery;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.asynch.FindOrder;
import deliverytrack.vss.com.deliverytrack.asynch.UpdateOrder;
import deliverytrack.vss.com.deliverytrack.constants.Constants;
import deliverytrack.vss.com.deliverytrack.fragments.orders.OrderdetailedFragment;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.lazyloader.ImageLoader;

/**
 * Created by Adi-Loch on 6/26/2015.
 */
public class OrderAdapter extends BaseAdapter implements View.OnClickListener {

    private ImageLoader imageLoader;
    private ArrayList<Order> orders;
    private Activity context;
    private Fragment mFragment;


    public OrderAdapter(Activity context, ArrayList<Order> pickedOrders, Fragment fragment) {
        this.context = context;
        this.orders = pickedOrders;
        this.mFragment = fragment;

        imageLoader = new ImageLoader(context);

    }

    @Override
    public void onClick(View v) {

        int position = (Integer) v.getTag();
        Order order = this.orders.get(position);

        switch (v.getId()) {

            case R.id.btn_deliver:
                DateFormat readFormat = new SimpleDateFormat("hh:mm aa");
                order.setDeliveryTime(readFormat.format(new Date()));
                order.setStatus(Constants.ORDER_DELIVERED);
                UpdateOrder updateOrder = new UpdateOrder(this.context, order, this.mFragment, order.getStatus());
                updateOrder.execute();
                break;

            case R.id.btn_pickup:
            case R.id.btn_accept_pickup:
            case R.id.btn_arrived:
                /**/
                ParseQuery query = new ParseQuery("order");
                query.whereEqualTo("objectId", order.getObjectId());
                FindOrder findOrder = new FindOrder(query, this.context, this.mFragment, 000);
                findOrder.execute();

                break;

            case R.id.btn_detail:
                OrderdetailedFragment odf = new OrderdetailedFragment();
                odf.setOrder(order);
                FragmentTransaction transaction = this.mFragment.getFragmentManager().beginTransaction();
                transaction.replace(R.id.container, odf, "OrderdetailedFragment");
                transaction.addToBackStack(null);
                transaction.commit();
                break;

            case R.id.call_customer:
                String phnum = order.getCustomerContactNo();
                if (phnum != null && phnum.length() > 0) {
                    Intent callIntent = new Intent(Intent.ACTION_CALL);
                    callIntent.setData(Uri.parse("tel:" + phnum));
                }

                break;

        }

    }

    public void setItems(ArrayList<Order> items) {
        Log.v("OrderAdapter","setItems");
        this.orders = items;
    }


    class ViewHolder {
        TextView title;
        TextView amount;
        TextView location;
        TextView agentNameorRestName;
        Button delivered;
        Button pickedUp;
        Button accept;
        Button arrived;

        ImageView thumbnail;

        TextView callButton;
        int position;
        Button btnDetail;
        TextView paymentMode;
        int phoneNumber;
    }


    @Override
    public int getCount() {
        return orders.size();
    }

    @Override
    public Order getItem(int position) {
        return orders.get(position);
    }

    @Override
    public long getItemId(int position) {
        return 0;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        final Order pickedUp = getItem(position);
        LayoutInflater inflater = context.getLayoutInflater();
        ViewHolder holder = null;
        if (convertView == null) {
            holder = new ViewHolder();
            convertView = inflater.inflate(R.layout.pickedup_row, null);
            holder.title = (TextView) convertView.findViewById(R.id.txt_order_name);
            holder.amount = (TextView) convertView.findViewById(R.id.txt_amount);
            holder.delivered = (Button) convertView.findViewById(R.id.btn_deliver);
            holder.location = (TextView) convertView.findViewById(R.id.txt_location);
            holder.agentNameorRestName = (TextView) convertView.findViewById(R.id.txt_agent_name_rest_name);
            holder.pickedUp = (Button) convertView.findViewById(R.id.btn_pickup);
            holder.accept = (Button) convertView.findViewById(R.id.btn_accept_pickup);
            holder.arrived = (Button) convertView.findViewById(R.id.btn_arrived);
            holder.btnDetail = (Button) convertView.findViewById(R.id.btn_detail);
            holder.callButton = (TextView) convertView.findViewById(R.id.call_customer);
            holder.paymentMode = (TextView) convertView.findViewById(R.id.payment_mode);
            holder.thumbnail = (ImageView) convertView.findViewById(R.id.thumbnail);

        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        holder.title.setText("Name:" + " " + pickedUp.getName());
        holder.amount.setText("Amount:" + " " + String.valueOf(pickedUp.getAmount()));
        holder.location.setText(pickedUp.getAddress());
        holder.callButton.setText(pickedUp.getCustomerContactNo());
        holder.paymentMode.setText(pickedUp.getPaymentMode());


        if (DeliveryTrackUtils.getType().equals("agentId")) {

            holder.agentNameorRestName.setText("Ordered By:" + " " + pickedUp.getRestName());

        } else if (DeliveryTrackUtils.getType().equals("restaurentId")) {

            holder.agentNameorRestName.setText("Picked By:" + " " + pickedUp.getAgentName());
        }
        holder.position = position;


        if (DeliveryTrackUtils.isAgent()) {
            if (pickedUp.getStatus().equals(Constants.ORDER_PENDING)) {
                holder.delivered.setVisibility(View.GONE);
                holder.pickedUp.setVisibility(View.GONE);
                holder.accept.setVisibility(View.VISIBLE);
                holder.arrived.setVisibility(View.GONE);
            } else if (pickedUp.getStatus().equals(Constants.ORDER_ACCEPT_PICKUP)) {
                holder.delivered.setVisibility(View.GONE);
                holder.pickedUp.setVisibility(View.GONE);
                holder.accept.setVisibility(View.GONE);
                holder.arrived.setVisibility(View.VISIBLE);

            } else if (pickedUp.getStatus().equals(Constants.ORDER_ARRIVED_LOCATION)) {
                holder.delivered.setVisibility(View.GONE);
                holder.pickedUp.setVisibility(View.VISIBLE);
                holder.accept.setVisibility(View.GONE);
                holder.arrived.setVisibility(View.GONE);

            } else if (pickedUp.getStatus().equals(Constants.ORDER_PICKEDUP)) {
                holder.delivered.setVisibility(View.VISIBLE);
                holder.pickedUp.setVisibility(View.GONE);
                holder.accept.setVisibility(View.GONE);
                holder.arrived.setVisibility(View.GONE);

            }
            holder.thumbnail.setVisibility(View.GONE);

        } else if (DeliveryTrackUtils.isRestaurentUser()) {
            holder.delivered.setVisibility(View.GONE);
            holder.pickedUp.setVisibility(View.GONE);
            holder.arrived.setVisibility(View.GONE);
            holder.accept.setVisibility(View.GONE);

            if (pickedUp.getSelfieURL() != null && !(pickedUp.getSelfieURL().equals(""))) {
                imageLoader.DisplayImage(pickedUp.getSelfieURL(),
                        holder.thumbnail);
                holder.thumbnail.setVisibility(View.VISIBLE);
            }

        }

        if (pickedUp.getStatus().equals(Constants.ORDER_DELIVERED)) {
            holder.delivered.setVisibility(View.GONE);
            holder.pickedUp.setVisibility(View.GONE);
            holder.arrived.setVisibility(View.GONE);
            holder.accept.setVisibility(View.GONE);
            holder.thumbnail.setVisibility(View.GONE);

        }


        holder.pickedUp.setTag(position);
        holder.pickedUp.setOnClickListener(this);
        holder.delivered.setTag(position);
        holder.delivered.setOnClickListener(this);
        holder.btnDetail.setTag(position);
        holder.btnDetail.setOnClickListener(this);
        holder.accept.setTag(position);
        holder.accept.setOnClickListener(this);
        holder.arrived.setTag(position);
        holder.arrived.setOnClickListener(this);
        holder.callButton.setTag(position);
        holder.callButton.setOnClickListener(this);

        convertView.setTag(holder);

        return convertView;
    }
}
