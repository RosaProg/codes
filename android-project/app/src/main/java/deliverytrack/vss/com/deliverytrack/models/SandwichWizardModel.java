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

package deliverytrack.vss.com.deliverytrack.models;

import android.content.Context;
import android.util.Log;

import deliverytrack.vss.com.deliverytrack.R;
import deliverytrack.vss.com.deliverytrack.utility.DeliveryTrackUtils;


public class SandwichWizardModel extends AbstractWizardModel {
    private final Context context;

    public SandwichWizardModel(Context context) {
        super(context);
        this.context = context;
    }

    @Override
    protected PageList onNewRootPageList(Context context) {

        Log.d("the context", "ffff" + context + " ");

        BranchPage brPage = new BranchPage(this, context.getResources().getString(R.string.login_type));
        if (DeliveryTrackUtils.getUserType().equals("Agent")) {
            brPage.addBranch(
                    context.getResources().getString(R.string.delivery_freelancer_register),
                    new SingleFixedChoicePage(this,
                            context.getResources().getString(R.string.mode_of_deliver))
                            .setChoices(context.getResources().getString(R.string.car),
                                    context.getResources().getString(R.string.motor_bike),
                                    context.getResources().getString(R.string.bicycle),
                                    context.getResources().getString(R.string.rikshaw),
                                    context.getResources().getString(R.string.foot))
                            .setRequired(true));
        } else {
            brPage.addBranch(context.getResources().getString(R.string.restaurant_register)).setRequired(true);

        }


        return new PageList(brPage, new AgentDetailPage(this, "Personal Details").setRequired(true),
                new AgentDocumentsPage(this, "Documents").setRequired(true),
                new AgentReferencePage(this, "Reference").setRequired(true),
                new BankDetailsPage(this, "Bank Details").setRequired(true)
        );
    }
}
