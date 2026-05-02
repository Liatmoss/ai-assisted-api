using System.Text.Json;
using SchemaDefinition.Models;

namespace SchemaDefinition.Storage;

/// <summary>
/// Persists schemas to a local JSON file on disk.
/// </summary>
public class LocalSchemaStorage
{
    private readonly string _filePath;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Initialises storage backed by the given file path.
    /// The parent directory is created automatically if it does not exist.
    /// </summary>
    /// <param name="filePath">Absolute or relative path to the backing JSON file.</param>
    public LocalSchemaStorage(string filePath)
    {
        _filePath = filePath;
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
    }

    /// <summary>Returns all stored schemas.</summary>
    public IReadOnlyList<Schema> GetAll()
    {
        return Load();
    }

    /// <summary>Returns a schema by its unique identifier, or <c>null</c> if not found.</summary>
    public Schema? GetById(Guid id)
    {
        return Load().FirstOrDefault(s => s.Id == id);
    }

    /// <summary>Persists a new schema and returns it with its assigned Id.</summary>
    public Schema Add(Schema schema)
    {
        var schemas = Load();
        schema.Id = Guid.NewGuid();
        schema.CreatedAt = DateTime.UtcNow;
        schema.UpdatedAt = DateTime.UtcNow;
        schemas.Add(schema);
        Save(schemas);
        return schema;
    }

    /// <summary>
    /// Updates an existing schema.
    /// Returns <c>true</c> when the schema was found and updated, <c>false</c> otherwise.
    /// </summary>
    public bool Update(Schema updated)
    {
        var schemas = Load();
        var index = schemas.FindIndex(s => s.Id == updated.Id);
        if (index < 0) return false;

        updated.UpdatedAt = DateTime.UtcNow;
        schemas[index] = updated;
        Save(schemas);
        return true;
    }

    /// <summary>
    /// Deletes the schema with the given identifier.
    /// Returns <c>true</c> when a schema was removed, <c>false</c> if it was not found.
    /// </summary>
    public bool Delete(Guid id)
    {
        var schemas = Load();
        var removed = schemas.RemoveAll(s => s.Id == id);
        if (removed == 0) return false;
        Save(schemas);
        return true;
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private List<Schema> Load()
    {
        if (!File.Exists(_filePath))
            return [];

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Schema>>(json, JsonOptions) ?? [];
    }

    private void Save(List<Schema> schemas)
    {
        var json = JsonSerializer.Serialize(schemas, JsonOptions);
        File.WriteAllText(_filePath, json);
    }
}
