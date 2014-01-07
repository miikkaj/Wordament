using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Cheat
{
    class Program
    {
        static void Main( string[] args )
        {
            var cmdLineArgs = new CommandLineArgs();

            if ( CommandLine.Parser.Default.ParseArguments( args, cmdLineArgs ) )
            {
                string[] baseDictionary = File.ReadAllLines( cmdLineArgs.InputFilePath );

                // First simple match, eliminate the words which contain letters that are not on the grid

                string combinedLetters = String.Join( null, cmdLineArgs.Letters );
                combinedLetters = Regex.Replace( combinedLetters, "[^a-zA-Z]", String.Empty );

                string gridRegexPattern = "[^" + combinedLetters + "]";

                List<string> newDictionary = (from word in baseDictionary
                                    where !Regex.IsMatch( word, gridRegexPattern )
                                    orderby word.Length descending
                                    select word).ToList<string>();

                // Second, remove all words which contain too many of the letters (so if we only have one n, 'annoy'
                // wouldn't be valid, even though it passed the first regex)
                IEnumerable<char> uniqueLetters = combinedLetters.Distinct();

                List<string> wordsToRemove = new List<string>();

                foreach ( char letter in uniqueLetters )
                {
                    string letterAsString = letter.ToString();
                    int count = Regex.Matches( combinedLetters, letterAsString ).Count;

                    foreach ( string word in newDictionary )
                    {
                        if ( Regex.Matches( word, letterAsString ).Count > count )
                        {
                            // add to list of words to be removed
                            wordsToRemove.Add( word );
                        }
                    }
                }

                newDictionary = newDictionary.Except( wordsToRemove ).ToList<string>();

                Console.WriteLine( "Ending count: {0}", newDictionary.Count() );

            }
        }
    }
}
