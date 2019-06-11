require File.expand_path(File.dirname(__FILE__) + '/../../spec_helper')

describe Myspot::DonationAmountsController do

  route_matches('/myspot/donations/amounts/edit',
                :get,
                :controller => 'myspot/donation_amounts',
                :action     => 'edit')

  route_matches('/myspot/donations/amounts',
                :put,
                :controller => 'myspot/donation_amounts',
                :action     => 'update')

  describe "when logged in with donations" do
    before do
      @user = Factory(:user)
      @donations = [Factory(:donation, :user => @user, :status => 'unpaid'),
                    Factory(:donation, :user => @user, :status => 'unpaid')]
      @_donation = Factory(:_donation, :user => @user, :purchase => nil,
                                 :amount => 10)
      @user.stub!(:current__donation).and_return(@_donation)
      controller.stubs(:current_user).returns(@user)
    end

    describe "on GET to edit" do
      before do
        @user.stub!(:donations).and_return(@donations)
        @donations.stub!(:unpaid).and_return(@donations)
      end

      it "should be successful" do
        do_edit
        response.should be_success
      end

      it "should find the logged in user" do
        controller.expects(:current_user).at_least(1).returns(@user)
        do_edit
      end

      it "should render the edit view" do
        do_edit
        response.should render_template('edit')
      end

      it "should find the user's donations" do
        do_edit
        controller.send(:unpaid_donations).should_not be_blank
      end

      it "should assign the  donation for the view" do
        do_edit
        controller.send(:_donation).should_not be_blank
      end

      def do_edit
        get :edit
      end
    end

    describe "on PUT to update with valid input" do
      it "should redirect to the new purchase page" do
        do_update
        response.should redirect_to(new_myspot_purchase_path)
      end

      it "should update the donation amounts" do
        do_update
        @donations.first.reload
        @donations.first.amount.to_f.should == 100.0
      end

      it "should update the group_id" do
        do_update
        @donations.first.reload.group_id.should == 17
      end

      it "should create a  donation if _donation_amount > 0" do
        do_update_with__donation
        assigns[:_donation].should be_an_instance_of(Donation)
      end

      describe "with only a  donation" do
        it "should not throw an error" do
          lambda { do_update_with_only__donation }.should_not raise_error
        end
        it "should redirect to purchase page" do
          do_update_with_only__donation
          response.should redirect_to(new_myspot_purchase_path)
        end
      end

      def do_update
        put :update, { :donation_amounts => { @donations.first.to_param => {:amount => 100, :group_id => '17'} }, :_donation_amount => "" }
      end

      def do_update_with__donation
        put :update, { :donation_amounts => { @donations.first.to_param => {:amount => 100} }, :_donation_amount => 1 }
      end
      def do_update_with_only__donation
        put :update, { :_donation_amount => 1 }
      end
    end

    describe "on PUT to update with invalid input" do
      it "render the edit view" do
        do_update
        response.should render_template('edit')
      end

      it "should not update the donation amounts" do
        lambda { do_update }.should_not change { @donations.first.amount }
      end

      def do_update
        put :update, { :donation_amounts => { @donations.first.to_param => {:amount => nil} } }
        @donations.first.reload
      end
    end
  end

  requires_login_for :get, :edit
  requires_login_for :put, :update

  describe "redirects" do
    it "show goes to edit" do
      get :show
      response.should redirect_to(edit_myspot_donations_amounts_path)
    end
  end
end
