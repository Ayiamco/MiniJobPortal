namespace inSpark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedAdmin : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO[dbo].[AspNetUsers] ([Id], [FullName], [Address], [Gender], [DateOfBirth], [ProfilePicturePath], [ResumePath], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES(N'a6462967-47f4-4805-a45c-37606cf75396', N'Joseph Achonu Chukwuyere', N'10 austin opara street mile3 PH', N'Male', N'2004-11-19 00:00:00', N'/FileStorage/ProfilePictureFiles/IMG-20190103-WA0002203913210.jpg', N'/FileStorage/ApplicantsResumeFiles/11.1 Client-side Development Cheat Sheet203913235.pdf', N'myadmin@gmail.com', 0, N'AHXL+v6jKFu+7Y4SgrPBX/zgxQroZauR5dH4O1XS2BWk/MRfGu7wKctXtjNHBzF9HA==', N'2d0f2eff-347d-4d00-afc1-0f0a9e08f223', NULL, 0, 0, NULL, 1, 0, N'myadmin@gmail.com')");
            Sql("INSERT INTO[dbo].[AspNetUserRoles]([UserId],RoleId) VALUES(N'a6462967-47f4-4805-a45c-37606cf75396',N'5290e882-c2c0-4fcb-a185-97359e514c35')");
        }

        public override void Down()
        {
            Sql("DELETE FROM [dbo].[AspNetUsers] WHERE Id=N'a6462967-47f4-4805-a45ca45c-37606cf75396'");
            Sql("DELETE FROM [dbo].[AspNetUserRoles] WHERE UserId=N'a6462967-47f4-4805-a45ca45c-37606cf75396'");
        }
    }
}
