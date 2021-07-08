using System.ComponentModel.DataAnnotations;

namespace Lukount.Dtos
{
	public record PersonDto(int Cedula,
		int Edad,
		string Nombre,
		string Apellido,
		string Genero);
	public record CreatePersonDto(
		[Required] int Cedula,
		[Required] int Edad,
		[Required] string Nombre,
		[Required] string Apellido,
		[Required] string Genero
	);

	public record UpdatePersonDto(
		[Required] int Cedula,
		[Required] int Edad,
		[Required] string Nombre,
		[Required] string Apellido,
		[Required] string Genero
	);

}