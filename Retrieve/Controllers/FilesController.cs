using System.Net.Mime;
using Azure;
using Data.Access.Abstractions.Exceptions;
using Data.Access.Abstractions.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Retrieve.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IEntityService<Document> _documentService;
        private readonly IFileService _fileService;
        
        public FilesController( ILogger<FilesController> logger, 
            IEntityService<Document> documentService, 
            IFileService fileService
        )
        {
            _logger = logger;
            _documentService = documentService;
            _fileService = fileService;
        }
    
        // GET api/<Document>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(ObjectId id)
        {
            var doc = await _documentService.Get(id);

            try
            {
                var file = await _fileService.GetFile(doc.File.FileName);

                file.Seek(0, SeekOrigin.Begin);

                return File(file, MediaTypeNames.Application.Octet, $"{doc.File.OriginalName}.{doc.File.Extension}");
            }
            catch (Exception e)
            {
                throw MongoRmExceptionFactory.DataAccessCriticalException("File access failed!");
            }
        }
    }
}
