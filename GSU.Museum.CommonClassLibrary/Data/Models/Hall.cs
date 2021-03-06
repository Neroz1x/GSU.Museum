﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GSU.Museum.CommonClassLibrary.Models
{
    public class Hall
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TitleRu { get; set; }
        public string TitleEn { get; set; }
        public string TitleBe { get; set; }
        public PhotoInfo Photo { get; set; }
        public List<Stand> Stands { get; set; }

        public Hall()
        {
            Photo = new PhotoInfo();
            Stands = new List<Stand>();
        }
    }
}
