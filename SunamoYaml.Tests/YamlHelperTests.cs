using SunamoTest;
using SunamoThisApp;
using System.Threading.Tasks;

namespace SunamoYaml.Tests;

public class YamlHelperTests
{
    string path = null;
    const string s = "s";
    const string ixTest = "text";

    //[Fact]
    public async Task LoadYaml()
    {
        LoadDefaultPath();

        // Setup the input
        var input = new StringReader(await File.ReadAllTextAsync(path));

        // Load the stream
        var yaml = new YamlStream();
        yaml.Load(input);

        // Examine the stream
        var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

        var text = mapping[ixTest];
        Assert.Equal(s, text.ToString());
    }

    private void LoadDefaultPath()
    {
        ThisApp.Name = "sunamo.Tests";
        ThisApp.Project = "SunamoYaml.Tests";

        path = TestHelper.GetFileInProjectsFolder("test.yaml");
    }

    //[Fact]
    public void SaveYaml()
    {
        LoadDefaultPath();

        // Four node types: Alias, Mapping, Scalar, Sequence

        var o = new
        {
            text = s,
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
        list.Add(o);

        var serializer = new Serializer();
        StringWriter sw = new StringWriter();
        serializer.Serialize(sw, list);
        File.WriteAllText(path, sw.ToString());
    }
}