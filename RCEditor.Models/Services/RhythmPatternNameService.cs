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
        }

        /// <summary>
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
                        0 => "SIDE STICK",
                        1 => "BOSSA",
                        2 => "BRUSH1",
                        3 => "BRUSH2",
                        4 => "CONGA 8BEAT",
                        5 => "CONGA 16BEAT",
                        6 => "CONGA 4BEAT",
                        7 => "CONGA SWING",
                        8 => "CONGA BOSSA",
                        9 => "CAJON1",
                        10 => "CAJON2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "BALLAD":
                    return patternNumber switch
                    {
                        0 => "SHUFFLE2",
                        1 => "SIDE STICK1",
                        2 => "SIDE STICK2", 
                        3 => "SIDE STICK3",
                        4 => "SIDE STICK4",
                        5 => "SHUFFLE1",
                        6 => "8BEAT",
                        7 => "16BEAT1",
                        8 => "16BEAT2",
                        9 => "SWING",
                        10 => "6/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "BLUES":
                    return patternNumber switch
                    {
                        0 => "3BEAT",
                        1 => "12BARS",
                        2 => "SHUFFLE1",
                        3 => "SHUFFLE2",
                        4 => "SWING",
                        5 => "6/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "JAZZ":
                    return patternNumber switch
                    {
                        0 => "JAZZ BLUES",
                        1 => "FAST 4BEAT",
                        2 => "HARD BOP",
                        3 => "BRUSH BOP",
                        4 => "BRUSH SWING",
                        5 => "FAST SWNG",
                        6 => "MED SWING",
                        7 => "SLOW LEGATO",
                        8 => "JAZZ SAMBA",
                        9 => "6/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "FUSION":
                    return patternNumber switch
                    {
                        0 => "16BEAT1",
                        1 => "16BEAT2",
                        2 => "16BEAT3",
                        3 => "16BEAT4",
                        4 => "16BEAT5",
                        5 => "16BEAT6",
                        6 => "16BEAT7",
                        7 => "SWING",
                        8 => "7/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "R&B":
                    return patternNumber switch
                    {
                        0 => "SWING1",
                        1 => "SWING2",
                        2 => "SWING3",
                        3 => "SIDE STICK1",
                        4 => "SIDE STICK2",
                        5 => "SIDE STICK3",
                        6 => "SHUFFLE1",
                        7 => "SHUFFLE2",
                        8 => "8BEAT1",
                        9 => "16BEAT",
                        10 => "7/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "SOUL":
                    return patternNumber switch
                    {
                        0 => "SWING1",
                        1 => "SWING2",
                        2 => "SWING3",
                        3 => "SWING4",
                        4 => "16BEAT1",
                        5 => "16BEAT2",
                        6 => "16BEAT3",
                        7 => "SIDESTK1",
                        8 => "SIDESTK2",
                        9 => "MOTOWN",
                        10 => "PERCUS",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "FUNK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "8BEAT4",
                        4 => "16BEAT1",
                        5 => "16BEAT2",
                        6 => "16BEAT3",
                        7 => "16BEAT4",
                        8 => "SWING1",
                        9 => "SWING2",
                        10 => "SWING3",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "POP":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "16BEAT1",
                        3 => "16BEAT2",
                        4 => "PERCUS1",
                        5 => "SHUFFLE1",
                        6 => "SHUFFLE2",
                        7 => "SIDE STICK1",
                        8 => "SIDE STICK2",
                        9 => "SWING1",
                        10 => "SWING2",
                        11 => "PERCUS2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "SOFT ROCK":
                    return patternNumber switch
                    {
                        0 => "16BEAT1",
                        1 => "16BEAT2",
                        2 => "16BEAT3",
                        3 => "16BEAT4",
                        4 => "8BEAT",
                        5 => "SWING1",
                        6 => "SWING2",
                        7 => "SWING3",
                        8 => "SWING4",
                        9 => "SIDE STICK1",
                        10 => "SIDE STICK2",
                        11 => "PERCUS1",
                        12 => "PERCUS2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "ROCK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "8BEAT4",
                        4 => "8BEAT5",
                        5 => "8BEAT6",
                        6 => "16BEAT1",
                        7 => "16BEAT2",
                        8 => "16BEAT3",
                        9 => "16BEAT4",
                        10 => "SHUFFLE1",
                        11 => "SHUFFLE2",
                        12 => "SWING1",
                        13 => "SWING2",
                        14 => "SWING3",
                        15 => "SWING4",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "ALT ROCK":
                    return patternNumber switch
                    {
                        0 => "RIDEBEAT",
                        1 => "8BEAT1",
                        2 => "8BEAT2",
                        3 => "8BEAT3",
                        4 => "8BEAT4",
                        5 => "16BEAT1",
                        6 => "16BEAT2",
                        7 => "16BEAT3",
                        8 => "16BEAT4",
                        9 => "SWING",
                        10 => "5/4 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "PUNK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "8BEAT4",
                        4 => "8BEAT5",
                        5 => "8BEAT6",
                        6 => "16BEAT1",
                        7 => "16BEAT2",
                        8 => "16BEAT3",
                        9 => "SIDE STICK",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "HEAVY ROCK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "16BEAT1",
                        4 => "16BEAT2",
                        5 => "16BEAT3",
                        6 => "SHUFFLE1",
                        7 => "SHUFFLE2",
                        8 => "SWING1",
                        9 => "SWING2",
                        10 => "SWING3",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "METAL":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "8BEAT4",
                        4 => "8BEAT5",
                        5 => "8BEAT6",
                        6 => "2XBD1",
                        7 => "2XBD2",
                        8 => "2XBD3",
                        9 => "2XBD4",
                        10 => "2XBD5",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "TRAD":
                    return patternNumber switch
                    {
                        0 => "TRAIN2",
                        1 => "ROCKN ROLL",
                        2 => "TRAIN1",
                        3 => "COUNTRY1",
                        4 => "COUNTRY2",
                        5 => "COUNTRY3",
                        6 => "FOXTROT",
                        7 => "TRAD1",
                        8 => "TRAD2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "WORLD":
                    return patternNumber switch
                    {
                        0 => "BOSSA1",
                        1 => "BOSSA2",
                        2 => "SAMBA1",
                        3 => "SAMBA2",
                        4 => "BOOGALOO",
                        5 => "MERENGUE",
                        6 => "REGGAE",
                        7 => "LATIN ROCK1",
                        8 => "LATIN ROCK2",
                        9 => "LATIN PERC",
                        10 => "SURDO",
                        11 => "LATIN1",
                        12 => "LATIN2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "BALLROOM":
                    return patternNumber switch
                    {
                        0 => "CUMBIA",
                        1 => "WALTZ1",
                        2 => "WALTZ2",
                        3 => "CHACHA",
                        4 => "BEGUINE",
                        5 => "RHUMBA",
                        6 => "TANGO1",
                        7 => "TANGO2",
                        8 => "JIVE",
                        9 => "CHARLSTON",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "ELECTRO":
                    return patternNumber switch
                    {
                        0 => "ELCTRO01",
                        1 => "ELCTRO02",
                        2 => "ELCTRO03",
                        3 => "ELCTRO04",
                        4 => "ELCTRO05",
                        5 => "ELCTRO06",
                        6 => "ELCTRO07",
                        7 => "ELCTRO08",
                        8 => "5/4 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "GUIDE":
                    return patternNumber switch
                    {
                        0 => "2/4 TRIPLE",
                        1 => "3/4",
                        2 => "3/4 TRIPLE",
                        3 => "4/4",
                        4 => "4/4 TRIPLE",
                        5 => "BD 8BEAT",
                        6 => "BD 16BEAT",
                        7 => "BD SHUFFLE",
                        8 => "HH 8BEAT",
                        9 => "HH 16BEAT",
                        10 => "HH SWING2",
                        11 => "8BEAT1",
                        12 => "8BEAT2",
                        13 => "8BEAT3",
                        14 => "8BEAT4",
                        15 => "5/4",
                        16 => "5/4 TRIPLE",
                        17 => "6/4",
                        18 => "6/4 TRIPLE",
                        19 => "7/4",
                        20 => "7/4 TRIPLE",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "USER":
                    return patternNumber switch
                    {
                        0 => "SIMPLE BEAT",
                        _ => $"USER {patternNumber+1}"
                    };
                default:
                    return $"Pattern {patternNumber+1}";
            }
        }

        /// <summary>
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
        }
    }
}
