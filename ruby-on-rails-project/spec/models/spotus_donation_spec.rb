require File.expand_path(File.dirname(__FILE__) + '/../spec_helper')

describe Donation do
  before do
    @user = Factory(:user)
    @donation1 = Factory(:donation, :user => @user, :amount => 10)
    @donation2 = Factory(:donation, :user => @user, :amount => 10)
  end

  describe "#unpaid" do
    before do
      @_donation = Factory(:_donation, :user => @user,
                                 :purchase          => nil,
                                 :amount            => 1)
    end

    it "should only return donations with a nil purchase_id" do
      @user._donations.unpaid.should include(@_donation)
    end
  end

  describe "#unpaid?" do
    before do
      @_donation = Factory(:_donation, :user => @user,
                                 :purchase          => nil,
                                 :amount            => 1)
    end
    it "should return true with no purchase id" do
      @_donation.unpaid?.should be_true
    end
    it "returns false otherwise" do
      @_donation.purchase = mock_model(Purchase)
      @_donation.unpaid?.should be_false
    end
  end

  describe "#paid" do
    before do
      @_donation = Factory(:_donation, :user => @user,
                                                   :purchase => Factory(:purchase),
                                                   :amount => 1)
    end

    it "should only return donations with a purchase_id" do
      @user._donations.paid.should include(@_donation)
    end
  end

  it "should set the default amount to 10% of the user's unpaid donations" do
    @user._donations.build.amount.should == 2
  end

  it "should round the default amount to the nearest dollar" do
    donation = Factory(:donation, :user => @user, :amount => 54)
    # user's total unpaid donations now == $74
    @user._donations.build.amount.should == BigDecimal.new("7.0")
  end

  it "should allow the user to donate more than 10%" do
    _donation = @user._donations.build(:amount => 3)
    _donation.amount.should == BigDecimal.new("3.0")
  end

  describe ".find_from_paypal" do
    before do
      @params = { "item_number2"=>"28", "item_name2"=>"Support ", "item_number1" => "10", "item_name1" => "PITCH: Some headline"}
    end
    subject do
      Donation.find_from_paypal(@params)
    end
    it "finds the  donation" do
      _donation = mock_model(Donation)
      Donation.should_receive(:find).with("28").and_return(_donation)
      should == _donation
    end
    it "returns nil if no  donation is present" do
      @params = { "item_number1" => "10", "item_name1" => "PITCH: Some headline"}
      should be_nil
    end
    it "deletes the  donation params" do
      Donation.stub!(:find)
      subject
      @params.should_not have_key("item_number2")
    end
  end
end
