package deliverytrack.vss.com.deliverytrack;

import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.Configuration;
import android.location.Location;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.parse.FunctionCallback;
import com.parse.ParseAnalytics;
import com.parse.ParseCloud;
import com.parse.ParseException;
import com.parse.ParseGeoPoint;
import com.parse.ParseInstallation;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;
import com.parse.PushService;
import com.parse.SaveCallback;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Locale;

import deliverytrack.vss.com.deliverytrack.Services.UploadAdharService;
import deliverytrack.vss.com.deliverytrack.adapters.NavDrawerListAdapter;
import deliverytrack.vss.com.deliverytrack.asynch.FindForQuery;
import deliverytrack.vss.com.deliverytrack.asynch.FindOrder;
import deliverytrack.vss.com.deliverytrack.asynch.FindVersion;
import deliverytrack.vss.com.deliverytrack.fragments.BankDetailsFragment;
import deliverytrack.vss.com.deliverytrack.fragments.MapViewFragment;
import deliverytrack.vss.com.deliverytrack.fragments.SettingsFragment;
import deliverytrack.vss.com.deliverytrack.fragments.orders.OrderArrivedAtLocation;
import deliverytrack.vss.com.deliverytrack.fragments.orders.OrderDelivered;
import deliverytrack.vss.com.deliverytrack.fragments.orders.OrderPendingAccept;
import deliverytrack.vss.com.deliverytrack.fragments.orders.OrderPendingDelivery;
import deliverytrack.vss.com.deliverytrack.fragments.orders.OrderPendingPickup;
import deliverytrack.vss.com.deliverytrack.fragments.orders.OrderdetailedFragment;
import deliverytrack.vss.com.deliverytrack.fragments.areas.AreasForToday;
import deliverytrack.vss.com.deliverytrack.fragments.TransactionFragment;
import deliverytrack.vss.com.deliverytrack.interfaces.OrderCallbacks;
import deliverytrack.vss.com.deliverytrack.interfaces.VersionCodeCallback;
import deliverytrack.vss.com.deliverytrack.models.NavDrawerItem;
import deliverytrack.vss.com.deliverytrack.models.Order;
import deliverytrack.vss.com.deliverytrack.models.TrainingModel;
import deliverytrack.vss.com.deliverytrack.receivers.AlarmReceiver;
import deliverytrack.vss.com.deliverytrack.receivers.BillGenerationAlarm;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.FloatingActionButton;
import deliverytrack.vss.com.deliverytrack.utility.LocationUtils;
import deliverytrack.vss.com.deliverytrack.utility.ParseHelper;


public class BaseActivity extends AppCompatActivity implements OrderCallbacks,
        OrderPendingDelivery.OnFragmentInteractionListener,
        TransactionFragment.OnFragmentInteractionListener,
        OrderPendingAccept.OnFragmentInteractionListener,
        OrderDelivered.OnFragmentInteractionListener,
        AreasForToday.OnFragmentInteractionListener,
        SettingsFragment.OnFragmentInteractionListener,
        BankDetailsFragment.OnFragmentInteractionListener,
        OrderdetailedFragment.OnFragmentInteractionListener,
        OrderPendingPickup.OnFragmentInteractionListener,
        OrderArrivedAtLocation.OnFragmentInteractionListener,
        View.OnClickListener, VersionCodeCallback, MapViewFragment.OnFragmentInteractionListener {


    private static final int MENU_SEARCH = 9;
    private double longitude = 0.0;
    private double latitude = 0.0;
    private DrawerLayout mDrawerLayout;
    private ListView mDrawerList;
    private ActionBarDrawerToggle mDrawerToggle;
    private CharSequence mTitle;
    private String[] navMenuTitles;

    private ArrayList<NavDrawerItem> navDrawerItems;
    private NavDrawerListAdapter adapter;
    private PendingIntent mRequestLocationUpdatesPendingIntent;
    private ArrayList<TrainingModel> inCompleteTraining = new ArrayList<>();

    private LocationUtils mLocationUtils;
    private Integer completedTestNo = 0;
    private int saveScore;
    private TrainingModel saveTraining;
    private ArrayList<TrainingModel> completedTrainingList = new ArrayList<>();
    private String userStatus;
    private String extra = null;

    private PendingIntent pendingIntent;
    private String order = null;
    private int versionCode = 0;
    private FloatingActionButton fabs;
    private boolean isNearByAgent = false;

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        extra = intent.getStringExtra("account");
        order = intent.getStringExtra("order");

    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_base);


        Intent alarmIntent = new Intent(BaseActivity.this, AlarmReceiver.class);
        pendingIntent = PendingIntent.getBroadcast(BaseActivity.this, 0, alarmIntent, 0);

        Calendar calendar = Calendar.getInstance();

        calendar.set(Calendar.HOUR_OF_DAY, 18); // For 1 PM or 2 PM
        calendar.set(Calendar.MINUTE, 0);
        calendar.set(Calendar.SECOND, 0);

        Intent billAlarmIntent = new Intent(BaseActivity.this, BillGenerationAlarm.class);
        PendingIntent pi = PendingIntent.getService(BaseActivity.this, 0, billAlarmIntent, 0);
        AlarmManager am = (AlarmManager) getSystemService(Context.ALARM_SERVICE);
        am.setRepeating(AlarmManager.RTC_WAKEUP, calendar.getTimeInMillis(),
                AlarmManager.INTERVAL_DAY, pi);


        ParseQuery query = new ParseQuery("AppVersion");
        FindVersion findVersion = new FindVersion(query, this, null, 000);
        findVersion.execute();


        setTitle("DeliveryTrack");
        startAt10();
        ParseAnalytics.trackAppOpened(getIntent());
        PushService.setDefaultPushCallback(this, BaseActivity.class);
        ParseInstallation.getCurrentInstallation().saveInBackground();

        navMenuTitles = getResources().getStringArray(R.array.nav_drawer_items);
        mDrawerLayout = (DrawerLayout) findViewById(R.id.drawer_layout);
        mDrawerList = (ListView) findViewById(R.id.list_slidermenu);


        fabs = (FloatingActionButton) findViewById(R.id.fab);
        fabs.setOnClickListener(this);


        navDrawerItems = new ArrayList<NavDrawerItem>();
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[0]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[1]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[2]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[3]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[4]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[5]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[6]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[7]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[8]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[9]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[10]));


        mDrawerList.setOnItemClickListener(new SlideMenuClickListener());
        adapter = new NavDrawerListAdapter(getApplicationContext(),
                navDrawerItems);
        mDrawerList.setAdapter(adapter);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        getSupportActionBar().setHomeButtonEnabled(true);

        userStatus = ParseUser.getCurrentUser().get("userStatus") != null ? ParseUser.getCurrentUser().get("userStatus").toString() : "";

        ParseGeoPoint point = (ParseGeoPoint) ParseUser.getCurrentUser().get("location");
        String url = "http://www.vsstechnology.com/deliverytrack/api/?q=update_location&user="+
                ParseUser.getCurrentUser().getUsername()+"&latitude="+point.getLatitude()
                +"&longitude="+point.getLongitude();
        Log.v("BASEACTIVITY", "url" + url);
        String[] params = {url};
        new LongOperation().execute(params);

        mDrawerToggle = new ActionBarDrawerToggle(this, mDrawerLayout, R.string.app_name, R.string.app_name) {
            public void onDrawerClosed(View view) {
                invalidateOptionsMenu();
            }

            public void onDrawerOpened(View drawerView) {
                invalidateOptionsMenu();
            }
        };
        mDrawerLayout.setDrawerListener(mDrawerToggle);


        boolean isAdharSaved = DeliveryTrackUtils.getSharedPreferences(this).getBoolean("isAdharSaved", false);
        if (!isAdharSaved) {
            Intent uploadAdar = new Intent(this, UploadAdharService.class);
            startService(uploadAdar);
        }


        showFabs();

    }


    @Override
    protected void onStart() {
        super.onStart();


    }


    public void cancel() {
        AlarmManager manager = (AlarmManager) getSystemService(Context.ALARM_SERVICE);
        manager.cancel(pendingIntent);
        Toast.makeText(this, "Alarm Canceled", Toast.LENGTH_SHORT).show();
    }

    public void startAt10() {
        AlarmManager manager = (AlarmManager) getSystemService(Context.ALARM_SERVICE);
        int interval = 10000 * 60 * 15;
        Calendar calendar = Calendar.getInstance();
        calendar.setTimeInMillis(System.currentTimeMillis());
        /* Repeating on every 5 minutes interval */
        manager.setRepeating(AlarmManager.RTC_WAKEUP, calendar.getTimeInMillis(),
                interval, pendingIntent);
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        return super.onCreateOptionsMenu(menu);
    }


    @Override
    protected void onPause() {
        super.onPause();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();

    }


    @Override
    protected void onResume() {
        super.onResume();
        if (extra != null && !extra.equals("")) {
            BankDetailsFragment opf = new BankDetailsFragment();
            replaceFragment(opf, "BankDetailsFragment");
            extra = null;
        } else if (order != null && !order.equals("")) {
            getOrdersPending();
            order = null;
        } else {


            //Log.d("The", completedTestNo + " ");
            if (!userStatus.equals("Active")) {
                Intent inactive = new Intent(this, SignUpActivity.class);
                startActivity(inactive);
            } else if (userStatus.equals("Active")) {

                getCompletedTrainingNumberForUser();
            }
        }


    }

    private void getMap() {

        MapViewFragment map = new MapViewFragment();
        replaceFragment(map, "MapViewFragment");
    }

    public void showFabs() {
        if (DeliveryTrackUtils.isRestaurentUser()) {
            fabs.setVisibility(View.VISIBLE);
        } else {
            fabs.setVisibility(View.GONE);
        }
    }

    private void addOrderCreateFrag() {
        Intent intent = new Intent(this, OrderCreationActivity.class);
        startActivity(intent);
    }


    @Override
    public void onClick(View v) {

        switch (v.getId()) {

            case R.id.fab:
                if (!userStatus.equals("InActive")) {
                    if (isNearByAgent) {
                        addOrderCreateFrag();
                    } else if (!isNearByAgent) {
                        DeliveryTrackUtils.showToast(this, "No Near by agent");
                    }
                } else {
                    DeliveryTrackUtils.showToast(this, getResources().getString(R.string.inactive_user_error));
                }
                break;
        }

    }

    private void getOrdersPendingDelivery() {
        Log.v("BaseActivity","getOrdersPendingDelivery");
        OrderPendingDelivery opf = new OrderPendingDelivery();
        replaceFragment(opf, "OrderPendingDelivery");

    }


    private void getOrdersPendingPickup() {
        OrderPendingPickup orderPendingPickup = new OrderPendingPickup();
        replaceFragment(orderPendingPickup, "OrderPendingPickup");
    }


    @Override
    public void getVersionCode(int versionCode) {
        this.versionCode = versionCode;
        Log.d("The version code", versionCode + " ");
        if (versionCode != DeliveryTrackUtils.getAppVersion(this)) {
            showForceUpdate();
        }
    }


    /**
     * Slide menu item click listener
     */
    private class SlideMenuClickListener implements
            ListView.OnItemClickListener {
        @Override
        public void onItemClick(AdapterView<?> parent, View view, int position,
                                long id) {
            // display view for selected nav drawer item

            if (position < 5) {

                if (versionCode != DeliveryTrackUtils.getAppVersion(BaseActivity.this)) {
                    showForceUpdate();
                }
                if (userStatus.equals("Active") && versionCode == DeliveryTrackUtils.getAppVersion(BaseActivity.this)) {
                    displayView(position, view);
                } else {
                    DeliveryTrackUtils.showToast(BaseActivity.this, getResources().getString(R.string.inactive_user_error));
                }

            } else {
                displayView(position, view);

            }


        }
    }

    private void displayView(int position, View view) {

        switch (position) {
            case 0:
                //My transactions
                getTransactions();
                break;

            case 1:
                //status pending - Orders Placed
                getOrdersPending();
                break;

            case 2:
                // satus accepted - Orders Accepted
                getOrdersArrivedAtLocation();
                break;

            case 3:
                // status arrived - Orders Delivery Agent Arrived
                getOrdersPendingPickup();
                break;

            case 4:
                // status pickup - Orders Picked Up
                getOrdersPendingDelivery();
                break;

            case 5:
                // status delivered - Orders Delivered
                getOrderDelivered();
                break;

            case 6:
                //MG Areas for Today
                getAreasForToday();
                break;

            case 7:
                // My Settings
                String userType = DeliveryTrackUtils.getUserType();
                if (userType.equals("Agent")) {
                    SettingsFragment stfm = new SettingsFragment();
                    replaceFragment(stfm, "SettingsFragment");
                } else {
                    Intent settings = new Intent(BaseActivity.this, SignUpActivity.class);
                    startActivity(settings);
                }

                break;

            case 8:
                //My Trainings
                getTrainigs(true);

                break;

            case 9:
                // Support
                Intent freshDesk = new Intent(this, FreshDesk.class);
                startActivity(freshDesk);
                break;


            case 10:
                // Logout
                ParseUser parseUser = ParseUser.getCurrentUser();
                parseUser.put("loggedIn", false);
                parseUser.saveInBackground(new SaveCallback() {

                    @Override
                    public void done(ParseException e) {
                        // TODO Auto-generated method stub
                        if (e != null) {
                            e.printStackTrace();
                        } else {
                            ParseUser.logOut();
                            Intent intent1 = new Intent(BaseActivity.this, RouteLoginActivity.class);
                            intent1.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK | Intent.FLAG_ACTIVITY_NEW_TASK);
                            startActivity(intent1);
                        }
                    }
                });

                String url = "http://www.vsstechnology.com/deliverytrack/api/?q=logout&user="+ParseUser.getCurrentUser().getUsername();
                Log.v("BASEACTIVITY LOGOUT", "url" + url);
                String[] params = {url};
                new LongOperation().execute(params);
                break;
        }

        mDrawerLayout.closeDrawers();

    }

    private void getOrdersArrivedAtLocation() {
        OrderArrivedAtLocation orderDelivered = new OrderArrivedAtLocation();
        replaceFragment(orderDelivered, "OrderArrivedAtLocation");

    }

    private void getOrdersPending() {
        OrderPendingAccept orderDelivered = new OrderPendingAccept();
        replaceFragment(orderDelivered, "OrderPendingAccept");

    }

    private void getAreasForToday(){
        AreasForToday areas = new AreasForToday();
        replaceFragment(areas, "MG Areas For Today");
    }

    private void getTransactions() {
        TransactionFragment opf = new TransactionFragment();
        replaceFragment(opf, "TransactionFragment");

    }

    private void getOrderDelivered() {
        OrderDelivered orderDelivered = new OrderDelivered();
        replaceFragment(orderDelivered, "OrderDelivered");
    }


    @Override
    public boolean onPrepareOptionsMenu(Menu menu) {
        menu.clear();
        getMenuInflater().inflate(R.menu.popup_menu, menu);

        if (DeliveryTrackUtils.isRestaurentUser()) {
            menu.add(0, MENU_SEARCH, Menu.NONE, R.string.search).setIcon(R.drawable.ic_search_white_24dp).setShowAsAction(MenuItem.SHOW_AS_ACTION_IF_ROOM);


        }
        return super.onPrepareOptionsMenu(menu);

    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (mDrawerToggle.onOptionsItemSelected(item)) {
            return true;
        }

        switch (item.getItemId()) {
            case R.id.hindi:
                Locale locale = new Locale("hi");
                DeliveryTrackUtils.saveLocale("hi", BaseActivity.this);
                Locale.setDefault(locale);
                Configuration config = new Configuration();
                config.locale = locale;
                getApplicationContext().getResources().updateConfiguration(config, getBaseContext().getResources().getDisplayMetrics());
                onConfigurationChanged(config);
                break;

            case R.id.english:
                Locale locale1 = new Locale("en");
                DeliveryTrackUtils.saveLocale("en", BaseActivity.this);
                Locale.setDefault(locale1);
                Configuration config1 = new Configuration();
                config1.locale = locale1;
                getApplicationContext().getResources().updateConfiguration(config1,
                        getBaseContext().getResources().getDisplayMetrics());
                onConfigurationChanged(config1);
                break;

            case MENU_SEARCH:
                showSearchBox();
                break;
        }


        return super.onOptionsItemSelected(item);

    }


    private void showSearchBox() {
        LayoutInflater li = LayoutInflater.from(this);
        View promptsView = li.inflate(R.layout.prompt, null);

        AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(
                this);

        // set prompts.xml to alertdialog builder
        alertDialogBuilder.setView(promptsView);

        final TextView text = (TextView) promptsView.findViewById(R.id.textView1);
        text.setText("Enter Customer No");
        final EditText userInput = (EditText) promptsView
                .findViewById(R.id.editTextDialogUserInput);
        userInput.setText("+91");

        // set dialog message
        alertDialogBuilder
                .setCancelable(false)
                .setPositiveButton("OK",
                        new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                // get user input and set it to result
                                // edit text

                                ParseQuery<ParseObject> query = new ParseQuery("order");
                                query.whereEqualTo("customerNumber", userInput.getText().toString());
                                FindOrder findForQuery = new FindOrder(query, BaseActivity.this, null, 789);
                                findForQuery.execute();
                            }
                        })
                .setNegativeButton("Cancel",
                        new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                dialog.cancel();
                            }
                        });

        // create alert dialog
        AlertDialog alertDialog = alertDialogBuilder.create();

        // show it
        alertDialog.show();

    }


    @Override
    public void restParseObjects(Order order) {

    }

    @Override
    public void onOrderSaveSuccess() {

    }

    @Override
    public void onOrderSaveFailure() {

    }

    @Override
    public void onOrderCanceled() {

    }


    @Override
    public void onFragmentInteraction(Uri uri) {

    }

    @Override
    public void isNearByAgent(boolean isNearBy) {
        isNearByAgent = isNearBy;

    }


    //  Mobihelp.init(this,new MobihelpConfig("https://vsstech.freshdesk.com", "deliverytrack-1-824986784fc8e3b43481e1590d05df7b", "d2ccfe727622db1fdcb7bc231c79219eb9502c2f"));


    @Override
    public void setTitle(CharSequence title) {
        mTitle = title;
        getSupportActionBar().setTitle(mTitle);
    }

    /**
     * When using the ActionBarDrawerToggle, you must call it during
     * onPostCreate() and onConfigurationChanged()...
     */

    @Override
    protected void onPostCreate(Bundle savedInstanceState) {
        super.onPostCreate(savedInstanceState);
        // Sync the toggle state after onRestoreInstanceState has occurred.
        mDrawerToggle.syncState();
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        // Pass any configuration change to the drawer toggls
        navMenuTitles = getResources().getStringArray(R.array.nav_drawer_items);
        mDrawerLayout = (DrawerLayout) findViewById(R.id.drawer_layout);
        mDrawerList = (ListView) findViewById(R.id.list_slidermenu);

        navDrawerItems = new ArrayList<NavDrawerItem>();
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[0]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[1]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[2]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[3]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[4]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[5]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[6]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[7]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[8]));
        navDrawerItems.add(new NavDrawerItem(navMenuTitles[9]));

        mDrawerList.setOnItemClickListener(new SlideMenuClickListener());
        adapter = new NavDrawerListAdapter(getApplicationContext(),
                navDrawerItems);
        mDrawerList.setAdapter(adapter);

        mDrawerToggle.onConfigurationChanged(newConfig);
    }

    /*
    methods related to training
     */

    public void getTrainigs(Boolean update) {
        Intent intent = new Intent(this, TrainingActivity.class);
        startActivity(intent);

    }


    public void getCompletedTrainingNumberForUser() {
        ParseQuery query = new ParseQuery("CompletedTrainings");
        query.whereEqualTo("userId", ParseUser.getCurrentUser().getObjectId());
        FindForQuery findQuery = new FindForQuery(query, this, null, 124);
        findQuery.execute();

    }


    @Override
    public void getParseObjects(List<ParseObject> parseObjects, int tableId) {

        if (tableId == 124) {  //completed number and not completed list
            if (parseObjects != null && parseObjects.size() > 0) {
                ParseObject training = parseObjects.get(0);
                completedTestNo = (Integer) training.get("trainingNum");
                /*To get from the */

                if (completedTestNo > 0) {
                    if (userStatus.equals("Active") && DeliveryTrackUtils.isAgent() && completedTestNo > 0) {
                        getOrdersPending();
                    } else if (userStatus.equals("Active") && DeliveryTrackUtils.isRestaurentUser() && completedTestNo > 0) {
                        getMap();
                    }
                } else if (completedTestNo == 0) {
                    getTrainigs(true);
                }

            } else {
                getTrainigs(true);

            }
        } else if (tableId == 789) {
            if (parseObjects != null && parseObjects.size() > 0) {
                ParseHelper helper = new ParseHelper();
                ArrayList<Order> order = helper.getOrdersVisibleDelivered(parseObjects);
                OrderdetailedFragment odf = new OrderdetailedFragment();
                odf.setOrder(order.get(0));
                replaceFragment(odf, "OrderdetailedFragment");
            }
        }

    }


    private void replaceFragment(Fragment fragment, String fragmentTag) {
        FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        transaction.replace(R.id.container, fragment, fragmentTag);
        transaction.addToBackStack(null);
        transaction.commit();

    }


    private void showForceUpdate() {
        AlertDialog alertDialog = new AlertDialog.Builder(BaseActivity.this).create();
        alertDialog.setTitle("Update App");
        alertDialog.setMessage("Kindly Update your app to continue working with DeliveryTrack");
        alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, "OK",
                new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        DeliveryTrackUtils.redirectPlaystore(BaseActivity.this);
                        dialog.dismiss();
                    }
                });
        alertDialog.show();

    }

}


