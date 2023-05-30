using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class GetFilesHandler : IGetFilesHandler
    {
        private readonly IQuery getFilesquery;
        public GetFilesHandler(IQuery getFilesquery)
        {
            this.getFilesquery = getFilesquery;
        }

        public FileManipulationResults GetFiles()
        {
            FileManipulationResults results;
            try
            {
                results = getFilesquery.GetResults();
                if (results.MSs == null || results.MSs.Count == 0)
                {
                    results.ResultType = ResultTypes.Warning;
                    results.ResultMessage = "The query returned 0 files";
                }
                else
                {
                    results.ResultMessage = $"The query returned {results.MSs.Count} files.";
                }
            }
            catch (Exception ex)
            {
                results = new FileManipulationResults
                {
                    ResultType = ResultTypes.Failed,
                    ResultMessage = ex.Message
                };
            }
            return results;
        }
    }
}
