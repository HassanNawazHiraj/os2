using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Assignment2
{
    public class TimeSlice
    {
        private int start;
        private int end;


        public int Start { get => start; set => start = value; }
        public int End { get => end; set => end = value; }


        public TimeSlice(int start, int end)
        {
            this.Start = start;
            this.End = end;
        }
    }
}
