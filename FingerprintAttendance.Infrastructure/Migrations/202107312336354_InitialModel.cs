namespace FingerprintAttendance.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttendanceLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RecordDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        UserName = c.String(nullable: false),
                        LeftHandId = c.Long(nullable: false),
                        RightHandId = c.Long(nullable: false),
                        Picture = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FingerPrints", t => t.LeftHandId, cascadeDelete: false)
                .ForeignKey("dbo.FingerPrints", t => t.RightHandId, cascadeDelete: false)
                .Index(t => t.LeftHandId)
                .Index(t => t.RightHandId);
            
            CreateTable(
                "dbo.FingerPrints",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Thumb = c.Binary(),
                        IndexFinger = c.Binary(),
                        MiddleFinger = c.Binary(),
                        RingFinger = c.Binary(),
                        LittleFinger = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Roles", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "RightHandId", "dbo.FingerPrints");
            DropForeignKey("dbo.Users", "LeftHandId", "dbo.FingerPrints");
            DropForeignKey("dbo.AttendanceLogs", "UserId", "dbo.Users");
            DropIndex("dbo.Roles", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "RightHandId" });
            DropIndex("dbo.Users", new[] { "LeftHandId" });
            DropIndex("dbo.AttendanceLogs", new[] { "UserId" });
            DropTable("dbo.Roles");
            DropTable("dbo.FingerPrints");
            DropTable("dbo.Users");
            DropTable("dbo.AttendanceLogs");
        }
    }
}
