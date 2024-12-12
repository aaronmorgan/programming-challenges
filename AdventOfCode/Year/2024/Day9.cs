using System.Text;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day9
{
    [Theory]
    [InlineData("Day9DevelopmentTesting1.txt", 1928, 2858)]
    [InlineData("Day9.txt", 6299243228569, 6326952672104)]
    public void Day9_Guard_Gallivant(string filename, long part1ExpectedAnswer, long part2ExpectedAnswer)
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

        string reversedBlocks = new string(new StringBuilder().AppendJoin("", blocks).ToString().Reverse().ToArray());

        string part2InputStr = new StringBuilder().AppendJoin("", blocks).ToString();

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
        long part1Result = CalculateChecksum(defraggedMemoryString);

        var part2DefraggedMemoryString = Part2(reversedBlocks.ToCharArray(), part2InputStr);
        long part2Result = CalculateChecksum(part2DefraggedMemoryString);

        Assert.Equal(part1ExpectedAnswer, part1Result);
        Assert.Equal(part2ExpectedAnswer, part2Result);

        return;

        bool IsOdd(int value) => value % 2 != 0;

        long CalculateChecksum(string inputStr)
        {
            long checksum = 0;
            char[] chars = inputStr.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '.') continue;
                checksum += (chars[i] - '0') * i;
            }

            return checksum;
        }

        string Part2(char[] reversedInputStr, string result)
        {
            var sb2 = new StringBuilder();

            char currentChar = reversedInputStr[0];
            int startIndex = 0;

            for (var index = startIndex; index < reversedInputStr.Length; index++)
            {
                var s = reversedInputStr[index];

                if (s == '.') continue;

                if (s == currentChar)
                {
                    sb2.Append(s);
                    continue;
                }

                int length = sb2.Length;

                var match = result.IndexOf(new string('.', length), 0, StringComparison.Ordinal);

                // We found room to insert this block of numbers.
                if (match > -1)
                {
                    result = result.Remove(match, length).Insert(match, sb2.ToString());

                    var tmp1 = result.ToCharArray();
                    Array.Reverse(tmp1);

                    var tmp2 = new string(tmp1);
                    var newIndex = tmp2.IndexOf(sb2.ToString(), StringComparison.Ordinal);

                    result = tmp2.Remove(newIndex, length).Insert(newIndex, new string('.', length));

                    tmp1 = result.ToCharArray();
                    Array.Reverse(tmp1);

                    result = new string(tmp1);
                }

                currentChar = s;

                sb2.Clear();
                sb2.Append(s);
            }

            return result;
        }
    }
}