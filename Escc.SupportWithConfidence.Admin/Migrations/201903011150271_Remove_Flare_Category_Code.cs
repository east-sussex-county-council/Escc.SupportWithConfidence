namespace Escc.SupportWithConfidence.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Flare_Category_Code : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Categories", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Code", c => c.String(nullable: false));
        }
    }
}
