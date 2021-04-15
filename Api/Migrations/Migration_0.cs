using FluentMigrator;

[Migration(0)]
public class Migration_0 : Migration
{
    public override void Down()
    {
        Delete.Index("ix_users").OnTable("users");
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

        Create.Index("ix_users")
        .OnTable("users")
        .OnColumn("id")
        .Ascending()
        .WithOptions().Clustered();
    }
}