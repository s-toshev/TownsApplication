using TownsApplication.Data;
using TownsApplication.Data.Models;

namespace TownApplication.IntegrationTests
{
    public class TownControllerIntegrationTests
    {
        private readonly TownController _controller;

        public TownControllerIntegrationTests()
        {
            _controller = new TownController();
            _controller.ResetDatabase();
        }

        [Fact]
        public void AddTown_ValidInput_ShouldAddTown()
        {
            // TODO: This test checks if the AddTown method correctly adds a town with valid inputs.

            //Arrange
            string townName = "Ghana";
            int population = 2925;
            TownDbContext townDbContext = new TownDbContext();


            //Act
            _controller.AddTown(townName, population);

            var town = _controller.GetTownByName(townName);

            Assert.NotNull(town);
            Assert.Equal(population, town.Population);
            Assert.Equal(townName, town.Name);
            Assert.Single(townDbContext.Towns);
            Assert.True(town.Name.Length > 3);


        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("AB")]
        public void AddTown_InvalidName_ShouldThrowArgumentException(string invalidName)
        {
            //Arrange
            int population = 1025;
            string exceptionMessage = Assert.Throws<ArgumentException>(() => _controller.AddTown(invalidName, population)).Message;
            TownDbContext townDbContext1 = new TownDbContext();

            //Act&Assert
            Assert.Throws<ArgumentException>(() => _controller.AddTown(invalidName, population));
            Assert.Equal("Invalid town name.", exceptionMessage);
            Assert.Empty(townDbContext1.Towns);

            // TODO: This test verifies that the AddTown method throws an ArgumentException for invalid names.

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddTown_InvalidPopulation_ShouldThrowArgumentException(int invalidPopulation)
        {
            // TODO: This test ensures that the AddTown method correctly handles invalid population values.

            //Arrange
            string townName = "Butan";
            string excetionMessage = Assert.Throws<ArgumentException>(() => _controller.AddTown(townName, invalidPopulation)).Message;
            TownDbContext townDbContext1 = new TownDbContext();

            //Act&Assert
            Assert.Throws<ArgumentException>(() => _controller.AddTown(townName, invalidPopulation));
            Assert.Equal("Population must be a positive number.", excetionMessage);
            Assert.Empty(townDbContext1.Towns);

        }





        [Fact]
        public void AddTown_DuplicateTownName_DoesNotAddDuplicateTown()
        {
            // TODO: This test verifies that the AddTown method does not add a duplicate town.


            //Arrange
            string townName = "Teheran";
            int townNamePopulation = 123000;

            string duplicatedTown = "Teheran";
            int duplicatedTownPopulation = 342500;

            TownDbContext townDbContext1 = new TownDbContext();


            //Act
            _controller.AddTown(townName, townNamePopulation);
            _controller.AddTown(duplicatedTown, duplicatedTownPopulation);

            var existingTown = _controller.GetTownByName(townName);

            //Assert
            Assert.Single(townDbContext1.Towns);
            Assert.Equal(existingTown.Name, townName);
            Assert.Equal(existingTown.Population, townNamePopulation);



        }

        [Fact]
        public void UpdateTown_ShouldUpdatePopulation()
        {
            // TODO: This test checks if the UpdateTown method correctly updates the population of a town.


            //Arrange
            string townName = "Teheran";
            int townNamePopulation = 8000;

            int updatedPopulation = 1300;
            _controller.AddTown(townName, townNamePopulation);

            Town town = _controller.GetTownByName(townName);

            TownDbContext townDbContext1 = new TownDbContext();


            //Act

            _controller.UpdateTown(town.Id, updatedPopulation);

            //Assert
            Assert.NotNull(town);
            Assert.Single(townDbContext1.Towns);
            Assert.Equal(town.Population, updatedPopulation);
            Assert.Equal(town.Name, townName);


        }


        [Fact]
        public void DeleteTown_ShouldDeleteTown()
        {
            // TODO: This test ensures that the DeleteTown method correctly removes a town from the database.

            //Arrange
            TownDbContext townDbContext1 = new TownDbContext();

            string townName = "Damascus";
            int population = 344453;
            _controller.AddTown(townName, population);


            //Act
            var town = _controller.GetTownByName(townName);

            Assert.NotEmpty(townDbContext1.Towns);
            Assert.Equal(town.Population, population);
            Assert.Equal(town.Name, townName);


            _controller.DeleteTown(town.Id);

            var deletedTown = _controller.GetTownByName(townName);

            //Assert
            Assert.Empty(townDbContext1.Towns);
            Assert.Null(deletedTown);


        }

        //Arrange


        //Act


        //Assert


        //To ADD complete list of tests in the near future.


        [Fact]
        public void ListTowns_ShouldReturnTowns()
        {
            // TODO: This test checks if the ListTowns method correctly returns a list of all towns in the database.

            //Arrange
            TownDbContext townDbContext1 = new TownDbContext();


            List<Town> towns = new List<Town>
            {
                new Town { Id = 1, Name = "New York", Population = 8537673 },
                new Town { Id = 2, Name = "Los Angeles", Population = 3979576 },
                new Town { Id = 3, Name = "Chicago", Population = 2693976 },
                new Town { Id = 4, Name = "Houston", Population = 2320268 }
            };

            foreach (var town in towns)
            {
                _controller.AddTown(town.Name, town.Population);
            }


            //Act
            var townList = _controller.ListTowns();

            //Assert
            Assert.NotEmpty(townDbContext1.Towns);
            Assert.Equal(townList.Count, towns.Count);

            for(var i = 0; i < townList.Count; i++)
            {
                Assert.Equal(towns[i].Id, townList[i].Id);
                Assert.Equal(towns[i].Name, townList[i].Name);
                Assert.Equal(towns[i].Population, townList[i].Population);

            }

          
        }
    }
}
