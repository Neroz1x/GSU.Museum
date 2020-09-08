using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GSU.Museum.Web.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
        public RefreshToken RefreshToken{ get; set; }
    }
}
