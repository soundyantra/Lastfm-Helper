using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;

namespace lastfm
{
    class LastfmStuff
    {
        public List<Track> favoriteTracks;

        private string username;
        private string password;
        private ApplicationContext db;
        private LastfmClient client = new LastfmClient("cbdea1c90bcf1408feb48c87ba055b23", "ae18a107c67187772e7d5248807634ce");


        public LastfmStuff(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public LastfmStuff(string username, string password, ApplicationContext db)
        {
            this.username = username;
            this.password = password;
            this.db = db;
        }

        public async Task<List<Track>> GetFavoriteTracksTask(int mode) //hmhmhmhmhm
        {
            var suc = await client.Auth.GetSessionTokenAsync(username, password);

            switch (mode)
            {
                case 0:
                    List<LastTrack> trash = new List<LastTrack>();
                    var wow = await client.User.GetLovedTracks("soundyantra", 1, 1000); //change
                    trash.AddRange(wow.Content);
                    //trash = trash.Distinct().ToList();
                    db = new ApplicationContext();
                    db.Tracks.Load();
                    foreach (var track in trash)
                    {
                        //int s = Convert.ToInt32(track.Duration.Value.TotalSeconds);
                        if (track.AlbumName == null)
                        {
                            var countValue = await client.Track.GetInfoAsync(track.Name,track.ArtistName,username);
                            
                            int playcount = countValue.Content.UserPlayCount.Value;

                            track.AlbumName = "";
                        }
                        db.Tracks.Add(new Track(track.Name, track.AlbumName, track.ArtistName, 10));
                    }
                    db.SaveChanges();
                    break;
                case 1:
                    db = new ApplicationContext();
                    db.Tracks.Load();
                    break;
            }
            List<Track> ts = db.Tracks.ToList();
            return ts; 
        }

        public List<T> GetNRandom<T>(List<T> list, int n)
        {
            List<T> newlist = new List<T>();
            Random r = new Random();
            //var thetrack = trash[r.Next(trash.Count)];
            for (int i = 0; i < n; i++)
            {
                newlist.Add(list[r.Next(list.Count)]);
            }
            return newlist;
        }

        public void ShowInBrowser(List<Track> tracks)
        {
            foreach (var track in tracks)
            {
                Process.Start("https://www.youtube.com/results?search_query=" + track.Artist + " " + track.Title);
            }
        }

        public async Task ScrobbleFromFolder(string dir)
        {
            var suc = await client.Auth.GetSessionTokenAsync(username, password);
            string[] files = Directory.GetFiles(dir);
            bool arewe = client.Auth.Authenticated;

            foreach (var item in files)
            {
                TagLib.File file = TagLib.File.Create(item);
                Scrobble s = new Scrobble(file.Tag.FirstArtist, file.Tag.Album, file.Tag.Title, DateTimeOffset.Now);
                var recomendsclient = await client.Track.ScrobbleAsync(s);
            }

        }
    }
}
