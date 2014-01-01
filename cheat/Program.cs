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

                var newDictionary = from word in baseDictionary
                                    where !Regex.IsMatch( word, gridRegexPattern )
                                    orderby word.Length descending
                                    select word;

                Console.WriteLine( "Ending count: {0}", newDictionary.Count() );

            }
        }
    }
}
