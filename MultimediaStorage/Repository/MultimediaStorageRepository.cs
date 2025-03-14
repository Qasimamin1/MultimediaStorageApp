using AutoMapper;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Shared;
using System.Net;

namespace Repository
{
    public class MultimediaStorageRepository : IMultimediaStorage
    {
        private ApplicationDBContext _db { get; }
        private readonly Response _response;
        private ApplicationDBContext dbContext;

        public MultimediaStorageRepository(ApplicationDBContext db, Response response)
        {
            _db = db;
            _response = response;
        }

        public MultimediaStorageRepository(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Fetches all the files from database into descending order and return in json form to show it on client side.
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetMedia()
        {
            try
            {
                //Asynchronously retrieves all multimedia files from the database, ordered by ID in descending order.
                var files = await _db.Multimedias.OrderByDescending(x => x.Id).ToListAsync();

                // Checks if the retrieved files list is not null and assigns it to the response data.
                if (files is not null)
                    _response.Data = files;
                else
                {
                    // // Sets the response properties to indicate that no files were found.
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = "File not found.";
                    _response.Data = null;
                }
                return _response;
            }
            catch (Exception)
            {
                //// Returns a new Response object in case of any exception, indicating an internal server error.
                return new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred during file retrieval.",
                    Data = null,
                };
            }
        }

        /// <summary>
        /// Create new directory if it's not present in the project and uploads file into wwwroot folder.
        /// After uploading, save the record in the database aswell.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// // Asynchronous method to add a media file.
        public async Task<Response> AddMedia(IFormFile file)
        {
            try
            {
                // Initializing a filename variable.
                string filename = "";
                // Extracting the file extension.
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                // Getting the name of the file without the extension.
                var name = file.FileName.Split('.')[0];
                // Creating a unique filename using the current timestamp.
                filename = name + DateTime.Now.Ticks.ToString() + extension;

                // Constructing the path to save the file.
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
                // Checking if the directory exists, and creating it if it doesn't.
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                // Combining the directory path and filename for the exact file path.
                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", filename);

                // Writing the file to the server's filesystem.
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Creating a new MultiMedia object to represent the file in the database.
                MultiMedia multiMedia = new MultiMedia
                {
                    FileName = filename,
                    FilePath = filepath + "\\" + filename,
                    FileType = extension,
                    IsDelete = false,
                };
                // Adding the new MultiMedia object to the database.
                await _db.Multimedias.AddAsync(multiMedia);
                // Saving changes to the database.
                await _db.SaveChangesAsync();
                // Setting up a success message and data in the response object.
                _response.Message = "File uploaded successfully!";
                _response.Data = multiMedia; // Send the newly added item as data
                return _response;
            }
            catch (Exception)
            {
                return new Response
                {
                    // Returning a response object in case of an exception.
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred during file upload.",
                    Data = null,
                };
            }
        }

        /// <summary>
        /// Fetch the record againt incoming id and check if it is been already deleted or not. 
        /// If not, then soft delete the record from database.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        /// // Asynchronous method for deleting a media file by its ID.
        public async Task<Response> DeleteMedia(int fileId)
        {
            try
            {
                // Asynchronously find the file with the given ID.
                var file = await _db.Multimedias.FirstOrDefaultAsync(x => x.Id == fileId);
                // Check if the file exists.
                if (file is not null)
                {
                    // Check if the file is already marked as deleted.
                    if (!file.IsDelete)
                    {
                        // Mark the file as deleted.
                        file.IsDelete = true;
                        _db.Multimedias.Update(file);
                        // Save the changes to the database.
                        await _db.SaveChangesAsync();
                        // Return a successful response.
                        _response.Data = file;
                        _response.Message = "File deleted successfully!";
                        return new Response
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = _response.Message,
                            Data = _response.Data,
                        };
                    }
                    else
                    {
                        // Return a response indicating the file is already deleted.
                        return new Response
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "This file has already been deleted!",
                            Data = _response.Data,
                        };
                    }
                }
                else
                {
                    //// Return a response indicating the file was not found.
                    return new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "File not found.",
                        Data = null,
                    };
                }
            }

            catch (Exception)
            // Return a response indicating an internal server error.
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred during file deletion.",
                    Data = null,
                };
            }
        }

    }
}
