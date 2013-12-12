using System.ComponentModel.DataAnnotations;

namespace Tuto.DataLayer.Models
{
    public class Entity
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
    }
}
