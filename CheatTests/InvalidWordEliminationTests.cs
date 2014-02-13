using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheatTests
{
    [TestClass]
    public class GridSearchTests : BaseClass
    {
        [TestMethod]
        // Create a dictionary with two words, one of which is valid
        public void SimplePositiveCase()
        {
            CreateDictionary( new string[] { "hello", "goodbye" } );

            string consoleOutput;

            RunCheatProgram( "h e l l x x x o x x x x x x x x", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        // Confirm that words which contain too many of the same letter are removed
        public void RemoveWordsWithDuplicateLetters()
        {
            CreateDictionary( new string[]  { "annoy", "any" } );

            string consoleOutput;

            RunCheatProgram( "a n o x x y x x x x x x x x x x", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        // Confirm that if the word exists more than once on the grid it only shows up once on the final list
        public void DuplicateWordOnlyCountedOnce()
        {
            CreateDictionary( new string[] { "woo" } );
            string consoleOutput;

            RunCheatProgram( "w o o x x x x x w o o x x x x x", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }
    }
}
