// using AdventOfCode.Utilities;
//
// namespace AdventOfCode._2023;
//
// public class Day13
// {
//     [Theory]
//     [InlineData("Day13DevelopmentTesting1.txt", 405, 0)]
//     [InlineData("Day13DevelopmentTesting1.txt", 400, 1)]
//     [InlineData("Day13.txt", 33356, 0)]
//     public void Day13_Part1_PointOfIncidence(string filename, int expectedAnswer, int allowedDifference)
//     {
//         var fileInput = FileLoader.ReadAllLines("2023/" + filename).ToList();
//
//         int rangeStartIndex = 0, overallResult = 0;
//         List<string> columns;
//         bool isRow;
//
//         for (var i = 0; i <= fileInput.Count; i++)
//         {
//             isRow = false;
//
//             // Check if we're either at the end of the file or we've encountered one of the
//             // blank line separators between grids in the input file.
//             if (i == fileInput.Count || fileInput[i] == string.Empty)
//             {
//                 // Look back over the input data and get the grid to process.
//                 var linesRange = fileInput.GetRange(rangeStartIndex, i - rangeStartIndex);
//                 ProcessPattern(linesRange);
//
//                 i++;
//                 rangeStartIndex = i;
//             }
//         }
//
//         Assert.Equal(expectedAnswer, overallResult);
//
//         return;
//
//         char[,] To2dArray(List<string> input)
//         {
//             char[,] result = new char[input.Count, input[0].Length];
//
//             for (var index = 0; index < input.Count; index++)
//             {
//                 var a = input[index];
//
//                 for (var i = 0; i < a.Length; i++)
//                 {
//                     var c = a[i];
//                     result[index, i] = c;
//                 }
//             }
//
//             return result;
//         }
//
//         void ProcessPattern(List<string> inputPattern)
//         {
//             // Look for mirror rows...
//             for (var i = 0; i < inputPattern.Count - 1; i++)
//             {
//                 char[,] z = To2dArray(inputPattern);
//                 
//                 var a = ExpandSearchOutwards(i, i + 1, z);
//                 
//                 // Transpose the array so we can always think of looking for lines of reflection on the horizontal
//                 // plane only.
//                 var c = TransposeArray(z);
//                 
//                 var d = ExpandSearchOutwards(i, i + 1, c);
//                 
//
//          //       var b = ExpandSearchOutwards(i, i + 1, TransposeArray(z));
//
//
//                 if (ExpandSearchOutwards(i, i + 1, inputPattern))
//                 {
//                     overallResult += (i + 1) * 100;
//                     isRow = true;
//                     break;
//                 }
//             }
//
//             // Look for mirror columns...
//             if (!isRow)
//             {
//                 columns = new(inputPattern.Count);
//
//                 for (var rowIndex = 0; rowIndex < inputPattern[0].Length; rowIndex++)
//                 {
//                     var tmpStr = string.Empty;
//
//                     foreach (var row in inputPattern)
//                     {
//                         tmpStr += row[rowIndex].ToString();
//                     }
//
//                     columns.Add(tmpStr);
//                 }
//
//                 for (var i = 0; i < columns.Count - 1; i++)
//                 {
//                     if (ExpandSearchOutwards(i, i + 1, columns))
//                     {
//                         overallResult += i + 1;
//                         break;
//                     }
//                 }
//             }
//         }
//
//         int FindReflectionLine()
//         {
//             return -1;
//         }
//
//         bool ExpandSearchOutwards(int x, int y, IReadOnlyList<string> data)
//         {
//             while (true)
//             {
//                 // Compare the two grid rows, if they differ we're not straddling a line of reflection.
//                 var stringsDifferent = AreStringsDifferent(data[x], data[y], allowedDifference);
//
//                 if (!stringsDifferent) return false;
//
//                 // We found a perfect series of reflections up to the pattern boundary.
//                 if (x == 0 || y == data.Count - 1) return true;
//
//                 x -= 1;
//                 y += 1;
//             }
//         }
//     }
//
//     private static bool AreStringsDifferent(string str1, string str2, int allowedDifference)
//     {
//         if (str1.Length != str2.Length) return false;
//
//         var differenceCount = 0;
//
//         for (var i = 0; i < str1.Length; i++)
//         {
//             if (str1[i] == str2[i]) continue;
//
//             differenceCount++;
//
//             if (differenceCount > allowedDifference) return false;
//         }
//
//         return differenceCount == allowedDifference;
//     }
//
//     /// <summary>
//     /// Returns a 2D array with axes transposed, i.e. x => y and y => x.
//     /// </summary>
//     private static T[,] TransposeArray<T>(T[,] array)
//     {
//         var rows = array.GetLength(0);
//         var columns = array.GetLength(1);
//         var transposedArray = new T[columns, rows];
//
//         for (var i = 0; i < rows; i++)
//         {
//             for (var j = 0; j < columns; j++)
//             {
//                 transposedArray[j, i] = array[i, j];
//             }
//         }
//
//         return transposedArray;
//     }
// }