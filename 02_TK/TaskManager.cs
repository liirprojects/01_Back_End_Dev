using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_TK
{
    public class TaskManager
    {
        private SortedDictionary<int, Queue<TaskItem>> task_list;
        public event Action<TaskItem>? TaskAssigned;
        public event Action<TaskItem>? TaskCompleted;
        public event Action<TaskItem>? TaskOverdue;
        public TaskManager()
        {
            task_list = new SortedDictionary<int, Queue<TaskItem>>();
        }
        // General tasks count
        public List<TaskItem> GetAllTasks()
        {
            return task_list.SelectMany(q => q.Value).ToList();
        }
        public int CountTaskList()
        {
            return task_list.Values.Sum(x => x.Count);
        }
        // List of counted tasks
        public Dictionary<Task_Priority, int> ReturnCountedList()
        {
            Dictionary<Task_Priority, int> countedDictionary = new Dictionary<Task_Priority, int>();

            foreach (var item in task_list)
            {
                Task_Priority task_Priority = (Task_Priority)item.Key;
                int rowElementsCount = item.Value.Count;

                countedDictionary[task_Priority] = rowElementsCount;
            }
            return countedDictionary;
        }
        // Check what list contains
        private void HandleTaskIfExists(TaskItem checking_item, Action<int> onsuccess, Action<string> onFail)
        {
            if (checking_item == null)
            {
                onFail.Invoke("The operation is not valid for null parameters");
                return;
            }

            Task_Priority task_Priority = checking_item.Priority;
            int searching_priority = (int)task_Priority;

            if (!task_list.ContainsKey(searching_priority))
            {
                onFail.Invoke("The queue with the given priority number does not exist");
                return;
            }

            if (!task_list[searching_priority].Contains(checking_item))
            {
                onFail.Invoke("The searching element was not found in the queue");
                return;
            }

            onsuccess.Invoke(searching_priority);
        }

        private void AddTask(TaskItem input_users_task)
        {
            int priority_number = (int)input_users_task.Priority;

            if (!task_list.ContainsKey(priority_number))
                task_list[priority_number] = new Queue<TaskItem>();

            if (!task_list[priority_number].Contains(input_users_task))
            {
                task_list[priority_number].Enqueue(input_users_task);

                // Add event reaction
                TaskAssigned?.Invoke(input_users_task);
            }
        }
        public void SafeAddTask(TaskItem task)
        {
            if (task == null)
            {
                return;
            }

            AddTask(task);
        }
        public void CompleteTask(TaskItem input_users_task)
        {
            HandleTaskIfExists(input_users_task,
            onsuccess: (priority_number) =>
            {
                if(input_users_task.DueDate < DateTime.Now)
                {
                    TaskOverdue?.Invoke(input_users_task);
                    return;
                }

                Queue<TaskItem> newQueueList = new Queue<TaskItem>();

                while (task_list[priority_number].Count > 0)
                {
                    var gettingCurrent = task_list[priority_number].Dequeue();

                    if (gettingCurrent != input_users_task)
                    {
                        newQueueList.Enqueue(gettingCurrent);
                    }
                }
                task_list[priority_number] = newQueueList;

                TaskCompleted?.Invoke(input_users_task);
            },
            onFail: (error_message) => Console.WriteLine("\nCompleting task error : [{0}]",error_message));
        }
        public void ReassignTask(TaskItem input_users_task, DateTime new_date)
        {
            HandleTaskIfExists(input_users_task,
                onsuccess: (_) =>
                {
                    input_users_task.ChangeDueDate(new_date);
                },
                onFail: (error_message) => Console.WriteLine($"\n[ReassignTask Error] {error_message}"));
        }
    }
}
