namespace Escc.SupportWithConfidence.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Category_Summary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Summary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "Summary");
        }
    }
}
