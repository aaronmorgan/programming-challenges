using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day5
{
    [Theory]
    [InlineData("Day5DevelopmentTesting1.txt", 143, 123)]
    [InlineData("Day5.txt", 5208, 6732)]
    public void Day5_Print_Queue(string filename, int part1ExpectedAnswer, int part2ExpectedAnswer)
    {
        List<(int, int)> pageRules = [];
        List<int[]> updates = [];

        var loadingRules = true;

        foreach (var line in InputParser.ReadAllLines("2024/" + filename))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                loadingRules = false;
                continue;
            }

            if (loadingRules)
            {
                var rule = line.Split("|").Select(int.Parse).ToArray();
                pageRules.Add((rule[0], rule[1]));

                continue;
            }

            updates.Add(line.Split(',').Select(int.Parse).ToArray());
        }

        var (part1Result, part2Result) = ValidateUpdates(updates);

        Assert.Equal(part1ExpectedAnswer, part1Result);
        Assert.Equal(part2ExpectedAnswer, part2Result);

        return;

        (int, int) ValidateUpdates(List<int[]> pages)
        {
            bool isValid = false;
            int result = 0;
            int part2 = 0;

            foreach (int[] page in pages)
            {
                for (var i = 0; i < page.Length; i++)
                {
                    if (i + 1 >= page.Length) break;

                    if (IsUpdateValid(page))
                    {
                        isValid = true;
                        continue;
                    }

                    Array.Sort(page, new PageComparer(pageRules.ToArray()));
                    part2 += page[(int)Math.Ceiling(page.Length / 2.0) - 1];

                    isValid = false;

                    break;
                }

                if (isValid)
                {
                    // Determine the middle element in the array and add it's value to the overall result.
                    result += page[(int)Math.Ceiling(page.Length / 2.0) - 1];
                }
            }

            return (result, part2);
        }


        bool IsUpdateValid(int[] page)
        {
            for (int i = 0; i < page.Length; i++)
            {
                var current = page[i];

                foreach (var rule in pageRules)
                {
                    var (l, r) = rule;

                    var indexLeft = Array.IndexOf(page, l);
                    var indexRight = Array.IndexOf(page, r);

                    if (current == l && indexRight != -1 && indexRight < i)
                    {
                        return false;
                    }

                    if (current == r && indexLeft != -1 && Array.IndexOf(page, l) > i)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    private class PageComparer((int left, int right)[] rules) : IComparer<int>
    {
        private readonly HashSet<(int left, int right)> _rules = [..rules];

        public int Compare(int x, int y)
        {
            if (_rules.Contains((x, y)))
            {
                return -1;
            }

            return _rules.Contains((y, x)) ? 1 : 0;
        }
    }
}