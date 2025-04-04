namespace _03_TK_A_
{
    internal class Program
    {
        /* Design and implement a Job Dispatcher System that assigns and manages jobs based on priority and worker specialization, with event notifications and custom execution logic.
                Requirements:

                •	Create classes:
              + o	JobItem: Includes Id, Title, Priority, AssignedWorker, DueDate, Status, Attempts, MaxAttempts.
              + o	Worker: Includes Id, Name, Specialization, and a JobQueue.

                •	Storage Structure:
              + o	Use Dictionary<int, Queue<JobItem>> for priority-based job queues.
              + o	Use Dictionary<string, List<Worker>> to group workers by specialization.

                •	Dispatcher Responsibilities:
              + o	AddJob(JobItem job) — Add job to priority queue.
              + o	AssignJobs() — Assign jobs to available workers by specialization.
              + o	ExecuteJob(JobItem job, Func<JobItem, bool> executionPolicy) — Execute jobs with custom strategy.
              + o	RetryFailedJobs() — Retry failed jobs if attempts < MaxAttempts.

                •	Events:
              o	Trigger events: JobAssigned, JobStarted, JobCompleted, JobFailed, JobRetried.

                •	Analytics via LINQ:
              o	Jobs per worker, by status, and overdue jobs.
              o	Optionally, track overall job statistics.

                •	Delegate usage:
              o	Use delegates for flexible execution rules (e.g., urgent tasks first). */

        static void Main(string[] args)
        {
            JobDispatcher jobDispatcher = new JobDispatcher();

            RegisterWorkers(jobDispatcher);
            CreateJobList(jobDispatcher);

            jobDispatcher.AssignJobs();

            // У каждого работника сколько работ и написать какие
            List<Worker> assigned_workers = jobDispatcher.ReturnAssignedWorkers();
            foreach (Worker worker in assigned_workers)
            {
                int jobs_counter = worker.Queue.Count;
                Console.WriteLine($"Worker ID: [{worker.Id}. Jobs to do: [{jobs_counter}]\n");

                var sortedQueue = worker.Queue.OrderBy(x => x.Priority).ToList();
                foreach (JobItem job_item in sortedQueue)
                {
                    Console.WriteLine($"Id {job_item.Id} : {job_item.Title}. " +
                        $"Priority - {job_item.Priority}. Due date : [{job_item.DueTime}]");
                }
                Console.WriteLine("\n");
            }

            Console.ReadKey();
        }
        static T ExeptionHandler<T>(Func<T> function)
        {
            try
            {
                return function();
            }
            catch(Exception ex)
            {
                Console.WriteLine($" !!! Error: {ex.Message} !!!\n");
                return default;
            }
        }
        static void RegisterWorkers(JobDispatcher jobDispatcher)
        {
            var adding_workers = new List<Worker>
            {
                ExeptionHandler(() =>  new Worker("Ivan Sochanov", Specialization.Developer)),
                ExeptionHandler(() => new Worker("Maya", Specialization.Manager)),
                ExeptionHandler(() => new Worker("Olga", Specialization.Director))
            }.Where(w => w != null).ToList(); 


            foreach (var worker in adding_workers)
            {
                jobDispatcher.AddWorker(worker);
            }
        }
        static void CreateJobList(JobDispatcher jobDispatcher)
        {
            Queue<JobItem> queueToRealize = new Queue<JobItem>(new List<JobItem>
            {
                new JobItem("Bug fix",Priority.Immediate, new DateTime(2025,03,28), Specialization.Developer),
                new JobItem("Test a new project",Priority.Low, new DateTime(2025,04,05), Specialization.Developer),
                new JobItem("Make a rota",Priority.Immediate, new DateTime(2025,03,28), Specialization.Manager),
                new JobItem("Code check", Priority.Important, new DateTime(2025,04,01),Specialization.Developer),
                new JobItem("Pay slips",Priority.Immediate, new DateTime(2025,03,28),Specialization.Director),
                new JobItem("Add new pattern",Priority.Important, new DateTime(2025,04,02), Specialization.Developer),
                new JobItem("Hire a new developer",Priority.Unnessasary, new DateTime(2025,04,12), Specialization.Director)
            });

            foreach (var job in queueToRealize)
            {
                jobDispatcher.AddJobToQueue(job);
            }
        }
    }
}
