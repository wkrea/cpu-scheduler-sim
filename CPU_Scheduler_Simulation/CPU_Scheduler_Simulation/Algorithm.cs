﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Algorithm
    {
        public int contextSwitchCost;
        //public int counter;           //if we wish to implement a counter age solution
        //age solution - when switching from readyIO to waitingCPU, organize the queue to put the oldest processes first

        public Algorithm() { }

        //TODO: beingAlgorithms(); 

        //first-come-first-serve algorithm - Hannh
        public List<PCB> fcfs(Queue<PCB> processes) 
        {
            //foreach (var process in processes)
            //    processes.Dequeue();
            return null; 
        }

        //shortest-process-next algorithm - Wilo
        public List<PCB> spn(Queue<PCB> processes)
        { 
            //after a process has completed, observe all processes that have arrived and use shortest service time
            return null; 
        }

        //shortest-remaining-time algorithm - Brady
        public List<PCB> srt(Queue<PCB> processes)
        { 
            //as processes arrive, compute service time
            //after computing, observe all processes that have arrived and use the one with the shortest service time
            return null;  
        }

        //highest-response-ratio-next algorithm - Wilo
        public List<PCB> hrrn(Queue<PCB> processes)
        { 
            //ration = (wait time + service time) / service time
            return null; 
        }

        //round robin algorithm - Hannah
        public List<PCB> rr(Queue<PCB> processes, int quantum)
        { 
            //create an empty queue
            //as quantums deplete the service time, match with arrival time
            //processes get added to queue when the arrive
            //alternate processes
            return null; 
        }

        //priority algorithm - Brady
        public List<PCB> priority(Queue<PCB> processes)
        { 
            //lowest interger priority number has highest priority
            return null; 
        }

        //feedback algorithms if we wish to implement a feedback age solution

        //version 1 feedback with quantum = 1 - Tommy
        public List<PCB> v1Feedback(Queue<PCB> processes)
        {
            int quantum = 1;
            var finished = false;   //when algorithm is complete
            var counter = 0;    //'timer' since we are modeling as discrete events
            var process = new PCB();    //temporary holder
            var startIndex = 0;     //start index of the ready queues
            var moveToNextQueue = true;

            //create some sample data
            Queue<PCB> sample = new Queue<PCB>
            (
                new[]{ 
                    new PCB(){name = 'A', arrivalTime = 0, serviceTime = 3},
                    new PCB(){name = 'B', arrivalTime = 2, serviceTime = 6},
                    new PCB(){name = 'C', arrivalTime = 4, serviceTime = 4},
                    new PCB(){name = 'D', arrivalTime = 6, serviceTime = 5},
                    new PCB(){name = 'E', arrivalTime = 8, serviceTime = 2}
                }
            );

            //create a queue of queues
            List<Queue<PCB>> rq = new List<Queue<PCB>>();
            for (int i = 0; i < 5; i++)
            {
                rq.Add(new Queue<PCB>());
            }

            while (!finished)
            {
                //if a process has arrive, get the process and start our index of queues back at 0
                if (sample.Count != 0)
                {
                    if (counter == sample.Peek().arrivalTime)
                    {
                        process = sample.Dequeue();
                        startIndex = 0;
                        rq[startIndex].Enqueue(process);
                    }
                }
                //if inbetween queues are empty, move along until we get to next queue that has elements
                while(rq[startIndex].Count == 0)
                    startIndex++;
                //take the process of the current queue
                process = rq[startIndex].Dequeue();
                //the process serves a certain amount of time
                Console.WriteLine("Process " +  process.name + " service time is " + process.serveTime(quantum));
                //this happens in one unit of time
                counter++;
                if (process.serviceTime == 0)
                {
                    process.finished = true;
                    continue;
                }
                    
                
                //if there are no more processes left in the current queue, move to the next queue
                if (rq[startIndex].Count == 0 && moveToNextQueue)
                {
                    startIndex++;
                    rq[startIndex].Enqueue(process);
                }
                else
                    rq[startIndex].Enqueue(process);

                //check if finished
                foreach (var queue in rq)
                {
                    //check if each queue is empty
                    if (queue.Count != 0)
                        break;
                    else if (queue.Count == 0)
                        continue;
                    finished = true;
                }
            }
            return null;
        }    

        //version 2 feedback with quantum = 2^i - Tommy
        public List<PCB> v2Feedback(Queue<PCB> processes)
        {
            //quantum = 2^i where i is the level of the queue starting at 0
            //use multiple queues until complete
            return null;
        }
    }
}
