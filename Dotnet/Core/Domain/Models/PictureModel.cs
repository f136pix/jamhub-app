using System.ComponentModel.DataAnnotations.Schema;
using DemoLibrary.Models;

namespace DemoLibrary.Business.Models
{
    public class PictureModel
    {
        public int Id { get; set; }

        [ForeignKey("Person")] public int PersonId { get; set; }
        public PersonModel Person { get; set; }

        public string PictureUrl { get; set; }
    }
}