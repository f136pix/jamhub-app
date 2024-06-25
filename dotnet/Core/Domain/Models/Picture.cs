using System.ComponentModel.DataAnnotations.Schema;
using DemoLibrary.Models;

namespace DemoLibrary.Business.Models
{
    public class Picture
    {
        public int Id { get; set; }

        [ForeignKey("Person")] public int PersonId { get; set; }
        public Person Person { get; set; }

        public string PictureUrl { get; set; }
    }
}