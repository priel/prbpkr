using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GilPokerProbability
{

    internal class Program
    {

        private static void Main(string[] args)
        {

            ConsoleKeyInfo consoleKeyInfo;
            //Console.WriteLine("Wellcome to Pio Poker");
            //Console.WriteLine("There are 3 types of games");
            //Console.WriteLine("1. helper\n 2. debug\n 3. 1v1");
            //Console.WriteLine("Please insert your type of game (1,2 or 3)");
            //int gameType = ModifiedConsole.GetInteger();
            int gameType = 3;
            do
            {
                Console.Clear();
                var game = new TexasHoldemPoker();
                switch (gameType)
                {
                    case 1:
                        game.StartHelperGame();
                        break;
                    case 2:
                        game.StartDebugGame();
                        break;
                    case 3:
                        game.Start1v1ArenaGame();
                        break;
                }
                consoleKeyInfo = Console.ReadKey();
            } while (true);
        }

    }
}


// for (int i =0;i<169;i++)
// {
//     tmpString[i] = lines[i].Split(' ').Skip(1).ToArray();
//     tmpString[i] = tmpString[i].Take(tmpString[i].Count() - 1).ToArray();
//     probToWin[i] = Array.ConvertAll(tmpString[i], new Converter<string, double>(Double.Parse));
//     Array.ConvertAll(tmpString[i], Double.Parse);
//     for (int j = 0; j < 9; j++)
//         probToWin[i][j] /= 100;
// }
// System.Xml.Serialization.XmlSerializer writer =
//new System.Xml.Serialization.XmlSerializer(typeof(double[][]));

// System.IO.StreamWriter file = new System.IO.StreamWriter(
//     @"C:\Users\priel\Desktop\pokerPreFlopProb.xml");
// writer.Serialize(file, probToWin);
// file.Close();

//Console.WriteLine("My array: {0}", string.Join("\n", probToWin[35].Select(v => v.ToString())));
////Console.WriteLine("My array: {0}", string.Join("\n", probToWin[35].Select(v => v.ToString())));
////lines[i].Split(' ').Skip(1).Select(double.parse).ToArray();

//Probabilities probably = new Probabilities();
//probably.InitPreFlopDB();

// List<string> lines = new List<string>();
// lines = System.IO.File.ReadAllLines(@"C:\Users\priel\Desktop\pokerProbDB.txt").ToList();
// string[][] tmpString = new string[169][];
// double [][] probToWin = new double[169][];