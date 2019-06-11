namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppliesToNameChange_Domain : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ProDomain", "AppliesTo", "DiscloseTo");            
        }
        
        public override void Down()
        {
            RenameColumn("dbo.ProDomain", "DiscloseTo", "AppliesTo");
        }
    }
}
