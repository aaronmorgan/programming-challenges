using Common.Extensions;
using Common.Utilities;

namespace Everybody.Codes._2024;

public class Quest2
{
    [Theory]
    [InlineData("AWAKEN THE POWER ADORNED WITH THE FLAMES BRIGHT IRE", true, 4)]
    [InlineData("THE FLAME SHIELDED THE HEART OF THE KINGS", true, 3)]
    [InlineData("POWE PO WER P OWE R", true, 2)]
    [InlineData("THERE IS THE END", true, 3)]
    [InlineData("Quest2_Part1.txt", false, 28)]
    public void Day2_Part1_TheRunesOfPower(string inputText, bool isTest, int expectedAnswer)
    {
        string[] words = isTest
            ? "THE,OWE,MES,ROD,HER".Split(',')
            : "LOR,LL,SI,OR,ST,CA,MO".Split(',');

        if (inputText.EndsWith(".txt"))
        {
            inputText = InputParser.ReadAllText("2024/" + inputText);
        }

        int result = 0;

        foreach (var word in words)
        {
            int wordLength = word.Length;

            for (var index = 0; index < inputText.Length; index++)
            {
                if (index + wordLength > inputText.Length) break;

                string s = inputText[index..(index + wordLength)];

                if (s.Equals(word))
                {
                    result++;
                }
            }
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Quest2_Part2_Test1.txt", true, 42)]
    [InlineData("Quest2_Part2.txt", false, 5253)]
    public void Day2_Part2_TheRunesOfPower(string filename, bool isTest, int expectedAnswer)
    {
        string[] words = isTest
            ? "THE,OWE,MES,ROD,HER,QAQ".Split(',')
            : "UCES,AFDFHKTQJF,BUYF,VI,WOW,BH,QMT,JIDP,BFSVDUBETM,BFDB,L,Q,HVUNZFIUOD,CA,ANT,MNDB,VIZ,JHKY,OW,GV,ZA,OCA,RKXSHCKKKA,FMM,LR,RARX,QSEC,YSGV,XME,JB,LF,IUFSVYIHXR,EGI,X,OO,AD,VLXCLMJOVV,ZMYCWLTTMI,N,EMMVQOZLSJ,HUYA,KJ,UZB,TAU,GSZGUIPQXZ,C,MEXNIFSGNB,QCA"
                .Split(',');

        string[] inputText = InputParser.ReadAllLines("2024/" + filename).ToArray();
        int result = 0;

        foreach (var line in inputText)
        {
            HashSet<int> runicSymbols = [];

            foreach (var word in words)
            {
                int wordLength = word.Length;

                // Read left to right...
                for (var index = 0; index < line.Length; index++)
                {
                    if (index + wordLength > line.Length) break;

                    string s = line[index..(index + wordLength)];

                    if (s.Equals(word))
                    {
                        for (int i = 0; i < word.Length; i++)
                        {
                            runicSymbols.Add(index + i);
                        }
                    }
                }

                // ...and then read right to left using the reversed version of the runic word.
                string wordReversed = new string(word.ToCharArray().Reverse().ToArray());

                for (var index = line.Length; index >= 0; index--)
                {
                    if (index - wordLength < 0) break;

                    string s = line[(index - wordLength)..index];

                    if (s.Equals(wordReversed))
                    {
                        for (int i = wordLength; i > 0; i--)
                        {
                            runicSymbols.Add(index - i);
                        }
                    }
                }
            }

            result += runicSymbols.Count;
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Quest2_Part3_Test1.txt", true, 10)]
    //[InlineData("Quest2_Part2.txt", false, 5253)]
    public void Day2_Part3_TheRunesOfPower(string filename, bool isTest, int expectedAnswer)
    {
        string[] words = isTest
            ? "THE,OWE,MES,ROD,RODEO".Split(',')
            : "TFFAABUCQE,FIJN,NU,DCFNMOWYTJ,BVMS,BLWD,UPNXZONAKZ,LO,YJZ,HEUF,MPF,URKMNNSTKS,BOW,UBPIWNIDKC,FI,D,OUPYZTPHHH,IEPAIWDDKE,SZH,XBPQBWLEBO,NJANMMKYIH,MTN,BI,HD,GJYX,LF,CA,TS,A,NPAV,AZQDSWMKPB,QRIOGITAIQ,BU,JE,MHQ,CM,BPHZ,KSV,IU,QIE,RVJT,E,ENHNPHEKIV,EGQDKVAFUV,OO,GB,ATK,M,BZTO,ELZCCIDLRZ,YX,IR,G,HI,XDJI,MQS,K,MDYDRCNKUC,XDJ,GKSA,OY,FB,LOL,C,QMH,DCS,UAJ,NE,B,RAQ,KYI,NYP,JSKKEUQXMG,NOHSXFTMSN,DMMNUNAWWC,DH,SSGF,TEG,EOQT,QGWPKULGSO,CWKAOBDJZF,YRZP,J,WM,U,FZKD,XAJK,AJDE,LMW,BIPK,KCT,TWPE,FXNF"
                .Split(',');

        char[,] array = InputParser.ReadAllLines("2024/" + filename)
            .ToList()
            .ToCharArray();

        // The approach is to replicate columns 1 and 2 at the right side of each row (allowing for the variable length
        // of each word being tested). This fakes us having to 'look' around the wrapped armour.
        // Similarily we replicate rows 1 and 2 at the bottom. 

        int result = 0;

        Doit2(array, true);
        var b = array.RotateArray();

        Doit2(b, false);

        // for (var row = 0; row < b.GetLength(0); row++)
        // {
        //     string s = string.Empty;
        //
        //     for (var col = 0; col < b.GetLength(1); col++)
        //     {
        //         s += b[row, col];
        //     }
        //
        //     DoIt(s);
        // }

        Assert.Equal(expectedAnswer, result); // Problem is when doing the vertical we don't know if a letter space is already falgged from the horizontal.
        // refactor to use a 2d array.

        return;

        void Doit2(char[,] inputArray, bool a)
        {
            // Process each horizontal line.
            for (var row = 0; row < inputArray.GetLength(0); row++)
            {
                string s = string.Empty;

                for (var col = 0; col < inputArray.GetLength(1); col++)
                {
                    s += inputArray[row, col];
                }

                // Put the first two characters at the end of the string, to 'simulate' us looking around the wrapped boundry.
                if (a)
                {
                    s += inputArray[row, 0];
                    s += inputArray[row, 1];
                }

                DoIt(s, a);
            }
        }

        void DoIt(string line, bool a)
        {
            HashSet<int> runicSymbols = [];

            foreach (var word in words)
            {
                int wordLength = word.Length;
                string tmpLine = line;

                if (a)
                {
                    
                // For the length of the word we're checking, add that number of characters from the front
                // of the string to the end, to simulate us looking around the wrapped body of armour.
                for (int i = 0; i < wordLength - 1; i++)
                {
                    tmpLine += line[i];
                }
                }

                // Read left to right...
                for (var index = 0; index < tmpLine.Length; index++)
                {
                    if (index + wordLength > tmpLine.Length) break;

                    string s = tmpLine[index..(index + wordLength)];

                    if (s.Equals(word))
                    {
                        for (int i = 0; i < word.Length; i++)
                        {
                            runicSymbols.Add(index + i);
                        }
                    }
                }

                // ...and then read right to left using the reversed version of the runic word.
                string wordReversed = new string(word.ToCharArray().Reverse().ToArray());

                for (var index = tmpLine.Length; index >= 0; index--)
                {
                    if (index - wordLength < 0) break;

                    string s = tmpLine[(index - wordLength)..index];

                    if (s.Equals(wordReversed))
                    {
                        for (int i = wordLength; i > 0; i--)
                        {
                            runicSymbols.Add(index - i);
                        }
                    }
                }
            }

            result += runicSymbols.Count;
        }
    }
}