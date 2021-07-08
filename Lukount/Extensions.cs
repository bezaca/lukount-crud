using Lukount.Dtos;
using Lukount.Entities;

namespace Lukount
{
	public static class Extensions
	{
		public static PersonDto AsDto(this Person person)
		{
			return new PersonDto(
				person.Cedula,
				person.Edad,
				person.Nombre,
				person.Apellido,
				person.Genero
			);
		}
	}
}