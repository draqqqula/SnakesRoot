using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEngine.Profiling.Interfaces;

public interface IProfileAggregator
{
    public TimeSpan GetTotalExecutionTime(object target);
    public void CreateSnapshot(Stream stream);
}
