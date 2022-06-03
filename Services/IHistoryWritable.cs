using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.Services
{
    public interface IHistoryWritable
    {

        void RegisterObserver(IHistoryWriter o);
        void RemoveObserver(IHistoryWriter o);
        void NotifyWriters(ActionExecutedContext context);
    }
}
