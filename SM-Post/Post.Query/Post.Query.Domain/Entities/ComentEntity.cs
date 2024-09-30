using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities
{
    [Table("Coment")]
    public class ComentEntity
    {
        [Key]
        public Guid CommentId { get; set; }
        public string Username { get; set; }    
        public DateTime ComentDate { get; set; }
        public string Coment { get; set; }
        public bool Edited { get; set; }
        public Guid PostId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual PostEntity Post { get; set; }
    }
}