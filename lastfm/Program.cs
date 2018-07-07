using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using TagLib.Riff;

namespace lastfm
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public DbSet<Track> Tracks { get; set; }
        public object Phones { get; internal set; }
    }

    public class Track
    {

        private string title;
        private int id;
        private string album;
        private string artist;
        private int length;

        public Track(string title, string album, string artist, int length)
        {
            Title = title;
            //Id = id;
            Album = album;
            Artist = artist;
            Length = length;
        }

        static public int method()
        {
            return 2 + 2;
        }

        public Track()
        {
            
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Album
        {
            get { return album; }
            set { album = value; }
        }

        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }
    }

    class Program
    {
        public static ApplicationContext db;
        public static void db_method(List<LastTrack> tracks)
        {
            //ApplicationContext db;
            db = new ApplicationContext();
            db.Tracks.Load();
            foreach (var track in tracks)
            {
                db.Tracks.Add(new Track(track.Name, track.AlbumName, track.ArtistName, Convert.ToInt32(track.Duration.Value.TotalSeconds)));
            }
            //db.Tracks.Add(new Track("wew", "www", "dddd", 45));
            db.SaveChanges();
            //List<Track> tracks = db.Tracks.Distinct().ToList();
            
        }

        public void StructuresTests()
        {
            (int, int) tuple = (5, 10);
            Hashtable hashtable = new Hashtable();
            ObservableCollection<Track> users = new ObservableCollection<Track>();
            LinkedList<Track> fff = new LinkedList<Track>();
            Dictionary<int, string> countries = new Dictionary<int, string>(5);
            countries.Add(1, "Russia");
            countries.Add(3, "Great Britain");
            countries.Add(2, "USA");
            countries.Add(4, "France");
            countries.Add(5, "China");

        }


        static void Main(string[] args)
        {
            LastfmStuff lastfm = new LastfmStuff(args[1], args[2],db);
            int num = Convert.ToInt32(args[0]);
            Console.WriteLine(num + " " + args[1]);
            string dir = @"D:\Music\Global Communication";
            while (true)
            {
                Console.WriteLine("Welcome!");

                Console.ReadLine();
                //db_method();
                //method(num, args[1], args[2]).Wait();
                List<Track> favoritesList = lastfm.GetFavoriteTracksTask(num).Result;
                //lastfm.ShowInBrowser(lastfm.GetNRandom(favoritesList, 10));
                lastfm.ScrobbleFromFolder(dir).Wait();
            }

        }
    }
}
