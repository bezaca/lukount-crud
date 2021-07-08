using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Lukount.Entities
{
	[BsonIgnoreExtraElements]
	public class Person
	{
		public int Cedula{get; set;}
		public int Edad { get; set; }
		public string Nombre { get; set; }
		public string Apellido { get; set; }
		public string Genero {get; set;}
	}
}