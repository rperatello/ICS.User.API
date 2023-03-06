using AutoMapper;
using ICS.User.Application.DTOs;
using ICS.User.Domain.Entities;
using ICS.User.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ICS.User.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionController : ControllerBase
{

    private readonly ILogger<PermissionController> _logger;
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public PermissionController(ILogger<PermissionController> logger, IUnitOfWork uof, IMapper mapper)
    {
        _logger = logger;
        _uof = uof;
        _mapper = mapper;
    }

    [EnableQuery]
    [HttpGet("")]
    public IActionResult GetPermissions()
    {
        try
        {
            var res = _uof.PermissionRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<PermissionDTO>>(res.AsEnumerable()));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}", Name="GetPermissionById")]
    public async Task<IActionResult> GetPermissionById(int id)
    {
        try
        {
            var res = await _uof.PermissionRepository.Where(p => p.Id == id);
            return Ok(_mapper.Map<PermissionDTO>(res));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("", Name = "SavePermission")]
    public async Task<IActionResult> SavePermission(PermissionToSaveDTO dto)
    {
        try
        {
            var permission = _mapper.Map<Permission>(dto);

            var UserPermissionsList = _uof.UserRepository.GetAll().Select(u => new UserPermission(u.Id, permission.Id, false));
            permission.UserPermission = await UserPermissionsList.ToListAsync();
            _uof.PermissionRepository.Add(permission);
            await _uof.Commit();

            var permissionDTO = _mapper.Map<PermissionDTO>(permission);

            return new CreatedAtRouteResult("GetPermissionById", new { id = permission.Id }, permissionDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}", Name = "UpdatePermission")]
    public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("Model and url have different id");

            var permission = _mapper.Map<Permission>(dto);

            _uof.PermissionRepository.Update(permission);
            await _uof.Commit();

            return Ok(_mapper.Map<PermissionDTO>(permission));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}", Name = "DeletePermission")]
    public async Task<IActionResult> DeletePermission(int id)
    {
        try
        {
            var permission = _uof.PermissionRepository.Where(x => x.Id == id).Result;

            if (permission is null) return NotFound();
            _uof.PermissionRepository.Delete(permission);
            await _uof.Commit();

            return Ok(_mapper.Map<PermissionDTO>(permission));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

}
