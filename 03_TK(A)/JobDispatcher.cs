using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _03_TK_A_
{
    public class JobDispatcher
    {
        private Dictionary<int, Queue<JobItem>> priorityJobs;
        private Dictionary<Specialization, List<Worker>> specialize_workers;

        public event Action<JobItem>? MakeChanges;

        public JobDispatcher()
        {
            priorityJobs = new Dictionary<int, Queue<JobItem>>();     
            specialize_workers = new Dictionary<Specialization, List<Worker>>();
        }
        public List<Worker> ReturnAssignedWorkers()
        {
            var returned_list = specialize_workers.SelectMany(pair => pair.Value).ToList();

            return returned_list;
        }
        // Add Worker to all workers List
        public void AddWorker(Worker worker)
        {
            Specialization specialization = worker.Specialization;

            if(!specialize_workers.ContainsKey(specialization)) 
                specialize_workers[specialization] = new List<Worker>();

            if(!specialize_workers[specialization].Contains(worker))
            {
                specialize_workers[specialization].Add(worker);
            }
        }
        // Add job to priority queue
        public void AddJobToQueue(JobItem jobItem)
        {
            Priority priority = jobItem.Priority;
            int priority_number = (int)priority;

            if(!priorityJobs.ContainsKey(priority_number)) 
                priorityJobs[priority_number] = new Queue<JobItem>();

            if (!priorityJobs[priority_number].Contains(jobItem))
                priorityJobs[priority_number].Enqueue(jobItem);
        }

        public void AssignJobs()
        {
            var sortedQuery = priorityJobs.OrderBy(p => p.Key).ToList();

            foreach (var selected_jobs in sortedQuery)
            {
                var key = selected_jobs.Key;
                var avaliable_values = selected_jobs.Value; //all queue of key values

                var remaining_jobs = new Queue<JobItem>();  

                while(avaliable_values.Count > 0)
                {
                    var found_job = avaliable_values.Dequeue();

                    if (found_job.AssignedWorker != null)
                        continue;

                    Specialization specialization = found_job.RequiredSpecialization;
                    if (specialize_workers.ContainsKey(specialization))
                    {
                        Worker? found_worker = specialize_workers[specialization].OrderBy(t => t.Queue.Count).FirstOrDefault();

                        if (found_worker != null)
                        {
                            found_job.SetTaskStatus(Status.Assigned);

                            found_job.AssignedWorker = found_worker;
                            found_worker.Queue.Enqueue(found_job);
                            continue;
                        }
                    }
                    remaining_jobs.Enqueue(found_job);
                }
                priorityJobs[key] = remaining_jobs;
            }
        }
        public void ExecuteJob(JobItem job, Func<JobItem, bool> executionPolicy)
        {
            if (job.Status == Status.Completed)
                return;

            if (job.Status == Status.Created)
                AssignJobs();

            bool result = executionPolicy.Invoke(job);   

            if(result == true)
            {
                job.IncrementAttempts();
                job.SetTaskStatus(Status.Completed);
            }
            else
            {
                job.IncrementAttempts();
                if(job.Attempts == JobItem.MaxAttempts)
                {
                    job.SetTaskStatus(Status.Failed);
                    return;
                }

                RetryFailedJobs(job,executionPolicy);
            }
        }
        private void RetryFailedJobs(JobItem job, Func<JobItem, bool> executionPolicy)
        {
            MakeChanges?.Invoke(job);

            ExecuteJob(job,executionPolicy);
        }
    }
}
