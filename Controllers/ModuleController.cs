using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SealabAPI.Base;
using SealabAPI.DataAccess.Entities;
using SealabAPI.DataAccess.Models.Constants;
using SealabAPI.DataAccess.Models;
using SealabAPI.DataAccess.Services;
using Microsoft.AspNetCore.Authorization;

namespace SealabAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ModuleController : BaseController<
        CreateModuleRequest,
        UpdateModuleRequest,
        DetailModuleResponse,
        Module>
    {
        private readonly ILogger<ModuleController> _logger;
        private readonly IModuleService _service;
        public ModuleController(ILogger<ModuleController> logger, IModuleService service) : base(service)
        {
            _logger = logger;
            _service = service;
        }
        [AllowAnonymous]
        [Authorize(Roles = "Student")]
        [HttpGet("submission/pa/{idStudent}")]
        public virtual ActionResult<List<ListSubmittedPAResponse>> GetListSubmittedPA(Guid idStudent)
        {
            try
            {
                List<ListSubmittedPAResponse> models = _service.GetListSubmittedPA(idStudent);
                return new SuccessApiResponse(string.Format(MessageConstant.Success), models);
            }
            catch (Exception ex)
            {
                return new ErrorApiResponse(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
        [AllowAnonymous]
        [Authorize(Roles = "Student")]
        [HttpGet("submission/prt/{idStudent}")]
        public virtual ActionResult<List<ListSubmittedPRTResponse>> GetListSubmittedPRT(Guid idStudent)
        {
            try
            {
                List<ListSubmittedPRTResponse> models = _service.GetListSubmittedPRT(idStudent);
                return new SuccessApiResponse(string.Format(MessageConstant.Success), models);
            }
            catch (Exception ex)
            {
                return new ErrorApiResponse(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
        [AllowAnonymous]
        [Authorize(Roles = "Student")]
        [HttpGet("submission/j/{idStudent}")]
        public virtual ActionResult<List<ListSubmittedJResponse>> GetListSubmittedJ(Guid idStudent)
        {
            try
            {
                List<ListSubmittedJResponse> models = _service.GetListSubmittedJ(idStudent);
                return new SuccessApiResponse(string.Format(MessageConstant.Success), models);
            }
            catch (Exception ex)
            {
                return new ErrorApiResponse(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
        [HttpPost("submission/list")]
        public virtual ActionResult<List<GetSubmissionsResponse>> GetSubmissionList(GetSubmissionsRequest model)
        {
            try
            {
                List<GetSubmissionsResponse> models = _service.GetSubmissions(model);
                return new SuccessApiResponse(string.Format(MessageConstant.Success), models);
            }
            catch (Exception ex)
            {
                return new ErrorApiResponse(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
        public override ActionResult<List<DetailModuleResponse>> GetList()
        {
            try
            {
                List<DetailModuleResponse> models = _baseService.GetAll<DetailModuleResponse>().OrderBy(x => x.SeelabsId).ToList();
                return new SuccessApiResponse(string.Format(MessageConstant.Success), models);
            }
            catch (Exception ex)
            {
                return new ErrorApiResponse(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
        [HttpPost("set-pa-status")]
        public async Task<ActionResult<List<DetailModuleResponse>>> PAStatus(SetPAStatusRequest model)
        {
            try
            {
                List<DetailModuleResponse> models = await _service.SetAssignmentStatus(model);
                return new SuccessApiResponse(string.Format(MessageConstant.Success), models);
            }
            catch (Exception ex)
            {
                return new ErrorApiResponse(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
        [HttpPost("set-prt-status")]
        public async Task<ActionResult<List<DetailModuleResponse>>> PRTStatus(SetPRTStatusRequest model)
        {
            try
            {
                List<DetailModuleResponse> models = await _service.SetAssignmentStatus(model);
                return new SuccessApiResponse(string.Format(MessageConstant.Success), models);
            }
            catch (Exception ex)
            {
                return new ErrorApiResponse(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
        [HttpPost("set-j-status")]
        public async Task<ActionResult<List<DetailModuleResponse>>> JStatus(SetJStatusRequest model)
        {
            try
            {
                List<DetailModuleResponse> models = await _service.SetAssignmentStatus(model);
                return new SuccessApiResponse(string.Format(MessageConstant.Success), models);
            }
            catch (Exception ex)
            {
                return new ErrorApiResponse(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
        /// <summary>
        /// This will delete all modules and related questions!, Use when reset season only!
        /// </summary>
        /// <param name = "confirm" > Type "delete all modules" </param>
        [HttpDelete("all")]
        public async Task<ActionResult> DeleteAllModules(string confirm)
        {
            try
            {
                if (confirm == "delete all modules")
                    await _service.DeleteAllModules();
                else
                    throw new ArgumentException("Type: 'delete all mpdules' to confirm");
                return new SuccessApiResponse(string.Format(MessageConstant.Success));
            }
            catch (Exception ex)
            {
                return new ErrorApiResponse(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
    }
}
