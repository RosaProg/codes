namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PMSFields_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EpisodeMilestone", "PractitionerId", c => c.String());
            AddColumn("dbo.EpisodeMilestone", "PractitionerName", c => c.String());
            AddColumn("dbo.User", "ExternalId", c => c.String(maxLength: 128));
            CreateIndex("dbo.User", "ExternalId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.User", new[] { "ExternalId" });
            DropColumn("dbo.User", "ExternalId");
            DropColumn("dbo.EpisodeMilestone", "PractitionerName");
            DropColumn("dbo.EpisodeMilestone", "PractitionerId");
        }
    }
}
