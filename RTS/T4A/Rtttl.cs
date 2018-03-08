using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RTS.T4A
{
    public class Rtttl
    {
        public static string CopyrightString = Properties.Settings.Default.MidiCopyright;
        public static string ErrorRtttl = Properties.Settings.Default.MidiErrorTone;

        public static RtttlTone ParseRtttl(string str)
        {
            string invalidDefaultMessage = "Invalid control pair: ";
            var rtttl = new RtttlTone();
            string[] sections = str.Split(':');

            if (sections.Length == 3)
            {
                rtttl.Name = sections[0];

                string[] defaults = sections[1].Split(',');

                foreach (var param in defaults)
                {
                    string[] paramSplit = param.Split('=');
                    if (paramSplit.Length < 2)
                    {
                        rtttl.HasParseError = true;
                        rtttl.ParseErrorMessage = invalidDefaultMessage + param;
                        return rtttl;
                    }
                    var paramName = paramSplit[0].ToLower();
                    var paramValue = paramSplit[1];

                    if (paramName == "o")
                    {
                        try
                        {
                            rtttl.Defaults.Octave = int.Parse(paramValue);
                        }
                        catch
                        {
                            rtttl.HasParseError = true;
                            rtttl.ParseErrorMessage = invalidDefaultMessage + param;
                            return rtttl;
                        }
                        if (!ValidateNoteOctave(rtttl.Defaults.Octave))
                        {
                            rtttl.HasParseError = true;
                            rtttl.ParseErrorMessage = invalidDefaultMessage + param;
                            return rtttl;
                        }
                    }
                    else if (paramName == "d")
                    {
                        try
                        {
                            rtttl.Defaults.Duration = int.Parse(paramValue);
                        }
                        catch
                        {
                            rtttl.HasParseError = true;
                            rtttl.ParseErrorMessage = invalidDefaultMessage + param;
                            return rtttl;
                        }
                        if (!ValidateNoteDuration(rtttl.Defaults.Duration))
                        {
                            rtttl.HasParseError = true;
                            rtttl.ParseErrorMessage = invalidDefaultMessage + param;
                            return rtttl;
                        }
                    }
                    else if (paramName == "b")
                    {
                        try
                        {
                            rtttl.Defaults.BPM = int.Parse(paramValue);
                        }
                        catch
                        {
                            rtttl.HasParseError = true;
                            rtttl.ParseErrorMessage = invalidDefaultMessage + param;
                            return rtttl;
                        }
                    }
                    else
                    {
                        rtttl.HasParseError = true;
                        rtttl.ParseErrorMessage = invalidDefaultMessage + param;
                        return rtttl;
                    }
                }

                string[] nts = sections[2].Replace(" ", "").Split(',');

                Regex noteRegex = new Regex(@"^(\d{1,2})?([a-zA-Z]#?)(\.)?(\d)?$");
                foreach (string s in nts)
                {
                    //Ignore empty notes, including a trailing comma at the end of the RTTTL
                    if (String.IsNullOrWhiteSpace(s))
                        continue;

                    var note = new Note();
                    var match = noteRegex.Match(s);

                    if (match.Groups.Count < 5 || match.Groups[2].Value == "")
                    {
                        rtttl.HasParseError = true;
                        rtttl.ParseErrorMessage = "Invalid note format: " + s;
                        return rtttl;
                    }

                    // Parse duration
                    try
                    {
                        note.Duration = match.Groups[1].Value == "" ? rtttl.Defaults.Duration : int.Parse(match.Groups[1].Value);
                    }
                    catch
                    {
                        rtttl.HasParseError = true;
                        rtttl.ParseErrorMessage = "Invalid note duration: " + s;
                        return rtttl;
                    }
                    if (!ValidateNoteDuration(note.Duration))
                    {
                        rtttl.HasParseError = true;
                        rtttl.ParseErrorMessage = "Invalid note duration: " + s;
                        return rtttl;
                    }

                    // Parse pitch
                    note.Pitch = match.Groups[2].Value;
                    if (!ValidateNotePitch(note.Pitch))
                    {
                        rtttl.HasParseError = true;
                        rtttl.ParseErrorMessage = "Invalid note pitch: " + s;
                        return rtttl;
                    }

                    // Parse dotted. Doesn't need validation
                    note.Dotted = match.Groups[3].Value == "" ? false : true;

                    // Parse octave
                    try
                    {
                        note.Octave = match.Groups[4].Value == "" ? rtttl.Defaults.Octave : int.Parse(match.Groups[4].Value);
                    }
                    catch
                    {
                        rtttl.HasParseError = true;
                        rtttl.ParseErrorMessage = "Invalid note octave: " + s;
                    }
                    if (!ValidateNoteOctave(note.Octave))
                    {
                        rtttl.HasParseError = true;
                        rtttl.ParseErrorMessage = "Invalid note octave: " + s;
                        return rtttl;
                    }
                    rtttl.Notes.Add(note);
                }
            }
            return rtttl;
        }

        private static bool ValidateNoteDotted(string dotted)
        {
            bool result = false;
            if (dotted == "" || dotted == ".")
                result = true;

            return result;
        }

        private static bool ValidateNoteDuration(int duration)
        {
            bool result = false;
            if (duration == 1 || duration == 2 || duration == 4 || duration == 8 || duration == 16 || duration == 32)
                result = true;

            return result;
        }

        private static bool ValidateNotePitch(string note)
        {
            note = note.ToUpperInvariant();
            bool result = false;
            if (note == "P" || note == "C" || note == "C#" || note == "D" || note == "D#" || note == "E" || note == "F" || note == "F#"
                || note == "G" || note == "G#" || note == "A" || note == "A#" || note == "B")
                result = true;

            return result;
        }

        private static bool ValidateNoteOctave(int scale)
        {
            bool result = false;
            if (scale >= 4 && scale <= 7)
                result = true;

            return result;
        }

        private static bool IsDotted(ref string nt)
        {
            bool r = false;
            char[] ntChars = nt.ToCharArray();
            for (int a = 0; a < ntChars.Length; a++)
            {
                if (ntChars[a] == '.')
                {
                    ntChars[a] = ' ';
                    r = true;
                    break;
                }
            }
            nt = new string(ntChars).Replace(" ", "");
            return r;
        }

        public static byte[] ConvertRtttlToMidi(Rtttl.RtttlTone rtttl, int program)
        {
            var midi = new List<byte>();

            var head = Midi.mf_write_header_chunk(0, 1, 384);

            var track_data = new List<byte>();
            track_data.AddRange(Midi.copy_right(CopyrightString));
            track_data.AddRange(Midi.track_name("MIDI by MidiGen 0.9"));
            track_data.AddRange(Midi.set_volume(127));
            track_data.AddRange(Midi.mf_write_tempo(rtttl.Defaults.BPM));
            track_data.AddRange(Midi.add_program(program));
            track_data.AddRange(notes2midi(rtttl));
            track_data.AddRange(Midi.end_track());
            //CharArrayDump(track_data.ToArray());

            var track_head = Midi.mf_write_track_chunk(track_data.ToArray());

            var track = new List<byte>();
            track.AddRange(track_head);
            track.AddRange(track_data);

            midi.AddRange(head);
            midi.AddRange(track);

            return midi.ToArray();
        }

        private static byte[] notes2midi(Rtttl.RtttlTone rtttl)
        {
            var r = new List<byte>();

            int rest = 0;

            foreach (var note in rtttl.Notes)
            {
                var pt = get_pitch(note.Pitch, note.Octave - 1);
                var tm = get_time(note.Duration, note.Dotted);
                if (pt == -1)
                {
                    rest = rest + tm;
                }
                else
                {
                    r.AddRange(Midi.write_note(rest, tm, pt));
                    rest = 0;
                }
            }

            return r.ToArray();
        }

        private static int get_pitch(string nt, int oc)
        {
            int r = 0;

            nt = nt.ToLower();
            if (nt == "p")
            {
                r = -1;
            }
            else
            {
                switch (nt)
                {
                    case "c": r = 0; break;
                    case "c#": r = 1; break;
                    case "d": r = 2; break;
                    case "d#": r = 3; break;
                    case "e": r = 4; break;
                    case "f": r = 5; break;
                    case "f#": r = 6; break;
                    case "g": r = 7; break;
                    case "g#": r = 8; break;
                    case "a": r = 9; break;
                    case "a#": r = 10; break;
                    case "b": r = 11; break;
                }
                r = 12 + (12 * oc) + r;
            }

            return r;
        }

        private static int get_time(int t, bool isDotted)
        {
            int r = 0;
            switch (t)
            {
                case 1: r = 1536; break;
                case 2: r = 768; break;
                case 4: r = 384; break;
                case 8: r = 192; break;
                case 16: r = 96; break;
                case 32: r = 48; break;
                case 64: r = 24; break;
            }

            if (isDotted)
            {
                r = r + (r / 2);
            }

            return r;
        }

        public class RtttlTone
        {
            public bool HasParseError { get; set; }
            public string ParseErrorMessage { get; set; }
            public string Name { get; set; }
            public NoteDefaults Defaults { get; set; }
            public List<Note> Notes { get; set; }

            public RtttlTone()
            {
                Name = "Ringtone";
                Defaults = new NoteDefaults();
                Notes = new List<Note>();
                HasParseError = false;
                ParseErrorMessage = String.Empty;
            }
        }

        public class NoteDefaults
        {
            public int Duration { get; set; }
            public int Octave { get; set; }
            public int BPM { get; set; }

            public NoteDefaults()
            {
                Duration = 4;
                Octave = 6;
                BPM = 63;
            }
        }

        public class Note
        {
            public int Duration { get; set; }
            public string Pitch { get; set; }
            public bool Dotted { get; set; }
            public int Octave { get; set; }
        }

    }
}