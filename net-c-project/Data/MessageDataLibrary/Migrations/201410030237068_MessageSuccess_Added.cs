namespace PCHI.PMS.MessageDataLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageSuccess_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "Success", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Message", "Success");
        }
    }
}
