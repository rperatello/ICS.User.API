using AutoMapper;
using ICS.User.Application.DTOs;
using ICS.User.Domain.Entities;
using ICS.User.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace ICS.User.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public UserController(ILogger<UserController> logger, IUnitOfWork uof, IMapper mapper)
    {
        _logger = logger;
        _uof = uof;
        _mapper = mapper;
    }    

    [EnableQuery]
    [HttpGet("users", Name = "GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var res = await _uof.UserRepository.GetAll().Include(up => up.UserPermission).ThenInclude(up => up.Permission).ToListAsync();
        return Ok(_mapper.Map<IEnumerable<UserDTO>>(res));
    }

    [HttpGet("{id}", Name = "GetUserById")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var res = await _uof.UserRepository.GetAll().Include(up => up.UserPermission).ThenInclude(up => up.Permission).Where(u => u.Id == id).SingleOrDefaultAsync();
        return Ok(_mapper.Map<UserDTO>(res));
    }


    [HttpPost("", Name = "AddUser")]
    public async Task<IActionResult> AddUser([FromBody] UserToSaveDTO userDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = _mapper.Map<Domain.Entities.User>(userDTO);
            user.PermissionsIdList ??= new List<int> { 1 };

            IList<int> AllPermissionsIdListInDatabase = (IList<int>)_uof.PermissionRepository.GetAll().Select(p => p.Id).ToList();

            if (!user.PermissionsIdList.All(p => AllPermissionsIdListInDatabase.Contains(p)))
                return BadRequest("Request has invalid permission");

            foreach (int permissionId in AllPermissionsIdListInDatabase)
            {
                var userPermission = new UserPermission(user.Id, permissionId, user.PermissionsIdList.Contains(permissionId));
                _uof.UserPermissionRepository.Update(userPermission);
            }

            _uof.UserRepository.Update(user);

            await _uof.Commit();

            user = await _uof.UserRepository.GetAll().Include(up => up.UserPermission).ThenInclude(up => up.Permission).Where(u => u.Login == user.Login).SingleOrDefaultAsync();

            var userDTOSaved = _mapper.Map<UserDTO>(user);

            return new CreatedAtRouteResult("GetUserById", new { id = userDTOSaved.Id }, userDTOSaved);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

}
