﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;
using MonoHook;
using UnityFusion.Editor.BuildProcessors;
using System.IO;

namespace UnityFusion.MonoHook
{
#if UNITY_2021 && UNITY_IOS
    [InitializeOnLoad]
    public class CopyStrippedAOTAssembliesHook
    {
        private static MethodHook _hook;

        static CopyStrippedAOTAssembliesHook()
        {
            if (_hook == null)
            {
                Type type = typeof(UnityEditor.EditorApplication).Assembly.GetType("UnityEditorInternal.AssemblyStripper");
                MethodInfo miTarget = type.GetMethod("StripAssembliesTo", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

                MethodInfo miReplacement = new StripAssembliesDel(OverrideStripAssembliesTo).Method;
                MethodInfo miProxy = new StripAssembliesDel(StripAssembliesToProxy).Method;

                _hook = new MethodHook(miTarget, miReplacement, miProxy);
                _hook.Install();
            }
        }

        private delegate bool StripAssembliesDel(string outputFolder, out string output, out string error, IEnumerable<string> linkXmlFiles, object runInformation);

        private static bool OverrideStripAssembliesTo(string outputFolder, out string output, out string error, IEnumerable<string> linkXmlFiles, object runInformation)
        {
            bool result = StripAssembliesToProxy(outputFolder, out output, out error, linkXmlFiles, runInformation);
            if (!result)
            {
                return false;
            }
            UnityEngine.Debug.Log($"== StripAssembliesTo outputDir:{outputFolder}");
            string outputStrippedDir = UnityFusion.Editor.SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget);
            Directory.CreateDirectory(outputStrippedDir);
            foreach (var aotDll in Directory.GetFiles(outputFolder, "*.dll"))
            {
                string dstFile = $"{outputStrippedDir}/{Path.GetFileName(aotDll)}";
                Debug.Log($"[RunAssemblyStripper] copy aot dll {aotDll} -> {dstFile}");
                File.Copy(aotDll, dstFile, true);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool StripAssembliesToProxy(string outputFolder, out string output, out string error, IEnumerable<string> linkXmlFiles, object runInformation)
        {
            Debug.LogError("== StripAssembliesToProxy ==");
            output = "";
            error = "";
            return true;
        }
    }
#endif
}
