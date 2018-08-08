using System;
using iTunesLib;
using TagLib;
using File = System.IO.File;

namespace id3sync
{
    public class RatingSync
    {
        private readonly bool simulateOnly;
        private readonly string playlistName;
        private readonly string email;

        public RatingSync(bool simulateOnly, string playlistName, string email)
        {
            this.simulateOnly = simulateOnly;
            this.playlistName = playlistName;
            this.email = email;
        }

        public void SyncRatings()
        {
            var iTunesApp = new iTunesApp();

            foreach (IITSource sources in iTunesApp.Sources)
            {
                foreach (IITPlaylist playlist in sources.Playlists)
                {
                    if (playlist.Name != playlistName) continue;

                    WriteRatings(playlist);
                    return;

                }
            }
            Console.WriteLine($"No playlist named {playlistName} found.");
        }

        private void WriteRatings(IITPlaylist playlist)
        {
            foreach (IITTrack playlistTrack in playlist.Tracks)
            {
                var file = (IITFileOrCDTrack)playlistTrack;
                if (File.Exists(file.Location))
                {
                    var tagFile = TagLib.File.Create(file.Location);
                    var tags = tagFile.GetTag(TagTypes.Id3v2);

                    var popm = TagLib.Id3v2.PopularimeterFrame.Get((TagLib.Id3v2.Tag)tags, email, true);

                    var computedRating = Math.Ceiling(ConvertToItunesRatingRange(popm.Rating));
                    var computedStars = ConvertToStarRange(computedRating);

                    Console.WriteLine($"Rating for {file.Artist}-{file.Name} => {computedRating} ({computedStars} stars)");

                    if (!simulateOnly)
                    {
                        file.Rating = (int)computedRating;
                    }
                }
            }
        }

        private static double ConvertToItunesRatingRange(byte popmRating)
        {
            return popmRating * 100 / 255d;
        }

        private static double ConvertToStarRange(double itunesRating)
        {
            return itunesRating * 5 / 100d;
        }
    }
}