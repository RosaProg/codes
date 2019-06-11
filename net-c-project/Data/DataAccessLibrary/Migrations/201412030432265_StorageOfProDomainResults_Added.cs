namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StorageOfProDomainResults_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProDomainResult",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DomainId = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                        ProDomainResultSet_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProDomain", t => t.DomainId, cascadeDelete: true)
                .ForeignKey("dbo.ProDomainResultSet", t => t.ProDomainResultSet_Id, cascadeDelete: true)
                .Index(t => t.DomainId)
                .Index(t => t.ProDomainResultSet_Id);
            
            CreateTable(
                "dbo.ProDomainResultSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionnaireUserResponseGroup", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProDomainResult", "ProDomainResultSet_Id", "dbo.ProDomainResultSet");
            DropForeignKey("dbo.ProDomainResultSet", "GroupId", "dbo.QuestionnaireUserResponseGroup");
            DropForeignKey("dbo.ProDomainResult", "DomainId", "dbo.ProDomain");
            DropIndex("dbo.ProDomainResultSet", new[] { "GroupId" });
            DropIndex("dbo.ProDomainResult", new[] { "ProDomainResultSet_Id" });
            DropIndex("dbo.ProDomainResult", new[] { "DomainId" });
            DropTable("dbo.ProDomainResultSet");
            DropTable("dbo.ProDomainResult");
        }
    }
}
