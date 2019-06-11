namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsPatientFlag_addedToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "IsPatient", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "IsPatient");
        }
    }
}
