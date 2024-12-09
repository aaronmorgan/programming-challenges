using System.Text;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day9
{
    [Theory]
    [InlineData("Day9DevelopmentTesting1.txt", 1928)]
    [InlineData("Day9.txt", 6299243228569)]
    public void Day9_Guard_Gallivant(string filename, long expectedAnswer)
    {
        char[] input = InputParser.ReadAllText("2024/" + filename).ToCharArray();
        List<string> blocks = [];

        int blockId = 0;

        for (var index = 0; index < input.Length; index++)
        {
            var c = input[index];
            var sectorLength = int.Parse(c.ToString());
            var isFreeSpace = IsOdd(index);

            if (sectorLength != 0)
            {
                blocks.Add(new string(isFreeSpace ? '.' : (char)(blockId + '0'), sectorLength));
            }

            if (!isFreeSpace) blockId++;
        }

        //.string reversedBlocks =new string(new StringBuilder().AppendJoin("", blocks).ToString().Replace(".", string.Empty).Reverse().ToArray());
        string reversedBlocks = new string(new StringBuilder().AppendJoin("", blocks).ToString().Reverse().ToArray());

        var sb = new StringBuilder();
        var offsetIndex = 0;

        foreach (var s in blocks)
        {
            if (s.StartsWith('.'))
            {
                var tmpSb = new StringBuilder();
                var i = offsetIndex;

                while (true)
                {
                    if (sb.Length + offsetIndex >= reversedBlocks.Length) break;

                    string nextChar = reversedBlocks[i..(i + 1)];

                    if (nextChar != ".")
                    {
                        tmpSb.Append(nextChar);
                        i++;
                    }
                    else
                    {
                        offsetIndex++;
                        i++;
                        continue;
                    }

                    if (tmpSb.Length == s.Length)
                    {
                        sb.Append(tmpSb);
                        offsetIndex += s.Length;
                        break;
                    }
                }
            }
            else
            {
                sb.Append(s);

                if (sb.Length + offsetIndex >= reversedBlocks.Length)
                {
                    break;
                }
            }
        }

        var defraggedMemoryString = sb.ToString()[..(reversedBlocks.Length - offsetIndex)];

        long result = CalculateChecksum(defraggedMemoryString);

        Assert.Equal(expectedAnswer, result);

        return;

        bool IsOdd(int value) => value % 2 != 0;

        long CalculateChecksum(string inputStr)
        {
            long checksum = 0;
            char[] chars = inputStr.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                checksum += (chars[i] - '0') * i;
            }

            return checksum;
        }
    }
}