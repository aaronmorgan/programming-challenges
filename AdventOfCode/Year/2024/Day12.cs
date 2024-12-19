using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day12
{
    [Theory]
    [InlineData("Day12DevelopmentTesting1.txt", 140)]
    //[InlineData("Day12DevelopmentTesting2.txt", 772)]
    //[InlineData("Day12DevelopmentTesting3.txt", 1930)]
    //[InlineData("Day12.txt", 25, 189547)]
    public void Day12_Garden_Groups(string filename, int expectedAnswer)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);
        int result = 0;

        // Enlarge array and add a border of 1 along each side.
        char[,] largeInput = new char[input.GetLength(0) + 2, input.GetLength(1) + 2];

        // Map the input array into the larger one.
        for (int row = 0; row < input.GetLength(0); row++)
        {
            for (int column = 0; column < input.GetLength(1); column++)
            {
                largeInput[row + 1, column + 1] = input[row, column];
            }
        }

        // Store the map for each garden plot with a Guid because we might have multiple disconnected plots for type X and they need to be treated separately.
        Dictionary<Guid, List<(char type, int row, int column)>> gardenMap = [];


        HashSet<(int row, int column)> seen = new HashSet<(int row, int column)>();

        var z = input.GetLength(0) * input.GetLength(1);
        while (seen.Count < input.GetLength(0) * input.GetLength(1))
        {
            // Walk the garden logging each plot of every CONNECTED planter we find.
            HashSet<(int row, int column)> connectedPlot = [];
            char letter = '\0';

            
            for (var row = 0; row < largeInput.GetLength(0); row++)
            {
                for (var col = 0; col < largeInput.GetLength(1); col++)
                {
                    if (seen.Contains((row, col))) continue;

                    if (largeInput[row, col] != '\0' && largeInput[row, col] == letter)

                    // Exclude our border cells.
                    if (letter == '\0')
                    {
                     //   seen.Add((row, col));
                        continue;
                    }
                    
                    connectedPlot.Add((row, col));
                    seen.Add((row, col));

                    // Look all around for connected plots of the same type.
                    foreach ((int dr, int dc) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
                    {
                        (int y, int x) pos = (row + dr, col + dc);

                        // We want to add all the connecting plots of the same type.
                        if (largeInput[pos.y, pos.x] == largeInput[row, col])
                        {
                            connectedPlot.Add(pos);
                            seen.Add(pos);
                        }
                    }


                    //
                    // _ = gardenMap.TryGetValue(letter, out List<(int row, int column)> plot);
                    //
                    // if (plot == null)
                    // {
                    //     plot = [];
                    //     gardenMap.Add(letter, plot);
                    // }
                    //
                    // plot.Add((row, col));
                }
            }
            
            if (letter == '\0') continue;
            
            List<(char type, int row, int column)> garden = [];
            foreach ((int dr, int dc) in connectedPlot)
                garden.Add((letter, dr, dc));
            
            gardenMap.Add(Guid.NewGuid(), garden);
        }

        // Create a ledger to track the number of tiles with plot Key, and the number of sides of each of those plots.
        Dictionary<Guid, (char type, int internalPlots, int internalEdges)> counts =
            gardenMap.ToDictionary(
                pair => pair.Key,
                pair => (pair.Value[0].type, 0, 0));

        Dictionary<char, (int edges, int area)> proxyResults = [];

        // foreach (KeyValuePair<char, List<(int row, int column)>> plot in gardenMap)
        // {
        //     (int edges, char surroundingPlotType) = CountEdges(plot);
        //
        //     // Store the calculated number of edges and area for this plot type.
        //     proxyResults[plot.Key] = (edges, plot.Value.Count);
        //
        //     if (surroundingPlotType != '\0')
        //     {
        //         // Add the number of plot B plots inside plot A.
        //         var d = counts[surroundingPlotType];
        //         d.internalPlots += plot.Value.Count;
        //         d.internalEdges += edges;
        //
        //         counts[surroundingPlotType] = d;
        //     }
        // }
        //
        // // Process the results, taking into consideration areas with internal plots of a different type.
        // foreach (var a in proxyResults)
        // {
        //     if (counts.TryGetValue(a.Key, out (int internalPlots, int internalEdges) plot))
        //     {
        //         var t = a.Value.area; // - plot.internalPlots; // Subtract all internal plots of a different type.
        //         var g = a.Value.edges; // + plot.internalEdges; // Add internal edges to the plot's existing edge count.
        //
        //         var c = t * g;
        //         result += c;
        //     }
        //     else
        //     {
        //         result += a.Value.edges * a.Value.area;
        //     }
        // }

        Assert.Equal(expectedAnswer, result);

        return;

        // Counts edges of the grid cell that do NOT have a garden plot of the same plant type.
        // Also returns the surrounding plot type if this garden is entirely surrounded by the same plot type.
        (int edges, char surroundingPlotType) CountEdges(KeyValuePair<char, List<(int row, int column)>> plot)
        {
            var edges = 0;
            List<char> surroundingPlotTypes = [];

            int areaCount = 0;

            foreach (var (row, column) in plot.Value)
            {
                foreach ((int dr, int dc) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
                {
                    // TODO: Determine if a plot is wholly within another by tracking all the surrounding plots.
                    // If they're of the same type we must be inside it. 

                    char adjacentPlot = largeInput[row + dr, column + dc];
                    if (adjacentPlot != plot.Key)
                    {
                        edges++;

                        if (!surroundingPlotTypes.Contains(adjacentPlot))
                        {
                            surroundingPlotTypes.Add(adjacentPlot);
                        }
                    }
                }
            }

            return surroundingPlotTypes.Count == 1
                ? (edges, surroundingPlotTypes[0])
                : (edges, '\0');
        }
    }
}