namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PCHIError_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PCHIError",
                c => new
                    {
                        ErrorCode = c.Int(nullable: false),
                        ErrorMessage = c.String(),
                        HelpLink = c.String(),
                        Source = c.String(),
                        HResult = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ErrorCode);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PCHIError");
        }
    }
}
