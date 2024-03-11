﻿using JsonProperty.EFCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public FormFieldDetails? DetailsJsonColumn { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public class FormFieldDetails
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public TextInputDetails? TextInput { get; set; }
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public NumberInputDetails? NumberInput { get; set; }
        }
        public class TextInputDetails
        {
            public int? MinLength { get; set; }
            public int? MaxLength { get; set; }
        }
        public class NumberInputDetails
        {
            public int? Min { get; set; }
            public int? Max { get; set; }
        }
    }
}
