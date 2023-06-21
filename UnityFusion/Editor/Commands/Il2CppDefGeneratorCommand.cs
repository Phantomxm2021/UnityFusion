using UnityFusion.Editor.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityFusion.Editor.Commands
{

    public static class Il2CppDefGeneratorCommand
    {

        [MenuItem("UnityFusion/Generate/Il2CppDef", priority = 104)]
        public static void GenerateIl2CppDef()
        {
            var options = new Il2CppDef.Il2CppDefGenerator.Options()
            {
                UnityVersion = Application.unityVersion,
                HotUpdateAssemblies = SettingsUtil.HotUpdateAssemblyNamesIncludePreserved,
                OutputFile = $"{SettingsUtil.LocalIl2CppDir}/libil2cpp/unityfusion/generated/UnityVersion.h",
                OutputFile2 = $"{SettingsUtil.LocalIl2CppDir}/libil2cpp/unityfusion/generated/AssemblyManifest.cpp",
            };

            var g = new Il2CppDef.Il2CppDefGenerator(options);
            g.Generate();
        }
    }
}
