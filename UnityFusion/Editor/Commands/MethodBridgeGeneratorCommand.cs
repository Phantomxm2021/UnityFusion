﻿using UnityFusion.Editor;
using UnityFusion.Editor.ABI;
using UnityFusion.Editor.Meta;
using UnityFusion.Editor.MethodBridge;
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
    using Analyzer = UnityFusion.Editor.MethodBridge.Analyzer;
    public class MethodBridgeGeneratorCommand
    {

        public static void CleanIl2CppBuildCache()
        {
            string il2cppBuildCachePath = SettingsUtil.Il2CppBuildCacheDir;
            if (!Directory.Exists(il2cppBuildCachePath))
            {
                return;
            }
            Debug.Log($"clean il2cpp build cache:{il2cppBuildCachePath}");
            Directory.Delete(il2cppBuildCachePath, true);
        }

        private static void GenerateMethodBridgeCppFile(Analyzer analyzer, PlatformABI platform, string templateCode, string outputFile)
        {
            var g = new Generator(new Generator.Options()
            {
                PlatformABI = platform,
                TemplateCode = templateCode,
                OutputFile = outputFile,
                GenericMethods = analyzer.GenericMethods,
                NotGenericMethods = analyzer.NotGenericMethods,
                SpeicalPreserveMethods = analyzer.SpeicalPreserveMethods,
            });

            g.PrepareMethods();
            g.Generate();
            Debug.LogFormat("== output:{0} ==", outputFile);
        }

        [MenuItem("UnityFusion/Generate/MethodBridge", priority = 101)]
        public static void CompileAndGenerateMethodBridge()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            CompileDllCommand.CompileDll(target);
            GenerateMethodBridge(target);
        }

        public static void GenerateMethodBridge(BuildTarget target)
        {
            List<string> hotUpdateDllNames = SettingsUtil.HotUpdateAssemblyNamesExcludePreserved;
            using (AssemblyReferenceDeepCollector collector = new AssemblyReferenceDeepCollector(MetaUtil.CreateHotUpdateAndAOTAssemblyResolver(target, hotUpdateDllNames), hotUpdateDllNames))
            {
                var analyzer = new Analyzer(new Analyzer.Options
                {
                    MaxIterationCount = Math.Min(20, SettingsUtil.UnityFusionSettings.maxMethodBridgeGenericIteration),
                    Collector = collector,
                });

                analyzer.Run();

                var tasks = new List<Task>();
                string templateCode = File.ReadAllText($"{SettingsUtil.TemplatePathInPackage}/MethodBridgeStub.cpp");
                foreach (PlatformABI platform in Enum.GetValues(typeof(PlatformABI)))
                {
                    string outputFile = $"{SettingsUtil.GeneratedCppDir}/MethodBridge_{platform}.cpp";
                    tasks.Add(Task.Run(() =>
                    {
                        GenerateMethodBridgeCppFile(analyzer, platform, templateCode, outputFile);
                    }));
                }
                Task.WaitAll(tasks.ToArray());
            }

            CleanIl2CppBuildCache();
        }
    }
}
