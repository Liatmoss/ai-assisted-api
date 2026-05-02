namespace SchemaDefinition.Models;

/// <summary>
/// Represents a single field within a schema.
/// </summary>
public class SchemaField
{
    /// <summary>Name of the field.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Data type of the field (e.g. "string", "int", "bool").</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Whether the field is required.</summary>
    public bool Required { get; set; }

    /// <summary>Optional description of the field.</summary>
    public string? Description { get; set; }
}
