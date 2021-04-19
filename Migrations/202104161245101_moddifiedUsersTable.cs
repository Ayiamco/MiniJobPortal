namespace inSpark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moddifiedUsersTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.String());
            AlterColumn("dbo.AspNetUsers", "ProfilePicturePath", c => c.String());
            AlterColumn("dbo.AspNetUsers", "ResumePath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "ResumePath", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "ProfilePicturePath", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Address", c => c.String(nullable: false));
        }
    }
}
