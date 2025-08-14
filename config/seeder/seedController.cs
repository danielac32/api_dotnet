using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend_ont_2.seeder;
using backend_ont_2.shared.apiResponse;

namespace backend_ont_2.seeder.controller
{
    [ApiController]
    [Route("seed/")]
    public class SeederController : ControllerBase
    {
        
        private readonly Seeder _seeder;
        private readonly ApiResponseService _apiResponseService;
        public SeederController(Seeder seeder, ApiResponseService apiResponseService)
        {
            _seeder = seeder;
            _apiResponseService = apiResponseService;
        }

        [HttpGet]
        public async Task<IActionResult> seed()
        {
            return await _apiResponseService.Execute(async () =>
            {
                await _seeder.Run();
                return _apiResponseService.OkResponse(message: "Seeder ejecutado!");
            });
        }
    }
}