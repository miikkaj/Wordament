using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace CleanDictionary
{
    class CommandLineArgs
    {
        [Option( "input", Required = true, HelpText = @"Dictionary file from /head/share/dict/web2" )]
        public string InputFilePath { get; set; }

        [Option( "output", Required = true, HelpText = "Output file" )]
        public string OutputFilePath { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild( this,
              ( HelpText current ) => HelpText.DefaultParsingErrorsHandler( this, current ) );
        }
    }
}
