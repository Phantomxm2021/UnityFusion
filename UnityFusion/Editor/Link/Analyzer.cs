using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using UnityFusion.Editor.Meta;
using UnityEditor;
using IAssemblyResolver = UnityFusion.Editor.Meta.IAssemblyResolver;

namespace UnityFusion.Editor.Link
{
    public class Analyzer
    {
        private readonly IAssemblyResolver _resolver;

        public Analyzer(IAssemblyResolver resolver)
        {
            _resolver = resolver;
        }

        public HashSet<TypeRef> CollectRefs(List<string> rootAssemblies)
        {
            using (var assCollector = new AssemblyCache(_resolver))
            {
                var rootAssemblyNames = new HashSet<string>(rootAssemblies);

                var typeRefs = new HashSet<TypeRef>(TypeEqualityComparer.Instance);
                foreach (var rootAss in rootAssemblies)
                {
                    var dnAss = assCollector.LoadModule(rootAss, false);
                    foreach (var type in dnAss.GetTypeRefs())
                    {
                        if (!rootAssemblyNames.Contains(type.DefinitionAssembly.Name.ToString()))
                        {
                            typeRefs.Add(type);
                        }
                    }
                }

                assCollector.Dispose();
                return typeRefs;
            }
        }
    }
}
