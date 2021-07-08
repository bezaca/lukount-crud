using System.Collections.Generic;
using System.Threading.Tasks;
using Lukount.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lukount.Repositories
{
	public class MongoDbPersons : IPersonRepository
	{

		// Mongo Configuration
		private const string databaseName = "lukount";
		private const string collectionName = "persons";
		private readonly IMongoCollection<Person> personCollection;

		private readonly FilterDefinitionBuilder<Person> filterBuilder = Builders<Person>.Filter;

		public MongoDbPersons(IMongoClient mongoClient)
		{
			IMongoDatabase database = mongoClient.GetDatabase(databaseName);
			personCollection = database.GetCollection<Person>(collectionName);
		}

		public async Task CreatePersonAsync(Person person)
		{
			await personCollection.InsertOneAsync(person);
		}

		public async Task DeletePersonAsync(int cedula)
		{
			var filter = filterBuilder.Eq(person => person.Cedula, cedula);
			await personCollection.DeleteOneAsync(filter);
		}

		public async Task<Person> GetPersonAsync(int cedula)
		{
			var filter = filterBuilder.Eq(item => item.Cedula, cedula);
			return await personCollection.Find(filter).SingleOrDefaultAsync();

		}

		public async Task<IEnumerable<Person>> GetPersonsAsync()
		{
			return await personCollection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task UpdatePersonAsync(Person person)
		{
			var filter = filterBuilder.Eq(existingPerson => existingPerson.Cedula, person.Cedula);
			await personCollection.ReplaceOneAsync(filter, person);
		}
	}
}