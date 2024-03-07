using System.ComponentModel.DataAnnotations;

namespace FormaaS.Models
{
    public class FormCreateUpdateRequest
    {
        [Required]
        public string Name { get; set; }
        public ICollection<FormFieldCreateUpdateRequest> Fields { get; set; } = [];
    }
}
