using JsonProperty.EFCore;
using System.ComponentModel.DataAnnotations;

namespace FormaaS.Entities
{
    public class FormField
    {
        [Key]
        public Guid Id { get; set; }
        public int FormId { get; set; }
        public Form Form { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public JsonDictionary Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
