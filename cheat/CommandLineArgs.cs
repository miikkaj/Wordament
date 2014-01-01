using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace Cheat
{
    class CommandLineArgs
    {
        [Option( "input", Required = true, HelpText = @"Dictionary file from /head/share/dict/web2" )]
        public string InputFilePath { get; set; }

        [OptionArray( "letters", Required = true, HelpText = "Letters on grid" )]
        public string[] Letters { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild( this,
              ( HelpText current ) => HelpText.DefaultParsingErrorsHandler( this, current ) );
        }
    }
}
