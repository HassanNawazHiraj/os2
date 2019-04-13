using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Assignment2
{
    class Priority
    {
        private string processName;
        private int brustTime;
        private int arrivalTime;
        private int priorityValue;
        public List<TimeSlice> processTimeSlice = new List<TimeSlice>();
        public int remainingBrustTime;
        public int lastSliceEndTime = -1;
        public int totalWaitingtime = 0;

        public string ProcessName { get => processName; set => processName = value; }
        public int BrustTime { get => brustTime; set => brustTime = value; }
        public int ArrivalTime { get => arrivalTime; set => arrivalTime = value; }
        public int PriorityValue { get => priorityValue; set => priorityValue = value; }

        public Priority(string processName, int brustTime, int arrivalTime, int priorityValue)
        {
            this.processName = processName;
            this.brustTime = brustTime;
            this.arrivalTime = arrivalTime;
            this.priorityValue = priorityValue;
        }
    }
}
