using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CleanDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdLineArgs = new CommandLineArgs();

            if ( CommandLine.Parser.Default.ParseArguments( args, cmdLineArgs ) )
            {
                string[] baseDictionary = File.ReadAllLines( cmdLineArgs.InputFilePath );

                Console.WriteLine( "Starting count: {0}", baseDictionary.Length );

                var newDictionary = from word in baseDictionary
                                    where word.Length < 17 && word.Length > 2
                                    select word;

                Console.WriteLine( "Ending count: {0}", newDictionary.Count() );

                File.WriteAllLines( cmdLineArgs.OutputFilePath, newDictionary );
            }
        }
    }
}
