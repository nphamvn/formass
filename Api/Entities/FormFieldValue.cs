using System.ComponentModel.DataAnnotations;

namespace FormaaS.Entities;

public class FormFieldValue
{
    [Key]
    public int Id { get; set; }

    public FormField Field { get; set; }
    
    public string StringValue { get; set; }
}