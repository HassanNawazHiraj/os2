using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Assignment2
{
    public class ProcessBlock
    {
        private string processName;
        private int startRange;
        private int endRange;
        private int brustTime;
        private int waitingTime;

        public string ProcessName { get => processName; set => processName = value; }
        public int StartRange { get => startRange; set => startRange = value; }
        public int EndRange { get => endRange; set => endRange = value; }
        public int BrustTime { get => brustTime; set => brustTime = value; }
        public int WaitingTime { get => waitingTime; set => waitingTime = value; }

        public ProcessBlock(string processName, int startRange, int endRange, int brustTime, int waitingTime)
        {
            this.processName = processName;
            this.startRange = startRange;
            this.endRange = endRange;
            this.brustTime = brustTime;
            this.waitingTime = waitingTime;
        }
    }
}
