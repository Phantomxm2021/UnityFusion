using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace UnityFusion.Editor
{
    public static class SettingsUtil
    {
        public static bool Enable
        { 
            get => UnityFusionSettings.Instance.enable;
            set 
            {
                UnityFusionSettings.Instance.enable = value;
                UnityFusionSettings.Save();
            }
        }

        public static string PackageName { get; } = "UnityFusion";

        public static string UnityFusionDataPathInPackage => $"Packages/{PackageName}/Data~";

        public static string TemplatePathInPackage => $"{UnityFusionDataPathInPackage}/Templates";

        public static string ProjectDir { get; } = Directory.GetParent(Application.dataPath).ToString();

        public static string ScriptingAssembliesJsonFile { get; } = "ScriptingAssemblies.json";

        public static string HotUpdateDllsRootOutputDir => UnityFusionSettings.Instance.hotUpdateDllCompileOutputRootDir;

        public static string AssembliesPostIl2CppStripDir => UnityFusionSettings.Instance.strippedAOTDllOutputRootDir;

        public static string UnityFusionDataDir => $"{ProjectDir}/UnityFusionData";

        public static string LocalUnityDataDir => $"{UnityFusionDataDir}/LocalIl2CppData-{Application.platform}";

        public static string LocalIl2CppDir => $"{LocalUnityDataDir}/il2cpp";

        public static string GeneratedCppDir => $"{LocalIl2CppDir}/libil2cpp/unityfusion/generated";

        public static string Il2CppBuildCacheDir { get; } = $"{ProjectDir}/Library/Il2cppBuildCache";

        public static string GetHotUpdateDllsOutputDirByTarget(BuildTarget target)
        {
            return $"{HotUpdateDllsRootOutputDir}/{target}";
        }

        public static string GetAssembliesPostIl2CppStripDir(BuildTarget target)
        {
            return $"{AssembliesPostIl2CppStripDir}/{target}";
        }

        class AssemblyDefinitionData
        {
            public string name;
        }

        /// <summary>
        /// 热更新dll列表。不包含 preserveHotUpdateAssemblies。
        /// </summary>
        public static List<string> HotUpdateAssemblyNamesExcludePreserved
        {
            get
            {
                var gs = UnityFusionSettings.Instance;
                var hotfixAssNames = (gs.hotUpdateAssemblyDefinitions ?? Array.Empty<AssemblyDefinitionAsset>()).Select(ad => JsonUtility.FromJson<AssemblyDefinitionData>(ad.text));

                var hotfixAssembles = new List<string>();
                foreach (var assName in hotfixAssNames)
                {
                    hotfixAssembles.Add(assName.name);
                }
                hotfixAssembles.AddRange(gs.hotUpdateAssemblies ?? Array.Empty<string>());
                return hotfixAssembles.ToList();
            }
        }

        public static List<string> HotUpdateAssemblyFilesExcludePreserved => HotUpdateAssemblyNamesExcludePreserved.Select(dll => dll + ".dll").ToList();


        public static List<string> HotUpdateAssemblyNamesIncludePreserved
        {
            get
            {
                List<string> allAsses = HotUpdateAssemblyNamesExcludePreserved;
                string[] preserveAssemblyNames = UnityFusionSettings.Instance.preserveHotUpdateAssemblies;
                if (preserveAssemblyNames != null && preserveAssemblyNames.Length > 0)
                {
                    foreach (var assemblyName in preserveAssemblyNames)
                    {
                        if (allAsses.Contains(assemblyName))
                        {
                            throw new Exception($"[HotUpdateAssemblyNamesIncludePreserved] assembly:'{assemblyName}' 重复");
                        }
                        allAsses.Add(assemblyName);
                    }
                }

                return allAsses;
            }
        }

        public static List<string> HotUpdateAssemblyFilesIncludePreserved => HotUpdateAssemblyNamesIncludePreserved.Select(ass => ass + ".dll").ToList();

        public static List<string> AOTAssemblyNames => UnityFusionSettings.Instance.patchAOTAssemblies.ToList();

        public static UnityFusionSettings UnityFusionSettings => UnityFusionSettings.Instance;
    }
}
