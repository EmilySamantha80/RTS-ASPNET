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

        public static bool ParseRtttl(string str, ref RtttlTone rtttl)
        {
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
                        return false;
                    }
                    var paramName = paramSplit[0].ToLower();
                    var paramValue = paramSplit[1];

                    if (paramName == "o")
                    {
                        rtttl.Defaults.Octave = int.Parse(paramValue);
                    }
                    else if (paramName == "d")
                    {
                        rtttl.Defaults.Duration = int.Parse(paramValue);
                    }
                    else if (paramName == "b")
                    {
                        rtttl.Defaults.BPM = int.Parse(paramValue);
                    }
                    else
                    {
                        return false;
                    }
                }

                string[] nts = sections[2].Replace(" ", "").Split(',');

                Regex noteRegex = new Regex(@"^(\d{1,2})?([a-zA-Z]#?)(\.)?(\d)?$");
                foreach (string s in nts)
                {
                    var note = new Note();
                    var match = noteRegex.Match(s);

                    if (match.Groups.Count < 5 || match.Groups[2].Value == "")
                    {
                        return false;
                    }

                    note.Duration = match.Groups[1].Value == "" ? rtttl.Defaults.Duration : int.Parse(match.Groups[1].Value);
                    note.Pitch = match.Groups[2].Value;
                    note.Dotted = match.Groups[3].Value == "" ? false : true;
                    note.Octave = match.Groups[4].Value == "" ? rtttl.Defaults.Octave : int.Parse(match.Groups[4].Value);
                    rtttl.Notes.Add(note);
                }
            }
            return true;
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

        public static char[] ConvertRtttlToMidi(Rtttl.RtttlTone rtttl, int program)
        {
            var midi = new List<char>();

            var head = Midi.mf_write_header_chunk(0, 1, 384);

            var track_data = new List<char>();
            track_data.AddRange(Midi.copy_right(CopyrightString));
            track_data.AddRange(Midi.track_name("MIDI by MidiGen 0.9"));
            track_data.AddRange(Midi.set_volume(127));
            track_data.AddRange(Midi.mf_write_tempo(rtttl.Defaults.BPM));
            track_data.AddRange(Midi.add_program(program));
            track_data.AddRange(notes2midi(rtttl));
            track_data.AddRange(Midi.end_track());
            //CharArrayDump(track_data.ToArray());

            var track_head = Midi.mf_write_track_chunk(track_data.ToArray());

            var track = new List<char>();
            track.AddRange(track_head);
            track.AddRange(track_data);

            midi.AddRange(head);
            midi.AddRange(track);

            return midi.ToArray();
        }

        private static char[] notes2midi(Rtttl.RtttlTone rtttl)
        {
            var r = new List<char>();

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
            public string Name { get; set; }
            public NoteDefaults Defaults { get; set; }
            public List<Note> Notes { get; set; }

            public RtttlTone()
            {
                Name = "Ringtone";
                Defaults = new NoteDefaults();
                Notes = new List<Note>();
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