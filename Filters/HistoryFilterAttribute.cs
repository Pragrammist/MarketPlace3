using HukSleva.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.Filters
{
    public class HistoryFilterAttribute : Attribute, IActionFilter
    { 
        IHistoryWritable _history;
        public HistoryFilterAttribute(IHistoryWritable history)
        {
            
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
            _history.NotifyWriters(context);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
