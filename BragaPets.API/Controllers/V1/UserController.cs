using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BragaPets.Domain.DTOs.Responses;
using BragaPets.Domain.Contracts.Services;
using BragaPets.Domain.DTOs.Requests;
using Elastic.Apm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;


namespace BragaPets.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/users")]
    public class UserController : ControllerBase
    {
        private const string ErrorMessage = "Houve um erro não tratado";
        private const string JsonApiType = "users";
        
        private readonly IUserService _userService;
        private readonly IDistributedCache _redis;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            IDistributedCache redis,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _redis = redis;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userService.GetAll(); 
                return Ok(CreateJsonApiResponse(result));
            }
            catch (Exception e)
            {
                var erro = new {Applicattion = "BragaPets", Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), Message = "Erro não tratado"};
                //Agent.Tracer.CaptureException(e);
                _logger.LogError(e, Newtonsoft.Json.JsonConvert.SerializeObject(erro));
                return StatusCode((int) HttpStatusCode.InternalServerError, ErrorMessage);
            }
        }
        
        [HttpGet]
        [Route("{uid}")]
        public async Task<IActionResult> GetByUid([FromRoute] Guid uid)
        {
            try
            {
                var result = await _userService.GetById(uid);
                return Ok(CreateJsonApiResponse(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);
                return StatusCode((int) HttpStatusCode.InternalServerError, ErrorMessage);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRequest dto)
        {
            try
            {
                var result = await _userService.Add(dto);
                return Created(string.Empty, CreateJsonApiResponse(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);
                return StatusCode((int) HttpStatusCode.InternalServerError, ErrorMessage);
            }
        }
        
        [HttpPut]
        [Route("{uid}")]
        public async Task<IActionResult> Put([FromRoute] Guid uid, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            try
            {
                var result = await _userService.Update(uid, userUpdateRequest);
                return Ok(CreateJsonApiResponse(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);
                return StatusCode((int) HttpStatusCode.InternalServerError, ErrorMessage);
            }
        }
        
        [HttpDelete]
        [Route("{uid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid uid)
        {
            try
            {
                await _userService.Delete(uid);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);
                return StatusCode((int) HttpStatusCode.InternalServerError, ErrorMessage);
            }
        }

        private BaseResponse CreateJsonApiResponse(UserResponse userResponse)
        {
            return new BaseResponse(userResponse, JsonApiType);
        }
        
        private BaseResponse CreateJsonApiResponse(IEnumerable<UserResponse> usersResponse)
        {
            return new BaseResponse(usersResponse, JsonApiType);
        }
    }
}