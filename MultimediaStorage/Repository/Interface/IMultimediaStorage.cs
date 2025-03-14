using Microsoft.AspNetCore.Http;
using Shared;

namespace Repository.Interface
{
    public interface IMultimediaStorage
    {
        public Task<Response> AddMedia(IFormFile file);
        public Task<Response> DeleteMedia(int file);
        public Task<Response> GetMedia();
    }
}
