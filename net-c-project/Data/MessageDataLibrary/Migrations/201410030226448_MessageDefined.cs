namespace PCHI.PMS.MessageDataLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageDefined : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageReference = c.String(maxLength: 100),
                        MessageText = c.String(),
                        dateTimeOfMessage = c.DateTime(nullable: false),
                        WasSent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.MessageReference);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Message", new[] { "MessageReference" });
            DropTable("dbo.Message");
        }
    }
}
