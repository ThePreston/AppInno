using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.AppInnovate.CDSurvey.Service.Models
{
    [Table("CDSurvey", Schema = "dbo" )]
    public class SurveyEntityModel    
    {
        [Key]
        public int Id { get; set; }

        public string role { get; set; }

        public string connectedProduct { get; set; }

        public string stage { get; set; }

        public string stageReason { get; set; }

        public string businessChallenge { get; set; }

        public string technicalChallenge { get; set; }

        public string strategicChallenge { get; set; }

        public string ioTSolution { get; set; }

        public string ioTSolutionOther { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; } //= DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool IsActive { get; set; } //= true;

    }
}
