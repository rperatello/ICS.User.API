using AutoMapper;
using ICS.User.Application.DTOs;
using ICS.User.Application.Utils;
using ICS.User.Domain.Entities;
using ICS.User.Domain.Enumerators;
using ICS.User.Domain.Interfaces;
using ICS.User.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

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

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserLoginDTO user)
    {
        StringBuilder logTxt = new StringBuilder("Login - Token Request\n");

        try
        {
            user.IP = "127.0.0.1";
            if (Request.Headers.ContainsKey("ClientIP"))
            {
                user.IP = Request.Headers["ClientIP"];
            }

            logTxt.AppendLine($"Request by: {user.Login} | IP: {user.IP}");

            var userSaved = _uof.UserRepository.GetAll()
                                .Include(up => up.UserPermission)
                                .ThenInclude(p => p.Permission)
                                .FirstOrDefaultAsync(u => u.Login == user.Login && u.Password == ConverterTool.ExtractDataFromBase64(user.Password))
                                .Result;

            if (userSaved is null) {
                logTxt.Append("Login or password invalid");
                _logger.LogWarning(logTxt.ToString());
                return BadRequest(ErrorsResponseDTO.InformError("Login or password invalid")); 
            }

            userSaved.PermissionsIdList = userSaved.UserPermission.Select(p => p.PermissionId).ToList();
            var token = TokenSystemTools.GenerateToken(userSaved);

            logTxt.Append("Token generated");
            _logger.LogInformation(logTxt.ToString());

            return Ok(new { token });

        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

    [Authorize]
    [EnableQuery]
    [HttpGet("users", Name = "GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var res = await _uof.UserRepository.GetAll().Include(up => up.UserPermission).ThenInclude(up => up.Permission).OrderBy(u => u.Id).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(res));
        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

    [Authorize]
    [EnableQuery]
    [HttpGet("{id}", Name = "GetUserById")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var res = await _uof.UserRepository.GetAll().Include(up => up.UserPermission).ThenInclude(up => up.Permission).Where(u => u.Id == id).SingleOrDefaultAsync();
            return Ok(_mapper.Map<UserDTO>(res));
        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

    [Authorize]
    [HttpPost("", Name = "AddUser")]
    public async Task<IActionResult> AddUser([FromBody] UserToSaveDTO userDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userDTO.Id != 0) return BadRequest(ErrorsResponseDTO.InformError("New record must have ID = 0"));
            if (String.IsNullOrWhiteSpace(userDTO.Password)) return BadRequest(ErrorsResponseDTO.InformError("Password is required"));

            userDTO.Password = ConverterTool.ExtractDataFromBase64(userDTO.Password.Trim());

            var user = _mapper.Map<Domain.Entities.User>(userDTO);
            user.PermissionsIdList = user.PermissionsIdList is null || user.PermissionsIdList?.Count == 0 ? new List<uint> { 1 } : user.PermissionsIdList;

            var hasSameLoginSaved = _uof.UserRepository.Where(l => l.Login.ToLower().Trim() == user.Login.ToLower().Trim()).Result?.Login;
            if (!String.IsNullOrWhiteSpace(hasSameLoginSaved)) return BadRequest(ErrorsResponseDTO.InformError("Login in use by another user"));

            IEnumerable<Permission> permissionsInDatabase = _uof.PermissionRepository.GetAll().AsEnumerable();
            IEnumerable<uint> permissionsIdListInDatabase = permissionsInDatabase.Select(p => p.Id);

            if (!user.PermissionsIdList.All(p => permissionsIdListInDatabase.Contains(p))) 
                return BadRequest(ErrorsResponseDTO.InformError("Request has invalid permission"));

            List<UserPermission> userPermissionsList = new List<UserPermission>();
            foreach (var permission in permissionsInDatabase)
            {
                UserPermission userPermission = new(user.Id, permission.Id, user.PermissionsIdList.Contains(permission.Id));
                userPermissionsList.Add(userPermission);
            }
            user.UserPermission = userPermissionsList;

            _uof.UserRepository.Add(user);
            await _uof.Commit();

            user = await _uof.UserRepository.GetAll().Include(up => up.UserPermission).ThenInclude(up => up.Permission).Where(u => u.Login == user.Login).SingleOrDefaultAsync();

            var userDTOSaved = _mapper.Map<UserDTO>(user);

            return new CreatedAtRouteResult("GetUserById", new { id = userDTOSaved.Id }, userDTOSaved);
        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

    [Authorize]
    [HttpPut("{id}", Name = "UpdateUser")]
    public async Task<IActionResult> UpdateUser(uint id, [FromBody] UserToSaveDTO userDTO)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id == 0) return BadRequest(ErrorsResponseDTO.InformError("Valid ID is required"));

            if (id != userDTO.Id) return BadRequest(ErrorsResponseDTO.InformError("Model and url have different ID"));

            userDTO.PermissionsIdList = userDTO.PermissionsIdList is null || userDTO.PermissionsIdList?.Count == 0 ? new List<uint> { 1 } : userDTO.PermissionsIdList;

            var originalUserData = await _uof.UserRepository.GetAll(true).Include(up => up.UserPermission).ThenInclude(p => p.Permission).Where(u => u.Id == userDTO.Id).FirstOrDefaultAsync();

            if (originalUserData is null) return NotFound(ErrorsResponseDTO.InformError("User not founded"));

            if (String.IsNullOrWhiteSpace(userDTO.Password?.ToLower()?.Trim()))
                userDTO.Password = originalUserData.Password;
            else
                userDTO.Password = ConverterTool.ExtractDataFromBase64(userDTO.Password.Trim());

            if (originalUserData?.Login?.ToLower()?.Trim() != userDTO?.Login?.ToLower()?.Trim())
            {
                var hasSameLoginSaved = _uof.UserRepository.Where(l => l.Login.ToLower().Trim() == userDTO.Login.ToLower().Trim()).Result?.Login;
                if (!String.IsNullOrWhiteSpace(hasSameLoginSaved))
                    return BadRequest(ErrorsResponseDTO.InformError("Login in use by another user"));
            }

            IEnumerable<Permission> permissionsInDatabase = _uof.PermissionRepository.GetAll().AsEnumerable();
            IEnumerable<uint> permissionsIdListInDatabase = permissionsInDatabase?.Select(p => p.Id);

            if (permissionsIdListInDatabase is null || permissionsIdListInDatabase?.Count() == 0) {
                _logger.LogError("Table Permission no has basic records");
                return StatusCode(StatusCodes.Status500InternalServerError, "Request not processed");
            }

            if (!userDTO.PermissionsIdList.All(p => permissionsIdListInDatabase.Contains(p)))
                return BadRequest(ErrorsResponseDTO.InformError("Request has invalid permission"));

            originalUserData = _mapper.Map(userDTO, originalUserData);

            originalUserData.UserPermission = originalUserData.UserPermission == null ? new List<UserPermission>() : originalUserData.UserPermission;

            var originalUserDataPermissionsIdList = originalUserData?.UserPermission?.Select(p => p.PermissionId);

            var permissionIdNotFoundedToOriginalUserData = permissionsInDatabase.Where(p => !originalUserDataPermissionsIdList.Contains(p.Id)).Select(p => p.Id);

            foreach (var permission in originalUserData.UserPermission)
            {
                permission.Allowed = userDTO.PermissionsIdList.Contains(permission.PermissionId) ? true : false;
            }

            foreach (var permissionId in permissionIdNotFoundedToOriginalUserData)
            {
                originalUserData.UserPermission.Add(new UserPermission(originalUserData.Id, permissionId, false));
            }

            _uof.UserRepository.Update(originalUserData);
            await _uof.Commit();

            originalUserData = await _uof.UserRepository.GetAll().Include(up => up.UserPermission).ThenInclude(up => up.Permission).Where(u => u.Id == originalUserData.Id).SingleOrDefaultAsync();

            var userDTOSaved = _mapper.Map<UserDTO>(originalUserData);

            return new CreatedAtRouteResult("GetUserById", new { id = userDTOSaved.Id }, userDTOSaved);
        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

    [Authorize(Roles = "0")]
    [HttpDelete("{id}", Name = "DeleteUser")]
    public async Task<IActionResult> DeleteUser(uint id)
    {
        try
        {
            if (id == 0) return BadRequest(ErrorsResponseDTO.InformError("Valid ID is required"));

            var user = _uof.UserRepository.Where(x => x.Id == id).Result;
            if (user is null) return NotFound(ErrorsResponseDTO.InformError("User not founded"));

            _uof.UserRepository.Delete(user);
            await _uof.Commit();

            return Ok(_mapper.Map<UserDTO>(user));
        }
        catch (Exception ex)
        {
            string error = ex.InnerException == null ? $"Message: {ex.Message}" : $"Message: {ex.Message} | InnerException: {ex.InnerException?.Message}";
            _logger.LogError(error);
            return StatusCode(500, ErrorsResponseDTO.InformError(error));
        }
    }

}
