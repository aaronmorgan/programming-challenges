using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day7
{
    [Theory]
    [InlineData("Day7DevelopmentTesting1.txt", 3749)]
    [InlineData("Day7.txt", 2654749936343)]
    public void Day7_Part1_Bridge_Repair(string filename, long expectedAnswer)
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
                CalculateOperandCombinations(equation.Operands.Length);
            }

            var combinations = operandCombinations[equation.Operands.Length];

            for (var j = 0; j < combinations.Count; j++)
            {
                var combination = combinations[j];

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

        void CalculateOperandCombinations(int operands)
        {
            // 2 operations (n) possible per slot (s) is n to the power of s, e.g. 2 to the power of 3 = 8.
            int maxOperatorCombinationCount = (int)Math.Pow(2, operands - 1);

            List<char[]> combinations = [];


            for (int i = 0; i < maxOperatorCombinationCount; i++)
            {
                string binary = Convert.ToString(i, 2).PadLeft(operands - 1, '0'); // Creates '010' e.g.

                char[] binaryArray = binary.ToCharArray();

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
                _ => throw new ArgumentOutOfRangeException(nameof(operatorBit))
            };
        }
    }

    private class Equation
    {
        public required long Answer { get; set; }
        public required long[] Operands { get; set; }
    }
}