using System.Diagnostics;
using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day13
{
    [Theory]
    [InlineData("Day13DevelopmentTesting1.txt", 1)]
    [InlineData("Day13.txt", 1651)]
    public void Day10_Claw_Contraption(string filename, int expectedAnswer)
    {
        Dictionary<int, Machine> machines = [];

        string[] input = InputParser.ReadAllLines("2024/" + filename).ToArray();

        for (int i = 0; i < input.Length; i++)
        {
            var machine = new Machine
            {
                Id = i,
                ButtonA = GetXYModifiers(input[i]),
                ButtonB = GetXYModifiers(input[i + 1]),
                Prize = GetXYModifiers(input[i + 2])
            };

            machines.Add(i, machine);

            i += 3;
        }

        var m = machines[0];
        int x = 8400;

        int aCount = 0, bCount = 0;

        List<(int aCcount, int bCount)> options = [];

        while (true)
        {
            x -= m.ButtonA.x;
            aCount++;
            x -= m.ButtonB.x;
            bCount++;

            if (x % (m.ButtonA.x) == 0)
            {
                options.Add((x / m.ButtonA.x, x));
            }

            if (x % (m.ButtonB.x) == 0)
            {
                options.Add((x, x / m.ButtonB.x));
            }
            
            if (x < 0) break;
        }

        int result = 0;

        Assert.Equal(expectedAnswer, result);

        return;

        (int x, int y) GetXYModifiers(string s)
        {
            string[] bits = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string sd = bits[^2].Trim(',');

            sd = sd[2..];
            int x = int.Parse(sd);

            int y = int.Parse(bits[^1][2..]);

            return (x, y);
        }
    }

    private class Machine
    {
        public int Id { get; set; }
        public (int x, int y) ButtonA { get; set; }
        public (int x, int y) ButtonB { get; set; }
        public (int x, int y) Prize { get; set; }
    }
}