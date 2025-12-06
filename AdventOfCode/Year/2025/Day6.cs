using Common.Utilities;

namespace AdventOfCode.Year._2025;

public class Day6
{
    [Theory]
    [InlineData("Day6DevelopmentTesting1.txt", 4277556)]
    [InlineData("Day6.txt", 4878670269096)]
    public void Day6_Part1_Trash_Compactor(string filename, long expectedAnswer)
    {
        string[,] input = InputParser.ReadSpaceSeparatedFile($"2025/{filename}");
        long result = 0;
        
        // Iterate over each column and row processing each problem left to right through the
        // array of problems.
        for (var col = 0; col < input.GetLength(1); col++)
        {
            List<long> numbers = [];

            for (var row = 0; row < input.GetLength(0); row++)
            {
                if (input[row, col] is "+" or "*")
                {
                    result += CalculateAnswer(numbers, input[row, col]);
                    continue;
                }

                numbers.Add(long.Parse(input[row, col]));
            }
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Day6DevelopmentTesting1.txt", 3263827)]
    [InlineData("Day6.txt", 8674740488592)]
    public void Day6_Part2_Trash_Compactor(string filename, long expectedAnswer)
    {
        List<string> input = InputParser.ReadAllLines($"2025/{filename}").ToList();

        List<Problem> problems = [];

        // Iterate over the whole line and determine how 'wide' each set of numbers is by counting the
        // space between operators. Operators are all left aligned.
        var lastLine = input.Last();
        var width = 0;
        var op = lastLine[0]; // Get the first operator.
        var offset = 0;

        for (var index = 1; index < lastLine.Length; index++)
        {
            var c = lastLine[index];
            width++;

            if (c != ' ' || index == lastLine.Length - 1)
            {
                // If we're at the end of the line there's no 'next' operator to break on, mock the boundary.
                if (index == lastLine.Length - 1)
                {
                    width += 2;
                }

                var problem = new Problem { Operator = op, Numbers = [] };
                problems.Add(problem);

                for (var i = 0; i < input.Count - 1; i++)
                {
                    problem.Numbers.Add(new string(input[i].Substring(offset, width - 1).ToCharArray().Reverse().ToArray()));
                }

                // Offset is the overall offset from the left so we can correctly read the next block of characters when used
                // with the 'width'.
                offset += width;

                // On with the next set of numbers.
                width = 0;
                op = c; // Get the next operator.
            }
        }

        long result = 0;

        foreach (var n in problems)
        {
            var numbers = PositionNumbersForPart2(n.Numbers);
            result += CalculateAnswer(numbers, n.Operator.ToString());
        }

        Assert.Equal(expectedAnswer, result);

        return;

        // We need to flip the numbers, creating a new array of numbers from reading the input set vertically.
        // They're already reversed, in the earlier routine.
        List<long> PositionNumbersForPart2(List<string> numberStrings)
        {
            string[] tmpNumberStrings = new string[numberStrings.Count];

            for (var index = 0; index < numberStrings.Count; index++)
            {
                var line = numberStrings[index];
                
                for (var i = 0; i < line.Length; i++)
                {
                    var c = line[i];
                    tmpNumberStrings[i] += c.ToString();
                }
            }
            
            List<long> final = [];

            // We now have the numbers left aligned and reorder, convert for processing.
            foreach (string s in tmpNumberStrings)
            {
                // Some of the test data is funny, guard against nulls.
                if (string.IsNullOrWhiteSpace(s))
                {
                    continue;
                }
                
                final.Add(long.Parse(s.TrimEnd()));
            }

            return final;
        }
    }
    
    /// <summary>
    /// Returns the sum for the given numbers and operator, e.g. + or *.
    /// </summary>
    private static long CalculateAnswer(List<long> numbers, string opCode)
    {
        switch (opCode)
        {
            case "+":
            {
                return numbers.Sum();
            }
            case "*":
            {
                long answer = numbers[0];

                for (var index = 1; index < numbers.Count; index++)
                {
                    var number = numbers[index];
                    answer *= number;
                }

                return answer;
            }
        }

        throw new InvalidOperationException($"Invalid operator '{opCode}'");
    }

    private class Problem
    {
        public required List<string> Numbers;
        public required char Operator;
    }
}
