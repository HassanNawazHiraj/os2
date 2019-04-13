using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OS_Assignment2
{
    public partial class Form1 : Form
    {
        List<RoundTrip> RoundTripList = new List<RoundTrip>();
        List<ShortestRemainingTime> shortestRemainingTimeList = new List<ShortestRemainingTime>();
        List<Priority> priorityList = new List<Priority>();
        private BindingSource source = new BindingSource();
        private int timeSlice;
        public string itime;
        public Form1()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by : Hassan Nawaz\nByteremix.com");

        }

        private void AddRowButton_Click(object sender, EventArgs e)
        {
            if (typeCombo.SelectedIndex == 0)
            {
                RoundTripList.Add(new RoundTrip("", 0, 0));
                source.DataSource = RoundTripList;
                source.ResetBindings(false);

            }

            if (typeCombo.SelectedIndex == 1)
            {
                shortestRemainingTimeList.Add(new ShortestRemainingTime("", 0, 0));
                source.DataSource = shortestRemainingTimeList;
                source.ResetBindings(false);

            }

            if (typeCombo.SelectedIndex == 2)
            {
                priorityList.Add(new Priority("", 0, 0,0));
                source.DataSource = priorityList;
                source.ResetBindings(false);

            }
        }

        private void typeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typeCombo.SelectedIndex == 0)
            {
                RoundTripList.Clear();
                //RoundTripList.Add(new RoundTrip("p1", 5, 0));
                //RoundTripList.Add(new RoundTrip("p2", 5, 2));
                //RoundTripList.Add(new RoundTrip("p3", 1, 15));
                source.ResetBindings(false);
                source.DataSource = RoundTripList;
                inputGrid.DataSource = source;

            }

            if (typeCombo.SelectedIndex == 1)
            {
                shortestRemainingTimeList.Clear();
                //shortestRemainingTimeList.Add(new ShortestRemainingTime("p1", 4, 0));
                //shortestRemainingTimeList.Add(new ShortestRemainingTime("p2", 1, 1));
                source.ResetBindings(false);
                source.DataSource = shortestRemainingTimeList;
                inputGrid.DataSource = source;

            }

            if (typeCombo.SelectedIndex == 2)
            {
                priorityList.Clear();
                //priorityList.Add(new Priority("p1", 4, 0,1));
                //priorityList.Add(new Priority("p2", 1, 1,2));
                source.ResetBindings(false);
                source.DataSource = priorityList;
                inputGrid.DataSource = source;

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public void clearOldDataForRoundTrip()
        {
            for (int i = 0; i < RoundTripList.Count; i++)
            {
                RoundTripList[i].lastSliceEndTime = -1;
                RoundTripList[i].totalWaitingtime = 0;
                //RoundTripList[i].remainingBrustTime = null;


            }
        }

        public void clearOldDataForSRT()
        {
            for (int i = 0; i < shortestRemainingTimeList.Count; i++)
            {
                shortestRemainingTimeList[i].lastSliceEndTime = -1;
                shortestRemainingTimeList[i].totalWaitingtime = 0;
                //RoundTripList[i].remainingBrustTime = null;


            }
        }

        public void clearOldDataForP()
        {
            for (int i = 0; i < priorityList.Count; i++)
            {
                priorityList[i].lastSliceEndTime = -1;
                priorityList[i].totalWaitingtime = 0;
                //RoundTripList[i].remainingBrustTime = null;


            }
        }
        private void RunProcessButton_Click(object sender, EventArgs e)
        {

            //check if valid number in time slice
            //try
            //{
            timeSlice = Int32.Parse(TimeSliceText.Text);
            if (timeSlice <= 0)
            {
                MessageBox.Show("invalid time slice");
            }
            else
            {
                //run the program
                if (preRunChecks())
                {
                    List<ProcessBlock> result = new List<ProcessBlock>();
                    //actual processing
                    if (typeCombo.SelectedIndex == 0)
                    {
                        clearOldDataForRoundTrip();
                        //RoundTrip
                        List<RoundTrip> inputList = RoundTripList.OrderBy(a => a.ArrivalTime).ToList();
                        List<RoundTrip> currentProcesses = new List<RoundTrip>();
                        //start range, end range, process state, current tick, current process from list, 
                        //process counter from main list
                        int sr = 0; int er = 0; string p = "idle";
                        int i = 0; int cp = 0; int pc = 0;

                        while (true)
                        {
                            RoundTrip nextProcess;
                            if ((pc < inputList.Count))
                            {
                                nextProcess = inputList[pc];
                            }
                            else
                            {
                                nextProcess = inputList.Last();
                            }
                            //no waiting for process. Just switch if required
                            if (p.Equals("idle"))
                            {
                                //process arrived
                                if (nextProcess.ArrivalTime <= i)
                                {
                                    if (!(sr == 0 && er == 0))
                                    {
                                        //this checks if the we are not at 0,0 meaning the tick is probably at 0 or no idle time.
                                        //log the idle here as well
                                        result.Add(new ProcessBlock("idle", sr, er, er - sr, 0));
                                    }

                                    p = nextProcess.ProcessName;
                                    nextProcess.processTimeSlice.Add(new TimeSlice(i, 0));
                                    nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                    currentProcesses.Add(nextProcess);
                                    pc++;
                                    if (pc < inputList.Count)
                                    {
                                        nextProcess = inputList[pc];
                                    }
                                    sr = i;
                                    er = i;
                                }

                                //if there are other processes that have arrived
                                //pc++;

                                while (nextProcess.ArrivalTime <= i && pc < inputList.Count)
                                {
                                    nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                    currentProcesses.Add(nextProcess);
                                    pc++;
                                    if ((pc < inputList.Count))
                                    {
                                        nextProcess = inputList[pc];
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                            }
                            else
                            {
                                //a process is already in processor
                                //check if its slice is done
                                int expectedEndRange1 = currentProcesses[cp].processTimeSlice.Last().Start + timeSlice;
                                int expectedEndRange2 = currentProcesses[cp].processTimeSlice.Last().Start + currentProcesses[cp].remainingBrustTime;
                                //range1 gets full range. like start 0-3 if time slice is 3
                                //range2 will correct error if brust time is 2 but slice is 3. then expected should be 0-2
                                //if either one of them is true then process has ended
                                if (i == expectedEndRange1 || i == expectedEndRange2)
                                {
                                    //process/time slice has ended
                                    RoundTrip currentProcess = currentProcesses[cp];
                                    currentProcess.processTimeSlice.Last().End = i;
                                    //add it to results
                                    int start = currentProcess.processTimeSlice.Last().Start;
                                    int end = currentProcess.processTimeSlice.Last().End;
                                    int brust = end - start;
                                    int wait;
                                    if (currentProcess.lastSliceEndTime == -1)
                                    {
                                        wait = currentProcess.processTimeSlice.Last().Start - currentProcess.ArrivalTime;
                                        currentProcess.lastSliceEndTime = end;
                                    }
                                    else
                                    {
                                        wait = start - currentProcess.lastSliceEndTime;
                                        currentProcess.lastSliceEndTime = end;
                                    }

                                    result.Add(new ProcessBlock(currentProcess.ProcessName, start,
                                        end, brust, wait));
                                    currentProcess.remainingBrustTime = currentProcess.remainingBrustTime - brust;

                                    if (currentProcess.remainingBrustTime == 0)
                                    {
                                        //remove it from current process queue
                                        currentProcesses.RemoveAt(cp);
                                        //pc++;

                                    }
                                    //has another process arrived?
                                    while (nextProcess.ArrivalTime <= i && pc < inputList.Count)
                                    {
                                        nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                        currentProcesses.Add(nextProcess);
                                        pc++;
                                        if ((pc < inputList.Count))
                                        {
                                            nextProcess = inputList[pc];
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    }
                                    if (currentProcesses.Count != 0)
                                    {
                                        if (cp < currentProcesses.Count - 1)
                                        {
                                            cp++;
                                        }
                                        else
                                        {
                                            cp = 0;
                                        }

                                        p = currentProcesses[cp].ProcessName;
                                        currentProcesses[cp].processTimeSlice.Add(new TimeSlice(i, 0));
                                        sr = i;
                                        er = i;

                                    }
                                    else
                                    {
                                        if (pc >= inputList.Count && currentProcesses.Count == 0) break;
                                        //check if a process has arrived or else set idle
                                        nextProcess = inputList[pc];
                                        if (nextProcess.ArrivalTime <= i)
                                        {
                                            p = nextProcess.ProcessName;
                                            nextProcess.processTimeSlice.Add(new TimeSlice(i, 0));
                                            nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                            currentProcesses.Add(nextProcess);
                                            sr = i;
                                            er = i;
                                        }
                                        else
                                        {
                                            //set idle
                                            p = "idle";
                                            sr = i;
                                            er = i;
                                        }

                                    }


                                    // if (pc == inputList.Count) break;
                                    //nextProcess = inputList[pc];


                                }

                            }
                            er++;
                            i++;
                        }
                    }

                    if (typeCombo.SelectedIndex == 1)
                    {
                        //shortest remaining time first
                        clearOldDataForSRT();
                        List<ShortestRemainingTime> inputList = shortestRemainingTimeList.OrderBy(a => a.ArrivalTime).ThenBy(b => b.BrustTime).ToList();
                        List<ShortestRemainingTime> currentProcesses = new List<ShortestRemainingTime>();

                        //start range, end range, process state, current tick, current process from list, 
                        //process counter from main list
                        int sr = 0; int er = 0; string p = "idle";
                        int i = 0; int cp = 0; int pc = 0;

                        while (true)
                        {
                            ShortestRemainingTime nextProcess;
                            if ((pc < inputList.Count))
                            {
                                nextProcess = inputList[pc];
                            }
                            else
                            {
                                nextProcess = inputList.Last();
                            }

                            if(p.Equals("idle"))
                            {
                                //process arrived
                                if (nextProcess.ArrivalTime <= i)
                                {
                                    if (!(sr == 0 && er == 0))
                                    {
                                        //this checks if the we are not at 0,0 meaning the tick is probably at 0 or no idle time.
                                        //log the idle here as well
                                        result.Add(new ProcessBlock("idle", sr, er, er - sr, 0));
                                    }

                                    p = nextProcess.ProcessName;
                                    nextProcess.processTimeSlice.Add(new TimeSlice(i, 0));
                                    nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                    currentProcesses.Add(nextProcess);
                                    pc++;
                                    if (pc < inputList.Count)
                                    {
                                        nextProcess = inputList[pc];
                                    }
                                    sr = i;
                                    er = i;
                                }

                                //if there are other processes that have arrived
                                //pc++;

                                while (nextProcess.ArrivalTime <= i && pc < inputList.Count)
                                {
                                    nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                    currentProcesses.Add(nextProcess);
                                    pc++;
                                    if ((pc < inputList.Count))
                                    {
                                        nextProcess = inputList[pc];
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                            } else
                            {

                                //a process is already in processor
                                //check if its slice is done
                                int expectedEndRange1 = currentProcesses[cp].processTimeSlice.Last().Start + timeSlice;
                                int expectedEndRange2 = currentProcesses[cp].processTimeSlice.Last().Start + currentProcesses[cp].remainingBrustTime;
                                //range1 gets full range. like start 0-3 if time slice is 3
                                //range2 will correct error if brust time is 2 but slice is 3. then expected should be 0-2
                                //if either one of them is true then process has ended
                                if (i == expectedEndRange1 || i == expectedEndRange2)
                                {
                                    //process/time slice has ended
                                    ShortestRemainingTime currentProcess = currentProcesses[cp];
                                    currentProcess.processTimeSlice.Last().End = i;
                                    //add it to results
                                    int start = currentProcess.processTimeSlice.Last().Start;
                                    int end = currentProcess.processTimeSlice.Last().End;
                                    int brust = end - start;
                                    int wait;
                                    if (currentProcess.lastSliceEndTime == -1)
                                    {
                                        wait = currentProcess.processTimeSlice.Last().Start - currentProcess.ArrivalTime;
                                        currentProcess.lastSliceEndTime = end;
                                    }
                                    else
                                    {
                                        wait = start - currentProcess.lastSliceEndTime;
                                        currentProcess.lastSliceEndTime = end;
                                    }

                                    result.Add(new ProcessBlock(currentProcess.ProcessName, start,
                                        end, brust, wait));
                                    currentProcess.remainingBrustTime = currentProcess.remainingBrustTime - brust;

                                    if (currentProcess.remainingBrustTime == 0)
                                    {
                                        //remove it from current process queue
                                        currentProcesses.RemoveAt(cp);
                                        //pc++;

                                    }
                                    //has another process arrived?
                                    while (nextProcess.ArrivalTime <= i && pc < inputList.Count)
                                    {
                                        nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                        currentProcesses.Add(nextProcess);
                                        pc++;
                                        if ((pc < inputList.Count))
                                        {
                                            nextProcess = inputList[pc];
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    }
                                    if (currentProcesses.Count != 0)
                                    {
                                        //no need for rotation
                                        //if (cp < currentProcesses.Count - 1)
                                        //{
                                        //    cp++;
                                        //}
                                        //else
                                        //{
                                        //    cp = 0;
                                        //}
                                        currentProcesses = currentProcesses.OrderBy(a => a.remainingBrustTime).ToList();
                                        cp = 0;
                                        p = currentProcesses[cp].ProcessName;
                                        currentProcesses[cp].processTimeSlice.Add(new TimeSlice(i, 0));
                                        sr = i;
                                        er = i;

                                    }
                                    else
                                    {
                                        if (pc >= inputList.Count && currentProcesses.Count == 0) break;
                                        //check if a process has arrived or else set idle
                                        nextProcess = inputList[pc];
                                        if (nextProcess.ArrivalTime <= i)
                                        {
                                            p = nextProcess.ProcessName;
                                            nextProcess.processTimeSlice.Add(new TimeSlice(i, 0));
                                            nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                            currentProcesses.Add(nextProcess);
                                            sr = i;
                                            er = i;
                                        }
                                        else
                                        {
                                            //set idle
                                            p = "idle";
                                            sr = i;
                                            er = i;
                                        }

                                    }


                                    // if (pc == inputList.Count) break;
                                    //nextProcess = inputList[pc];


                                }




                            }


                            er++;
                            i++;

                        }
                    }

                    if (typeCombo.SelectedIndex == 2)
                    {
                        //shortest remaining time first
                        clearOldDataForP();
                        List<Priority> inputList = priorityList.OrderBy(a => a.ArrivalTime).ThenBy(b => b.PriorityValue).ToList();
                        List<Priority> currentProcesses = new List<Priority>();

                        //start range, end range, process state, current tick, current process from list, 
                        //process counter from main list
                        int sr = 0; int er = 0; string p = "idle";
                        int i = 0; int cp = 0; int pc = 0;

                        while (true)
                        {
                            Priority nextProcess;
                            if ((pc < inputList.Count))
                            {
                                nextProcess = inputList[pc];
                            }
                            else
                            {
                                nextProcess = inputList.Last();
                            }

                            if (p.Equals("idle"))
                            {
                                //process arrived
                                if (nextProcess.ArrivalTime <= i)
                                {
                                    if (!(sr == 0 && er == 0))
                                    {
                                        //this checks if the we are not at 0,0 meaning the tick is probably at 0 or no idle time.
                                        //log the idle here as well
                                        result.Add(new ProcessBlock("idle", sr, er, er - sr, 0));
                                    }

                                    p = nextProcess.ProcessName;
                                    nextProcess.processTimeSlice.Add(new TimeSlice(i, 0));
                                    nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                    currentProcesses.Add(nextProcess);
                                    pc++;
                                    if (pc < inputList.Count)
                                    {
                                        nextProcess = inputList[pc];
                                    }
                                    sr = i;
                                    er = i;
                                }

                                //if there are other processes that have arrived
                                //pc++;

                                while (nextProcess.ArrivalTime <= i && pc < inputList.Count)
                                {
                                    nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                    currentProcesses.Add(nextProcess);
                                    pc++;
                                    if ((pc < inputList.Count))
                                    {
                                        nextProcess = inputList[pc];
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                            }
                            else
                            {

                                //a process is already in processor
                                //check if its slice is done
                                int expectedEndRange1 = currentProcesses[cp].processTimeSlice.Last().Start + timeSlice;
                                int expectedEndRange2 = currentProcesses[cp].processTimeSlice.Last().Start + currentProcesses[cp].remainingBrustTime;
                                //range1 gets full range. like start 0-3 if time slice is 3
                                //range2 will correct error if brust time is 2 but slice is 3. then expected should be 0-2
                                //if either one of them is true then process has ended
                                if (i == expectedEndRange1 || i == expectedEndRange2)
                                {
                                    //process/time slice has ended
                                    Priority currentProcess = currentProcesses[cp];
                                    currentProcess.processTimeSlice.Last().End = i;
                                    //add it to results
                                    int start = currentProcess.processTimeSlice.Last().Start;
                                    int end = currentProcess.processTimeSlice.Last().End;
                                    int brust = end - start;
                                    int wait;
                                    if (currentProcess.lastSliceEndTime == -1)
                                    {
                                        wait = currentProcess.processTimeSlice.Last().Start - currentProcess.ArrivalTime;
                                        currentProcess.lastSliceEndTime = end;
                                    }
                                    else
                                    {
                                        wait = start - currentProcess.lastSliceEndTime;
                                        currentProcess.lastSliceEndTime = end;
                                    }

                                    result.Add(new ProcessBlock(currentProcess.ProcessName, start,
                                        end, brust, wait));
                                    currentProcess.remainingBrustTime = currentProcess.remainingBrustTime - brust;

                                    if (currentProcess.remainingBrustTime == 0)
                                    {
                                        //remove it from current process queue
                                        currentProcesses.RemoveAt(cp);
                                        //pc++;

                                    }
                                    //has another process arrived?
                                    while (nextProcess.ArrivalTime <= i && pc < inputList.Count)
                                    {
                                        nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                        currentProcesses.Add(nextProcess);
                                        pc++;
                                        if ((pc < inputList.Count))
                                        {
                                            nextProcess = inputList[pc];
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    }
                                    if (currentProcesses.Count != 0)
                                    {
                                        //no need for rotation
                                        //if (cp < currentProcesses.Count - 1)
                                        //{
                                        //    cp++;
                                        //}
                                        //else
                                        //{
                                        //    cp = 0;
                                        //}
                                        currentProcesses = currentProcesses.OrderBy(a => a.PriorityValue).ToList();
                                        cp = 0;
                                        p = currentProcesses[cp].ProcessName;
                                        currentProcesses[cp].processTimeSlice.Add(new TimeSlice(i, 0));
                                        sr = i;
                                        er = i;

                                    }
                                    else
                                    {
                                        if (pc >= inputList.Count && currentProcesses.Count == 0) break;
                                        //check if a process has arrived or else set idle
                                        nextProcess = inputList[pc];
                                        if (nextProcess.ArrivalTime <= i)
                                        {
                                            p = nextProcess.ProcessName;
                                            nextProcess.processTimeSlice.Add(new TimeSlice(i, 0));
                                            nextProcess.remainingBrustTime = nextProcess.BrustTime;
                                            currentProcesses.Add(nextProcess);
                                            sr = i;
                                            er = i;
                                        }
                                        else
                                        {
                                            //set idle
                                            p = "idle";
                                            sr = i;
                                            er = i;
                                        }

                                    }


                                    // if (pc == inputList.Count) break;
                                    //nextProcess = inputList[pc];


                                }




                            }


                            er++;
                            i++;

                        }
                    }


                    resultGrid.DataSource = result;
                    //calculate average waiting time
                    float totalWaitTime = 0;
                    foreach (ProcessBlock r in result)
                    {
                        if (!r.ProcessName.Equals("idle"))
                        {
                            totalWaitTime += r.WaitingTime;
                        }

                    }
                    averageWaitTimeLabel.Text = (totalWaitTime / result.Count) + " ms";
                    itime = "Waiting time\n";
                    if(typeCombo.SelectedIndex == 0)
                    {

                 
                    int total = 0;
                    for (int i = 0; i < RoundTripList.Count; i++)
                    {
                        int count = 0;
                        foreach (ProcessBlock r in result)
                        {
                            if (r.ProcessName.Equals(RoundTripList[i].ProcessName))
                            {
                                count += r.WaitingTime;
                            }
                        }
                        total += count;
                        itime += RoundTripList[i].ProcessName + " : " + count + "\n";
                    }
                    //try
                    //{
                        averageWaitTimeLabel.Text = (float) total / (float) RoundTripList.Count + "";
                        //} catch
                        //{
                        //    averageWaitTimeLabel.Text = "N/A";
                        //}
                    }

                    if (typeCombo.SelectedIndex == 1)
                    {


                        int total = 0;
                        for (int i = 0; i < shortestRemainingTimeList.Count; i++)
                        {
                            int count = 0;
                            foreach (ProcessBlock r in result)
                            {
                                if (r.ProcessName.Equals(shortestRemainingTimeList[i].ProcessName))
                                {
                                    count += r.WaitingTime;
                                }
                            }
                            total += count;
                            itime += shortestRemainingTimeList[i].ProcessName + " : " + count + "\n";
                        }
                        //try
                        //{
                        averageWaitTimeLabel.Text = (float) total / (float) shortestRemainingTimeList.Count + "";
                        //} catch
                        //{
                        //    averageWaitTimeLabel.Text = "N/A";
                        //}
                    }

                    if (typeCombo.SelectedIndex == 2)
                    {


                        int total = 0;
                        for (int i = 0; i < priorityList.Count; i++)
                        {
                            int count = 0;
                            foreach (ProcessBlock r in result)
                            {
                                if (r.ProcessName.Equals(priorityList[i].ProcessName))
                                {
                                    count += r.WaitingTime;
                                }
                            }
                            total += count;
                            itime += priorityList[i].ProcessName + " : " + count + "\n";
                        }
                        //try
                        //{
                        averageWaitTimeLabel.Text = (float) total / (float) priorityList.Count + "";
                        //} catch
                        //{
                        //    averageWaitTimeLabel.Text = "N/A";
                        //}
                    }




                }
            }
            //} catch(Exception ex)
            //{
            //    MessageBox.Show("unknown error\nDetails: "+ex.Message);
            //}

        }

        private bool preRunChecks()
        {
            foreach (RoundTrip r in RoundTripList)
            {
                if (r.BrustTime <= 0)
                {
                    MessageBox.Show("Brust time can't be zero or negative");
                    return false;
                }
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            //source.DataSource = RoundTripList;
            //inputGrid.DataSource = source;
            //source.ResetBindings(false);
            //typeCombo.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(itime);
        }
    }
}
