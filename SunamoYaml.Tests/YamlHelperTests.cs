using SunamoTest;
using SunamoThisApp;
using System.Threading.Tasks;

namespace SunamoYaml.Tests;

/// <summary>
/// Tests for YAML loading and saving functionality using YamlDotNet library.
/// </summary>
public class YamlHelperTests
{
    private string filePath = null!;
    private const string expectedValue = "s";
    private const string yamlKey = "text";

    /// <summary>
    /// Tests loading a YAML file and verifying that a specific key-value pair can be read correctly.
    /// </summary>
    [Fact]
    public async Task LoadYaml()
    {
        LoadDefaultPath();

        var reader = new StringReader(await File.ReadAllTextAsync(filePath));

        var yamlStream = new YamlStream();
        yamlStream.Load(reader);

        var mapping = (YamlMappingNode)yamlStream.Documents[0].RootNode;

        var text = mapping[yamlKey];
        Assert.Equal(expectedValue, text.ToString());
    }

    private void LoadDefaultPath()
    {
        ThisApp.Name = "sunamo.Tests";
        ThisApp.Project = "SunamoYaml.Tests";

        filePath = TestHelper.GetFileInProjectsFolder("test.yaml");
    }

    /// <summary>
    /// Tests serializing an object graph to YAML format and writing it to a file.
    /// </summary>
    [Fact]
    public void SaveYaml()
    {
        LoadDefaultPath();

        var testObject = new
        {
            text = expectedValue,
            date = new DateTime(2007, 8, 6),
            anonymousType = new
            {
                given = "Dorothy",
                family = "Gale"
            },
            items = new[]
            {
                new
                {
                    id = 0,
                    name = "a"
                },
                new
                {
                    id = 1,
                    name = "b"
                }
            }
        };

        var list = new List<object>();
        list.Add(testObject);

        var serializer = new Serializer();
        var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, list);
        File.WriteAllText(filePath, stringWriter.ToString());

        Assert.True(File.Exists(filePath));
        var content = File.ReadAllText(filePath);
        Assert.Contains("Dorothy", content);
        Assert.Contains("Gale", content);
    }
}
