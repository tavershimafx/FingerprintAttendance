namespace FingerprintAttendance.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFingerPrintId_Nulls : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "LeftHandId", "dbo.FingerPrints");
            DropForeignKey("dbo.Users", "RightHandId", "dbo.FingerPrints");
            DropIndex("dbo.Users", new[] { "LeftHandId" });
            DropIndex("dbo.Users", new[] { "RightHandId" });
            AlterColumn("dbo.Users", "LeftHandId", c => c.Long());
            AlterColumn("dbo.Users", "RightHandId", c => c.Long());
            CreateIndex("dbo.Users", "LeftHandId");
            CreateIndex("dbo.Users", "RightHandId");
            AddForeignKey("dbo.Users", "LeftHandId", "dbo.FingerPrints", "Id");
            AddForeignKey("dbo.Users", "RightHandId", "dbo.FingerPrints", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RightHandId", "dbo.FingerPrints");
            DropForeignKey("dbo.Users", "LeftHandId", "dbo.FingerPrints");
            DropIndex("dbo.Users", new[] { "RightHandId" });
            DropIndex("dbo.Users", new[] { "LeftHandId" });
            AlterColumn("dbo.Users", "RightHandId", c => c.Long(nullable: false));
            AlterColumn("dbo.Users", "LeftHandId", c => c.Long(nullable: false));
            CreateIndex("dbo.Users", "RightHandId");
            CreateIndex("dbo.Users", "LeftHandId");
            AddForeignKey("dbo.Users", "RightHandId", "dbo.FingerPrints", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Users", "LeftHandId", "dbo.FingerPrints", "Id", cascadeDelete: true);
        }
    }
}
