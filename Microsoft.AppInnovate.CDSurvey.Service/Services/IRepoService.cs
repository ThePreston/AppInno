using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AppInnovate.CDSurvey.Service
{
    public interface IRepoService<T> where T : class
    {
        public Task<IList<T>> GetData();

        public IQueryable<T> GetDataAsQueryable();

        public Task<bool> InsertData(T data);

    }
}