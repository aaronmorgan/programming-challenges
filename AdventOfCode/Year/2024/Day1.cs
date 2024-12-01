using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day1
{
    [Theory]
    [InlineData("Day1DevelopmentTesting1.txt", 11, 31)]
    [InlineData("Day1.txt", 2113135, 19097157)]
    public void Day1_Part1_HistorianHysteria(string filename, int expectedAnswer, int expectedSimilarityScore)
    {
        var input = InputParser.ReadAllLines("2024/" + filename).ToArray();

        var list1 = new List<int>(input.Length);
        var list2 = new List<int>(input.Length);

        foreach (var line in input)
        {
            var tmpStr = line.Split("   ");

            list1.Add(int.Parse(tmpStr[0]));
            list2.Add(int.Parse(tmpStr[1]));
        }

        list1.Sort();
        list2.Sort();

        int distance = 0, similarity = 0;

        for (var i = 0; i < list1.Count; i++)
        {
            if (list1[i] > list2[i])
            {
                distance += list1[i] - list2[i];
            }
            else
            {
                distance += list2[i] - list1[i];
            }

            // Part 2, multiple the number in list1 by it's number of occurrences in list2.
            similarity += list1[i] * list2.FindAll(x => x == list1[i]).Count;
        }

        Assert.Equal(expectedAnswer, distance);
        Assert.Equal(expectedSimilarityScore, similarity);
    }
}