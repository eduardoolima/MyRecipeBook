using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DataBaseVersions.Table_User, "Create table to save user info")]
    public class Version0000001 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Users")
                .WithColumn("Name").AsString(128).NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("Password").AsString(2000).NotNullable();
        }
    }
}
