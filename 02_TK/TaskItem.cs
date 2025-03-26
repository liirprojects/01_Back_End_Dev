using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_TK
{
    public class TaskItem
    {
        public int Id { get; }
        private static int next_id = 1;
        public string Title { get; }
        public Task_Priority Priority { get; set; }
        private DateTime dueDate;
        public DateTime DueDate 
        { 
            get { return dueDate; }
            private set
            {
                if (value < DateTime.Now)
                    throw new ArgumentException($"Task {Id} : Input date {value} is in the past.");

                dueDate = value;
            }
        }
        public void ChangeDueDate(DateTime newDate)
        {
            if (newDate < DateTime.Now || newDate < DueDate)
                throw new ArgumentException("Not possible to change the date that is less than current date or in the past.");

            DueDate = newDate;
        }
        private string status;
        public string Status
        {
            get { return status; }
            private set { status = value; }
        }
        public TaskItem(Task_Priority task_Priority, string title, DateTime due_dateTime)
        {
            Id = next_id++;
            status = "Created";

            this.Priority = task_Priority;
            this.Title = title;
            this.DueDate = due_dateTime;
        }
    }
}
