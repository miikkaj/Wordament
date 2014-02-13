using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheatTests
{
    [TestClass]
    public class InvalidWordEliminationTests : BaseClass
    {
        [TestMethod]
        // Make sure words which would have passed when we didn't check if the letters were all connected will no longer pass
        public void LettersUnconnected()
        {
            CreateDictionary( new string[] { "woo" } );

            string consoleOutput;

            RunCheatProgram( "w o x o x x x x x x x x x x x x", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 0" );
            Assert.IsTrue( ValidateResultCount( 0, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        // Confirm that words cannot be made by re-visiting letters which already make up part of the word
        public void NoBacktracking()
        {
            CreateDictionary( new string[] { "wow" } );

            string consoleOutput;

            RunCheatProgram( "w o x x x x x x x x x x x x x w", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 0" );
            Assert.IsTrue( ValidateResultCount( 0, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }
    }
}
