using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AppInnovate.CDSurvey.Service;
using Microsoft.AppInnovate.CDSurvey.Service.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using jsonModel = Microsoft.AppInnovate.CDSurvey.Model;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Microsoft.AppInnovate.CDSurvey.Model;

namespace Microsoft.AppInnovate.CDSurvey.Controllers
{

    [Route("api/")]
    [ApiController]
    public class AppInnoSurveyController : ControllerBase
    {
        public AppInnoSurveyController(IRepoService<SurveyEntityModel> efService, ILogger<AppInnoSurveyController> logger)
        {
            efSvc = efService;
            _logger = logger;
        }

        private readonly IRepoService<SurveyEntityModel> efSvc;

        private readonly ILogger<AppInnoSurveyController> _logger;

        [HttpPost("Survey")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> InsertData([FromBody]jsonModel.CDSurvey surveyModel)
        {
            _logger.LogInformation("Entered GetProdEFData");

            try
            {

                var surveyEFModel = new SurveyEntityModel();
                try
                {
                    surveyEFModel = surveyModel.ToEntity();
                }
                catch (Exception modelEx)
                {
                    _logger.LogError(modelEx, $"JSON Serialization Issue = {surveyModel}");
                    return BadRequest(modelEx);
                }

                await efSvc.InsertData(surveyEFModel);
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

        [HttpGet("Survey")]
        [EnableQuery]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SurveyEntityModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IQueryable<SurveyEntityModel>> GetData()
        {
            _logger.LogInformation("Entered GetData");
            try
            {
                return Ok(efSvc.GetDataAsQueryable());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

    }
}