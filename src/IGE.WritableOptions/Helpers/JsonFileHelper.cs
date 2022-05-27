namespace IGE.WritableOptions.Helpers;

using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

public static class JsonFileHelper
{
  public static Func<JsonSerializerOptions> DefaultSerializerOptions => new(() =>
  {
    return new()
    {
      WriteIndented = true,
      Converters = { new JsonStringEnumConverter() },
    };
  });

  public static void AddOrUpdateSection<T>(
    string fullPath,
    string sectionName,
    Action<T> applyChanges,
    JsonSerializerOptions? serializerOptions = null)
    where T : class, new()
  {

    if (serializerOptions is null)
      serializerOptions = DefaultSerializerOptions.Invoke();

    var jsonContent = ReadOrCreateJsonFile(fullPath);

    var rootNode = JsonNode.Parse(jsonContent);

    var updatedObject =
      rootNode?[sectionName]?.Deserialize<T>(serializerOptions) ?? new();

    applyChanges(updatedObject);

    rootNode![sectionName] =
      JsonSerializer.SerializeToNode(updatedObject, serializerOptions);

    File.WriteAllText(fullPath, string.Empty, Encoding.UTF8);

    var fileStream = File.OpenWrite(fullPath);

    var writer = new Utf8JsonWriter(fileStream, new()
    {
      Indented = true,
    });

    rootNode.WriteTo(writer, serializerOptions);

    writer.Flush();

    fileStream.Close();
  }

  private static byte[] ReadOrCreateJsonFile(string fullPath)
  {
    if (!File.Exists(fullPath))
    {
      var fileDirectory = Path.GetDirectoryName(fullPath);

      if (!string.IsNullOrEmpty(fileDirectory))
        Directory.CreateDirectory(fileDirectory);

      File.WriteAllText(fullPath, "{}", Encoding.ASCII);
    }

    return File.ReadAllBytes(fullPath);
  }
}
