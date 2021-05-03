using PhoneStore.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Core.Infrastructure.Logging
{
    public interface ILogger
    {
        void DeleteLog(Log log);
        
        void DeleteLogs(IList<Log> logs);

        /// <summary>
        /// Clears a log
        /// </summary>
        void ClearLog();
        
        IPagedList<Log> GetAllLogs(int pageIndex = 0, int pageSize = int.MaxValue);
        
        Log GetLogById(int logId);
        
        IList<Log> GetLogByIds(int[] logIds);
        
        Log InsertLog(string request, string requestInformation, string response, string responseInformation);

        void LogInformation(string request, string requestInformation, string response, string responseInformation);
    }
}
