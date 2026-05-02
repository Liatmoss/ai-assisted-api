using SchemaDefinition.Models;
using SchemaDefinition.Storage;

namespace SchemaDefinition.Tests;

public class LocalSchemaStorageTests : IDisposable
{
    private readonly string _testFilePath;
    private readonly LocalSchemaStorage _storage;

    public LocalSchemaStorageTests()
    {
        _testFilePath = Path.Combine(Path.GetTempPath(), $"schemas_test_{Guid.NewGuid()}.json");
        _storage = new LocalSchemaStorage(_testFilePath);
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }

    [Fact]
    public void GetAll_WhenEmpty_ReturnsEmptyList()
    {
        var result = _storage.GetAll();

        Assert.Empty(result);
    }

    [Fact]
    public void Add_ValidSchema_AssignsIdAndPersists()
    {
        var schema = new Schema { Name = "Customer" };

        var added = _storage.Add(schema);

        Assert.NotEqual(Guid.Empty, added.Id);
        Assert.Single(_storage.GetAll());
    }

    [Fact]
    public void Add_MultipleSchemas_AllPersisted()
    {
        _storage.Add(new Schema { Name = "Order" });
        _storage.Add(new Schema { Name = "Product" });

        Assert.Equal(2, _storage.GetAll().Count);
    }

    [Fact]
    public void GetById_ExistingId_ReturnsCorrectSchema()
    {
        var added = _storage.Add(new Schema { Name = "Invoice" });

        var result = _storage.GetById(added.Id);

        Assert.NotNull(result);
        Assert.Equal("Invoice", result.Name);
    }

    [Fact]
    public void GetById_MissingId_ReturnsNull()
    {
        var result = _storage.GetById(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public void Update_ExistingSchema_ReturnsTrueAndPersistsChanges()
    {
        var added = _storage.Add(new Schema { Name = "Draft" });
        added.Name = "Final";

        var success = _storage.Update(added);

        Assert.True(success);
        Assert.Equal("Final", _storage.GetById(added.Id)!.Name);
    }

    [Fact]
    public void Update_MissingSchema_ReturnsFalse()
    {
        var ghost = new Schema { Id = Guid.NewGuid(), Name = "Ghost" };

        var success = _storage.Update(ghost);

        Assert.False(success);
    }

    [Fact]
    public void Delete_ExistingSchema_ReturnsTrueAndRemoves()
    {
        var added = _storage.Add(new Schema { Name = "Temp" });

        var success = _storage.Delete(added.Id);

        Assert.True(success);
        Assert.Empty(_storage.GetAll());
    }

    [Fact]
    public void Delete_MissingId_ReturnsFalse()
    {
        var success = _storage.Delete(Guid.NewGuid());

        Assert.False(success);
    }

    [Fact]
    public void Schema_SupportsFields_WithTypeAndRequiredFlag()
    {
        var schema = new Schema
        {
            Name = "Person",
            Fields =
            [
                new SchemaField { Name = "FirstName", Type = "string", Required = true },
                new SchemaField { Name = "Age",       Type = "int",    Required = false }
            ]
        };

        var added = _storage.Add(schema);
        var loaded = _storage.GetById(added.Id);

        Assert.NotNull(loaded);
        Assert.Equal(2, loaded.Fields.Count);
        Assert.Equal("FirstName", loaded.Fields[0].Name);
        Assert.Equal("string",    loaded.Fields[0].Type);
        Assert.True(loaded.Fields[0].Required);
        Assert.Equal("Age", loaded.Fields[1].Name);
        Assert.False(loaded.Fields[1].Required);
    }
}
