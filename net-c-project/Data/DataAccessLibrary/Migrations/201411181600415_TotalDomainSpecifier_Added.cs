namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalDomainSpecifier_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProDomain", "IsTotalDomain", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProDomain", "IsTotalDomain");
        }
    }
}
