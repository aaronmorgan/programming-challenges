using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day17
{
    [Theory]
    [InlineData("Day17DevelopmentTesting1.txt", "4,6,3,5,6,3,5,2,1,0")]
    [InlineData("Day17.txt", "3,5,0,1,5,1,5,1,0")]
    public void Day14_Part1_Chronospatial_Computer(string filename, string expectedAnswer)
    {
        var input = InputParser.ReadAllLines("2024/" + filename).ToArray();
        var (_, output) = RunProgramInput(Part.One, new Machine(input));

        Assert.Equal(expectedAnswer, string.Join(',', output));
    }

    [Theory]
    [InlineData("Day17DevelopmentTesting2.txt", 117_440)]
    //[InlineData("Day17.txt", 0)]
    public void Day14_Part2_Chronospatial_Computer(string filename, long expectedAnswer)
    {
        string[] input = InputParser.ReadAllLines("2024/" + filename).ToArray();
        bool result;

        long i = expectedAnswer;
        var machine = new Machine(input, expectedAnswer);

        while (true)
        {
            var (program, output) = RunProgramInput(Part.Two, machine.SetRegisterA(i));

            var b = string.Join(',', program);
            var a = string.Join(',', output);

            if (i % 100_000_000 == 0) Console.WriteLine($"{i} | {a}");

            if (!b.Equals(a))
            {
                i++;
                continue;
            }

            result = true;
            break;
        }

        Assert.True(result);
    }

    private static (int[] program, IEnumerable<long> output) RunProgramInput(Part part, Machine input)
    {
        List<long> output = [];

        for (var i = 0; i < input.Program.Length; i += 2)
        {
            var opcode = input.Program[i];
            var operand = input.Program[i + 1];

            switch (opcode)
            {
                case 0: // 'adv' performs division
                {
                    input.RegisterA = (int)(input.RegisterA / Math.Pow(2, GetComboOperandValue(operand)));
                    break;
                }
                case 1: // 'bxl' bitwise XOR
                {
                    input.RegisterB ^= operand;
                    break;
                }
                case 2: // 'bst' modulo 8
                {
                    input.RegisterB = GetComboOperandValue(operand) % 8;
                    break;
                }
                case 3: // 'jnz' jump to pointer.
                {
                    // Subtract 2 as we do NOT apply the normal 2 step movement in this case.
                    if (input.RegisterA != 0) i = operand - 2;

                    break;
                }
                case 4: // 'bxc' bitwise XOR
                {
                    input.RegisterB ^= input.RegisterC;
                    break;
                }
                case 5: // 'out' 
                {
                    output.Add(GetComboOperandValue(operand) % 8);
                    break;
                }
                case 6: // 'bdv' 
                {
                    input.RegisterB = (int)(input.RegisterA / Math.Pow(2, GetComboOperandValue(operand)));
                    break;
                }
                case 7: // 'cdv'
                {
                    input.RegisterC = (int)(input.RegisterA / Math.Pow(2, GetComboOperandValue(operand)));
                    break;
                }
                default: throw new Exception("Invalid operand: " + opcode);
            }

            // Short circuit and bail early if we're not on the right track.
            if (part == Part.Two)
            {
                var isValid = true;
                for (var j = 0; j < output.Count; j++)
                {
                    if (output[j] != input.Program[j])
                    {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid) return (input.Program, []);
            }
        }

        return (input.Program, output);

        long GetComboOperandValue(int operand)
        {
            return operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => input.RegisterA,
                5 => input.RegisterB,
                6 => input.RegisterC,
                _ => throw new Exception("Invalid operand: " + operand)
            };
        }
    }

    private class Machine
    {
        public long RegisterA { get; set; }
        public long RegisterB { get; set; }
        public long RegisterC { get; set; }
        public int[] Program { get; }

        public Machine(string[] input, long registerADefaultValue = -1)
        {
            RegisterA = registerADefaultValue > 0 ? registerADefaultValue : int.Parse(input[0].Split(":")[^1].Trim());
            RegisterB = int.Parse(input[1].Split(":")[^1].Trim());
            RegisterC = int.Parse(input[2].Split(":")[^1].Trim());

            Program = input[4].Split(" ")[^1].Split(",").Select(int.Parse).ToArray();
        }

        public Machine SetRegisterA(long value)
        {
            RegisterA = value;
            return this;
        }
    }
}