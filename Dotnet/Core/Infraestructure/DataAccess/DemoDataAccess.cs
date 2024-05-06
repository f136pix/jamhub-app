using DemoLibrary.Models;

namespace DemoLibrary.Infraestructure.DataAccess;

public interface IDataAccess
{
    public List<PersonModel> GetPeople();
    public PersonModel InsertPerson(string firstName, string lastName);
}

public class DemoDataAccess : IDataAccess
{
    // private List<PersonModel> people = new List<PersonModel>();
    private List<PersonModel> people = new();

    public DemoDataAccess()
    {
        people.Add(new PersonModel { Id = 1, FirstName = "Filipe", LastName = "Furlan" });
        people.Add(new PersonModel { Id = 2, FirstName = "John", LastName = "Snow" });
    }

    public List<PersonModel> GetPeople()
    {
        return people;
    }

    public PersonModel InsertPerson(string firstName, string lastName)
    {
        var p = new PersonModel { FirstName = firstName, LastName = lastName };
        p.Id = people.Max(person => person.Id) + 1;
        people.Add(p);
        return p;
    }
}