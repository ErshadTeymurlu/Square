using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Square.Models
{
    public class Worker : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string JobTitle { get; set; }
        public string Motto { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile Img { get; set; }

    }
}