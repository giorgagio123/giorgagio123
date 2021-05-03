using PhoneStore.Core;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Core.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Services.Infrastructure.Logging
{
    public class DefaultLogger : ILogger
    {
        private readonly IRepository<Log> _logRepository;
        protected readonly WebHelper _webHelper;

        public DefaultLogger(IRepository<Log> logRepository, WebHelper webHelper)
        {
            _logRepository = logRepository;
            _webHelper = webHelper;
        }

        public void ClearLog()
        {
            throw new NotImplementedException();
        }

        public void DeleteLog(Log log)
        {
            throw new NotImplementedException();
        }

        public void DeleteLogs(IList<Log> logs)
        {
            throw new NotImplementedException();
        }

        public IPagedList<Log> GetAllLogs(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _logRepository.Table;

            return new PagedList<Log>(query, pageIndex, pageSize);
        }

        public Log GetLogById(int logId)
        {
            throw new NotImplementedException();
        }

        public IList<Log> GetLogByIds(int[] logIds)
        {
            throw new NotImplementedException();
        }

        public Log InsertLog(string request, string requestInformation, string response, string responseInformation)
        {
            var log = new Log
            {
                Request = request,
                RequestInformation = requestInformation,
                Response = response,
                ResponseInformation = responseInformation,
                IpAddress = _webHelper.GetCurrentIpAddress()
            };

            _logRepository.Insert(log);

            return log;
        }

        public void LogInformation(string request, string requestInformation, string response, string responseInformation)
        {
            InsertLog(request, requestInformation, response, responseInformation);
        }
    }
}
