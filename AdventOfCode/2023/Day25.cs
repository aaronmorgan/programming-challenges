using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day25
{
    [Theory]
    [InlineData("Day25DevelopmentTesting1.txt", 54)]
    //[InlineData("Day25.txt", 397061)]
    public void Day25_Part1_Aplenty(string filename, int expectedAnswer)
    {
        Dictionary<string, List<string>> wiringDiagram = [];
        List<string> list1 = [];
        List<string> list2 = [];

        foreach (var line in FileLoader.ReadAllLines("2023/" + filename))
        {
            var strings = line.Split(' ');

            wiringDiagram.Add(strings[0].Trim(':'), strings.Skip(1).ToList());
        }


        List<string> bb = [];
        foreach (var a in wiringDiagram)
        {
            foreach (var b1 in a.Value[0])
            foreach (var b2 in a.Value[1])
            foreach (var b3 in a.Value[2])
            {
                List<string> seen = [];
                
                
            }
        }

        return;

        string doit(List<string> a, List<string> c)
        {
            foreach (var b in a)
            {
                if (!c.Contains(b)) c.Add(b);

                if (wiringDiagram.TryGetValue(b, out var value)) return doit(value, c);
            }
            
            return null; // just to make it complile.
        }
    }
}
