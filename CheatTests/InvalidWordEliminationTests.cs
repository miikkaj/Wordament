using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheatTests
{
    [TestClass]
    public class InvalidWordEliminationTests
    {
        const string testFolder = "C:\\test";
        const string dictionaryFolder = "C:\\test\\dictionary\\";
        const string dictionaryFile = dictionaryFolder + "dict.txt";
        const string exeFile = "C:\\test\\Cheat.exe";
        const string exeArgumentPattern = "--input " + dictionaryFile + " --letters {0}";
        static TestContext testContextInstance;

        [ClassInitialize]
        public static void ClassInit( TestContext context )
        {
            // make sure that all the files are located in the test folder
            Assert.IsTrue( File.Exists( exeFile ) );
            testContextInstance = context;
        }

        [TestInitialize]
        public void Init()
        {
            // clear up old dictionaries
            if ( Directory.Exists( dictionaryFolder ) )
            {
                Directory.Delete( dictionaryFolder, true );
            }
            Directory.CreateDirectory( dictionaryFolder );
        }
    

        [TestMethod]
        // Create a dictionary with two words, one of which is valid
        public void SimplePositiveCase()
        {
            string[] dictionary = { "hello", "goodbye" };
            File.WriteAllLines( dictionaryFile, dictionary );

            string consoleOutput;

            RunCheatProgram( "h e l l o", out consoleOutput );

            testContextInstance.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            testContextInstance.WriteLine( "Result count correct" );
        }

        [TestMethod]
        // Confirm that words which contain too many of the same letter are removed
        public void RemoveWordsWithDuplicateLetters()
        {
            string[] dictionary = { "annoy", "any" };
            File.WriteAllLines( dictionaryFile, dictionary );

            string consoleOutput;

            RunCheatProgram( "a n o y", out consoleOutput );

            testContextInstance.WriteLine( "Validate result count, expecting 1" );
            Assert.IsTrue( ValidateResultCount( 1, consoleOutput ) );
            testContextInstance.WriteLine( "Result count correct" );
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete temporary files
            if ( Directory.Exists( dictionaryFolder ) )
            {
                Directory.Delete( dictionaryFolder, true );
            }
        }

        /// <summary>
        /// Runs the program, and retrieves the console output so we can confirm the results
        /// </summary>
        /// <param name="letters">input to check against the dictionary</param>
        /// <param name="consoleOutput">output to use for validation</param>
        private void RunCheatProgram( string letters, out string consoleOutput )
        {
            string arguments = String.Format( exeArgumentPattern, letters );
            testContextInstance.WriteLine( "Running: {0} {1}", exeFile, arguments );

            Process cheatProcess = new Process();
            cheatProcess.StartInfo.UseShellExecute = false;
            cheatProcess.StartInfo.RedirectStandardOutput = true;
            cheatProcess.StartInfo.FileName = exeFile;
            cheatProcess.StartInfo.Arguments = arguments;
            cheatProcess.Start();

            testContextInstance.WriteLine( "Cheat.exe output:" );

            consoleOutput = cheatProcess.StandardOutput.ReadToEnd();
            testContextInstance.WriteLine( consoleOutput );

            cheatProcess.WaitForExit();
        }

        /// <summary>
        /// Confirms that the amount of results matches the expected result
        /// </summary>
        /// <param name="expected">number of results</param>
        /// <param name="consoleOutput">output from the program running</param>
        /// <returns>true if the count matches</returns>
        private bool ValidateResultCount( int expected, string consoleOutput )
        {
            if ( consoleOutput.Contains( String.Format( "Ending count: {0}", expected ) ) )
            {
                return true;
            }
            else return false;
        }
    }
}
