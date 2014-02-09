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
        string outputStream = String.Empty;

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

            // clear the output stream
            outputStream = String.Empty;
        }
    

        [TestMethod]
        // Create a dictionary with two words, one of which is valid
        public void SimplePositiveCase()
        {
            string[] dictionary = { "hello", "goodbye" };
            File.WriteAllLines( dictionaryFile, dictionary );

            string consoleOutput;

            RunCheatProgram( "h e l l x x x o x x x x x x x x", out consoleOutput );

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

            RunCheatProgram( "a n o x x y x x x x x x x x x x", out consoleOutput );

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
            cheatProcess.StartInfo.RedirectStandardError = true;
            cheatProcess.StartInfo.FileName = exeFile;
            cheatProcess.StartInfo.Arguments = arguments;

            cheatProcess.OutputDataReceived += ( sender, args ) => RecordProcessOutput( args.Data );
            cheatProcess.ErrorDataReceived += ( sender, args ) => RecordProcessOutput( args.Data );

            cheatProcess.Start();
            cheatProcess.BeginOutputReadLine();
            cheatProcess.BeginErrorReadLine();

            cheatProcess.WaitForExit( 5000 );
            
            // make sure the output threads have also terminated
            // more info: http://stackoverflow.com/questions/16095292/how-to-avoid-race-condition-between-process-termination-notification-and-standar
            cheatProcess.WaitForExit();
            
            consoleOutput = outputStream;
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

        /// <summary>
        /// Used to record the console output from the cheat.exe process in the .trx logfile in case anything bad happens
        /// </summary>
        /// <param name="output">output stream</param>
        private void RecordProcessOutput( string output )
        {
            if ( output != null )
            {
                outputStream += output;
                Console.WriteLine( output );
            }
        }
    }
}
