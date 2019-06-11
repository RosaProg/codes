/*
 * Copyright 2013 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package deliverytrack.vss.com.deliverytrack;

import java.util.List;
import java.util.Locale;

import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentStatePagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.util.TypedValue;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseUser;
import com.parse.SaveCallback;
import com.parse.SignUpCallback;

import deliverytrack.vss.com.deliverytrack.Services.UploadAdharService;
import deliverytrack.vss.com.deliverytrack.asynch.AddDefaultCommission;
import deliverytrack.vss.com.deliverytrack.fragments.registration.ReviewFragment;
import deliverytrack.vss.com.deliverytrack.interfaces.CommissionCallBacks;
import deliverytrack.vss.com.deliverytrack.interfaces.ModelCallbacks;
import deliverytrack.vss.com.deliverytrack.interfaces.PageFragmentCallbacks;
import deliverytrack.vss.com.deliverytrack.models.AbstractWizardModel;
import deliverytrack.vss.com.deliverytrack.models.AgentDetailPage;
import deliverytrack.vss.com.deliverytrack.models.AgentDocumentsPage;
import deliverytrack.vss.com.deliverytrack.models.AgentReferencePage;
import deliverytrack.vss.com.deliverytrack.models.BankDetailsPage;
import deliverytrack.vss.com.deliverytrack.models.MultipleFixedChoicePage;
import deliverytrack.vss.com.deliverytrack.models.Page;
import deliverytrack.vss.com.deliverytrack.models.SandwichWizardModel;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;
import deliverytrack.vss.com.deliverytrack.utility.StepPagerStrip;


public class SignUpActivity extends AppCompatActivity implements
        PageFragmentCallbacks, ReviewFragment.Callbacks, ModelCallbacks, CommissionCallBacks {
    private ViewPager mPager;
    private MyPagerAdapter mPagerAdapter;

    private boolean mEditingAfterReview;

    private AbstractWizardModel mWizardModel;

    private boolean mConsumePageSelectedEvent;

    private Button mNextButton;
    private Button mPrevButton;
    String credit, debit;
    Integer total = 0;
    private List<Page> mCurrentPageSequence;
    private StepPagerStrip mStepPagerStrip;
    private ProgressDialog dialog;
    private LinearLayout lyt;

    @Override
    protected void onRestoreInstanceState(Bundle savedInstanceState) {
        super.onRestoreInstanceState(savedInstanceState);
    }

    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_signup);
        Intent intent = getIntent();
        if (intent.hasExtra("MESSAGE")) {
            String pushMessage = getIntent().getDataString();

            showPopDialog(pushMessage);
        }
        mWizardModel = new SandwichWizardModel(this);
        dialog = new ProgressDialog(this);

        if (savedInstanceState != null) {
            mWizardModel.load(savedInstanceState.getBundle("model"));

        }

        setTitle("Settings");

        mWizardModel.registerListener(this);

        mPagerAdapter = new MyPagerAdapter(getSupportFragmentManager());
        mPager = (ViewPager) findViewById(R.id.pager);
        mPager.setAdapter(mPagerAdapter);
        mStepPagerStrip = (StepPagerStrip) findViewById(R.id.strip);
        mStepPagerStrip
                .setOnPageSelectedListener(new StepPagerStrip.OnPageSelectedListener() {
                    @Override
                    public void onPageStripSelected(int position) {
                        position = Math.min(mPagerAdapter.getCount() - 1,
                                position);
                        if (mPager.getCurrentItem() != position) {
                            mPager.setCurrentItem(position);
                        }
                    }
                });

        mNextButton = (Button) findViewById(R.id.next_button);
        mPrevButton = (Button) findViewById(R.id.prev_button);
        lyt = (LinearLayout) findViewById(R.id.lyt);

        mPager.setOnPageChangeListener(new ViewPager.SimpleOnPageChangeListener() {
            @Override
            public void onPageSelected(int position) {
                mStepPagerStrip.setCurrentPage(position);

                if (mConsumePageSelectedEvent) {
                    mConsumePageSelectedEvent = false;
                    return;
                }

                mEditingAfterReview = false;
                updateBottomBar();
            }
        });

        mNextButton.setOnClickListener(new View.OnClickListener() {


            @Override
            public void onClick(View view) {
                if (mPager.getCurrentItem() == mCurrentPageSequence.size()) {
                    // String data =
                    // mWizardModel.findByKey("Salad type:Dressing").getData().getString(MultipleFixedChoicePage.SIMPLE_DATA_KEY);
                    String type;
                    String mode = null;
                    String name = null;
                    String password = null;
                    String confirmPassword = null;
                    String asaddress = null;
                    String asaphone = null;
                    String asarea = null;
                    String adharno = null;
                    String docs = null;
                    String reference1 = null;
                    String reference2 = null;
                    String mobilenumber = null;
                    String mobilenumber2 = null;
                    String rel1 = null;
                    String rel2 = null;
                    String restName = null;
                    String restAddress = null;
                    String restpincode = null;
                    String email = null;
                    String manageName = null;
                    String managerMobileNumber = null;
                    String entitiy = null;
                    String restArea = null;
                    String curAddress = null;
                    String permanentAddress = null;
                    String perpin = null;
                    String curpin = null;
                    String curArea = null;
                    String perArea = null;


                    if (DeliveryTrackUtils.getUserType().equals("Agent")) {
                        type = DeliveryTrackUtils.getUserType();
                        mode = mWizardModel
                                .findByKey("Register as Delivery Freelancer:Mode of Delivery").getData()
                                .getString(MultipleFixedChoicePage.SIMPLE_DATA_KEY) != null ? mWizardModel
                                .findByKey("Register as Delivery Freelancer:Mode of Delivery").getData()
                                .getString(MultipleFixedChoicePage.SIMPLE_DATA_KEY) : "";

                        name = mWizardModel.findByKey("Personal Details")
                                .getData()
                                .getString(AgentDetailPage.FULL_NAME);

                        email = mWizardModel.findByKey("Personal Details")
                                .getData()
                                .getString(AgentDetailPage.EMAIL);


                        asaddress = mWizardModel
                                .findByKey("Personal Details").getData()
                                .getString(AgentDetailPage.CURADDRESS);
                        asaphone = mWizardModel.findByKey("Personal Details")
                                .getData()
                                .getString(AgentDetailPage.MOBILE_NUMBER);
                        asarea = mWizardModel.findByKey("Personal Details")
                                .getData()
                                .getString(AgentDetailPage.AREACUR);

                        adharno = mWizardModel.findByKey("Documents")
                                .getData()
                                .getString(AgentDocumentsPage.ADHAR_NO);

                        docs = mWizardModel.findByKey("Documents")
                                .getData()
                                .getString(AgentDocumentsPage.DOCS);

                        reference1 = mWizardModel.findByKey("Reference")
                                .getData()
                                .getString(AgentReferencePage.REFERENCE1);

                        reference2 = mWizardModel.findByKey("Reference")
                                .getData()
                                .getString(AgentReferencePage.REFERENCE2);

                        mobilenumber = mWizardModel.findByKey("Reference")
                                .getData()
                                .getString(AgentReferencePage.MOB1);
                        mobilenumber2 = mWizardModel.findByKey("Reference")
                                .getData()
                                .getString(AgentReferencePage.MOB2);
                        rel1 = mWizardModel.findByKey("Reference")
                                .getData()
                                .getString(AgentReferencePage.RELATIONSHIP1);
                        rel2 = mWizardModel.findByKey("Reference")
                                .getData()
                                .getString(AgentReferencePage.RELATIONSHIP2);


                        curAddress = mWizardModel.findByKey("Personal Details").
                                getData().getString(AgentDetailPage.CURADDRESS);

                        permanentAddress = mWizardModel.findByKey("Personal Details").
                                getData().getString(AgentDetailPage.PERADDRESS);

                        curArea = mWizardModel.findByKey("Personal Details").
                                getData().getString(AgentDetailPage.AREACUR);

                        perArea = mWizardModel.findByKey("Personal Details").
                                getData().getString(AgentDetailPage.AREAPER);

                        curpin = mWizardModel.findByKey("Personal Details").
                                getData().getString(AgentDetailPage.AGENTPINCODECUR);

                        perpin = mWizardModel.findByKey("Personal Details").
                                getData().getString(AgentDetailPage.AGENTPINCODEPER);


                    } else {
                        type = DeliveryTrackUtils.getUserType();

                        restName = mWizardModel.findByKey("Personal Details")
                                .getData()
                                .getString(AgentDetailPage.REST_NAME);

                        restAddress = mWizardModel.findByKey("Personal Details")
                                .getData()
                                .getString(AgentDetailPage.ADDRESS_REST);

                        restArea = mWizardModel.findByKey("Personal Details")
                                .getData()
                                .getString(AgentDetailPage.RESTAREA);


                        restpincode = mWizardModel.findByKey("Personal Details")
                                .getData()
                                .getString(AgentDetailPage.PINCODE);


                        email = mWizardModel.findByKey("Personal Details")
                                .getData()
                                .getString(AgentDetailPage.REST_EMAIL);


                        manageName = mWizardModel.findByKey("Reference")
                                .getData()
                                .getString(AgentReferencePage.MANAGER_NAME);


                        managerMobileNumber = mWizardModel.findByKey("Reference")
                                .getData()
                                .getString(AgentReferencePage.MOBILE_NUMBERREST);


                        entitiy = mWizardModel.findByKey("Documents")
                                .getData()
                                .getString(AgentDocumentsPage.ENTITY);


                    }


                    final String exactName = mWizardModel.findByKey("Bank Details")
                            .getData()
                            .getString(BankDetailsPage.EXACT_NAME);
                    final String bankname = mWizardModel.findByKey("Bank Details")
                            .getData()
                            .getString(BankDetailsPage.BANK_NAME);
                    final String bankAcc = mWizardModel.findByKey("Bank Details")
                            .getData()
                            .getString(BankDetailsPage.BANK_ACCOUNT);
                    final String ifscCode = mWizardModel.findByKey("Bank Details")
                            .getData()
                            .getString(BankDetailsPage.IFSCCODE);


                    if (type.equals("Agent") && name != null
                            && type != null) {


                        final ParseUser user = ParseUser.getCurrentUser();
                        Object userTotal = user.get("total");
                        double amountOfuser = 0;
                        if (userTotal instanceof Integer) {
                            amountOfuser = ((Integer) userTotal).doubleValue();

                        } else if (userTotal instanceof Double) {
                            amountOfuser = (Double) user.get("total");
                        }
                        user.setUsername(asaphone);
                        user.setEmail(email != null ? email : "");
                        user.put("name", type);
                        user.put("phone", asaphone);
                        user.put("area", asarea != null ? asarea : "");
                        user.put("address", asaddress != null ? asaddress : "");
                        user.put("credit", "0.0");
                        user.put("debit", "0.0");
                        if (amountOfuser > 0) {
                            user.put("total", amountOfuser);
                        } else {
                            user.put("total", total);
                        }
                        user.put("bankname", bankname != null ? bankname : "");
                        user.put("bankaccount", bankAcc != null ? bankAcc : "");
                        user.put("bankifsc", ifscCode != null ? ifscCode : "");
                        user.put("exactname", exactName != null ? exactName : "");
                        user.put("adharCardNo", adharno != null ? adharno : "");
                        user.put("reference1name", reference1 != null ? reference1 : "");
                        user.put("reference1rel", rel1 != null ? rel1 : "");
                        user.put("reference2rel", rel2 != null ? rel2 : "");
                        user.put("reference2name", reference2 != null ? reference2 : "");
                        user.put("reference2phone", mobilenumber != null ? mobilenumber : "");
                        user.put("reference1phone", mobilenumber2 != null ? mobilenumber2 : "");
                        user.put("documentType", docs != null ? docs : "");
                        user.put("mode", mode != null ? mode : "");
                        user.put("userFullName", name != null ? name : "");
                        user.put("perAreaDA", perArea != null ? perArea : "");
                        user.put("currentAreaDA", curArea != null ? curArea : "");
                        user.put("currentAddressDA", curAddress != null ? curAddress : "");
                        user.put("perAddressDA", permanentAddress != null ? permanentAddress : "");
                        user.put("currentPinCodeDA", curpin != null ? curpin : "");
                        user.put("perPinCodeDA", perpin != null ? perpin : "");
                        user.put("email", email != null ? email : "");


                        if (mobilenumber != null && !mobilenumber.equals("") && mobilenumber.length() != 10) {
                            DeliveryTrackUtils.showToast(SignUpActivity.this, "Reference Mobile number must be ten digits");
                            return;
                        }
                        if (mobilenumber2 != null && !mobilenumber2.equals("") && mobilenumber2.length() != 10) {
                            DeliveryTrackUtils.showToast(SignUpActivity.this, " Reference Mobile number must be ten digits");
                            return;
                        }

                        if (mobilenumber != null && mobilenumber2 != null && mobilenumber.equals(mobilenumber2)) {
                            DeliveryTrackUtils.showToast(SignUpActivity.this, " Reference number should not be the same");

                            return;
                        }

                        if (reference1 != null && !reference1.equals("") &&
                                reference2 != null && !reference2.equals("")
                                && rel1 != null && !rel1.equals("")
                                && rel2 != null && !rel2.equals("") && email != null && !email.equals("") &&
                                mobilenumber != null && !mobilenumber.equals("") &&
                                mobilenumber2 != null && !mobilenumber2.equals("")
                                && mode != null && !mode.equals("") && adharno != null && !adharno.equals("")
                                && bankAcc != null && !bankAcc.equals("") &&
                                exactName != null && !exactName.equals("") &&
                                bankname != null &&
                                !bankname.equals("") &&
                                ifscCode != null && !ifscCode.equals("") &&
                                curArea != null && !curArea.equals("")) {

                            user.put("userStatus", "Active");
                        }


                        dialog.show();
                        user.saveInBackground(new SaveCallback() {
                            @Override
                            public void done(ParseException e) {
                                dialog.dismiss();
                                if (e != null) {
                                    Toast.makeText(SignUpActivity.this, e.toString(), Toast.LENGTH_LONG).show();
                                } else if (e == null) {
                                    if (user.get("userStatus").toString().equals("Active")) {
                                        DeliveryTrackUtils.showToast(SignUpActivity.this, "Now you are an Active user you can now view the orders posted by Restaurants!!!");

                                    }


                                    AddDefaultCommission defaultCommission = new AddDefaultCommission(ParseUser.getCurrentUser().getObjectId(), DeliveryTrackUtils.AGENT, SignUpActivity.this);
                                    defaultCommission.execute();
                                }

                            }
                        });


                    } else if (type.equals("Restaurant") && restName != null
                            && type != null) {
                        final ParseUser user = ParseUser.getCurrentUser();

                        Object userTotal = user.get("total");
                        double amountOfuser = 0;
                        if (userTotal instanceof Integer) {
                            amountOfuser = ((Integer) userTotal).doubleValue();

                        } else if (userTotal instanceof Double) {
                            amountOfuser = (Double) user.get("total");
                        }

                        user.setUsername(managerMobileNumber);
                        user.put("name", type);
                        user.put("area", restArea != null ? restArea : "");
                        user.put("phone", managerMobileNumber != null ? managerMobileNumber : "");
                        user.put("exactname", exactName != null ? exactName : "");
                        user.put("pincode", restpincode != null ? restpincode : "");
                        user.setEmail(email != null ? email : "");
                        user.put("address", restAddress != null ? restAddress : "");
                        user.put("credit", "0.0");
                        user.put("debit", "0.0");
                        if (amountOfuser > 0) {
                            user.put("total", amountOfuser);
                        } else {
                            user.put("total", total);
                        }

                        user.put("bankname", bankname != null ? bankname : "");
                        user.put("bankaccount", bankAcc != null ? bankAcc : "");
                        user.put("bankifsc", ifscCode != null ? ifscCode : "");
                        user.put("entitytype", entitiy != null ? entitiy : "");
                        user.put("managerName", manageName != null ? manageName : "");
                        user.put("restName", restName != null ? restName : "");

                        if (entitiy != null && !entitiy.equals("")
                                && manageName != null && !manageName.equals("")
                                && managerMobileNumber != null && !managerMobileNumber.equals("")
                                && bankAcc != null && !bankAcc.equals("") && exactName != null && !exactName.equals("")
                                && bankname != null && !bankname.equals("") && ifscCode != null && !ifscCode.equals("")) {
                            user.put("userStatus", "Active");
                        }

                        dialog.show();
                        user.saveInBackground(new SaveCallback() {
                            @Override
                            public void done(ParseException e) {
                                dialog.dismiss();
                                if (e != null) {
                                    DeliveryTrackUtils.showToast(SignUpActivity.this, e.toString());


                                } else if (e == null) {
                                    if (user.get("userStatus").toString().equals("Active")) {
                                        DeliveryTrackUtils.showToast(SignUpActivity.this, "Now you are an Active user you can now view and post orders!!!");

                                    }
                                    AddDefaultCommission defaultCommission = new AddDefaultCommission(ParseUser.getCurrentUser().getObjectId(), DeliveryTrackUtils.RESTAURANT, SignUpActivity.this);
                                    defaultCommission.execute();

                                }

                            }
                        });

                    }

                } else {
                    if (mEditingAfterReview) {
                        mPager.setCurrentItem(mPagerAdapter.getCount() - 1);
                    } else {
                        mPager.setCurrentItem(mPager.getCurrentItem() + 1);
                    }
                }
            }
        });


        mPrevButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                mPager.setCurrentItem(mPager.getCurrentItem() - 1);
            }
        });

        onPageTreeChanged();
        updateBottomBar();


    }

    public void showPopDialog(String pushMessage) {
        new AlertDialog.Builder(SignUpActivity.this)
                .setTitle("Registration Reminder")
                .setMessage(pushMessage)
                .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {

                    }
                })
                .show();
    }


    @Override
    public void onPageTreeChanged() {
        mCurrentPageSequence = mWizardModel.getCurrentPageSequence();
        recalculateCutOffPage();
        mStepPagerStrip.setPageCount(mCurrentPageSequence.size() + 1);
        mPagerAdapter.notifyDataSetChanged();
        updateBottomBar();
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        newConfig.locale = new Locale(DeliveryTrackUtils.getLocale(this));


    }

    private void updateBottomBar() {
        int position = mPager.getCurrentItem();
        if (position == mCurrentPageSequence.size()) {
            lyt.setVisibility(View.VISIBLE);
            mNextButton.setText(R.string.finish);
            mNextButton.setBackgroundResource(R.drawable.finish_background);
            mNextButton.setTextAppearance(this, R.style.TextAppearanceFinish);
        } else {
            lyt.setVisibility(View.GONE);

            mNextButton.setText(mEditingAfterReview ? R.string.review
                    : R.string.next);
            mNextButton
                    .setBackgroundResource(R.drawable.selectable_item_background);
            TypedValue v = new TypedValue();
            getTheme().resolveAttribute(android.R.attr.textAppearanceMedium, v,
                    true);
            mNextButton.setTextAppearance(this, v.resourceId);
            mNextButton.setEnabled(position != mPagerAdapter.getCutOffPage());
        }

        mPrevButton
                .setVisibility(position <= 0 ? View.INVISIBLE : View.VISIBLE);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        mWizardModel.unregisterListener(this);
    }

    @Override
    protected void onSaveInstanceState(Bundle outState) {
        super.onSaveInstanceState(outState);
        outState.putBundle("model", mWizardModel.save());
        Log.d("the values", mWizardModel.toString() + " ");
    }

    @Override
    public AbstractWizardModel onGetModel() {
        return mWizardModel;
    }

    @Override
    protected void onPause() {
        super.onPause();
    }

    @Override
    public void onEditScreenAfterReview(String key) {
        for (int i = mCurrentPageSequence.size() - 1; i >= 0; i--) {
            if (mCurrentPageSequence.get(i).getKey().equals(key)) {
                mConsumePageSelectedEvent = true;
                mEditingAfterReview = true;
                mPager.setCurrentItem(i);
                updateBottomBar();
                break;
            }
        }
    }

    @Override
    public void onPageDataChanged(Page page) {
        if (page.isRequired()) {
            if (recalculateCutOffPage()) {
                mPagerAdapter.notifyDataSetChanged();
                updateBottomBar();
            }
        }
    }

    @Override
    public Page onGetPage(String key) {
        return mWizardModel.findByKey(key);
    }

    private boolean recalculateCutOffPage() {
        // Cut off the pager adapter at first required page that isn't completed
        int cutOffPage = mCurrentPageSequence.size() + 1;
        for (int i = 0; i < mCurrentPageSequence.size(); i++) {
            Page page = mCurrentPageSequence.get(i);
            if (page.isRequired() && !page.isCompleted()) {
                cutOffPage = i;
                break;
            }
        }

        if (mPagerAdapter.getCutOffPage() != cutOffPage) {
            mPagerAdapter.setCutOffPage(cutOffPage);
            return true;
        }

        return false;
    }

    @Override
    public void isCommissionSaved(boolean isSaved) {

        Intent i = new Intent(SignUpActivity.this,
                BaseActivity.class);
        startActivity(i);

        SharedPreferences.Editor editor = DeliveryTrackUtils.getSharedPreferences(SignUpActivity.this).edit();
        editor.putBoolean("isRegistered", true);
        editor.commit();

        Intent uploadAdar = new Intent(SignUpActivity.this, UploadAdharService.class);
        startService(uploadAdar);


        Toast.makeText(getApplicationContext(),
                "Updated Successfully",
                Toast.LENGTH_LONG).show();


    }

    public class MyPagerAdapter extends FragmentStatePagerAdapter {
        private int mCutOffPage;
        private Fragment mPrimaryItem;

        public MyPagerAdapter(FragmentManager fm) {
            super(fm);
        }

        @Override
        public Fragment getItem(int i) {
            if (i >= mCurrentPageSequence.size()) {
                return new ReviewFragment();
            }

            return mCurrentPageSequence.get(i).createFragment();
        }

        @Override
        public int getItemPosition(Object object) {
            // TODO: be smarter about this
            if (object == mPrimaryItem) {
                // Re-use the current fragment (its position never changes)
                return POSITION_UNCHANGED;
            }

            return POSITION_NONE;
        }

        @Override
        public void setPrimaryItem(ViewGroup container, int position,
                                   Object object) {
            super.setPrimaryItem(container, position, object);
            mPrimaryItem = (Fragment) object;
        }

        @Override
        public int getCount() {
            if (mCurrentPageSequence == null) {
                return 0;
            }
            return Math.min(mCutOffPage + 1, mCurrentPageSequence.size() + 1);
        }

        public void setCutOffPage(int cutOffPage) {
            if (cutOffPage < 0) {
                cutOffPage = Integer.MAX_VALUE;
            }
            mCutOffPage = cutOffPage;
        }

        public int getCutOffPage() {
            return mCutOffPage;
        }
    }
}
