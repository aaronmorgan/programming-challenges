using System.Diagnostics;
using Common.Utilities;

namespace AdventOfCode.Year._2023;

public class Day7
{
    [Theory]
    [InlineData("Day7DevelopmentTesting1.txt", 6440)]
    [InlineData("Day7.txt", 247961593)]
    public void Day7_Part1_CamelCards(string filename, int expectedAnswer)
    {
        var result = 0;
        var fileInput = InputParser.ReadAllLines("2023/" + filename).ToArray();

        SortedDictionary<string, int> highCardHands = new();
        SortedDictionary<string, int> onePairHands = new();
        SortedDictionary<string, int> twoPairHands = new();
        SortedDictionary<string, int> fullHouseHands = new();
        SortedDictionary<string, int> threeOfAKindHands = new();
        SortedDictionary<string, int> fourOfAKindHands = new();
        SortedDictionary<string, int> fiveOfAKindHands = new();

        foreach (var game in fileInput)
        {
            var gameData = game.Split(' ');
            gameData[0] = RemapGameCards(gameData[0]);

            var groupedByCount = gameData[0]
                .GroupBy(c => c)
                .GroupBy(g => g.Count(), g => g.Key)
                .OrderByDescending(g => g.Key)
                .ToArray();

            switch (groupedByCount)
            {
                case [{ Key: 5 }]:
                    AddGameToDictionary(fiveOfAKindHands, gameData);
                    continue;
                case [{ Key: 4 }, _]:
                    AddGameToDictionary(fourOfAKindHands, gameData);
                    continue;
                case [{ Key: 3 }, { Key: 2 }]:
                    AddGameToDictionary(fullHouseHands, gameData);
                    continue;
                case [{ Key: 3 }, _]:
                    AddGameToDictionary(threeOfAKindHands, gameData);
                    continue;
                case [{ Key: 2 }, _] when groupedByCount[0].Count() == 2:
                    AddGameToDictionary(twoPairHands, gameData);
                    continue;
                case [{ Key: 2 }, { Key: 1 }]:
                    AddGameToDictionary(onePairHands, gameData);
                    continue;
            }

            if (groupedByCount[0].Key == 1 && groupedByCount[0].Count() == 5)
            {
                AddGameToDictionary(highCardHands, gameData);
            }
        }

        var i = 0;

        CalculateHandWinnings(highCardHands);
        CalculateHandWinnings(onePairHands);
        CalculateHandWinnings(twoPairHands);
        CalculateHandWinnings(threeOfAKindHands);
        CalculateHandWinnings(fullHouseHands);
        CalculateHandWinnings(fourOfAKindHands);
        CalculateHandWinnings(fiveOfAKindHands);

        Assert.Equal(expectedAnswer, result);

        return;

        // Remap the game cards into characters that naturally sort.
        // Map A=>E before doing T=A or we map A from T=>A=>E.
        string RemapGameCards(string hand) => hand.Replace('A', 'E').Replace('T', 'A').Replace('J', 'B').Replace('Q', 'C').Replace('K', 'D');

        void AddGameToDictionary(IDictionary<string, int> dictionary, IReadOnlyList<string> gameData)
        {
            dictionary.Add(gameData[0], int.Parse(gameData[1]));
        }

        void CalculateHandWinnings(SortedDictionary<string, int> dictionary)
        {
            foreach (var winningHand in dictionary)
            {
                i++;
                result += winningHand.Value * i;
            }
        }
    }

    [Theory]
    [InlineData("Day7DevelopmentTesting1.txt", 5905)]
    //[InlineData("Day7.txt", 248200273)] // 250255050 249008166 248200273 too high.  130821232
    public void Day7_Part2_CamelCards(string filename, int expectedAnswer)
    {
        var result = 0;
        var fileInput = InputParser.ReadAllLines("2023/" + filename).ToArray();

        List<string> allHands = [];

        SortedDictionary<string, int> highCardHands = new();
        SortedDictionary<string, int> onePairHands = new();
        SortedDictionary<string, int> twoPairHands = new();
        SortedDictionary<string, int> fullHouseHands = new();
        SortedDictionary<string, int> threeOfAKindHands = new();
        SortedDictionary<string, int> fourOfAKindHands = new();
        SortedDictionary<string, int> fiveOfAKindHands = new();

        foreach (var game in fileInput)
        {
            var gameData = game.Split(' ');
            gameData[0] = RemapGameCards(gameData[0]);

            var groupedByCount = gameData[0]
                .GroupBy(c => c)
                .GroupBy(g => g.Count(), g => g.Key)
                .OrderByDescending(g => g.Key)
                .ToArray();

            char[] handSorted = gameData[0].ToArray();
            Array.Sort(handSorted);
            Array.Reverse(handSorted);

            gameData[0] = UpgradeJokerCard(groupedByCount, new string(handSorted));

            if (fiveOfAKindHands.ContainsKey(gameData[0]))
            {
                Debugger.Break();
            }

            if (fourOfAKindHands.ContainsKey(gameData[0]))
            {
                Debugger.Break();
            }

            if (fullHouseHands.ContainsKey(gameData[0]))
            {
                Debugger.Break();
            }

            if (threeOfAKindHands.ContainsKey(gameData[0]))
            {
                Debugger.Break();
            }

            if (twoPairHands.ContainsKey(gameData[0]))
            {
                Debugger.Break();
            }

            if (onePairHands.ContainsKey(gameData[0]))
            {
                Debugger.Break();
            }

            if (highCardHands.ContainsKey(gameData[0]))
            {
                Debugger.Break();
            }

            // Need to reorder the cards after doing any Joker upgrades...
            groupedByCount = gameData[0]
                .GroupBy(c => c)
                .GroupBy(g => g.Count(), g => g.Key)
                .OrderByDescending(g => g.Key)
                .ToArray();

            if (groupedByCount is [{ Key: 5 }])
            {
                AddGameToDictionary(fiveOfAKindHands, gameData);
                continue;
            }

            if (groupedByCount is [{ Key: 4 }, _])
            {
                AddGameToDictionary(fourOfAKindHands, gameData);
                continue;
            }

            if (groupedByCount is [{ Key: 3 }, { Key: 2 }])
            {
                AddGameToDictionary(fullHouseHands, gameData);
                continue;
            }

            if (groupedByCount is [{ Key: 3 }, _])
            {
                AddGameToDictionary(threeOfAKindHands, gameData);
                continue;
            }

            if (groupedByCount is [{ Key: 2 }, _] && groupedByCount[0].Count() == 2)
            {
                AddGameToDictionary(twoPairHands, gameData);
                continue;
            }

            if (groupedByCount is [{ Key: 2 }, { Key: 1 }])
            {
                AddGameToDictionary(onePairHands, gameData);
                continue;
            }

            if (groupedByCount[0].Key == 1 && groupedByCount[0].Count() == 5)
            {
                AddGameToDictionary(highCardHands, gameData);
            }
        }

        var i = 0;

        CalculateHandWinnings(highCardHands);
        CalculateHandWinnings(onePairHands);
        CalculateHandWinnings(twoPairHands);
        CalculateHandWinnings(threeOfAKindHands);
        CalculateHandWinnings(fullHouseHands);
        CalculateHandWinnings(fourOfAKindHands);
        CalculateHandWinnings(fiveOfAKindHands);

        Assert.Equal(expectedAnswer, result);

        return;

        string UpgradeJokerCard(IGrouping<int, char>[] grouping, string str)
        {
            var a = str;
            try
            {
                // TODO I think the problem is here; as per the spec:
                // ...for the purpose of breaking ties between two hands of the same type, J is always treated as J, not the card it's pretending to be.

                if (!str.Contains('1')) return str + "1";

                char card = grouping[0].Take(1).ToArray()[0];

                if (card == 'E') return str.Replace('1', 'E') + "2";
                if (card == 'D') return str.Replace('1', 'D') + "2";
                if (card == 'C') return str.Replace('1', 'C') + "2";
                if (card == 'A') return str.Replace('1', 'A') + "2";

                if (card == '9') return str.Replace('1', '9') + "2";
                if (card == '8') return str.Replace('1', '8') + "2";
                if (card == '7') return str.Replace('1', '7') + "2";
                if (card == '6') return str.Replace('1', '6') + "2";
                if (card == '5') return str.Replace('1', '5') + "2";
                if (card == '4') return str.Replace('1', '4') + "2";
                if (card == '3') return str.Replace('1', '3') + "2";
                if (card == '2') return str.Replace('1', '2') + "2";


                return str;
            }
            finally
            {
                if (allHands.Contains(str))
                {
                Debugger.Break();
                }
                else
                {
                    allHands.Add(str);
                }
            }
        }

        // string UpgradeJokerCard(string str)
        // {
        // // wrong; should be checking the hand and upgrading it if necessary.
        //     if (str.Contains('E')) return str.Replace('J', 'E');
        //     if (str.Contains('D')) return str.Replace('J', 'D');
        //     if (str.Contains('C')) return str.Replace('J', 'C');
        //     if (str.Contains('A')) return str.Replace('J', 'A');
        //     
        //     if (str.Contains('9')) return str.Replace('J', '9');
        //     if (str.Contains('8')) return str.Replace('J', '8');
        //     if (str.Contains('7')) return str.Replace('J', '7');
        //     if (str.Contains('6')) return str.Replace('J', '6');
        //     if (str.Contains('5')) return str.Replace('J', '5');
        //     if (str.Contains('4')) return str.Replace('J', '4');
        //     if (str.Contains('3')) return str.Replace('J', '3');
        //     if (str.Contains('2')) return str.Replace('J', '2');
        //     
        //     return str;
        // }

        // Remap the game cards into characters that naturally sort.
        // Map A=>E before doing T=A or we map A from T=>A=>E.
        //string RemapGameCards(string hand) => hand.Replace('A', 'E').Replace('T', 'A').Replace('Q', 'C').Replace('K', 'D');
        string RemapGameCards(string hand) => hand.Replace('A', 'E').Replace('T', 'A').Replace('J', '1').Replace('Q', 'C').Replace('K', 'D');

        void AddGameToDictionary(IDictionary<string, int> dictionary, IReadOnlyList<string> gameData)
        {
            if (!dictionary.ContainsKey(gameData[0]))
            {
                dictionary.Add(gameData[0], int.Parse(gameData[1]));
            }
            else
            {
                Char x = 'A';
                Char y = (Char)(Convert.ToUInt16(gameData[0][4]) + 1);

                dictionary.Add(gameData[0][..4] + y, int.Parse(gameData[1]));
            }
        }

        void CalculateHandWinnings(SortedDictionary<string, int> dictionary)
        {
            foreach (var winningHand in dictionary)
            {
                i++;
                result += winningHand.Value * i;
            }
        }
    }
}
