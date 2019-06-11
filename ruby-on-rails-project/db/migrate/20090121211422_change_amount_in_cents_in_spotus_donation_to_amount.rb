class TempDonation < ActiveRecord::Base
  set_table_name :_donations
end

class ChangeAmountInCentsInDonationToAmount < ActiveRecord::Migration
  def self.up
    add_column :_donations, :amount, :decimal, :precision => 15, :scale => 2
    TempDonation.update_all('amount = amount_in_cents / 100')
    remove_column :_donations, :amount_in_cents
  end

  def self.down
    add_column :_donations, :amount_in_cents, :integer
    TempDonation.update_all('amount_in_cents = amount * 100')
    remove_column :_donations, :amount
  end
end
