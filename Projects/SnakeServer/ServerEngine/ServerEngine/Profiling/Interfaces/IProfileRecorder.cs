using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEngine.Profiling.Interfaces;

public interface IProfileRecorder
{
    public void Start(object target);
    public void Stop(object target);
}
