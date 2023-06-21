using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityFusion.Editor.Meta
{
    public interface IAssemblyResolver
    {
        string ResolveAssembly(string assemblyName, bool throwExIfNotFind);
    }
}
