using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lukount.Dtos;
using Lukount.Entities;
using Lukount.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lukount.Controllers
{
	[ApiController]
	[Route("v1/person")]

	public class PersonController : ControllerBase
	{
		private readonly IPersonRepository repository;

		public PersonController(IPersonRepository repository)
		{
			this.repository = repository;
		}

		// GET /v1/person
		[HttpGet]

		public async Task<IEnumerable<PersonDto>> GetPersonsAsync()
		{
			var persons = (await repository.GetPersonsAsync())
						  .Select(person => person.AsDto());

			return persons;

		}

		// GET /v1/person/{cedula}
		[HttpGet("{cedula}")]
		public async Task<ActionResult<PersonDto>> GetPersonAsync(int cedula)
		{
			var person = await repository.GetPersonAsync(cedula);

			if (person is null)
			{
				return NotFound();
			}

			return person.AsDto();

		}

		// POST /v1/person
		[HttpPost]
		public async Task<ActionResult<PersonDto>> CreateItemAsync(CreatePersonDto personDto)
		{
			Person person = new()
			{
				Cedula = personDto.Cedula,
				Edad = personDto.Edad,
				Nombre = personDto.Nombre,
				Apellido = personDto.Apellido,
				Genero = personDto.Genero
			};


			await repository.CreatePersonAsync(person);

			return CreatedAtAction(nameof(GetPersonAsync), new { Cedula = person.Cedula }, person.AsDto());

		}

		// PUT /v1/person/{cedula}
		[HttpPut("{cedula}")]
		public async Task<ActionResult> UpdatePersonAsync(int cedula, UpdatePersonDto personDto)
		{
			var existingPerson = await repository.GetPersonAsync(cedula);

			if (existingPerson is null)
			{
				return NotFound();
			}

			existingPerson.Cedula = personDto.Cedula;
			existingPerson.Edad = personDto.Edad;
			existingPerson.Nombre = personDto.Nombre;
			existingPerson.Apellido = personDto.Apellido;
			existingPerson.Genero = personDto.Genero;


			await repository.UpdatePersonAsync(existingPerson);
			return NoContent();
		}

		// DELETE /v1/person/{cedula}
		[HttpDelete("{cedula}")]
		public async Task<ActionResult> DeletePersonAsync(int cedula)
		{
			var existingPerson = await repository.GetPersonAsync(cedula);

			if (existingPerson is null)
			{
				return NotFound();
			}

			await repository.DeletePersonAsync(cedula);
			return NoContent();
		}
	}


}