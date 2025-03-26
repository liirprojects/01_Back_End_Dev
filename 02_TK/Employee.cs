using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_TK
{
    public class Employee
    {
        public int Id { get; }
        private int nextId = 1;

        public string Employee_Name { get; set; }
        public string Employee_Surname { get; set; }

        public Employee(string emp_name, string emp_surname, TaskManager taskManager)
        {
            this.Employee_Name = emp_name;
            this.Employee_Surname = emp_surname;

            Id = nextId++;
            this.taskManager = taskManager;
        }

        public TaskManager taskManager { get; private set; }
    }
}
