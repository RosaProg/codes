namespace DSPrima.UserSessionStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SessionStoreData",
                c => new
                    {
                        SessionKey = c.String(nullable: false, maxLength: 450),
                        SessionData = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SessionKey);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SessionStoreData");
        }
    }
}
