using AutoMapper;
using ICS.User.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICS.User.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ILogger<PermissionController> _logger;
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public TestController(ILogger<PermissionController> logger, IUnitOfWork uof, IMapper mapper)
    {
        _logger = logger;
        _uof = uof;
        _mapper = mapper;
    }

    /// <summary>
    /// Check api and database connection
    /// </summary>
    /// <returns></returns>
    [HttpGet("check")]
    public IActionResult CheckApi()
    {
        try
        {
            if (!_uof.HasDatabaseConnection())
            {
                throw new Exception("Failed to connect database !");
            }

            return Ok($"Running - {DateTime.Now.ToLongTimeString()}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
