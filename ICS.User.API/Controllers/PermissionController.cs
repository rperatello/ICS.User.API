using AutoMapper;
using ICS.User.Application.DTOs;
using ICS.User.Domain.Entities;
using ICS.User.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

    [EnableQuery]
    [HttpGet("{id}", Name="GetPermissionById")]
    public async Task<IActionResult> GetPermissionById(uint id)
    {
        try
        {
            if (id == 0) return BadRequest(ErrorsResponseDTO.InformError("Valid ID is required"));

            var res = await _uof.PermissionRepository.Where(p => p.Id == id);
            if (res is null) return NotFound(ErrorsResponseDTO.InformError("Permission not founded"));

            return Ok(_mapper.Map<PermissionDTO>(res));
        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

    [HttpPost("", Name = "SavePermission")]
    public async Task<IActionResult> SavePermission(PermissionToSaveDTO dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.Id != 0) return BadRequest(ErrorsResponseDTO.InformError("New record must have ID = 0"));

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
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

    [HttpPut("{id}", Name = "UpdatePermission")]
    public async Task<IActionResult> UpdatePermission(uint id, [FromBody] PermissionDTO dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            if (id == 0) return BadRequest(ErrorsResponseDTO.InformError("Valid ID is required"));

            if (id != dto.Id)
                return BadRequest(ErrorsResponseDTO.InformError("Model and url have different ID"));

            if (id >= 1 && id <= 4)
                return BadRequest(ErrorsResponseDTO.InformError("Permission blocked to edit"));

            var permission = _uof.PermissionRepository.Where(x => x.Id == id).Result;
            if (permission is null) return NotFound(ErrorsResponseDTO.InformError("Permission not founded"));

            var permissionToSave = _mapper.Map<Permission>(dto);            

            _uof.PermissionRepository.Update(permissionToSave);
            await _uof.Commit();

            return Ok(_mapper.Map<PermissionDTO>(permissionToSave));
        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

    [HttpDelete("{id}", Name = "DeletePermission")]
    public async Task<IActionResult> DeletePermission(uint id)
    {
        try
        {
            if (id == 0) return BadRequest(ErrorsResponseDTO.InformError("Valid ID is required"));

            if (id >= 1 && id <= 4)
                return BadRequest(ErrorsResponseDTO.InformError("Permission blocked to delete"));

            var permission = _uof.PermissionRepository.Where(x => x.Id == id).Result;
            if (permission is null) return NotFound(ErrorsResponseDTO.InformError("Permission not founded"));

            _uof.PermissionRepository.Delete(permission);
            await _uof.Commit();

            return Ok(_mapper.Map<PermissionDTO>(permission));
        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

}
