using System;
using System.Collections.Generic;
using System.Text;

namespace MapVote
{
    internal class DebugManager
    {
        public static void InitializeDebug()
        {
            PopulateMockData();
        }

        private static void PopulateMockData()
        {
            // Simulate master client picking 3 random maps
            MapVote.SelectedMaps = new System.Collections.Generic.List<string>
            {
                "Level - Arctic",
                "Level - Manor",
                "Level - Wizard"
            };

            MapVote.CurrentVotes[10] = "Level - Arctic";
            MapVote.CurrentVotes[11] = "Level - Manor";
            MapVote.CurrentVotes[12] = "Level - Wizard";
            MapVote.CurrentVotes[13] = "Level - Wizard";
            MapVote.CurrentVotes[14] = "Level - Wizard";
            MapVote.CurrentVotes[15] = MapVote.VOTE_RANDOM_LABEL;
        }
    }
}
