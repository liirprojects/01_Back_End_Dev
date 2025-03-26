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
        + •	Create TaskItem with Id, Title, Priority, DueDate, Status.  
        + •	Store tasks in SortedDictionary<int, Queue<TaskItem>>.
        + •	Implement TaskManager to manage task flow(AddTask(), CompleteTask(), ReassignTask()).
        + •	Raise events like TaskAssigned, TaskCompleted, TaskOverdue.
        •	Use LINQ to generate task analytics by employee, status, or due dates.
        + •	Add delegate support for custom task execution policies.
Challenge: Add overdue monitoring that automatically raises alerts and reassigns tasks dynamically. */

        static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();
            SubscribeToTaskEvents(taskManager);

            TaskItem adding_task1 = ExecuteWithExceptionHandling(() 
                => new TaskItem(Task_Priority.Important, "English lesson", new DateTime(2025,3,20,14,30,0)));

            TaskItem adding_task2 = ExecuteWithExceptionHandling(()
                => new TaskItem(Task_Priority.Medium, "Gym session", new DateTime(2025, 3, 12, 12, 20, 0)));

            // Adding tasks to the list
            taskManager.SafeAddTask(adding_task1);
            taskManager.SafeAddTask(adding_task2);

            // Completing the task
            taskManager.CompleteTask(adding_task2);

            int countedElements = taskManager.CountTaskList();
            Console.WriteLine("\nTasks to be completed : {0}", countedElements);

            foreach (var item in taskManager.ReturnCountedList())
            {
                Console.WriteLine("Key: [{0}] Count : {1}", item.Key, item.Value);
            }

            // Employee LINQ QUERIES
            Employee employee = new Employee("Valeriia", "Spasibukhova", taskManager);

            TaskAnalytics taskAnalytics = new TaskAnalytics(employee);

            // Получить список всех задач сотрудника со флагом "Assigned".
            var query = taskAnalytics.ReturnLinqQuery(tasks => tasks.Where(s => s.Priority == Task_Priority.Important));

            // Получить задачи, отсортированные по дате дедлайна по возрастанию.
            var query2 = employee.taskManager.GetAllTasks().OrderBy(d => d.DueDate).ToList();

            //Подсчитать количество задач с каждым приоритетом 
            Dictionary<Task_Priority,int> count = employee.taskManager
                .GetAllTasks().GroupBy(task => task.Priority)
                .ToDictionary(group => group.Key, group => group.Count());
                
            // Проверить, есть ли у сотрудника хотя бы одна задача с приоритетом Important
            bool query3 = taskAnalytics.ReturnLinqQuery(tasks => tasks.Any(task => task.Priority == Task_Priority.Important));

            //Получить первую задачу, у которой дедлайн ближе всего.
            var query4 = employee.taskManager.GetAllTasks().OrderBy(task => task.DueDate).FirstOrDefault();

            //Найти задачи, дедлайн которых в этой неделе
            var dateNow = DateTime.Now;

            var startDate = dateNow.Date.AddDays(-(int)dateNow.DayOfWeek + (dateNow.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));

            var endDate = startDate.AddDays(7);

            var dateQuery = employee.taskManager.GetAllTasks().Where(d => d.DueDate >= startDate && d.DueDate < endDate);

            //Проверить, есть ли у сотрудника просроченные задачи.
            bool query5 = employee.taskManager.GetAllTasks().Any(t => t.DueDate < DateTime.Now);
 
            //Получить задачи, у которых статус "Created" и дедлайн через 3 дня или раньше.
            var query6 = employee.taskManager.GetAllTasks()
                .Where(p => p.Priority == Task_Priority.Important)
                .Where(d => d.DueDate > DateTime.Now && d.DueDate <= DateTime.Now.AddDays(3))
                .ToList();




            Console.ReadLine();
        }
        // Exception Handler
        static T ExecuteWithExceptionHandling<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                Console.WriteLine($" !!! Error: {ex.Message}\n");
                return default;
            }
        }
        // Method for subscribing for the event (user does it)
        private static void SubscribeToTaskEvents(TaskManager taskManager)
        {
            taskManager.TaskAssigned += task => PrintTaskEvent("assigned", task);
            taskManager.TaskCompleted += task => PrintTaskEvent("completed", task);
            taskManager.TaskOverdue += task => PrintTaskEvent("OVERDUE", task);
        }
        private static void PrintTaskEvent(string eventType, TaskItem task)
        {
            Console.WriteLine($"Task {eventType} -> ID: [{task.Id}] | Title: {task.Title}");
        }

    }
}
