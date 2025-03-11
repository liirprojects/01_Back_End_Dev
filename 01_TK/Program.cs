using System.Net.Http.Headers;

namespace _01_TK
{
    // 1.	Design a Repository<T> class that handles CRUD operations using a generic type T.
    // Use a List<T> internally to store the data and implement methods like Add, Update, Delete, and GetAll().
    // Implement a LINQ query to find all elements that meet a certain condition (e.g., Age > 30).
    internal class Program
    {
        static void Main(string[] args)
        {
            Repository<Employee> repository = new Repository<Employee>();

            repository.Add(new Employee() { Name = "Alex", Age = 34, Nationality = "Ukranian" });
            repository.Add(new Employee() { Name = "Oscar", Age = 29, Nationality = "Welsh" });
            repository.Add(new Employee() { Name = "Maria", Age = 22, Nationality = "Italian" });


            // Updating
            repository.UpdatePersonnalInfo(n => n.Name == "Maria", e =>
            {
                e.Age = 23;
            });

            // Deleting
            repository.DeleteRepository(n => n.Name == "Alex");

            foreach (var employees in repository.GetAll())
            {
                Console.WriteLine("{0}: age: [{1}]. Nationality - {2}", employees.Name, employees.Age, employees.Nationality);
            }
            
            Console.ReadKey();
        }
    }
    class Employee
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
    }
    class Repository<T> where T : class
    {
        private List<T> values_list = new List<T>();

        public void Add(T adding_value)
        {
            if(!values_list.Any(v => v == adding_value)) 
                values_list.Add(adding_value);
        }

        /* public bool UpdateRepository(T input_value, Func<T,bool> function)
        {
            T searching_element = values_list.First(function);

            if(searching_element != null)
            {
                int existing_index = values_list.IndexOf(searching_element);
                values_list[existing_index] = input_value;

                return true;
            }
            return false;
        } */
        public bool UpdatePersonnalInfo(Func<T, bool> update_func, Action<T> action)
        {
            T element_to_update = values_list.First(update_func);

            if(element_to_update != null)
            {
                action(element_to_update);
                return true;
            }
            return false;    
        }
        public void DeleteRepository(Predicate<T> remove_predicate)
        {
            T searching_element = values_list.Where(new Func<T, bool>(remove_predicate)).First();
            values_list.Remove(searching_element);
        }

        public List<T> GetAll() {return values_list; }
    }    
}
