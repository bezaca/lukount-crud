using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lukount.Controllers;
using Lukount.Dtos;
using Lukount.Entities;
using Lukount.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Lukount.UnitTests
{
	public class PersonControllerTests
	{

		private readonly Mock<IPersonRepository> repositoryStub = new();
		private readonly Random rand = new();

		// Test Convention
		// UnitOfWork_StateUnderTest_ExpectedBehavior()

		[Fact]
		public async Task GetPersonAsync_WithUnexistingPerson_ReturnsNotFound()
		{
			// Arrange 
			repositoryStub.Setup(repo => repo.GetPersonAsync(It.IsAny<int>()))
						  .ReturnsAsync((Person)null);

			var controller = new PersonController(repositoryStub.Object);

			// Act 
			var result = await controller.GetPersonAsync(rand.Next(10000000));

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
			result.Result.Should().BeOfType<NotFoundResult>();

		}

		[Fact]
		public async Task GetPersonAsync_WithExistingPerson_ReturnsExpectedPerson()
		{
			// Arrange
			Person expectedPerson = CreateRandomPerson();

			repositoryStub.Setup(repo => repo.GetPersonAsync(It.IsAny<int>()))
						  .ReturnsAsync(expectedPerson);

			var controller = new PersonController(repositoryStub.Object);

			// Act
			var result = await controller.GetPersonAsync(rand.Next(10000));

			// Assert
			result.Value.Should().BeEquivalentTo(expectedPerson);
		}

		[Fact]
		public async Task GetPersonsAsync_WithExistingItems_ReturnsAllPersons()
		{
			// Arrange
			var expectedPersons = new[] {CreateRandomPerson() , CreateRandomPerson(), CreateRandomPerson()};

			repositoryStub.Setup(repo => repo.GetPersonsAsync())
						  .ReturnsAsync(expectedPersons);

			var controller = new PersonController(repositoryStub.Object);

			// Act
			var actualPersons = await controller.GetPersonsAsync();

			// Assert
			actualPersons.Should().BeEquivalentTo(expectedPersons);

		}

		[Fact]
		public async Task CreatePersonAsync_WithPersonToCreate_ReturnsCreatedPerson()
		{
			// Arrange
			var personToCreate = new CreatePersonDto(
				rand.Next(100000),
				rand.Next(100),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString()
			);

			var controller = new PersonController(repositoryStub.Object);

			// Act
			var result = await controller.CreateItemAsync(personToCreate);

			// Assert
			var createdPerson = (result.Result as CreatedAtActionResult).Value as PersonDto;
			createdPerson.Should().BeEquivalentTo(
				createdPerson,
				options => options.ComparingByMembers<PersonDto>().ExcludingMissingMembers()
			);
			createdPerson.Nombre.Should().NotBeEmpty();

		}

		[Fact]
		public async Task UpdatePersonAsync_WithExistingPerson_ReturnsNoContent()
		{
			// Arrange
			Person existingPerson = CreateRandomPerson();

			repositoryStub.Setup(repo => repo.GetPersonAsync(It.IsAny<int>()))
						  .ReturnsAsync(existingPerson);

			var personCedula = existingPerson.Cedula;
			var personToUpdate = new UpdatePersonDto(
				rand.Next(100000),
				existingPerson.Edad + 3,
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString()
			);


			var controller = new PersonController(repositoryStub.Object);
			// Act
			var result = await controller.UpdatePersonAsync(personCedula, personToUpdate);

			// Assert
			result.Should().BeOfType<NoContentResult>();
		}

		[Fact]
		public async Task DeletePersonAsync_WithExistingPerson_ReturnsNoContent()
		{
			// Arrange
			Person existingPerson = CreateRandomPerson();

			repositoryStub.Setup(repo => repo.GetPersonAsync(It.IsAny<int>()))
						  .ReturnsAsync(existingPerson);

			var controller = new PersonController(repositoryStub.Object);

			// Act
			var result = await controller.DeletePersonAsync(existingPerson.Cedula);

			// Assert
			result.Should().BeOfType<NoContentResult>(); 
		}

		private Person CreateRandomPerson()
		{
			return new()
			{
				Cedula = rand.Next(100000),
				Edad = rand.Next(100),
				Nombre = Guid.NewGuid().ToString(),
				Apellido = Guid.NewGuid().ToString(),
				Genero = Guid.NewGuid().ToString()
			};
		}
	}
}