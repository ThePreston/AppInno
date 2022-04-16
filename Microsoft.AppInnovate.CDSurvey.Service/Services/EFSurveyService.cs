using System;
using Microsoft.AppInnovate.CDSurvey.Service.EF;
using Microsoft.AppInnovate.CDSurvey.Service.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AppInnovate.CDSurvey.Service
{
    public class EFSurveyService : IRepoService<SurveyEntityModel>
    {
        public EFSurveyService(SurveyContext context, ILogger<EFSurveyService> logger)
        {
            _context = context;
            _logger = logger;
        }

        private readonly ILogger<EFSurveyService> _logger;

        private readonly SurveyContext _context;

        public async Task<IList<SurveyEntityModel>> GetData()
        {
            return await new TaskFactory().StartNew(() => { return _context.survey.ToList(); });
        }

        public IQueryable<SurveyEntityModel> GetDataAsQueryable() => _context.survey.AsQueryable();


        public async Task<bool> InsertData(SurveyEntityModel  surveyEntityModel)
        {
            _logger.LogInformation("Entered into Insert data");

            _context.survey.Add(surveyEntityModel);
            return await _context.SaveChangesAsync() > 0;

        }
    }
}