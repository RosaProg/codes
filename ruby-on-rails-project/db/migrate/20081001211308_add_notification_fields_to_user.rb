class AddNotificationFieldsToUser < ActiveRecord::Migration
  def self.up
    change_table :users do |t|
      t.boolean :notify_tips, :notify_pitches, :notify_stories, :notify__news
    end
  end

  def self.down
    change_table :users do |t|
      t.remove :notify_tips, :notify_pitches, :notify_stories, :notify__news
    end
  end
end
