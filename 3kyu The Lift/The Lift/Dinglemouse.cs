﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable enable

public class Dinglemouse
{
    public static int[] TheLift(int[][] queues, int capacity)
    {
        List<int> result = new List<int>();
        Floor[] floors = queues.Select((queue, floor) => new Floor(queue.ToList(), floor)).ToArray();

        Lift lift = new Lift(floors, capacity);
        result.Add(lift.CurrentFloor);

        while (ThereArePeopleWaitingForTheLift(floors) || ThereArePeopleInTheLift(lift))
        {
            lift.SetDown();
            lift.Collect();
            int newFloor = lift.MoveToNextFloor();
            if (result.Last() != newFloor)
            {
                result.Add(newFloor);
            }
        }

        return result.ToArray();
    }

    private static bool ThereArePeopleInTheLift(Lift lift)
    {
        return !lift.IsEmpty();
    }

    private static bool ThereArePeopleWaitingForTheLift(Floor[] floors)
    {
        bool result = floors.Any(floor => floor.Queue.Any());
        return result;
    }

    private enum Direction { Up, Down };

    private class Floor
    {
        public int FloorNumber { get; private set; }

        public IList<int> Queue { get; private set; }

        public Floor(IList<int> queue, int floor)
        {
            this.Queue = queue;
            this.FloorNumber = floor;
        }

        public bool HasUpCall()
        {
            bool result = this.Queue.Any(request => request > this.FloorNumber);
            return result;

        }

        public bool HasDownCall()
        {
            bool result = this.Queue.Any(request => request < this.FloorNumber);
            return result;

        }

        public override string ToString()
        {
            return $"{this.FloorNumber}: {string.Join(", ", this.Queue)}";
        }
    }

    private class Lift
    {
        private int capacity;

        private Floor[] floors;

        private List<int> occupants = new List<int>();

        private Direction currentDirection = Direction.Up;

        public Lift(Floor[] floors, int capacity)
        {
            this.floors = floors;
            this.capacity = capacity;   
        }

        public int CurrentFloor { get; private set; } = 0;

        public bool IsEmpty()
        {
            bool result = !this.occupants.Any();
            return result;
        }

        public int MoveToNextFloor()
        {
            int result = 0;
            int? nextFloor = null;
            int? nextCallFloor;
            int? nextRequestFloor;
            if (this.currentDirection == Direction.Up)
            {
                nextCallFloor = this.FindNextUpCall();
                nextRequestFloor = this.FindNextUpRequest();
                if (nextCallFloor is not null && nextRequestFloor is not null)
                {
                    nextFloor = Math.Min(nextCallFloor.Value, nextRequestFloor.Value);
                }
                else if (nextCallFloor is null && nextRequestFloor is  null)
                {
                    nextFloor = this.FindHighestDownCall();
                    if (nextFloor is null)
                    {
                        nextFloor = this.FindLowestUpCall();
                    }
                    else
                    {
                        this.currentDirection = Direction.Down;
                    }
                }
                else
                {
                    nextFloor = nextCallFloor ?? nextRequestFloor;
                }
            }
            else
            {
                nextCallFloor = this.FindNextDownCall();
                nextRequestFloor = this.FindNextDownRequest();
                if (nextCallFloor is not null && nextRequestFloor is not null)
                {
                    nextFloor = Math.Max(nextCallFloor.Value, nextRequestFloor.Value);
                }
                else if (nextCallFloor is null && nextRequestFloor is null)
                {
                    nextFloor = this.FindLowestUpCall();
                    if (nextFloor is null)
                    {
                        nextFloor = this.FindHighestDownCall();
                    }
                    else
                    {
                        this.currentDirection = Direction.Up;
                    }
                }
                else
                {
                    nextFloor = nextCallFloor ?? nextRequestFloor;
                }
            }

            if (nextFloor is not null)
            {
                result = nextFloor.Value;
                this.CurrentFloor = result;
            }

            return result;
        }

        public void SetDown()
        {
            this.occupants.RemoveAll(request => request == this.CurrentFloor);
        }

        public void Collect()
        {
            IList<int> newPassengers;
            Floor floor = this.floors[this.CurrentFloor];
            if (this.currentDirection == Direction.Up)
            {
                newPassengers = floor.Queue.Where(passenger => passenger > this.CurrentFloor).Take(this.capacity - this.occupants.Count).ToList();
            }
            else
            {
                newPassengers = floor.Queue.Where(passenger => passenger < this.CurrentFloor).Take(this.capacity - this.occupants.Count).ToList();
            }

            this.occupants.AddRange(newPassengers);
            foreach(int newPassenger in newPassengers)
            {
                floor.Queue.Remove(newPassenger);
            }
        }

        public override string ToString()
        {
            return $"At {this.CurrentFloor}, Occupants: {string.Join(", ", this.occupants)}";
        }

        private int? FindNextUpRequest()
        {
            int? result = null;
            IEnumerable<int> upRequests = this.occupants.Where(request => request > this.CurrentFloor);
            if (upRequests.Any())
            {
                result = upRequests.Min();
            }

            return result;
        }

        private int? FindNextDownRequest()
        {
            int? result = null;
            IEnumerable<int> downRequests = this.occupants.Where(request => request < this.CurrentFloor);
            if (downRequests.Any())
            {
                result = downRequests.Max();
            }

            return result;
        }

        private int? FindNextUpCall()
        {
            int? result = null;
            Floor? nextFloor = this.floors.Skip(this.CurrentFloor + 1).FirstOrDefault(floor => floor.HasUpCall());
            if (nextFloor is not null)
            {
                result = nextFloor.FloorNumber;
            }

            return result;
        }

        private int? FindNextDownCall()
        {
            int? result = null;
            Floor? nextFloor = this.floors.Reverse().Skip(this.floors.Length - this.CurrentFloor).FirstOrDefault(floor => floor.HasDownCall());
            if (nextFloor is not null)
            {
                result = nextFloor.FloorNumber;
            }

            return result;
        }

        private int? FindHighestDownCall()
        {
            int? result = null;
            Floor? nextFloor = this.floors.Reverse().FirstOrDefault(floor => floor.HasDownCall());
            if (nextFloor is not null)
            {
                result = nextFloor.FloorNumber;
            }

            return result;
        }

        private int? FindLowestUpCall()
        {
            int? result = null;
            Floor? nextFloor = this.floors.FirstOrDefault(floor => floor.HasUpCall());
            if (nextFloor is not null)
            {
                result = nextFloor.FloorNumber;
            }

            return result;
        }
    }
}