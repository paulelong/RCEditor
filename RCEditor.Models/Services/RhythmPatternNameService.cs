using System;
using System.Collections.Generic;
using System.Text;

namespace RCEditor.Models.Services
{
    /// <summary>
    /// Service to provide friendly names for rhythm patterns based on genre
    /// </summary>
    public static class RhythmPatternNameService
    {
        /// <summary>
        /// Gets a friendly name for the rhythm genre based on its numeric value
        /// </summary>
        public static string GetGenreName(int genreValue)
        {
            // Genre names from RC-600 documentation
            return genreValue switch
            {
                0 => "ACOUSTIC",
                1 => "BALLAD",
                2 => "BLUES",
                3 => "JAZZ",
                4 => "FUSION",
                5 => "R&B",
                6 => "SOUL",
                7 => "FUNK",
                8 => "POP",
                9 => "SOFT ROCK",
                10 => "ROCK",
                11 => "ALT ROCK",
                12 => "PUNK",
                13 => "HEAVY ROCK",
                14 => "METAL",
                15 => "TRAD",
                16 => "WORLD",
                17 => "BALLROOM",
                18 => "ELECTRO",
                19 => "GUIDE",
                20 => "USER",
                _ => $"Unknown ({genreValue})"
            };
        }        /// <summary>
        /// Gets a pattern name for a specific genre and pattern number
        /// </summary>
        public static string GetPatternNameByGenre(string genre, int patternNumber)
        {
            // Return the pattern name based on genre and pattern number (0-based)
            switch (genre)
            {
                case "ACOUSTIC":
                    return patternNumber switch
                    {
                        0 => "SIDE STICK - 4/4",
                        1 => "BOSSA - 4/4",
                        2 => "BRUSH1 - 4/4",
                        3 => "BRUSH2 - 4/4",
                        4 => "CONGA 8BEAT - 4/4",
                        5 => "CONGA 16BEAT - 4/4",
                        6 => "CONGA 4BEAT - 4/4",
                        7 => "CONGA SWING - 4/4",
                        8 => "CONGA BOSSA - 4/4",
                        9 => "CAJON1 - 4/4",
                        10 => "CAJON2 - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "BALLAD":
                    return patternNumber switch
                    {
                        0 => "SHUFFLE2 - 3/4",
                        1 => "SIDE STICK1 - 4/4",
                        2 => "SIDE STICK2 - 4/4", 
                        3 => "SIDE STICK3 - 4/4",
                        4 => "SIDE STICK4 - 4/4",
                        5 => "SHUFFLE1 - 4/4",
                        6 => "8BEAT - 4/4",
                        7 => "16BEAT1 - 4/4",
                        8 => "16BEAT2 - 4/4",
                        9 => "SWING - 4/4",
                        10 => "6/8 BEAT - 6/8",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "BLUES":
                    return patternNumber switch
                    {
                        0 => "3BEAT - 3/4",
                        1 => "12BARS - 4/4",
                        2 => "SHUFFLE1 - 4/4",
                        3 => "SHUFFLE2 - 4/4",
                        4 => "SWING - 4/4",
                        5 => "6/8 BEAT - 6/8",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "JAZZ":
                    return patternNumber switch
                    {
                        0 => "JAZZ BLUES - 4/4",
                        1 => "FAST 4BEAT - 4/4",
                        2 => "HARD BOP - 4/4",
                        3 => "BRUSH BOP - 4/4",
                        4 => "BRUSH SWING - 4/4",
                        5 => "FAST SWNG - 4/4",
                        6 => "MED SWING - 4/4",
                        7 => "SLOW LEGATO - 4/4",
                        8 => "JAZZ SAMBA - 4/4",
                        9 => "6/8 BEAT - 6/8",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "FUSION":
                    return patternNumber switch
                    {
                        0 => "16BEAT1 - 4/4",
                        1 => "16BEAT2 - 4/4",
                        2 => "16BEAT3 - 4/4",
                        3 => "16BEAT4 - 4/4",
                        4 => "16BEAT5 - 4/4",
                        5 => "16BEAT6 - 4/4",
                        6 => "16BEAT7 - 4/4",
                        7 => "SWING - 4/4",
                        8 => "7/8 BEAT - 7/8",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "R&B":
                    return patternNumber switch
                    {
                        0 => "SWING1 - 4/4",
                        1 => "SWING2 - 4/4",
                        2 => "SWING3 - 4/4",
                        3 => "SIDE STICK1 - 4/4",
                        4 => "SIDE STICK2 - 4/4",
                        5 => "SIDE STICK3 - 4/4",
                        6 => "SHUFFLE1 - 4/4",
                        7 => "SHUFFLE2 - 4/4",
                        8 => "8BEAT1 - 4/4",
                        9 => "16BEAT - 4/4",
                        10 => "7/8 BEAT - 7/8",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "SOUL":
                    return patternNumber switch
                    {
                        0 => "SWING1 - 4/4",
                        1 => "SWING2 - 4/4",
                        2 => "SWING3 - 4/4",
                        3 => "SWING4 - 4/4",
                        4 => "16BEAT1 - 4/4",
                        5 => "16BEAT2 - 4/4",
                        6 => "16BEAT3 - 4/4",
                        7 => "SIDESTK1 - 4/4",
                        8 => "SIDESTK2 - 4/4",
                        9 => "MOTOWN - 4/4",
                        10 => "PERCUS - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "FUNK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1 - 4/4",
                        1 => "8BEAT2 - 4/4",
                        2 => "8BEAT3 - 4/4",
                        3 => "8BEAT4 - 4/4",
                        4 => "16BEAT1 - 4/4",
                        5 => "16BEAT2 - 4/4",
                        6 => "16BEAT3 - 4/4",
                        7 => "16BEAT4 - 4/4",
                        8 => "SWING1 - 4/4",
                        9 => "SWING2 - 4/4",
                        10 => "SWING3 - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "POP":
                    return patternNumber switch
                    {
                        0 => "8BEAT1 - 4/4",
                        1 => "8BEAT2 - 4/4",
                        2 => "16BEAT1 - 4/4",
                        3 => "16BEAT2 - 4/4",
                        4 => "PERCUS1 - 4/4",
                        5 => "SHUFFLE1 - 4/4",
                        6 => "SHUFFLE2 - 4/4",
                        7 => "SIDE STICK1 - 4/4",
                        8 => "SIDE STICK2 - 4/4",
                        9 => "SWING1 - 4/4",
                        10 => "SWING2 - 4/4",
                        11 => "PERCUS2 - 6/8",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "SOFT ROCK":
                    return patternNumber switch
                    {
                        0 => "16BEAT1 - 4/4",
                        1 => "16BEAT2 - 4/4",
                        2 => "16BEAT3 - 4/4",
                        3 => "16BEAT4 - 4/4",
                        4 => "8BEAT - 4/4",
                        5 => "SWING1 - 4/4",
                        6 => "SWING2 - 4/4",
                        7 => "SWING3 - 4/4",
                        8 => "SWING4 - 4/4",
                        9 => "SIDE STICK1 - 4/4",
                        10 => "SIDE STICK2 - 4/4",
                        11 => "PERCUS1 - 4/4",
                        12 => "PERCUS2 - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "ROCK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1 - 4/4",
                        1 => "8BEAT2 - 4/4",
                        2 => "8BEAT3 - 4/4",
                        3 => "8BEAT4 - 4/4",
                        4 => "8BEAT5 - 4/4",
                        5 => "8BEAT6 - 4/4",
                        6 => "16BEAT1 - 4/4",
                        7 => "16BEAT2 - 4/4",
                        8 => "16BEAT3 - 4/4",
                        9 => "16BEAT4 - 4/4",
                        10 => "SHUFFLE1 - 4/4",
                        11 => "SHUFFLE2 - 4/4",
                        12 => "SWING1 - 4/4",
                        13 => "SWING2 - 4/4",
                        14 => "SWING3 - 4/4",
                        15 => "SWING4 - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "ALT ROCK":
                    return patternNumber switch
                    {
                        0 => "RIDEBEAT - 4/4",
                        1 => "8BEAT1 - 4/4",
                        2 => "8BEAT2 - 4/4",
                        3 => "8BEAT3 - 4/4",
                        4 => "8BEAT4 - 4/4",
                        5 => "16BEAT1 - 4/4",
                        6 => "16BEAT2 - 4/4",
                        7 => "16BEAT3 - 4/4",
                        8 => "16BEAT4 - 4/4",
                        9 => "SWING - 4/4",
                        10 => "5/4 BEAT - 5/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "PUNK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1 - 4/4",
                        1 => "8BEAT2 - 4/4",
                        2 => "8BEAT3 - 4/4",
                        3 => "8BEAT4 - 4/4",
                        4 => "8BEAT5 - 4/4",
                        5 => "8BEAT6 - 4/4",
                        6 => "16BEAT1 - 4/4",
                        7 => "16BEAT2 - 4/4",
                        8 => "16BEAT3 - 4/4",
                        9 => "SIDE STICK - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "HEAVY ROCK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1 - 4/4",
                        1 => "8BEAT2 - 4/4",
                        2 => "8BEAT3 - 4/4",
                        3 => "16BEAT1 - 4/4",
                        4 => "16BEAT2 - 4/4",
                        5 => "16BEAT3 - 4/4",
                        6 => "SHUFFLE1 - 4/4",
                        7 => "SHUFFLE2 - 4/4",
                        8 => "SWING1 - 4/4",
                        9 => "SWING2 - 4/4",
                        10 => "SWING3 - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "METAL":
                    return patternNumber switch
                    {
                        0 => "8BEAT1 - 4/4",
                        1 => "8BEAT2 - 4/4",
                        2 => "8BEAT3 - 4/4",
                        3 => "8BEAT4 - 4/4",
                        4 => "8BEAT5 - 4/4",
                        5 => "8BEAT6 - 4/4",
                        6 => "2XBD1 - 4/4",
                        7 => "2XBD2 - 4/4",
                        8 => "2XBD3 - 4/4",
                        9 => "2XBD4 - 4/4",
                        10 => "2XBD5 - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "TRAD":
                    return patternNumber switch
                    {
                        0 => "TRAIN2 - 2/4",
                        1 => "ROCKN ROLL - 4/4",
                        2 => "TRAIN1 - 4/4",
                        3 => "COUNTRY1 - 4/4",
                        4 => "COUNTRY2 - 4/4",
                        5 => "COUNTRY3 - 4/4",
                        6 => "FOXTROT - 4/4",
                        7 => "TRAD1 - 4/4",
                        8 => "TRAD2 - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "WORLD":
                    return patternNumber switch
                    {
                        0 => "BOSSA1 - 4/4",
                        1 => "BOSSA2 - 4/4",
                        2 => "SAMBA1 - 4/4",
                        3 => "SAMBA2 - 4/4",
                        4 => "BOOGALOO - 4/4",
                        5 => "MERENGUE - 4/4",
                        6 => "REGGAE - 4/4",
                        7 => "LATIN ROCK1 - 4/4",
                        8 => "LATIN ROCK2 - 4/4",
                        9 => "LATIN PERC - 4/4",
                        10 => "SURDO - 4/4",
                        11 => "LATIN1 - 4/4",
                        12 => "LATIN2 - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "BALLROOM":
                    return patternNumber switch
                    {
                        0 => "CUMBIA - 2/4",
                        1 => "WALTZ1 - 3/4",
                        2 => "WALTZ2 - 3/4",
                        3 => "CHACHA - 4/4",
                        4 => "BEGUINE - 4/4",
                        5 => "RHUMBA - 4/4",
                        6 => "TANGO1 - 4/4",
                        7 => "TANGO2 - 4/4",
                        8 => "JIVE - 4/4",
                        9 => "CHARLSTON - 4/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "ELECTRO":
                    return patternNumber switch
                    {
                        0 => "ELCTRO01 - 4/4",
                        1 => "ELCTRO02 - 4/4",
                        2 => "ELCTRO03 - 4/4",
                        3 => "ELCTRO04 - 4/4",
                        4 => "ELCTRO05 - 4/4",
                        5 => "ELCTRO06 - 4/4",
                        6 => "ELCTRO07 - 4/4",
                        7 => "ELCTRO08 - 4/4",
                        8 => "5/4 BEAT - 5/4",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "GUIDE":
                    return patternNumber switch
                    {
                        0 => "2/4 TRIPLE - 2/4",
                        1 => "3/4 - 3/4",
                        2 => "3/4 TRIPLE - 3/4",
                        3 => "4/4 - 4/4",
                        4 => "4/4 TRIPLE - 4/4",
                        5 => "BD 8BEAT - 4/4",
                        6 => "BD 16BEAT - 4/4",
                        7 => "BD SHUFFLE - 4/4",
                        8 => "HH 8BEAT - 4/4",
                        9 => "HH 16BEAT - 4/4",
                        10 => "HH SWING1 - 4/4",
                        11 => "HH SWING2 - 4/4",
                        12 => "8BEAT1 - 4/4",
                        13 => "8BEAT2 - 4/4",
                        14 => "8BEAT3 - 4/4",
                        15 => "8BEAT4 - 4/4",
                        16 => "5/4 - 5/4",
                        17 => "5/4 TRIPLE - 5/4",
                        18 => "6/4 - 6/4",
                        19 => "6/4 TRIPLE - 6/4",
                        20 => "7/4 - 7/4",
                        21 => "7/4 TRIPLE - 7/4",
                        22 => "5/8 - 5/8",
                        23 => "6/8 - 6/8",
                        24 => "7/8 - 7/8",
                        25 => "8/8 - 8/8",
                        26 => "9/8 - 9/8",
                        27 => "10/8 - 10/8",
                        28 => "11/8 - 11/8",
                        29 => "12/8 - 12/8",
                        30 => "13/8 - 13/8",
                        31 => "14/8 - 14/8",
                        32 => "15/8 - 15/8",
                        _ => $"Pattern {patternNumber+1}"
                    };                case "USER":
                    return patternNumber switch
                    {
                        0 => "SIMPLE BEAT - 4/4",
                        _ => $"USER {patternNumber+1}"
                    };
                default:
                    return $"Pattern {patternNumber+1}";
            }
        }        /// <summary>
        /// Gets a description for the specified variation
        /// </summary>
        public static string GetVariationDescription(char variation)
        {
            // Variations are generally similar across genres (A is basic, B-D are more complex)
            return variation switch
            {
                'A' => "Basic",
                'B' => "Variation with fills",
                'C' => "More complex variation",
                'D' => "Most complex variation",
                _ => $"Variation {variation}"
            };
        }        /// <summary>
        /// Gets a friendly name for the rhythm kit based on its value
        /// </summary>
        public static string GetKitName(string kit)
        {
            // If the kit is already a friendly name or empty, just return it
            if (string.IsNullOrEmpty(kit))
                return "Unknown";
            
            // Try to parse the kit as a number
            if (int.TryParse(kit, out int kitValue))
            {
                // Kit names from RC-600 documentation
                return kitValue switch
                {
                    0 => "STUDIO",
                    1 => "LIVE",
                    2 => "LIGHT",
                    3 => "HEAVY",
                    4 => "ROCK",
                    5 => "METAL",
                    6 => "JAZZ",
                    7 => "BRUSH",
                    8 => "CAJON",
                    9 => "808",
                    10 => "909",
                    11 => "808+909",
                    _ => $"Unknown ({kitValue})"
                };
            }
            
            // If it couldn't be parsed as a number, it might already be a name
            return kit;
        }
    }
}
