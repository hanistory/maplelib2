using System;

namespace MapleLib.WzLib.WzStructure.Data
{

    public static class Tables
    {
        public static string[] PortalTypeNames = new string[] { 
            "Start Point",
            "Invisible",
            "Visible",
            "Collision",
            "Changable",
            "Changable Invisible",
            "Town Portal", 
            "Script",
            "Script Invisible",
            "Script Collision",
            "Hidden",
            "Script Hidden",
            "Vertical Spring",
            "Custom Impact Spring",
            "Unknown (PCIG)" };

        public static string[] BackgroundTypeNames = new string[] {
            "Regular",
            "Horizontal Copies",
            "Vertical Copies",
            "H+V Copies",
            "Horizontal Moving+Copies",
            "Vertical Moving+Copies",
            "H+V Copies, Horizontal Moving",
            "H+V Copies, Vertical Moving"
        };
    }

    public enum QuestState
    {
        Available = 0,
        InProgress = 1,
        Completed = 2
    }
    
}