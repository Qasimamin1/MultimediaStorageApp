using Repository.Interface;
using Shared;

namespace API.Controllers
{
    [ApiController]
    public class MultimediaStorageController : BaseController
    {
        private readonly IMultimediaStorage _multimediaStorage;

        public MultimediaStorageController(IMultimediaStorage multimediaStorage)
        {
            _multimediaStorage = multimediaStorage;
        }

        // To fetch all the files from database.
        [HttpGet("get-media")]
        [AllowAnonymous]
        public async Task<Response> GetMedia() => await _multimediaStorage.GetMedia();

        // To add new files in database.
        [HttpPost("add-media")]
        [AllowAnonymous]
        public async Task<Response> AddMedia(IFormFile file) => await _multimediaStorage.AddMedia(file);

        // To delete specific file.
        [HttpGet("delete-media")]
        [AllowAnonymous]
        public async Task<Response> DeleteMedia(int file) => await _multimediaStorage.DeleteMedia(file);

    }
}
