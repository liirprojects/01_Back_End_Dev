using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _03_TK_A_
{
    public class JobItem
    {
        public int Id { get; }
        private static int _next_id = 1;
        public string Title { get; set; }
        public Priority Priority { get; set; }
        public Worker? AssignedWorker { get; set; }
        public Specialization RequiredSpecialization { get; private set; }

        private DateTime dueDate;
        public DateTime DueTime
        {
            get { return dueDate; }
            private set
            {
                if (value < DateTime.Now)
                    throw new ArgumentException($"Job Id : {Id}.You can not assign the Due Time with past value");

                dueDate = value;
            }
        }
        public Status Status { get; private set; }
        public void SetTaskStatus(Status status)
        {
            Status = status;
        }
        public int Attempts { get; private set; }
        public void IncrementAttempts()
        {
            Attempts = Attempts + 1;
        }
        public const int MaxAttempts = 3;
        public JobItem(string title, Priority priority, DateTime dueDate, Specialization requiredSpecialization)
        {
            Id = _next_id++;

            this.Title = title;
            this.Priority = priority;
            //this.AssignedWorker = worker;
            this.DueTime = dueDate;

            Status = Status.Created;
            Attempts = 0;
            RequiredSpecialization = requiredSpecialization;
        }
    }
}
