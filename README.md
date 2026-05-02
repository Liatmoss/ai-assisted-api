# ai-assisted-api

Agent-assisted backend development system.

---

## Original Prompt

> I need your help to create a project that will allow a user to define a schema. The project should:
> - Be build in C#
> - Include a src folder as well a place for tests
> - Have a folder in place for future validations
> - All data stored locally
>
> Readme should contain a copy of this prompt to show the progression of the project as it is developed

---

## Project Structure

```
ai-assisted-api/
├── SchemaDefinition.slnx          # .NET solution file
├── src/
│   └── SchemaDefinition/          # Core class library
│       ├── Models/
│       │   ├── Schema.cs          # Schema aggregate (Id, Name, Fields, timestamps)
│       │   └── SchemaField.cs     # Individual field definition (Name, Type, Required)
│       └── Storage/
│           └── LocalSchemaStorage.cs  # JSON-based local persistence (CRUD)
├── tests/
│   └── SchemaDefinition.Tests/    # xUnit test project
│       └── LocalSchemaStorageTests.cs
└── validations/                   # Placeholder – future validation rules
```

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download) or later

### Build
```bash
dotnet build SchemaDefinition.slnx
```

### Run tests
```bash
dotnet test SchemaDefinition.slnx
```

## Local Storage

Schemas are serialised as a JSON array and written to a single file on disk.
Pass the desired file path when constructing `LocalSchemaStorage`:

```csharp
var storage = new LocalSchemaStorage("data/schemas.json");

var schema = storage.Add(new Schema
{
    Name = "Customer",
    Fields =
    [
        new SchemaField { Name = "FirstName", Type = "string", Required = true },
        new SchemaField { Name = "Age",       Type = "int",    Required = false }
    ]
});
```
