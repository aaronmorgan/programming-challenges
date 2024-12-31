using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day7
{
    [Theory]
    [InlineData("Day7DevelopmentTesting1.txt", 3749, Part.One)]
    [InlineData("Day7DevelopmentTesting1.txt", 11387, Part.Two)]
    [InlineData("Day7.txt", 2654749936343, Part.One)]
    [InlineData("Day7.txt", 124060392153684, Part.Two)]
    public void Day7_Part1_Bridge_Repair(string filename, long expectedAnswer, Part part)
    {
        List<Equation> equations = [];
        Dictionary<int, List<char[]>> operandCombinations = new Dictionary<int, List<char[]>>();

        foreach (string instruction in InputParser.ReadAllLines("2024/" + filename))
        {
            string[] tmp = instruction.Split(":");

            equations.Add(new Equation
            {
                Answer = long.Parse(tmp[0]),
                Operands = tmp[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse).ToArray()
            });
        }

        long totalCalibrationResult = 0;

        foreach (Equation equation in equations)
        {
            if (!operandCombinations.ContainsKey(equation.Operands.Length))
            {
                if (part == Part.One)
                {
                    Part1CalculateOperandCombinations(equation.Operands.Length);
                }
                else
                {
                    Part2CalculateOperandCombinations(equation.Operands.Length);
                }
            }

            var combinations = operandCombinations[equation.Operands.Length];

            foreach (var combination in combinations)
            {
                long sum = equation.Operands[0];
                int index = 0;

                while (true)
                {
                    if (index >= equation.Operands.Length - 1) break;

                    sum = DoCalculation(sum, equation.Operands[index + 1], combination[index]);

                    index++;
                }

                if (sum == equation.Answer)
                {
                    totalCalibrationResult += sum;
                    break;
                }
            }
        }

        Assert.Equal(expectedAnswer, totalCalibrationResult);

        return;

        // Creates operator combinations using binary, so we can represent two states and simply increment
        // an integer counter to get the next combination.
        void Part1CalculateOperandCombinations(int operands)
        {
            // 2 operations (n) possible per slot (s) is n to the power of s, e.g. 2 to the power of 3 = 8.
            int maxOperatorCombinationCount = (int)Math.Pow(2, operands - 1);

            List<char[]> combinations = [];

            for (int i = 0; i < maxOperatorCombinationCount; i++)
            {
                // Create a base 2 representation of the operator combination using an incrementing integer as the seed.
                string binary = Convert.ToString(i, 2).PadLeft(operands - 1, '0'); // Creates '010' e.g.

                char[] binaryArray = binary.ToCharArray();

                combinations.Add(binaryArray);
            }

            // Cache the combination under the key of number of operands.
            operandCombinations[operands] = combinations;
        }

        // Creates operator combinations using binary, so we can represent two states and simply increment
        // an integer counter to get the next combination.
        void Part2CalculateOperandCombinations(int operands)
        {
            // 3 operations (n) possible per slot (s) is n to the power of s, e.g. 2 to the power of 3 = 8.
            int maxOperatorCombinationCount = (int)Math.Pow(3, operands - 1);

            List<char[]> combinations = [];

            for (int i = 0; i < maxOperatorCombinationCount; i++)
            {
                // Create a base 3 (ternary) representation of the operator combination using an incrementing integer
                // as the seed.
                string ternary = ToBase3(i).PadLeft(operands - 1, '0'); // Creates '020' e.g.

                char[] binaryArray = ternary.ToCharArray();

                combinations.Add(binaryArray);
            }

            // Cache the combination under the key of number of operands.
            operandCombinations[operands] = combinations;
        }

        // Perform the operation on our two operands using the bit 0 or 1 as the key for + or *.
        static long DoCalculation(long o1, long o2, char operatorBit)
        {
            return operatorBit switch
            {
                '0' => o1 + o2,
                '1' => o1 * o2,
                '2' => long.Parse(o1.ToString() + o2),    
                _ => throw new ArgumentOutOfRangeException(nameof(operatorBit))
            };
        }
        
        // Converts in integer to base 3 (ternary) returning the value as a string.
        string ToBase3(int number)
        {
            if (number == 0) return "0";
    
            var digits = new List<int>();
            int n = Math.Abs(number);
    
            while (n > 0)
            {
                digits.Add(n % 3);
                n /= 3;
            }
    
            digits.Reverse();
            return (number < 0 ? "-" : "") + string.Join("", digits);
        }
    }

    private class Equation
    {
        public required long Answer { get; init; }
        public required long[] Operands { get; init; }
    }
}