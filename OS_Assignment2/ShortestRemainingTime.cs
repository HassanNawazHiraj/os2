using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Assignment2
{
    class ShortestRemainingTime
    {

        private string processName;
        private int brustTime;
        private int arrivalTime;
        public List<TimeSlice> processTimeSlice = new List<TimeSlice>();
        public int remainingBrustTime;
        public int lastSliceEndTime = -1;
        public int totalWaitingtime = 0;

        public string ProcessName { get => processName; set => processName = value; }
        public int BrustTime { get => brustTime; set => brustTime = value; }
        public int ArrivalTime { get => arrivalTime; set => arrivalTime = value; }

        public ShortestRemainingTime(string processName, int brustTime, int arrivalTime)
        {
            this.processName = processName;
            this.brustTime = brustTime;
            this.arrivalTime = arrivalTime;
        }
    }
}
