using CommandLine;

namespace id3sync
{
    internal class Arguments
    {
        [Option('p', "playlist", Required = true, HelpText = "Playlist with tracks to sync.")]
        public string PlaylistName { get; set; }
        [Option('s', "simulate", Required = false, HelpText = "Don't actually change ratings, only output computed changes.")]
        public bool Simulate { get; set; }
        [Option('e', "email", Required = true, HelpText = "The 'email' field for the POPM-Tag. Used to identify the program which reads/wrote the rating. E.g. 'MusicBee', 'no@email' for MediaMonkey...")]
        public string Email { get; set; }
    }
}
