using System;
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
        Console.WriteLine("aeba");
        Create.Table("users")
        .WithColumn("id").AsFixedLengthString(36).PrimaryKey("id")
        .WithColumn("name").AsString().NotNullable()
        .WithColumn("email").AsString().NotNullable()
        .WithColumn("password").AsString().NotNullable()
        .WithColumn("default_delivery_adress").AsString().Nullable()
        .WithColumn("creation_date").AsDateTime().NotNullable();

        Create.Index("ix_users")
        .OnTable("users")
        .OnColumn("id")
        .Ascending()
        .WithOptions().Clustered();
    }
}