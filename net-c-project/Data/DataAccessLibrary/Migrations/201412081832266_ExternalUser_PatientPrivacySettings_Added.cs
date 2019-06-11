namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExternalUser_PatientPrivacySettings_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patient", "ShareDataWithResearcher", c => c.Boolean());
            AddColumn("dbo.Patient", "ShareDataForQualityAssurance", c => c.Boolean());
            AddColumn("dbo.User", "IsExternalUser", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "IsExternalUser");
            DropColumn("dbo.Patient", "ShareDataForQualityAssurance");
            DropColumn("dbo.Patient", "ShareDataWithResearcher");
        }
    }
}
