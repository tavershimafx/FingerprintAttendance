namespace FingerprintAttendance.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThumnail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FingerPrints", "ThumbThumbnail", c => c.Binary());
            AddColumn("dbo.FingerPrints", "IndexFingerThumbnail", c => c.Binary());
            AddColumn("dbo.FingerPrints", "MiddleFingerThumbnail", c => c.Binary());
            AddColumn("dbo.FingerPrints", "RingFingerThumbnail", c => c.Binary());
            AddColumn("dbo.FingerPrints", "LittleFingerThumbnail", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FingerPrints", "LittleFingerThumbnail");
            DropColumn("dbo.FingerPrints", "RingFingerThumbnail");
            DropColumn("dbo.FingerPrints", "MiddleFingerThumbnail");
            DropColumn("dbo.FingerPrints", "IndexFingerThumbnail");
            DropColumn("dbo.FingerPrints", "ThumbThumbnail");
        }
    }
}
