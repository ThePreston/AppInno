using Microsoft.AppInnovate.CDSurvey.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Microsoft.AppInnovate.CDSurvey.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task SubmitSurvey()
        {
            _logger.LogInformation($"SubmitSurvey");

            try
            {

                var body = await new StreamReader(Request.Body).ReadToEndAsync();

                using var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["SurveyAPIUrlHeader"]);
                
                var resp = await client.SendAsync(GetRequestMessage(body));

                _logger.LogInformation($"Status Code =  {resp.StatusCode}");
                _logger.LogInformation($"Message =  {await resp.Content.ReadAsStringAsync()}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }

        private HttpRequestMessage GetRequestMessage(string bodyContent)
        {
            return new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_config["SurveyAPIUrl"]),
                Content = new StringContent(bodyContent, Encoding.UTF8, "application/json")
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
