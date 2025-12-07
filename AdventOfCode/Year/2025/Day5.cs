using Common.Utilities;

namespace AdventOfCode.Year._2025;

public class Day5
{
    [Theory]
    [InlineData("Day5DevelopmentTesting1.txt", 3)]
    [InlineData("Day5.txt", 623)]
    public void Day5_Part1_Cafeteria(string filename, long expectedAnswer)
    {
        var input = InputParser.ReadAllLines("2025/" + filename).ToList();

        List<(long min, long max)> ingredients = [];
        List<long> ingredientIds = [];

        for (var index = 0; index < input.Count; index++)
        {
            var line = input[index];

            if (!string.IsNullOrEmpty(line))
            {
                var parts = line.Split('-');

                ingredients.Add((long.Parse(parts[0]), long.Parse(parts[1])));

                continue;
            }

            // We've hit the line break, the rest of the input are the ingredient ids.
            CreateIngredientIds(input.GetRange(index + 1, input.Count - index - 1));
            
            break;
        }

        HashSet<long> results = [];

        foreach (long id in ingredientIds)
        {
            foreach (var ingredient in ingredients)
            {
                if (id >= ingredient.min && id <= ingredient.max)
                {
                    results.Add(id);
                }
            }
        }

        Assert.Equal(expectedAnswer, results.Count);

        return;

        void CreateIngredientIds(List<string> lines)
        {
            ingredientIds.AddRange(lines.Select(long.Parse));
        }
    }

    [Theory]
    [InlineData("Day5DevelopmentTesting1.txt", 14)]
    [InlineData("Day5.txt", 353507173555373)]
    public void Day5_Part2_Cafeteria(string filename, long expectedAnswer)
    {
        var inputLines = InputParser.ReadAllLines("2025/" + filename);

        List<Ingredient> ingredients = [];

        foreach (var line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            var parts = line.Split('-');

            ingredients.Add(new Ingredient { Min = long.Parse(parts[0]), Max = long.Parse(parts[1]) });
        }

        ingredients = ingredients.OrderBy(x => x.Min).ToList();

        long ingredientIndex = -1;
        long count = 0;

        // With the sorted ingredients list we work 'left' from the lowest value and hope
        // there are no gaps in the ranges....
        foreach (var ingredient in ingredients)
        {
            if (ingredientIndex >= ingredient.Min)
            {
                ingredient.Min = ingredientIndex + 1;
            }

            if (ingredient.Min <= ingredient.Max)
            {
                // Increment our counter by the diff between the current max and our new? minimum value.
                count += ingredient.Max - ingredient.Min + 1;
            }

            // Reassign our index if it's the current ingredient range has a higher max value.
            ingredientIndex = Math.Max(ingredientIndex, ingredient.Max);
        }

        Assert.Equal(expectedAnswer, count);
    }

    private class Ingredient
    {
        public long Min;
        public long Max;
    }
}
