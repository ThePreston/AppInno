using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AppInnovate.CDSurvey.Service.Models;

namespace Microsoft.AppInnovate.CDSurvey.Service.EF
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options, SqlConnection connection) : base(options)
        {
            _conn = connection;
        }

        private SqlConnection _conn;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conn);            
        }

        public DbSet<SurveyEntityModel> survey { get; set; }

    }
}