class AddCreditIdToDonations < ActiveRecord::Migration
  def self.up
    add_column :_donations, :credit_id, :integer
  end

  def self.down
    remove_column :_donations, :credit_id
  end
end
