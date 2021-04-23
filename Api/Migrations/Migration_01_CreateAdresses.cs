using FluentMigrator;

[Migration(01)]
public class _01_Create_Delivery_Adresses : Migration
{
    public override void Down()
    {
        Delete.Index("ix_delivery_adresses").OnTable("delivery_adresses");
        Delete.Table("delivery_adresses");
    }

    public override void Up()
    {
        Create.Table("delivery_adresses")
        .WithColumn("id").AsFixedLengthString(36).PrimaryKey("delivery_adress_id")
        .WithColumn("user").AsFixedLengthString(36).ForeignKey("users", "id")
        .WithColumn("state").AsString().Nullable()
        .WithColumn("city").AsString().Nullable()
        .WithColumn("district").AsString().Nullable()
        .WithColumn("street").AsString().Nullable()
        .WithColumn("number").AsInt32().Nullable()
        .WithColumn("reference").AsString().Nullable();

        Create.Index("ix_delivery_adresses")
        .OnTable("delivery_adresses")
        .OnColumn("id")
        .Ascending()
        .WithOptions().Clustered();
    }
}