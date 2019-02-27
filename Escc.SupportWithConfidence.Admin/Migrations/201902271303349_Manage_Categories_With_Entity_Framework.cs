namespace Escc.SupportWithConfidence.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Manage_Categories_With_Entity_Framework : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Sequence = c.Int(nullable: false),
                        Code = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ParentId = c.Int(),
                        Depth = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Categories");
        }
    }
}
