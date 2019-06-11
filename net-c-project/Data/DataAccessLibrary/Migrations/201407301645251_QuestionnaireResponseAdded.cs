namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionnaireResponseAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionnaireResponse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        usedId = c.Int(nullable: false),
                        ResponseValue = c.Double(),
                        ResponseText = c.String(),
                        Item_Id = c.Int(),
                        Option_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionnaireElement", t => t.Item_Id)
                .ForeignKey("dbo.QuestionnaireItemOption", t => t.Option_Id)
                .Index(t => t.Item_Id)
                .Index(t => t.Option_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireResponse", "Option_Id", "dbo.QuestionnaireItemOption");
            DropForeignKey("dbo.QuestionnaireResponse", "Item_Id", "dbo.QuestionnaireElement");
            DropIndex("dbo.QuestionnaireResponse", new[] { "Option_Id" });
            DropIndex("dbo.QuestionnaireResponse", new[] { "Item_Id" });
            DropTable("dbo.QuestionnaireResponse");
        }
    }
}
