using Microsoft.AppInnovate.CDSurvey.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AppInnovate.CDSurvey.Model
{
    public class CDSurvey
    {

        public string role { get; set; }

        public string connectedProduct { get; set; }

        public string stage { get; set; }

        public string stageOther { get; set; }

        public string businessChallenge { get; set; }

        public string technicalChallenge { get; set; }

        public string strategicChallenge { get; set; }

        public string ioTSolution { get; set; }

        public string ioTSolutionOther { get; set; }
    }

    public static class SurveyMap { 
        public static SurveyEntityModel ToEntity(this CDSurvey survey)
        {
            return new SurveyEntityModel() {
                role = survey.role,
                connectedProduct = survey.connectedProduct,
                stage = survey.stage,
                stageReason = survey.stageOther,
                businessChallenge = survey.businessChallenge,
                strategicChallenge = survey.strategicChallenge,
                ioTSolution = survey.ioTSolution,
                ioTSolutionOther = survey.ioTSolutionOther,
                technicalChallenge = survey.technicalChallenge
            };
        }
    }
}
