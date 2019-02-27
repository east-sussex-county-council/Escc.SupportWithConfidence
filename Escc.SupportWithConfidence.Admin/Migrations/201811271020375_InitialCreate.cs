namespace Escc.SupportWithConfidence.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accreditations",
                c => new
                    {
                        AccreditationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Website = c.String(),
                    })
                .PrimaryKey(t => t.AccreditationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Accreditations");
        }
    }
}
