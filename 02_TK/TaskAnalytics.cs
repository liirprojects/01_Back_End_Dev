using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_TK
{
    public class TaskAnalytics
    {
        private readonly Employee _employee;

        public TaskAnalytics(Employee employee)
        {
            this._employee = employee;
        }

        public T ReturnLinqQuery<T>(Func<IEnumerable<TaskItem>, T> func)
        {
            var tasks = _employee.taskManager.GetAllTasks();
            return func(tasks);
        }

    }
}
