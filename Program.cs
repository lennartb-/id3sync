using System;
using System.Collections.Generic;
using CommandLine;

namespace id3sync
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Arguments>(args)
                .WithParsed(RunOptionsAndReturnExitCode)
                .WithNotParsed(HandleParseError);

        }

        private static void RunOptionsAndReturnExitCode(Arguments opts)
        {
            var ratingSync = new RatingSync(opts.Simulate, opts.PlaylistName, opts.Email);
            ratingSync.SyncRatings();
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Invalid arguments");
            Console.WriteLine("Press key to continue...");
            Console.ReadKey();
        }
    }
}
