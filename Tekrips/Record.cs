using Kriptok.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekrips
{
    public class Record
    {
        public int Points;

        public int Lines;
    }

    public static class Records
    {
        public static Record Load()
        {
            try
            {
                return Memory<Record>.Load();
            }
            catch (Exception)
            {
                return new Record();
            }
        }

        public static void Save(int lines, int points)
        {
            Memory<Record>.Current.Lines = Math.Max(Memory<Record>.Current.Lines, lines);
            Memory<Record>.Current.Points = Math.Max(Memory<Record>.Current.Points, points);
            Memory<Record>.Save();
        }
    }
}
