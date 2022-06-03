using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.Services
{
    public class HistoryWrite : IHistoryWritable
    {
        public HistoryWrite()
        {
            writers = new List<IHistoryWriter>();
        }
        List<IHistoryWriter> writers;
        public void NotifyWriters(ActionExecutedContext context)
        {
            foreach (var w in writers)
                w.Update(new object());
        }

        public void RegisterObserver(IHistoryWriter o)
        {
            if(o != null)
            writers.Add(o);
        }

        public void RemoveObserver(IHistoryWriter o)
        {
            if (writers.Contains(o))
                writers.Remove(o);
        }
    }
}
