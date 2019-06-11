namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwoStepAuthenticationProvider_AddedToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "TwoFactorAuthenticationProvider", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "TwoFactorAuthenticationProvider");
        }
    }
}
