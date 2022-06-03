using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.Services
{
    public interface IHistoryWriter
    {
        public void Update(object obj);
    }
}
