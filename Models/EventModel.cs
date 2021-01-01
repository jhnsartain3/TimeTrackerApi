using System;
using System.ComponentModel.DataAnnotations;
using DatabaseInteraction.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class EventModel : EntityBase
    {
        [Required]
        [BsonElement("projectid")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Display(Name = "Project ID", Prompt = "5eb9eb10a0e5812c7caa399f",
            Description = "The unique project ID attaching the record to the correct project")]
        public string ProjectId { get; set; }

        [Required]
        [BsonElement("startdatetime")]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [BsonElement("enddatetime")]
        [DataType(DataType.Time)]
        public DateTime? EndDateTime { get; set; }

        [BsonElement("description")]
        [MinLength(3)]
        public string Description { get; set; }
    }
}