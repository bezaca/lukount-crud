using System.Collections.Generic;
using System.Threading.Tasks;
using Lukount.Entities;

namespace Lukount.Repositories
{
	public interface IPersonRepository
	{
		Task<Person> GetPersonAsync(int cedula);
		Task<IEnumerable<Person>> GetPersonsAsync();
		Task CreatePersonAsync(Person person);
		Task UpdatePersonAsync(Person person);
		Task DeletePersonAsync(int cedula);

	}
}