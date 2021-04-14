using FluentMigrator;

[Migration(04_13_2021_000)]
public class Migration_04_13_2021_000 : Migration
{
    public override void Down()
    {
        Delete.Table("users");
    }

    public override void Up()
    {
        Create.Table("users")
        .WithColumn("id").AsString().NotNullable().Identity().PrimaryKey()
        .WithColumn("name").AsString().NotNullable()
        .WithColumn("email").AsString().NotNullable()
        .WithColumn("password").AsString().NotNullable()
        .WithColumn("default_delivery_adress").AsString().NotNullable()
        .WithColumn("creation_date").AsDateTime().NotNullable();
    }
}