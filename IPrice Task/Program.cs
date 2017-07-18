using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace IPrice_Task
{
    class Program
    {
        public static string CleanText(string dirtyString)
        {
            return new String(dirtyString.ToLower().Where(Char.IsLetterOrDigit).ToArray());
        }

        static void Main(string[] args)
        {
            string wordsDir = @"C:\Users\a-alosam\Desktop\IPrice\reuters21578";
            string bookFile = @"C:\Users\a-alosam\Desktop\IPrice\plot.list\plot.list";
            string outputFile = @"C:\Users\a-alosam\Desktop\IPrice\wordsFreq.txt";
            Dictionary<string, int> words = new Dictionary<string, int>();

            var sw = new Stopwatch();
            sw.Start();
            foreach (var wordsfile in Directory.GetFiles(wordsDir, "*.sgm", SearchOption.AllDirectories))
            {
                using (StreamReader reader = new StreamReader(wordsfile))
                {
                    while (!reader.EndOfStream)
                    {
                        var lineWords = reader.ReadLine().Trim().Split(' ');
                        foreach (var word in lineWords)
                        {
                            var cleanWord = CleanText(word);
                            if (!words.ContainsKey(cleanWord) && cleanWord != string.Empty)
                            {
                                words[cleanWord] = 0;
                            }
                        }
                    }          
                }
            }

            sw.Stop();
            Console.WriteLine("Reading words: " + sw.Elapsed.ToString());
            sw.Restart();
            string dummyline = null;
            int skipLines = 15;
            using (StreamReader reader = new StreamReader(bookFile))
            {
                while (skipLines-- > 0)
                {
                    dummyline = reader.ReadLine();
                }

                while (!reader.EndOfStream)
                {
                    dummyline = reader.ReadLine().Trim(); ;
                    if (dummyline == string.Empty || dummyline[0] == '-') continue;

                    var lineWords = dummyline.Split(' ');
                    foreach (var word in lineWords)
                    {
                        var cleanWord = CleanText(word);
                        if (words.ContainsKey(cleanWord))
                        {
                            words[cleanWord]++;
                        }
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("Reading Book: " + sw.Elapsed.ToString());
 

            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                foreach(var word in words)
                {
                    if (word.Value != 0)
                    {
                        writer.WriteLine(string.Format("{0}\t{1}", word.Key, word.Value));
                    }
                }
            }

        }
    }
}
