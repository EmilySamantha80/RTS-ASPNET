using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RTS.T4A
{
    public class Midi
    {

        public static byte eputc(int code)
        {
            return Convert.ToByte(code);
        }

        public static byte[] Write32Bit(int data)
        {
            var r = new byte[4];

            r[0] = eputc((data >> 24) & 0xff);
            r[1] = eputc((data >> 16) & 0xff);
            r[2] = eputc((data >> 8) & 0xff);
            r[3] = eputc((data & 0xff));

            return r;
        }

        public static byte[] Write16Bit(int data)
        {
            var r = new byte[2];

            r[0] = eputc((data & 0xff00) >> 8);
            r[1] = eputc(data & 0xff);

            return r;
        }

        public static byte[] mf_write_header_chunk(int format, int ntracks, int division)
        {
            var r = new List<byte>();

            int ident = 0x4d546864;
            int length = 6;

            r.AddRange(Write32Bit(ident));
            r.AddRange(Write32Bit(length));
            r.AddRange(Write16Bit(format));
            r.AddRange(Write16Bit(ntracks));
            r.AddRange(Write16Bit(division));

            return r.ToArray();
        }

        public static byte[] mf_write_track_chunk(byte[] track)
        {
            var r = new List<byte>();

            int trkhdr = 0x4d54726b;

            r.AddRange(Write32Bit(trkhdr));
            r.AddRange(Write32Bit(track.Length));

            return r.ToArray();
        }

        public static byte[] WriteVarLen(int value)
        {
            var r = new List<byte>();

            int buffer = 0;

            buffer = value & 0x7f;
            while ((value >>= 7) > 0)
            {
                buffer <<= 8;
                buffer |= 0x80;
                buffer += (value & 0x7f);
            }
            while (true)
            {
                r.Add(eputc(buffer & 0xff));

                if ((buffer & 0x80) > 0)
                {
                    buffer >>= 8;
                }
                else
                {
                    return r.ToArray();
                }
            }
        }

        public static byte[] mf_write_tempo(int t)
        {
            var r = new List<byte>();

            int tempo = (int)((double)60000000 / t);

            r.Add(eputc(0));
            r.Add(eputc(0xff));
            r.Add(eputc(0x51));
            r.Add(eputc(3));
            r.Add(eputc((0xff & (tempo >> 16))));
            r.Add(eputc((0xff & (tempo >> 8))));
            r.Add(eputc((0xff & tempo)));

            return r.ToArray();
        }

        public static byte[] mf_write_midi_event(int delta_time, int type, int chan, int[] data)
        {
            //		Console.WriteLine("delta_time:" + delta_time);
            //		Console.WriteLine("type:" + type);
            //		Console.WriteLine("chan:" + chan);

            var r = new List<byte>();

            int c = 0;

            r.AddRange(WriteVarLen(delta_time));

            c = type | chan;

            r.Add(eputc(c));

            foreach (int i in data)
            {
                r.Add(eputc(i));
            }

            return r.ToArray();
        }

        public static int[] data(int p1, int p2)
        {
            var r = new int[2];

            r[0] = p1;
            r[1] = p2;

            return r;
        }

        public static int[] data1(int p1)
        {
            var r = new int[1];

            r[0] = p1;

            return r;
        }

        public static byte[] end_track()
        {
            var r = new List<byte>();

            r.Add(eputc(0));
            r.Add(eputc(0xff));
            r.Add(eputc(0x2f));
            r.Add(eputc(0));

            return r.ToArray();
        }

        public static byte[] add_program(int prg)
        {
            var r = new List<byte>();

            r.AddRange(mf_write_midi_event(0, 0xc0, 0, data1(prg)));

            return r.ToArray();
        }

        public static byte[] write_note(int s, int d, int p)
        {
            var r = new List<byte>();

            r.AddRange(mf_write_midi_event(s, 0x90, 0, data(p, 100)));
            r.AddRange(mf_write_midi_event(d, 0x80, 0, data(p, 0)));

            return r.ToArray();
        }

        public static byte[] copy_right(string str)
        {
            var r = new List<byte>();

            r.Add(eputc(0));
            r.Add(eputc(0xff));
            r.Add(eputc(0x02));
            r.Add(eputc(str.Length));
            r.AddRange(Encoding.ASCII.GetBytes(str));

            return r.ToArray();
        }

        public static byte[] track_name(string str)
        {
            var r = new List<byte>();

            r.Add(eputc(0));
            r.Add(eputc(0xff));
            r.Add(eputc(0x03));
            r.Add(eputc(str.Length));
            r.AddRange(Encoding.ASCII.GetBytes(str));

            return r.ToArray();
        }

        public static byte[] set_volume(int volume)
        {
            //Volume 0-127

            var r = new List<byte>();

            r.AddRange(mf_write_midi_event(0, 0xb0, 0, data(0x07, volume)));

            return r.ToArray();
        }


        public static int dotted(int nt)
        {
            return nt + (nt / 2);
        }


    }

}