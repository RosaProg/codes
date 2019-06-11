namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DomainDiscloseTo_RenamedTo_Audience : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ProDomain", "DiscloseTo", "Audience");
            //AddColumn("dbo.ProDomain", "Audience", c => c.Int(nullable: false));
            //DropColumn("dbo.ProDomain", "DiscloseTo");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.ProDomain", "Audience", "DiscloseTo");
            //AddColumn("dbo.ProDomain", "DiscloseTo", c => c.Int(nullable: false));
            //DropColumn("dbo.ProDomain", "Audience");
        }
    }
}
