using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prototyping.Common.Dtos;

namespace Prototyping.Business.Cqrs
{
    public class TournamentMongoDto : ITournament
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}