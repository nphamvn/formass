using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FormaaS.Models
{
    [JsonConverter(typeof(FormFieldCreateUpdateRequestJsonConverter))]
    public class FormFieldCreateUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public bool Required { get; set; }
        [Required]
        public FormFieldDetailsCreateUpdateRequest Details { get; set; }
    }

    public class FormFieldCreateUpdateRequestJsonConverter : JsonConverter<FormFieldCreateUpdateRequest>
    {
        public override FormFieldCreateUpdateRequest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader readerClone = reader;

            string? type = null;
            while (readerClone.Read())
            {
                if (readerClone.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = readerClone.GetString()!;
                    
                    if (propertyName.Equals(nameof(FormFieldCreateUpdateRequest.Type), StringComparison.OrdinalIgnoreCase))
                    {
                        readerClone.Read();
                        type = readerClone.GetString();
                        break;
                    }
                }
            }
            ArgumentNullException.ThrowIfNullOrEmpty(type);

            return GetFormFieldCreateUpdateRequest(ref reader, type);
        }

        private FormFieldCreateUpdateRequest? GetFormFieldCreateUpdateRequest(ref Utf8JsonReader reader, string type)
        {
            var field = new FormFieldCreateUpdateRequest();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return field;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case nameof(FormFieldCreateUpdateRequest.Id):
                            field.Id = reader.GetGuid();
                            break;
                        case nameof(FormFieldCreateUpdateRequest.Name):
                            field.Name = reader.GetString();
                            break;
                        case nameof(FormFieldCreateUpdateRequest.Type):
                            field.Type = reader.GetString();
                            break;
                        case nameof(FormFieldCreateUpdateRequest.Required):
                            field.Required = reader.GetBoolean();
                            break;

                        case nameof(FormFieldCreateUpdateRequest.Details):
                            {
                                switch (type)
                                {
                                    case "TextInput":
                                        field.Details = JsonSerializer.Deserialize<FormFieldTextInputDetails>(ref reader);
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, FormFieldCreateUpdateRequest value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class FormFieldDetailsCreateUpdateRequest
    {

    }

    public class FormFieldTextInputDetails : FormFieldDetailsCreateUpdateRequest
    {
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }
}