using UnityFusion.Editor.AOT;
using UnityFusion.Editor.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UnityFusion.Editor.Commands
{
    using Analyzer = UnityFusion.Editor.AOT.Analyzer;
    public static class AOTReferenceGeneratorCommand
    {

        [MenuItem("UnityFusion/Generate/AOTGenericReference", priority = 102)]
        public static void CompileAndGenerateAOTGenericReference()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            CompileDllCommand.CompileDll(target);
            GenerateAOTGenericReference(target);
        }

        public static void GenerateAOTGenericReference(BuildTarget target)
        {
            var gs = SettingsUtil.UnityFusionSettings;
            List<string> hotUpdateDllNames = SettingsUtil.HotUpdateAssemblyNamesExcludePreserved;

            using (AssemblyReferenceDeepCollector collector = new AssemblyReferenceDeepCollector(MetaUtil.CreateHotUpdateAndAOTAssemblyResolver(target, hotUpdateDllNames), hotUpdateDllNames))
            {
                var analyzer = new Analyzer(new Analyzer.Options
                {
                    MaxIterationCount = Math.Min(20, gs.maxGenericReferenceIteration),
                    Collector = collector,
                });

                analyzer.Run();

                var writer = new GenericReferenceWriter();
                writer.Write(analyzer.AotGenericTypes.ToList(), analyzer.AotGenericMethods.ToList(), $"{Application.dataPath}/{gs.outputAOTGenericReferenceFile}");
                AssetDatabase.Refresh();
            }
        }
    }
}
