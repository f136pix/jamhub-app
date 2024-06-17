using DemoLibrary.Models;

namespace DemoLibrary.Infraestructure.DataAccess;

public interface IDataAccess
{
    public List<Person> GetPeople();
    public Person InsertPerson(string firstName, string lastName);
}

public class DemoDataAccess : IDataAccess
{
    // private List<PersonModel> people = new List<PersonModel>();
    private List<Person> people = new();

    public DemoDataAccess()
    {
        people.Add(new Person { Id = 1, FirstName = "Filipe", LastName = "Furlan" });
        people.Add(new Person { Id = 2, FirstName = "John", LastName = "Snow" });
    }

    public List<Person> GetPeople()
    {
        return people;
    }

    public Person InsertPerson(string firstName, string lastName)
    {
        var p = new Person { FirstName = firstName, LastName = lastName };
        p.Id = people.Max(person => person.Id) + 1;
        people.Add(p);
        return p;
    }
}