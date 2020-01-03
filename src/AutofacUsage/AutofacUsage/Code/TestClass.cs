using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Code
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }

    public interface IPersonRepository
    {
        IEnumerable<Person> GetAll();
        Person Get(int id);
        Person Add(Person item);
        bool Update(Person item);
        bool Delete(int id);
    }

    public class PersonRepository : IPersonRepository
    {
        List<Person> person = new List<Person>();

        public PersonRepository()
        {
            Add(new Person { Id = 1, Name = "joye.net1", Age = 18, Address = "中国上海" });
            Add(new Person { Id = 2, Name = "joye.net2", Age = 18, Address = "中国上海" });
            Add(new Person { Id = 3, Name = "joye.net3", Age = 18, Address = "中国上海" });
        }
        public IEnumerable<Person> GetAll()
        {
            return person;
        }
        public Person Get(int id)
        {
            return person.Find(p => p.Id == id);
        }
        public Person Add(Person item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            person.Add(item);
            return item;
        }
        public bool Update(Person item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int index = person.FindIndex(p => p.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            person.RemoveAt(index);
            person.Add(item);
            return true;
        }
        public bool Delete(int id)
        {
            person.RemoveAll(p => p.Id == id);
            return true;
        }
    }
}