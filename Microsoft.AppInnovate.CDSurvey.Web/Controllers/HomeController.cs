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

            var body = await new StreamReader(Request.Body).ReadToEndAsync();

            var dataObj = JsonConvert.DeserializeObject<jsonModel>(body);

            using var reqMessage = GetRequestMessage(dataObj); // body);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["SurveyAPIUrlHeader"]);
            //client.DefaultRequestHeaders.Accept
                  //.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resp = await client.SendAsync(reqMessage);

            _logger.LogInformation($"Status Code =  {resp.StatusCode}");
            _logger.LogInformation($"Message =  {await resp.Content.ReadAsStringAsync()}");

        }

        private HttpRequestMessage GetRequestMessage(jsonModel dataObj)
        {
            return new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_config["SurveyAPIUrl"]),
                Content = new StringContent(JsonConvert.SerializeObject(dataObj), Encoding.UTF8, "application/json")
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
