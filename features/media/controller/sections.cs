// Controllers/resumenController.cs
using Microsoft.AspNetCore.Mvc;
using backend_ont_2.shared.apiResponse;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

namespace backend_ont_2.features.media.controller.sections
{
    [ApiController]
    [Route("sections")]
    public class SectionsController : ControllerBase
    {

        private readonly ApiResponseService _apiResponseService;
        public SectionsController(ApiResponseService apiResponseService)
        {
            _apiResponseService = apiResponseService;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {

            return _apiResponseService.OkResponse(new { sections= AppStrings.Sections });

        }
    }
}