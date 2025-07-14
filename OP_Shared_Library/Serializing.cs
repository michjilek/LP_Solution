using YamlDotNet.Serialization;

namespace OP_Shared_Library;
public static class Serializing
{
    // Serialization for *.yaml
    public static List<T> LoadYamlList<T>(string url, Stream stream)
    {
        try
        {
            using var reader = new StreamReader(stream);

            var deserializer = new DeserializerBuilder().Build();

            var items = deserializer.Deserialize<List<T>>(reader);
            return items ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading YAML: {ex.Message}");
            return new List<T>();
        }
    }
    public static string SerializeYaml<T>(List<T> items)
    {
        try
        {
            // Create a YAML serializer with indented sequences for better readability
            var serializer = new SerializerBuilder()
                .WithIndentedSequences()
                .Build();

            // Serialize the list of items into a YAML string
            var yaml = serializer.Serialize(items);

            return yaml;
        }
        catch (Exception ex)
        {
            // Log the error and return an empty string in case of failure
            Console.WriteLine($"Error serializing YAML: {ex.Message}");
            return string.Empty;
        }
    }
}
