using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RTS.MIDI
{
    public class RTTTL
    {
        public class RtttlNote
        {
            public int? Duration = null;
            public string Note = String.Empty;
            public int? Octave = null;
        }

        private RtttlNote ParseRtttlNote(string note)
        {
            RtttlNote result = new RtttlNote();
            Regex regex = new Regex(@"(?<Duration>\d+)?(?<Note>\w)?(?<Sharp>#)?(?<Octave>\d)?");
            Match match = regex.Match(note);
            if (match.Success)
            {
                if (match.Groups[1].Value != "")
                {
                    result.Duration = int.Parse(match.Groups[1].Value);
                }
                if (match.Groups[2].Value != "")
                {
                    result.Note = match.Groups[2].Value.ToLower();
                }
                if (match.Groups[3].Value != "")
                {
                    result.Note += match.Groups[3].Value;
                }
                if (match.Groups[4].Value != "")
                {
                    result.Octave = int.Parse(match.Groups[4].Value);
                }
            }
            return result;
        }

        private int ParseRtttlNoteToMidiInt(RtttlNote note)
        {
            int result = 0;

            switch (note.Note)
            {
                case "c":
                    result = 0;
                    break;
                case "c#":
                    result = 1;
                    break;
                case "d":
                    result = 2;
                    break;
                case "d#":
                    result = 3;
                    break;
                case "e":
                    result = 4;
                    break;
                case "f":
                    result = 5;
                    break;
                case "f#":
                    result = 6;
                    break;
                case "g":
                    result = 7;
                    break;
                case "g#":
                    result = 8;
                    break;
                case "a":
                    result = 9;
                    break;
                case "a#":
                    result = 10;
                    break;
                case "b":
                    result = 11;
                    break;
                case "p":
                    result = -1;
                    break;
            }
            if (result != -1)
            {
                result = result + (((int)note.Octave) * 12);
            }

            return result;
        }

        private int ParseRtttlNoteToMidDuration(RtttlNote note)
        {
            int result = 0;
            switch (note.Duration)
            {
                case 32:
                    result = 2;
                    break;
                case 16:
                    result = 4;
                    break;
                case 8:
                    result = 8;
                    break;
                case 4:
                    result = 16;
                    break;
                case 2:
                    result = 32;
                    break;
                case 1:
                    result = 64;
                    break;
                default:
                    result = (int)note.Duration;
                    break;
            }


            return result;

        }

        public byte[] ParseRtttl(string rtttl)
        {
            int defaultDuration = 0;
            int defaultOctave = 0;
            int defaultBeat = 0;
            var song = new MIDISong();
            //Split the sections of the RTTTL file
            string[] sections = rtttl.Split(':');
            //Add the title of the song
            song.AddTrack(sections[0]);
            song.SetTimeSignature(0, 4, 4);
            //Split the track defaults
            string[] defaults = sections[1].Split(',');
            //Go through each of the track defaults and assign the values
            foreach (string s in defaults)
            {
                string[] value = s.Split('=');
                if (value[0] == "d")
                {
                    defaultDuration = int.Parse(value[1]);
                }
                else if (value[0] == "o")
                {
                    defaultOctave = int.Parse(value[1]);
                }
                else if (value[0] == "b")
                {
                    defaultBeat = int.Parse(value[1]);
                }
            }
            song.SetTempo(0, defaultBeat);
            //Parse each note in the song
            foreach (string note in sections[2].Split(','))
            {
                RtttlNote result = ParseRtttlNote(note);
                if (result.Duration == null)
                {
                    result.Duration = defaultDuration;
                }
                if (result.Octave == null)
                {
                    result.Octave = defaultOctave;
                }
                int midiNote = ParseRtttlNoteToMidiInt(result);
                int midiDuration = ParseRtttlNoteToMidDuration(result);

                song.AddNote(0, 0, midiNote, midiDuration);

            }
            song.AddNote(0, 0, -1, 8);

            //Save and play the song
            try
            {
                MemoryStream ms = new MemoryStream();
                song.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] src = ms.GetBuffer();
                byte[] dst = new byte[src.Length];
                for (int i = 0; i < src.Length; i++)
                {
                    dst[i] = src[i];
                }
                ms.Close();

                return dst;

            }
            catch { }

            return new byte[0];
        }
    }
}