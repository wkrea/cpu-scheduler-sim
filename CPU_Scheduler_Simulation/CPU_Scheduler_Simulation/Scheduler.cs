﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combinatorics.Collections;

namespace CPU_Scheduler_Simulation
{
    public class Scheduler
    {
        // this simulation will be using two processors
        public List<CPU> cpus = new List<CPU>();
        public List<PCB> finishedProcesses = new List<PCB>();
        public IList<int> switchAlgs;
        public int numCPUs;
        public int q1; // quantum 1 (rr)
        public int q2; // quantum 2 (rr)

        public Scheduler() { }  // default contructor

        public Scheduler(int quantum1, int quantum2) {
            q1 = quantum1;
            q2 = quantum2;
        }

        public void setQuantums(int quantum1, int quantum2) {
            q1 = quantum1;
            q2 = quantum2;
        }

        //for each idle processor, select thread from ready queue
        //version: first come first served to whichever processor is idle
        public void loadBalancing(List<PCB> processes, IList<int> v)
        {
            switchAlgs = v;
            addCPUs();
            var numProcesses = processes.Count;     // number of processors
            // assigning each half of the number of processors to each processor
            int counter = 0;
            for (int i = 0; i < numProcesses; i++)
            {
                cpus[counter].waitingCPU.Enqueue(processes[i]);
                counter = (counter + 1) % numCPUs;
            }

            runCPUs(); // runs the algorthms on processes on each CPU
        }

        public void addCPUs()
        {
            for (int i = 0; i < numCPUs; i++)
            {
                cpus.Add(new CPU());
            }
        }

        public int calcTotalTime() // returns the total time spent across both CPUs
        {
            int sum = 0;
            for (int i = 0; i < numCPUs; i++)
            {
                sum += cpus[i].algorithms.timeCounter;
            }
            
            return sum;
        }

        public void runCPUs()
        {
            for (int i = 0; i < cpus.Count; i++)
            {
                Console.WriteLine("~~~~ BEGIN CPU "+(i+1)+" ~~~~");
                runCPU(cpus[i]);
                Console.WriteLine("~~~~ END CPU "+(i+1)+" ~~~~\n");
            }

            foreach (var c in cpus)
            {
                foreach (var p in c.algorithms.finishedProcesses)
                {
                    finishedProcesses.Add(p);
                }
            }
        }

        public List<PCB> resetTempCounters(List<PCB> processes)
        {
            foreach (var p in processes)
            {
                p.resetTempCounters();
            }

            return processes;
        }

        public void runCPU(CPU currCPU)
        {
            int index = 0;
            int switchAlg = switchAlgs[index]; // starting algorithm
            bool CPUburst = true;
            List<PCB> nonEmptyProcesses = new List<PCB>(); // TEST
            do
            {
                nonEmptyProcesses = resetTempCounters(nonEmptyProcesses);
                nonEmptyProcesses = nonEmptyProcesses.OrderBy(p => p.arrivalTime).ToList();
                if (CPUburst && currCPU.waitingCPU.Count != 0) // if it's time to process the CPU bursts of processes
                {
                    Console.WriteLine("~IN CPU BURST");
                    switch (switchAlg)
                    {

                        //case 0: nonEmptyProcesses = currCPU.algorithms.fcfs(currCPU.waitingCPU, CPUburst); break;
                        //case 1: nonEmptyProcesses = currCPU.algorithms.spn(currCPU.waitingCPU, CPUburst); break; // uncomment when done
                        //case 2: nonEmptyProcesses = currCPU.algorithms.srt(currCPU.waitingCPU); break; // uncomment when done
                        case 3: nonEmptyProcesses = currCPU.algorithms.hrrn(currCPU.waitingCPU,  CPUburst); break; // uncomment when done
                        //case 4: nonEmptyProcesses = currCPU.algorithms.rr(currCPU.waitingCPU, q1); break;
                        //case 5: nonEmptyProcesses = currCPU.algorithms.rr(currCPU.waitingCPU, q2); break;
                        //case 6: nonEmptyProcesses = currCPU.algorithms.priority(currCPU.waitingCPU, CPUburst); break; // uncomment when done
                        //case 7: nonEmptyProcesses = currCPU.algorithms.v1Feedback(currCPU.waitingCPU, CPUburst); break; // uncomment when done
                       // case 8: nonEmptyProcesses = currCPU.algorithms.v2Feedback(currCPU.waitingCPU, CPUburst); break; // uncomment when done

                        default: Console.WriteLine("Algorithm at index " + switchAlg + " does not exist (yet); Skipping algorithm...\n");
                                    switchAlg = (switchAlg + 1) % 9;
                                    continue; // will ignore rest of do/while and go through again with updated value for switch statement
                    }
                    currCPU.waitingIO.Clear();
                    nonEmptyProcesses = nonEmptyProcesses.OrderBy(p => p.arrivalTime).ToList();
                    for (int i = 0; i < nonEmptyProcesses.Count; i++)
                    {
                        currCPU.waitingIO.Enqueue(nonEmptyProcesses[i]);
                    }
                    switchAlg = switchAlgs[++index];
                    //Console.ReadKey();
                }
                else if (currCPU.waitingIO.Count != 0)// if it's time to process the IO bursts of processes
                {
                    Console.WriteLine("~IN I/O BURST");
                    nonEmptyProcesses = currCPU.algorithms.fcfs(currCPU.waitingIO, CPUburst); // will always run FCFS in IO burst
                    currCPU.waitingCPU.Clear();
                    for (int i = 0; i < nonEmptyProcesses.Count; i++)
                    {
                            currCPU.waitingCPU.Enqueue(nonEmptyProcesses[i]);
                    }
                }
                CPUburst = !CPUburst;
            } while ((currCPU.waitingIO.Count != 0 && !CPUburst) || (currCPU.waitingCPU.Count != 0 && CPUburst)); 
        }
    }
}
