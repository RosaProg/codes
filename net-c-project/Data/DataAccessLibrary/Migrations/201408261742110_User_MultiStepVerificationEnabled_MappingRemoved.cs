namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_MultiStepVerificationEnabled_MappingRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "MultiStepVerificationEnabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "MultiStepVerificationEnabled", c => c.Boolean(nullable: false));
        }
    }
}
