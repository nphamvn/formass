namespace FormaaS.Models;

public class FormSubmitRequest
{
    public Guid FieldId { get; set; }
    public object Value { get; set; }
}