/*
 * This Program is made to solve the best route through Mario Kart World's Connected tracks 
 * by means of brute force. It solves the route in reverse, starting from Peach Stadium,
 * and takes the time FROM surrounding tracks to work backwards, creating an open ended tree
 * that allows for variable start points. 
 * 
 * The data in the `List<Track> tracks` List should specify which courses can be traveled from, 
 * as well as the time it takes to travel from there.


*/

using System.Net;
using System.Runtime.Intrinsics.Arm;

internal class Program
{
    static string path = @"D:\Programs\MK.txt";

    // enum as placeholder values for each course ID
    enum TrackID
    {
        AcoH    ,
        AirF    ,
        BooC    ,
        BowC    ,
        CheF    ,
        ChoM    ,
        CroC    ,
        DanD    ,
        DesH    ,
        DinJ    ,
        DkP     ,
        DkS     ,
        DryB    ,
        FarO    ,
        GreR    ,
        KooB    ,
        MarB    ,
        MarC    ,
        MooM    ,
        PeaB    ,
        PeaS    ,
        SalS    ,
        ShyB    ,
        SkyS    ,
        StaP    ,
        ToaF    ,
        WarSh   ,
        WarSt   ,
        WhiS
    }
    // track data, including name, an ID using the enum above, and every possible connection
    class Track
    {
        public Track() {}

        // name of Track in plain text
        public string trackName { get; set; }
        // unique ID of the Track
        public TrackID trackID { get; set; }

        // array containing origin ID + travel time
        public Connection[] origin { get; set; }

    }
    // possible tracks that can be traveled from, including their time. set in the track List (hard coded (horrible))
    class Connection
    {
        // ID of the origin Track
        public TrackID Track { get; set; }
        // time in seconds to travel from this track
        public int travelTime { get; set; }
        // constructor to set values
        public Connection(TrackID fromTrack, int seconds) {
            Track = fromTrack;
            travelTime = seconds;
        }

        
    }
    // entries into a table of valid results only
    class Results
    {
        // the final string of all tracks in sequence for that results
        public string tracks { get; set; }
        // the final total time, used to compare and find best result
        public int time { get; set; }
    }

    private static void Main(string[] args)
    {
        // the big fuckin list and I hard coded it like a dum
        // connections "from" Peach Stadium are not necessary since it's the final destination
        List<Track> tracks = new List<Track>() {
        new Track() { trackID = TrackID.AcoH, trackName = "Acorn Heights", 
            origin = new Connection[] {
            new Connection(TrackID.BooC, 127),
            new Connection(TrackID.DanD, 173),
            new Connection(TrackID.DryB, 106),
            new Connection(TrackID.MarC, 104),
            new Connection(TrackID.ToaF, 148)
            } },
        new Track() { trackID = TrackID.AirF, trackName = "Airship Fortress", 
            origin = new Connection[] {
            new Connection(TrackID.BowC, 133),
            new Connection(TrackID.DryB, 153),
            new Connection(TrackID.ShyB, 161),
            new Connection(TrackID.ToaF, 134),
            new Connection(TrackID.WarSt, 131)
            } },
        new Track() { trackID = TrackID.BooC, trackName = "Boo Cinema", 
            origin = new Connection[] {
            new Connection(TrackID.AcoH, 129),
            new Connection(TrackID.DanD, 141),
            new Connection(TrackID.DryB, 160),
            new Connection(TrackID.MarC, 120),
            new Connection(TrackID.StaP, 130)
            } },
        new Track() { trackID = TrackID.BowC, trackName = "Bowser's Castle", 
            origin = new Connection[] {
            new Connection(TrackID.AirF, 135),
            new Connection(TrackID.ChoM, 178),
            new Connection(TrackID.DryB, 133),
            new Connection(TrackID.MarC, 154),
            new Connection(TrackID.ToaF, 141),
            new Connection(TrackID.WarSt, 134)
            } },
        new Track() { trackID = TrackID.CheF, trackName = "Cheep Cheep Falls", 
            origin = new Connection[] {
            new Connection(TrackID.ChoM,    166),
            new Connection(TrackID.DanD,    127),
            new Connection(TrackID.DkP,     149),
            new Connection(TrackID.FarO,    111),
            new Connection(TrackID.MooM,    135),
            new Connection(TrackID.SalS,    116),
            new Connection(TrackID.SkyS,    189),
            new Connection(TrackID.StaP,    197),
            new Connection(TrackID.WarSh,   170)
            } },
        new Track() { trackID = TrackID.ChoM, trackName = "Choco Mountain", 
            origin = new Connection[] {
            new Connection(TrackID.BowC,    153),
            new Connection(TrackID.CheF,    147),
            new Connection(TrackID.CroC,    111),
            new Connection(TrackID.MarB,    103),
            new Connection(TrackID.MooM,    111),
            new Connection(TrackID.ShyB,    136),
            new Connection(TrackID.ToaF,    124),
            new Connection(TrackID.WarSt,   114),
            new Connection(TrackID.WhiS,    157)
            } },
        new Track() { trackID = TrackID.CroC, trackName = "Crown City", 
            origin = new Connection[] {
            new Connection(TrackID.ChoM,    115),
            new Connection(TrackID.DesH,    175),
            new Connection(TrackID.DkS,     113),
            new Connection(TrackID.FarO,    136),
            new Connection(TrackID.KooB,    98),
            new Connection(TrackID.MarB,    133),
            new Connection(TrackID.MooM,    153),
            new Connection(TrackID.WarSt,   177),
            new Connection(TrackID.WhiS,    144)
            } },
        new Track() { trackID = TrackID.DanD, trackName = "Dandelion Depths", 
            origin = new Connection[] {
            new Connection(TrackID.AcoH,    178),
            new Connection(TrackID.BooC,    139),
            new Connection(TrackID.CheF,    134),
            new Connection(TrackID.DkP,     115),
            new Connection(TrackID.MarC,    138),
            new Connection(TrackID.MooM,    130),
            new Connection(TrackID.SkyS,    186),
            new Connection(TrackID.StaP,    122),
            new Connection(TrackID.ToaF,    190)
            } },
        new Track() { trackID = TrackID.DesH, trackName = "Desert Hills", 
            origin = new Connection[] {
            new Connection(TrackID.CroC, 158),
            new Connection(TrackID.DkS, 155),
            new Connection(TrackID.MarB, 95),
            new Connection(TrackID.ShyB, 105),
            new Connection(TrackID.WhiS, 96)
            } },
        new Track() { trackID = TrackID.DinJ, trackName = "Dino Dino Jungle", 
            origin = new Connection[] {
            new Connection(TrackID.FarO, 106),
            new Connection(TrackID.GreR, 91),
            new Connection(TrackID.KooB, 102),
            new Connection(TrackID.PeaB, 163),
            new Connection(TrackID.SalS, 142)
            } },
        new Track() { trackID = TrackID.DkP, trackName = "DK Pass", 
            origin = new Connection[] {
            new Connection(TrackID.CheF, 111),
            new Connection(TrackID.DanD, 105),
            new Connection(TrackID.MooM, 175),
            new Connection(TrackID.SalS, 121),
            new Connection(TrackID.SkyS, 120),
            new Connection(TrackID.StaP, 110),
            new Connection(TrackID.WarSh, 142)
            } },
        new Track() { trackID = TrackID.DkS, trackName = "DK Spaceport",
            origin = new Connection[] {
            new Connection(TrackID.CroC, 184),
            new Connection(TrackID.KooB, 155),
            new Connection(TrackID.WhiS, 159)
            } },
        new Track() { trackID = TrackID.DryB, trackName = "Dry Bones Burnout", 
            origin = new Connection[] {
            new Connection(TrackID.AcoH, 110),
            new Connection(TrackID.AirF, 160),
            new Connection(TrackID.BooC, 157),
            new Connection(TrackID.BowC, 150),
            new Connection(TrackID.MarC, 99),
            new Connection(TrackID.MooM, 167),
            new Connection(TrackID.ToaF, 102),
            new Connection(TrackID.WarSt, 158)
            } },
        new Track() { trackID = TrackID.FarO, trackName = "Faraway Oasis", 
            origin = new Connection[] {
            new Connection(TrackID.CheF, 113),
            new Connection(TrackID.CroC, 132),
            new Connection(TrackID.DinJ, 105),
            new Connection(TrackID.GreR, 112),
            new Connection(TrackID.KooB, 123),
            new Connection(TrackID.PeaB, 139),
            new Connection(TrackID.SalS, 120)
            } },
        new Track() { trackID = TrackID.GreR, trackName = "Great ? Block Ruins", 
            origin = new Connection[] {
            new Connection(TrackID.DinJ, 105),
            new Connection(TrackID.FarO, 121),
            new Connection(TrackID.PeaB, 118),
            new Connection(TrackID.SalS, 124)
            } },
        new Track() { trackID = TrackID.KooB, trackName = "Koopa Troopa Beach", 
            origin = new Connection[] {
            new Connection(TrackID.CroC, 81),
            new Connection(TrackID.DesH, 180),
            new Connection(TrackID.DinJ, 84),
            new Connection(TrackID.DkS, 87),
            new Connection(TrackID.FarO, 91),
            new Connection(TrackID.GreR, 142),
            new Connection(TrackID.WhiS, 123)
            } },
        new Track() { trackID = TrackID.MarB, trackName = "Mario Bros. Circuit", 
            origin = new Connection[] {
            new Connection(TrackID.ChoM, 109),
            new Connection(TrackID.CroC, 120),
            new Connection(TrackID.DesH, 103),
            new Connection(TrackID.DkS, 165),
            new Connection(TrackID.ShyB, 99),
            new Connection(TrackID.ToaF, 175),
            new Connection(TrackID.WarSt, 117),
            new Connection(TrackID.WhiS, 105)
            } },
        new Track() { trackID = TrackID.MarC, trackName = "Mario Circuit", 
            origin = new Connection[] {
            new Connection(TrackID.AcoH, 128),
            new Connection(TrackID.BooC, 90),
            new Connection(TrackID.BowC, 148),
            new Connection(TrackID.DanD, 108),
            new Connection(TrackID.DryB, 84),
            new Connection(TrackID.MooM, 98),
            new Connection(TrackID.StaP, 156),
            new Connection(TrackID.ToaF, 111)
            } },
        new Track() { trackID = TrackID.MooM, trackName = "Moo Moo Meadows", 
            origin = new Connection[] {
            new Connection(TrackID.CheF, 121),
            new Connection(TrackID.ChoM, 107),
            new Connection(TrackID.CroC, 146),
            new Connection(TrackID.DanD, 120),
            new Connection(TrackID.DkP, 181),
            new Connection(TrackID.DryB, 156),
            new Connection(TrackID.MarC, 111),
            new Connection(TrackID.ToaF, 124)
            } },
        new Track() { trackID = TrackID.PeaB, trackName = "Peach Beach", 
            origin = new Connection[] {
            new Connection(TrackID.DinJ, 194),
            new Connection(TrackID.FarO, 154),
            new Connection(TrackID.GreR, 134),
            new Connection(TrackID.SalS, 131),
            new Connection(TrackID.WarSh, 140)
            } },
        new Track() { trackID = TrackID.PeaS, trackName = "Peach Stadium", 
            origin = new Connection[] {
            new Connection(TrackID.CheF, 101),
            new Connection(TrackID.ChoM, 83),
            new Connection(TrackID.CroC, 113),
            new Connection(TrackID.DkS, 178),
            new Connection(TrackID.FarO, 104),
            new Connection(TrackID.KooB, 108),
            new Connection(TrackID.MarC, 166),
            new Connection(TrackID.MooM, 97),
            new Connection(TrackID.ToaF, 165)
            } },
        new Track() { trackID = TrackID.SalS, trackName = "Salty Salty Speedway", 
            origin = new Connection[] {
            new Connection(TrackID.CheF, 121),
            new Connection(TrackID.DinJ, 144),
            new Connection(TrackID.DkP, 133),
            new Connection(TrackID.FarO, 130),
            new Connection(TrackID.GreR, 110),
            new Connection(TrackID.PeaB, 103),
            new Connection(TrackID.SkyS, 189),
            new Connection(TrackID.WarSh, 129)
            } },
        new Track() { trackID = TrackID.ShyB, trackName = "Shy Guy Bazaar", 
            origin = new Connection[] {
            new Connection(TrackID.AirF, 112),
            new Connection(TrackID.ChoM, 145),
            new Connection(TrackID.DesH, 114),
            new Connection(TrackID.MarB, 109),
            new Connection(TrackID.WarSt, 114)
            } },
        new Track() { trackID = TrackID.SkyS, trackName = "Sky High Sundae", 
            origin = new Connection[] {
            new Connection(TrackID.DkP, 118),
            new Connection(TrackID.WarSh, 160),
            new Connection(TrackID.DanD, 177),
            new Connection(TrackID.StaP, 120)
            } },
        new Track() { trackID = TrackID.StaP, trackName = "Starview Peak", 
            origin = new Connection[] {
            new Connection(TrackID.BooC, 155),
            new Connection(TrackID.CheF, 160),
            new Connection(TrackID.DanD, 132),
            new Connection(TrackID.DkP, 123),
            new Connection(TrackID.MarC, 195),
            new Connection(TrackID.SkyS, 129),
            new Connection(TrackID.WarSh, 187)
            } },
        new Track() { trackID = TrackID.ToaF, trackName = "Toad's Factory", 
            origin = new Connection[] {
            new Connection(TrackID.BowC, 120),
            new Connection(TrackID.AcoH, 138),
            new Connection(TrackID.AirF, 139),
            new Connection(TrackID.ChoM, 120),
            new Connection(TrackID.DanD, 109),
            new Connection(TrackID.DryB, 89),
            new Connection(TrackID.MarB, 170),
            new Connection(TrackID.MarC, 109),
            new Connection(TrackID.MooM, 118),
            new Connection(TrackID.WarSt, 123)
            } },
        new Track() { trackID = TrackID.WarSh, trackName = "Wario Shipyard", 
            origin = new Connection[] {
            new Connection(TrackID.CheF, 187),
            new Connection(TrackID.DkP, 143),
            new Connection(TrackID.PeaB, 121),
            new Connection(TrackID.SalS, 118),
            new Connection(TrackID.SkyS, 125),
            new Connection(TrackID.StaP, 228)
            } },
        new Track() { trackID = TrackID.WarSt, trackName = "Wario Stadium", 
            origin = new Connection[] {
            new Connection(TrackID.BowC, 129),
            new Connection(TrackID.AirF, 131),
            new Connection(TrackID.ChoM, 133),
            new Connection(TrackID.CroC, 183),
            new Connection(TrackID.DryB, 158),
            new Connection(TrackID.MarB, 132),
            new Connection(TrackID.ShyB, 135),
            new Connection(TrackID.ToaF, 128)
            } },
        new Track() { trackID = TrackID.WhiS, trackName = "Whistlestop Summit", 
            origin = new Connection[] {
            new Connection(TrackID.ChoM, 169),
            new Connection(TrackID.CroC, 127),
            new Connection(TrackID.DesH, 110),
            new Connection(TrackID.DkS, 107),
            new Connection(TrackID.MarB, 108)
            } },
            };

        var watch = System.Diagnostics.Stopwatch.StartNew();
        // Initialize the solver. the index of tracks[] will set the final destination (should be Peach Stadium)
        FindBestRoute(tracks, tracks[20]);
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Console.WriteLine("Finished in: " + (elapsedMs.ToString()));

        /*foreach (Track track in tracks)
        {
            Console.WriteLine(track.trackName + " can travel to:");
            foreach (Connection i in track.destination)
            {
                Console.WriteLine($"{GetTrackName(tracks, i.Track)}" + " in " + i.travelTime + " seconds.");
            }
            Console.WriteLine("");

            
        }*/
    }

    private static void FindBestRoute(List<Track> tracks, Track finalTrack)
    {
        List<TrackID> lockOut = new List<TrackID>(); // initialize list of tracks that have already been visited
        List<TrackID> tracksLeft = new List<TrackID>() { 
        TrackID.AcoH, 
        TrackID.AirF,
        TrackID.BooC    ,
        TrackID.BowC    ,
        TrackID.CheF    ,
        TrackID.ChoM    ,
        TrackID.CroC    ,
        TrackID.DanD    ,
        TrackID.DesH    ,
        TrackID.DinJ    ,
        TrackID.DkP     ,
        TrackID.DkS     ,
        TrackID.DryB    ,
        TrackID.FarO    ,
        TrackID.GreR    ,
        TrackID.KooB    ,
        TrackID.MarB    ,
        TrackID.MarC    ,
        TrackID.MooM    ,
        TrackID.PeaB    ,
        TrackID.SalS    ,
        TrackID.ShyB    ,
        TrackID.SkyS    ,
        TrackID.StaP    ,
        TrackID.ToaF    ,
        TrackID.WarSh   ,
        TrackID.WarSt   ,
        TrackID.WhiS }; // initialize list of tracks that haven't already been visited
        lockOut.Add(finalTrack.trackID); // add the ending track to the list so it's not empty
        int totalTime = 0; // keep track of total time in route
        int tracksVisited = 1; // keep track of how many tracks have been traveled to in the tree
        List<Results> results = new List<Results>(); // initialize list of final valid results
        
        // an array to keep track of each course's remaining options for tracks that can travel to it
        int[] options = { 5, 5, 5, 6, 9, 9, 9, 9, 5, 5, 7, 3, 8, 7, 4, 7, 8, 8, 8, 5, 9, 8, 5, 4, 7, 10, 6, 8, 5 };

        // do the thing
        Travel(tracks, finalTrack, lockOut, tracksLeft, totalTime, tracksVisited, results, options);

        // find the shortest time from all valid results, and find its index in the list
        int bestResult = results.Min(r => r.time);
        int finalResult = results.FindIndex(r => r.time == bestResult);

        string message = ("\nThe Best Path is:\n" + results[finalResult].tracks + "Rainbow Road" + ": " + results[finalResult].time + " seconds");
        // display the final result with the shortest time
        Console.WriteLine(message);

        File.WriteAllText(path, message);
        // display all final results for validation
        Console.WriteLine("\nAll Results: " + results.Count);
        File.AppendAllText(path, ("\n\nAll Results: " + results.Count) + "\n");
        foreach (Results r in results)
        {
            Console.WriteLine(r.tracks + "Rainbow Road" + ": " + r.time + " seconds\n");
            File.AppendAllText(path, (r.tracks + "Rainbow Road" + ": " + r.time + " seconds\n\n"));
        }

    }

    private static void Travel(List<Track> tracks, Track currentTrack, List<TrackID> lockOut, List<TrackID> tracksLeft, 
        int totalTime, int tracksVisited, List<Results> results, int[] options)
    {
        /* At every step, we're checking for routes that have the potential to be incomplete.
         * 
         * We're gonna step 1 connection in deeper to see if moving to the current node has created a vulnerability
         * tracks become vulnerable if they only have 1 possible escape route.
         * 
         * If so, during recursion we're going to force it to only go down paths that have the 1 escape route,
         * to prevent it from being cut off and creating an incomplete route.
         */
        bool vulnerable = CheckForVulnerableTrack(currentTrack, options, lockOut);

        /* Another rule is going to be running a prediction on the remaining time in the route.
         * Using a lower bounds and applying them to the rest of the tracks available,
         * it comes up with a prediction of the route and compares it to the current best time
         */

        // alternate method of predicting route, by attempting to get the best split from all of the remaining tracks and adding together
        /*
        int remainingBestTime = 0;
        foreach (TrackID ID in tracksLeft) 
            {
                remainingBestTime += tracks[(int)ID].origin.Min(r => r.travelTime);
            }
        */
        int remainingBestTime = (105 * (29 - tracksVisited));
        int bestPossibleTime = totalTime + remainingBestTime;

        // abandon processing if the total time is already slower than the best time, but not if there are no results yet
        if (results.Count > 0 && bestPossibleTime > results.Min(r => r.time))
        {
            return;
        }
        

        // checks if all tracks have been visited, and puts the result into the list of valid results
        if (tracksVisited == (tracks.Count) /* "&& lockOut[(tracks.Count - 1)] == TrackID.KooTB" use this if you want to specify a start point*/ )
            {
            // used to create the concatenated string to put into the results table
            string r = string.Empty;
            // reverse the lockout list to create the route from start to end
            lockOut.Reverse();
            // saves the order to a string to be stored in results
            foreach (TrackID x in lockOut)
                {
                    r += tracks[(int)x].trackName + " -> ";
                }
            results.Add(new Results() { tracks = r, time = totalTime });
            // keeps track of results found during runtime
            Console.Clear();
            Console.WriteLine("Results: " + results.Count);
            // un-reverses the list for continued processing (otherwise it gets jumbled)
            lockOut.Reverse();
            return;
        }
        // process each track you can travel from, to the current track
        foreach (Connection origin in currentTrack.origin)
        {
            // if the next track has already been visited, skip
            if (lockOut.Contains(origin.Track))
            {
                continue;
            }
            // if a vulnerable track is present, ignore all but the vulnerable tracks (should catch if there are multiple)
            else if (vulnerable && options[(int)origin.Track] != 1)
            {
                continue;
            }
            // otherwise, it's ok to proceed
            else 
            {
                // this loop helps keep track of how many options each track has available to them, stored in the array "options"
                foreach (Connection x in tracks[(int)origin.Track].origin) 
                {
                    // don't take away options from tracks that have one-ways and don't rely on origin to have options
                    if ((origin.Track == TrackID.CheF && x.Track == TrackID.SkyS)
                        || (origin.Track == TrackID.PeaS && (x.Track == TrackID.DkS || x.Track == TrackID.ToaF))
                        || (origin.Track == TrackID.SalS && x.Track == TrackID.SkyS)
                        || (origin.Track == TrackID.KooB && (x.Track == TrackID.GreR || x.Track == TrackID.WhiS || x.Track == TrackID.DesH))
                        || (origin.Track == TrackID.MarB && x.Track == TrackID.DkS)
                        || (origin.Track == TrackID.DesH && x.Track == TrackID.DkS)
                        )
                    {
                        continue;
                    }
                    // process option removal normally
                    options[(int)x.Track]--;
                }
                // take away options from one-ways that the origin doesn't account for
                if (origin.Track == TrackID.SkyS)
                {
                    options[(int)TrackID.CheF]--;
                    options[(int)TrackID.SalS]--;
                }
                else if (origin.Track == TrackID.DkS)
                {
                    options[(int)TrackID.MarB]--;
                    options[(int)TrackID.DesH]--;
                }
                else if (origin.Track == TrackID.GreR || origin.Track == TrackID.WhiS || origin.Track == TrackID.DesH)
                {
                    options[(int)TrackID.KooB]--;
                }

                // increment how many tracks have been traveled to
                tracksVisited++;
                // add the origin track to the lockout list, so it doesn't get chosen again after movement
                lockOut.Add(origin.Track);
                tracksLeft.Remove(origin.Track);
                // add the travel time it took to get to current track from origin track
                totalTime += origin.travelTime;
                // process the origin track and it's connections
                Travel(tracks, tracks[(int)origin.Track], lockOut, tracksLeft, totalTime, tracksVisited, results, options);
                // after this point, each tree will resolve itself and all of its possible connections

                // remove the last processed track from the list (like a stack)
                lockOut.Remove(origin.Track);
                tracksLeft.Add(origin.Track);
                // rewind 1 level and take another branch from the current track, also subtracting the most recent time
                tracksVisited--;
                totalTime -= origin.travelTime;
                // reverse the changes to the "options" loop 
                foreach (Connection x in tracks[(int)origin.Track].origin)
                {
                    if ((origin.Track == TrackID.CheF && x.Track == TrackID.SkyS)
                        || (origin.Track == TrackID.PeaS && (x.Track == TrackID.DkS || x.Track == TrackID.ToaF))
                        || (origin.Track == TrackID.SalS && x.Track == TrackID.SkyS)
                        || (origin.Track == TrackID.KooB && (x.Track == TrackID.GreR || x.Track == TrackID.WhiS || x.Track == TrackID.DesH))
                        || (origin.Track == TrackID.MarB && x.Track == TrackID.DkS)
                        || (origin.Track == TrackID.DesH && x.Track == TrackID.DkS)
                        )
                    {
                        continue;
                    }
                    
                    options[(int)x.Track]++;
                }
                if (origin.Track == TrackID.SkyS)
                {
                    options[(int)TrackID.CheF]++;
                    options[(int)TrackID.SalS]++;
                }
                if (origin.Track == TrackID.DkS)
                {
                    options[(int)TrackID.MarB]++;
                    options[(int)TrackID.DesH]++;
                }
                if (origin.Track == TrackID.GreR || origin.Track == TrackID.WhiS || origin.Track == TrackID.DesH)
                {
                    options[(int)TrackID.KooB]++;
                }
            }

        }
    }

    private static bool CheckForVulnerableTrack(Track currentTrack, int[] options, List<TrackID> lockOut)
    {
        bool x = false;
        foreach (Connection origin in currentTrack.origin)
        {
            // if we've already visited the track, we don't care if it only has 1 escape route
            if (lockOut.Contains(origin.Track)) { continue; }

            // if a vulnerable track is found, sound the alarm
            if (options[(int)origin.Track] == 1)
            {
                x = true;
            }
        }
        return x;
    }

    // was used in testing
    private static string GetTrackName(List<Track> list, TrackID ID)
    {
        int x = (int)ID;
        string trackName = list[x].trackName;

        return trackName;
    }
}

    // SAMPLE LIST CONFIRMED WORKS

/*      List<Track> tracks = new List<Track>() {
        new Track() { trackID = TrackID.BowC, trackName = "Bowser's Castle",
            origin = new Connection[2] {
                new Connection(TrackID.DryBB, 10),
                new Connection(TrackID.AcoH, 20) } },
        new Track() { trackID = TrackID.DryBB, trackName = "Dry Bones Burnout",
            origin = new Connection[3] {
                new Connection(TrackID.BowC, 10),
                new Connection(TrackID.AcoH, 10),
                new Connection(TrackID.BooC, 20) } },
        new Track() { trackID = TrackID.AcoH, trackName = "Acorn Heights",
            origin = new Connection[4] {
                new Connection(TrackID.BowC, 20),
                new Connection(TrackID.DryBB, 10),
                new Connection(TrackID.BooC, 10),
                new Connection(TrackID.StaP, 20) } },
        new Track() { trackID = TrackID.BooC, trackName = "Boo Cinema",
            origin = new Connection[4] {
                new Connection(TrackID.DryBB, 20),
                new Connection(TrackID.AcoH, 10),
                new Connection(TrackID.StaP, 10),
                new Connection(TrackID.SkyH, 20)} },
        new Track() { trackID = TrackID.StaP, trackName = "Starview Peak",
            origin = new Connection[3] {
                new Connection(TrackID.AcoH, 20),
                new Connection(TrackID.BooC, 10),
                new Connection(TrackID.SkyH, 10)} },
        new Track() { trackID = TrackID.StaP, trackName = "Sky High Sundae",
            origin = new Connection[2] {
                new Connection(TrackID.BooC, 20),
                new Connection(TrackID.StaP, 10) } },
            };*/