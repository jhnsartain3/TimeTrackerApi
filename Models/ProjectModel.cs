using System.ComponentModel.DataAnnotations;
using DatabaseInteraction.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class ProjectModel : EntityBase
    {
        [Required]
        [BsonElement("name")]
        [MinLength(3)]
        public string Name { get; set; }

        [BsonElement("description")]
        [MinLength(3)]
        public string Description { get; set; }

        [BsonElement("group")] [MinLength(3)] public string Group { get; set; }
    }
}