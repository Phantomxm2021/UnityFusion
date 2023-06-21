using UnityFusion.Editor.ABI;
using UnityFusion.Editor.Link;
using UnityFusion.Editor.Meta;
using UnityFusion.Editor.ReversePInvokeWrap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UnityFusion.Editor.Commands
{

    public static class ReversePInvokeWrapperGeneratorCommand
    {

        [MenuItem("UnityFusion/Generate/ReversePInvokeWrapper", priority = 103)]

        public static void CompileAndGenerateReversePInvokeWrapper()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            CompileDllCommand.CompileDll(target);
            GenerateReversePInvokeWrapper(target);
        }

        public static void GenerateReversePInvokeWrapper(BuildTarget target)
        {
            List<string> hotUpdateDlls = SettingsUtil.HotUpdateAssemblyNamesExcludePreserved;
            using (var cache = new AssemblyCache(MetaUtil.CreateHotUpdateAndAOTAssemblyResolver(target, hotUpdateDlls)))
            {
                var analyzer = new ReversePInvokeWrap.Analyzer(cache, hotUpdateDlls);
                analyzer.Run();


                string templateCode = File.ReadAllText($"{SettingsUtil.TemplatePathInPackage}/ReversePInvokeMethodStub.cpp");
                foreach (PlatformABI abi in Enum.GetValues(typeof(PlatformABI)))
                {
                    string outputFile = $"{SettingsUtil.GeneratedCppDir}/ReversePInvokeMethodStub_{abi}.cpp";

                    List<ABIReversePInvokeMethodInfo> methods = analyzer.BuildABIMethods(abi);
                    Debug.Log($"GenerateReversePInvokeWrapper. abi:{abi} wraperCount:{methods.Sum(m => m.Count)} output:{outputFile}");
                    var generator = new Generator();
                    generator.Generate(templateCode, abi, methods, outputFile);
                }
            }
            MethodBridgeGeneratorCommand.CleanIl2CppBuildCache();
        }
    }
}
