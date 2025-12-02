using Common.Utilities;

namespace Everybody.Codes._2024;

public class Quest1
{
    [Theory]
    [InlineData("Quest1_Part1_Test1.txt", 5)]
    [InlineData("Quest1_Part1.txt", 1328)]
    public void Day1_Part1_TheBattleForTheFarmlands(string filename, int expectedAnswer)
    {
        char[] input = InputParser.ReadAllText("2024/" + filename).ToCharArray();
        int result = 0;

        foreach (char c in input)
        {
            switch (c)
            {
                case 'A': break;
                case 'B':
                    result += 1;
                    break;
                case 'C':
                    result += 3;
                    break;
            }
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Quest1_Part2_Test1.txt", 2, 28)]
    [InlineData("Quest1_Part2.txt", 2, 5615)]
    [InlineData("Quest1_Part3_Test1.txt", 3, 30)]
    [InlineData("Quest1_Part3.txt", 3, 28032)]
    public void Day1_Part2_Part3_TheBattleForTheFarmlands(string filename, int size, int expectedAnswer)
    {
        var attackWaves = InputParser.ReadAllText("2024/" + filename).Chunk(size).ToList();

        int result = 0;

        var damageTable = new Dictionary<char, int>
        {
            { 'A', 0 },
            { 'B', 1 },
            { 'C', 3 },
            { 'D', 5 },
            { 'x', 0 }
        };

        foreach (var monsters in attackWaves)
        {
            // Apply base damage with no modifier.
            result += damageTable[monsters[0]];
            result += damageTable[monsters[1]];

            if (size == 3) result += damageTable[monsters[2]];

            // Identifier the number of 'x' in the wave, if there's only one monster per wave no modifier is applied.
            var countEmptySpaces = 0;
            for (var i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] == 'x') countEmptySpaces++;
            }

            var applyModififer = size switch
            {
                2 => countEmptySpaces == 0,
                3 => countEmptySpaces <= 1,
                _ => false
            };

            if (!applyModififer) continue;
            
            if (size == 2) result += 2;
            else
            {
                switch (countEmptySpaces)
                {
                    case 0:
                        result += 6;
                        break;
                    case 1:
                        result += 2;
                        break;
                }
            }
        }

        Assert.Equal(expectedAnswer, result);
    }
}