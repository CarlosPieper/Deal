using FluentMigrator;

[Migration(00)]
public class _00_CreateUser : Migration
{
    public override void Down()
    {
        Delete.Index("ix_users").OnTable("users");
        Delete.Table("users");
    }

    public override void Up()
    {
        Create.Table("users")
        .WithColumn("id").AsFixedLengthString(36).PrimaryKey("user_id")
        .WithColumn("name").AsString().NotNullable()
        .WithColumn("email").AsString().NotNullable()
        .WithColumn("password").AsString().NotNullable()
        .WithColumn("default_delivery_adress").AsString().Nullable()
        .WithColumn("creation_date").AsDateTime().NotNullable()
        .WithColumn("last_update_date").AsDateTime().NotNullable();

        Create.Index("ix_users")
        .OnTable("users")
        .OnColumn("id")
        .Ascending()
        .WithOptions().Clustered();
    }
}