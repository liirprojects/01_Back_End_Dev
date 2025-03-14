using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using System;
namespace _02_TK
{
    internal class Program
    {
        // ✅ Task 2: Task Scheduler with Priority Queues and Event Triggers
        // Objective:
            /* Develop a task management system where tasks are scheduled by priority and tracked with event notifications.
        Details:
        •	Create TaskItem with Id, Title, Priority, DueDate, Status.
        •	Store tasks in SortedDictionary<int, Queue<TaskItem>>.
        •	Implement TaskManager to manage task flow(AddTask(), CompleteTask(), ReassignTask()).
        •	Raise events like TaskAssigned, TaskCompleted, TaskOverdue.
        •	Use LINQ to generate task analytics by employee, status, or due dates.
        •	Add delegate support for custom task execution policies.
Challenge: Add overdue monitoring that automatically raises alerts and reassigns tasks dynamically. */

        static void Main(string[] args)
        {

        }
    }
    public class TaskItem
    {
        public int Id { get; }
        private static int next_id = 1;
        public string? Title { get; }
        public Task_Priority Priority { get; set; }
        private DateTime dueDate;
        public DateTime DueDate
        {
            get { return dueDate; }
            private set
            {
                if (value < DateTime.Now)
                    throw new ArgumentException("Invalid datetime value");
            }
        }
        private string status;
        public string Status
        {
            get { return status; }
            private set { status = value; }
        }
        public TaskItem(Task_Priority task_Priority)
        {
            Id = next_id++;
            this.Priority = task_Priority;
            status = "Created";
        }
    }
    public class TaskManager
    {
        public SortedDictionary<int, Queue<TaskItem>> task_list;
        public event Action<TaskItem>? TaskAssigned;
        public event Action<TaskItem>? taskAssigned;
        public TaskManager()
        {
            task_list = new SortedDictionary<int, Queue<TaskItem>>();
        }

        public void AddTask(TaskItem input_users_task)
        {
            Task_Priority task_Priority = input_users_task.Priority;
            int priority_number = (int)task_Priority;

            if(!task_list.ContainsKey(priority_number))
                task_list[priority_number] = new Queue<TaskItem>();

            if (!task_list[priority_number].Contains(input_users_task))
            {
                task_list[priority_number].Enqueue(input_users_task);
                TaskAssigned?.Invoke(input_users_task);
            }
        }
        public void CompleteTask(TaskItem input_users_task)
        {

        }
    }  
}
