using ICS.User.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICS.User.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ILogger<ContactsController> _logger;
    private readonly IUnitOfWork _uof;

    public ContactsController(ILogger<ContactsController> logger, IUnitOfWork uof)
    {
        _logger = logger;
        _uof = uof;
    }


}
