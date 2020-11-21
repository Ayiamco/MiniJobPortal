namespace inSpark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRples : DbMigration
    {

        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'5290e882-c2c0-4fcb-a185-97359e514c35', N'CanAddJobs')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'c9848e50-9377-4983-8a52-b0f95ba05075', N'CanApplyForJobs')
                ");
        }

        public override void Down()
        {
            Sql("DELETE FROM  [dbo].[AspNetRoles]");
        }
    }
}