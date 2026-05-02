namespace SchemaDefinition.Models;

/// <summary>
/// Represents a user-defined schema composed of typed fields.
/// </summary>
public class Schema
{
    /// <summary>Unique identifier for the schema.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Human-readable name of the schema.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional description of the schema's purpose.</summary>
    public string? Description { get; set; }

    /// <summary>The list of fields that make up this schema.</summary>
    public List<SchemaField> Fields { get; set; } = [];

    /// <summary>Timestamp (UTC) when the schema was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Timestamp (UTC) when the schema was last updated.</summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
