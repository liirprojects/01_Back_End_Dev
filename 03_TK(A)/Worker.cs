using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_TK_A_
{
    public class Worker
    {
        private static int _next_id = 1;
        public int Id { get;}
        public string Name { get; private set; }

        public Specialization Specialization { get; private set; }
        public Worker(string name, Specialization specialization)
        {
            Id = _next_id++;

            this.Name = name;
            Specialization = specialization;
        }
        public Queue<JobItem>? Queue { get; private set; } = new Queue<JobItem>();
    }
}
