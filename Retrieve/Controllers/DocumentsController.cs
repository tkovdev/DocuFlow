using Data.Access.Abstractions;
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
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly IEntityService<Document> _documentService;

        public DocumentsController(ILogger<DocumentsController> logger, IEntityService<Document> documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }
        
        // GET: api/<Document>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var documents = await _documentService.Get();
                var result = documents.Select(x => new { Id = x.Id.ToString(), Name = x.Name });
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception e)
            {
                if (e.Source == MongoRmExceptionFactory.DataAccessWarningExceptionType)
                    return StatusCode(StatusCodes.Status400BadRequest, e.Message);
                if (e.Source == MongoRmExceptionFactory.DataAccessCriticalExceptionType)
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                    
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred.");
            }
        }

        // GET api/<Document>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(ObjectId id)
        {
            try
            {
                var result = await _documentService.Get(id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception e)
            {
                if (e.Source == MongoRmExceptionFactory.DataAccessWarningExceptionType)
                    return StatusCode(StatusCodes.Status400BadRequest, e.Message);
                if (e.Source == MongoRmExceptionFactory.DataAccessCriticalExceptionType)
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                    
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred.");
            }
        }
    }
}
