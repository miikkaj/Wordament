using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheatTests
{
    public class BaseClass
    {
        const string testFolder = "C:\\test";
        const string dictionaryFolder = "C:\\test\\dictionary\\";
        const string dictionaryFile = dictionaryFolder + "dict.txt";
        const string exeFile = "C:\\test\\Cheat.exe";
        const string exeArgumentPattern = "--input " + dictionaryFile + " --letters {0}";
        string outputStream = String.Empty;

        [ClassInitialize]
        public static void ClassInitialize( TestContext context )
        {
            // make sure that all the files are located in the test folder
            Assert.IsTrue( File.Exists( exeFile ) );
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
        public void RunCheatProgram( string letters, out string consoleOutput )
        {
            string arguments = String.Format( exeArgumentPattern, letters );
            Console.WriteLine( "Running: {0} {1}", exeFile, arguments );

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
        public bool ValidateResultCount( int expected, string consoleOutput )
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

        /// <summary>
        /// Create a dictionary file which contains the words
        /// </summary>
        /// <param name="words">words to include in the dictionary</param>
        public void CreateDictionary( string[] words )
        {
            File.WriteAllLines( dictionaryFile, words );
        }
    }
}
