using Xunit;
using API.Controllers;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaStorage.Tests
{
    //Confirm that the GetMedia method of MultimediaStorageRepository correctly
    //retrieves media files from the database when they exist
    public class MultimediaStorageRepositoryTests
    {
        [Fact]
        public async Task GetMedia_ReturnsMediaFiles_WhenFilesExist()
        {
            // Arrange  //In this phase, you set up the testing environment. 
            //Creates a new in-memory database for testing, ensuring that tests don't
            //affect the actual database.
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDBContext(options))
            {
                context.Multimedias.AddRange(
                    //The reason for using this line of code in a unit test is to
                    //simulate a scenario where the multimedia storage system already
                    //contains files. By adding these entities to the in-memory database
                    //, the test can mimic the behavior of the system under normal
                    //conditions where media files exist
                    new MultiMedia { FileName = "file1.jpg", FileType = "image/jpeg", FilePath = "/path/to/file1.jpg", IsDelete = false },
                    new MultiMedia { FileName = "file2.jpg", FileType = "image/jpeg", FilePath = "/path/to/file2.jpg", IsDelete = false }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDBContext(options))
            {
                var repository = new MultimediaStorageRepository(context, new Response());

                // Act  The "Act" phase focuses on executing the behavior that you want to test. In your example, this is where the GetMedia method is called.
                var response = await repository.GetMedia();

                // Assert The final phase is where you verify that the action taken in the "Act" step produced the expected results
                Assert.NotNull(response);  // checks that the response object returned by the GetMedia method is not nul
                Assert.NotNull(response.Data); //checks that the Data property of the response object is not null
                Assert.IsAssignableFrom<IEnumerable<MultiMedia>>(response.Data);
                var mediaFiles = response.Data as IEnumerable<MultiMedia>;
                Assert.Equal(2, mediaFiles.Count()); // Assuming we added 2 files in the Arrange section
            }
        }

      
    }
}
