namespace DemoLibrary.Domain.Exceptions;


    public class PersonNotFoundException : Exception
    {
        public PersonNotFoundException(int id)
            : base($"Person with ID {id} not found.")
        {
        }
    }