using Common.Utilities;

namespace AdventOfCode.Year._2025;

public class Day3
{
    [Theory]
    [InlineData("Day3DevelopmentTesting1.txt", 2, 357)]
    [InlineData("Day3DevelopmentTesting1.txt", 12, 3121910778619)]
    [InlineData("Day3.txt", 2, 17613)]
    [InlineData("Day3.txt", 12, 175304218462560)]
    public void Day3_Part1_and_Part2_Lobby(string filename, int batteriesSize, long expectedAnswerPart1)
    {
        var lines = InputParser.ReadAllLines("2025/" + filename);
        
        long outputJoltage = 0;

        foreach (var line in lines)
        {
            var batteries = new long[batteriesSize];
            var batteryIndex = 0;

            for (var index = 0; index < line.Length; index++)
            {
                var tempBattery = long.Parse(char.ConvertFromUtf32(line[index]));
                
                if (batteryIndex > 0
                    && batteriesSize - (batteryIndex -1) <= line.Length - index // Check that theres more then enough batteries left to satisfy the number we need.
                    && tempBattery > batteries[batteryIndex - 1] && batteryIndex < batteriesSize && index < line.Length - 1)
                {
                    // Look back through the batteries we've stored, can this one replace the last one without
                    // exceeding the constraint that we must have enough batteries left to parse that will sum our stored
                    // batteries to the lenght of batteriesSize.
                    while (batteryIndex > 0 && tempBattery > batteries[batteryIndex - 1] && batteriesSize - (batteryIndex -1) <= line.Length - index)
                    {
                        batteryIndex--;
                    }

                    batteries[batteryIndex] = tempBattery;

                    if (batteryIndex < batteriesSize - 1)
                    {
                        batteries[batteryIndex + 1] = 0;
                        batteryIndex++;
                    }

                    continue;
                }

                // Is this battery higher in value but not the last one in the bank?
                if (tempBattery > batteries[batteryIndex] && batteryIndex < batteriesSize && index < line.Length - 1)
                {
                    batteries[batteryIndex] = tempBattery;

                    if (batteryIndex < batteriesSize - 1)
                    {
                        batteries[batteryIndex + 1] = 0;
                        batteryIndex++;
                    }

                    continue;
                }

                if (tempBattery > batteries[batteryIndex] && batteryIndex < batteriesSize)
                {
                    batteries[batteryIndex] = tempBattery;

                    // If we're not at the end battery increment the index and move along one.                     
                    if (batteryIndex < batteriesSize - 1)
                    {
                        batteryIndex++;
                    }
                }
            }

            string joltageStr = batteries.Aggregate("", (current, battery) => current + battery);

            outputJoltage += long.Parse(joltageStr);
        }

        Assert.Equal(expectedAnswerPart1, outputJoltage);
    }
}
