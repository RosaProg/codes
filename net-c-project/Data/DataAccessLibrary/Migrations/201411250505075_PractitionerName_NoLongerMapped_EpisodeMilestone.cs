namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PractitionerName_NoLongerMapped_EpisodeMilestone : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EpisodeMilestone", "PractitionerName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EpisodeMilestone", "PractitionerName", c => c.String());
        }
    }
}
