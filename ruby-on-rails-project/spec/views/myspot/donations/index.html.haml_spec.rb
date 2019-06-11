require File.dirname(__FILE__) + "/../../../spec_helper"

describe 'myspot/donations/index' do
  before do
    @user = Factory(:user)
    @pitches = [active_pitch, active_pitch]
    @donations = @pitches.collect {|pitch| Factory(:donation, :user => @user, :pitch => pitch) }
    @user.stub!(:has__donation?).and_return(true)
    @user.stub!(:last_paid__donation).and_return(Factory(:_donation))
    @user.stub!(:paid__donations_sum).and_return(35)
    assigns[:donations] = @donations
    template.stub!(:current_user).and_return(@user)
    template.stub!(:will_paginate)
  end

  it 'should render' do
    do_render
  end

  it "should render a row for the user's _donation" do
    do_render
    response.should have_tag("img[src*=support_.png]")
  end

  def do_render
    render 'myspot/donations/index'
  end
end
