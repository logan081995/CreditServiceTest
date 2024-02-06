using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;

namespace CreditServiceTest.Models
{
    public interface IDocument
    {
        [BsonId]
        string Id { get; }
    }

    public abstract class Document : IDocument
    {
        public Document() { }

        [Browsable(false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
