using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day17
{
    // see Day23_Part1_ALongWalk
    [Theory]
    [InlineData("Day17DevelopmentTesting1.txt", "4,6,3,5,6,3,5,2,1,0")]
    [InlineData("Day17.txt", "3,5,0,1,5,1,5,1,0")]
    public void Day14_Chronospatial_Computer(string filename, string expectedAnswer)
    {
        string[] input = InputParser.ReadAllLines("2024/" + filename).ToArray();

        int registerA = int.Parse(input[0].Split(":")[^1].Trim());
        int registerB = int.Parse(input[1].Split(":")[^1].Trim());
        int registerC = int.Parse(input[2].Split(":")[^1].Trim());

        int[] program = input[4].Split(" ")[^1].Split(",").Select(int.Parse).ToArray();

        List<int> output = [];

        for (int i = 0; i < program.Length; i += 2)
        {
            var opcode = program[i];
            var operand = program[i + 1];

            switch (opcode)
            {
                case 0: // 'adv' performs division
                {
                    var b = GetComboOperandValue(operand);
                    var a = (int)(registerA / Math.Pow(2, b));
                    registerA = a;
                    break;
                }
                case 1: // 'bxl' bitwise XOR
                {
                    var b = registerB ^ operand;
                    registerB = b;
                    break;
                }
                case 2: // 'bst' modulo 8
                {
                    var a = GetComboOperandValue(operand);
                    var b = a % 8;
                    registerB = b;
                    break;
                }
                case 3: // 'jnz' jump to pointer.
                {
                    if (registerA != 0)
                    {
                        // Subtract 2 as we do NOT apply the normal 2 step movement in this case.
                        i = operand - 2;
                    }

                    break;
                }
                case 4: // 'bxc' bitwise XOR
                {
                    var a = registerB ^ registerC;
                    registerB = a;
                    break;
                }
                case 5: // 'out' 
                {
                    var a = GetComboOperandValue(operand);
                    var b = a % 8;
                    output.Add(b);

                    break;
                }
                case 6: // 'bdv' 
                {
                    var b = GetComboOperandValue(operand);
                    var a = (int)(registerA / Math.Pow(2, b));
                    registerB = a;
                    break;
                }
                case 7: // 'cdv'
                {
                    var b = GetComboOperandValue(operand);
                    var a = (int)(registerA / Math.Pow(2, b));
                    registerC = a;
                    break;
                }
                default: throw new Exception("Invalid operand: " + opcode);
            }
        }

        string result = string.Join(',', output);

        Assert.Equal(expectedAnswer, result);

        return;

        int GetComboOperandValue(int operand)
        {
            return operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => registerA,
                5 => registerB,
                6 => registerC,
                _ => throw new Exception("Invalid operand: " + operand)
            };
        }
    }
}