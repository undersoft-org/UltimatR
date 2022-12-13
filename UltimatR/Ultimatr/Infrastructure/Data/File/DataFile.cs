
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

namespace UltimatR
{
    public class DataFile : IFormFile
    {
        private IFormFile _formFile;

        public DataFile(FileContainer container, string name, string extension)
        {
            BasePath = container.BasePath;
            FileStream stream = new FileStream($"{container.BasePath}/{name}.{extension}", FileMode.OpenOrCreate);
            _formFile = new FormFile(stream, 0, stream.Length, name, $"{name}.{extension}");
        }
        public DataFile(string basePath, string name, string extension)

        {
            BasePath = basePath;
            FileStream stream = new FileStream($"{basePath}/{name}.{extension}", FileMode.OpenOrCreate);
            _formFile = new FormFile(stream, 0, stream.Length, name, $"{name}.{extension}");
        }
        public DataFile(string basePath, string fileName)             
        {
            BasePath = basePath;
            FileStream stream = new FileStream($"{basePath}/{fileName}", FileMode.OpenOrCreate);
            _formFile = new FormFile(stream, 0, stream.Length, fileName, fileName);
        }
        public DataFile(string path)
        {
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            string[] splittedPath = path.Split('.');
            int l = splittedPath.Length;
            BasePath = splittedPath.Concat(splittedPath.Where((x,y) => y != l - 2 && y != l - 1)).FirstOrDefault();
            _formFile = new FormFile(stream, 0, stream.Length, splittedPath[l - 2], $"{splittedPath[l - 2]}.{splittedPath[l - 1]}");
        }

        public string BasePath { get; set; }

        public string ContentType => _formFile.ContentType;

        public string ContentDisposition => _formFile.ContentDisposition;

        public IHeaderDictionary Headers => _formFile.Headers;

        public long Length => _formFile.Length;

        public string Name => _formFile.Name;

        public string FileName => _formFile.FileName;

        public void CopyTo(Stream target)
        {
            _formFile.CopyTo(target);
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            return _formFile.CopyToAsync(target, cancellationToken);
        }

        public Stream OpenReadStream()
        {
            return _formFile.OpenReadStream();
        }
    }
}
