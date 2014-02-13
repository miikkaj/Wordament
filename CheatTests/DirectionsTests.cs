using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheatTests
{
    [TestClass]
    // Checks that words can be found going in all compass directions
    public class DirectionsTests : BaseClass
    {
        [TestMethod]
        public void NorthToSouth()
        {
            CreateDictionary( new string[] { "boot" } );

            string consoleOutput;

            RunCheatProgram( "b x x x o x x x o x x x t x x x", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        public void EastToWest()
        {
            CreateDictionary( new string[] { "boot" } );

            string consoleOutput;

            RunCheatProgram( "b o o t x x x x x x x x x x x x", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        public void WestToEast()
        {
            CreateDictionary( new string[] { "boot" } );

            string consoleOutput;

            RunCheatProgram( "x x x x x x x x x x x x t o o b", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        public void SouthToNorth()
        {
            CreateDictionary( new string[] { "boot" } );

            string consoleOutput;

            RunCheatProgram( "x x x t x x x o x x x o x x x b", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        public void NorthwestToSoutheast()
        {
            CreateDictionary( new string[] { "boot" } );

            string consoleOutput;

            RunCheatProgram( "b x x x x o x x x x o x x x x t", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        public void SoutheastToNorthwest()
        {
            CreateDictionary( new string[] { "boot" } );

            string consoleOutput;

            RunCheatProgram( "t x x x x o x x x x o x x x x b", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        public void NortheastToSouthwest()
        {
            CreateDictionary( new string[] { "boot" } );

            string consoleOutput;

            RunCheatProgram( "x x x b x x o x x o x x t x x x", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

        [TestMethod]
        public void SouthwestToNortheast()
        {
            CreateDictionary( new string[] { "boot" } );

            string consoleOutput;

            RunCheatProgram( "x x x t x x o x x o x x b x x x", out consoleOutput );

            Console.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            Console.WriteLine( "Result count correct" );
        }

    }
}
