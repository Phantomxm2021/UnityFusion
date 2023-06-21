﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;

namespace UnityFusion.Editor.Commands
{
    public class CompileDllCommand
    {
        public static void CompileDll(string buildDir, BuildTarget target, bool developmentBuild)
        {
            var group = BuildPipeline.GetBuildTargetGroup(target);

            ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
            scriptCompilationSettings.group = group;
            scriptCompilationSettings.target = target;
            if (developmentBuild)
            {
                scriptCompilationSettings.options |= ScriptCompilationOptions.DevelopmentBuild;
            }
            Directory.CreateDirectory(buildDir);
            ScriptCompilationResult scriptCompilationResult = PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, buildDir);
#if UNITY_2022
            UnityEditor.EditorUtility.ClearProgressBar();
#endif
            Debug.Log("compile finish!!!");
        }

        public static void CompileDll(BuildTarget target, bool developmentBuild = false)
        {
            CompileDll(SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target), target, developmentBuild);
        }

        [MenuItem("UnityFusion/CompileDll/ActiveBuildTarget", priority = 100)]
        public static void CompileDllActiveBuildTarget()
        {
            CompileDll(EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("UnityFusion/CompileDll/ActiveBuildTarget_Development", priority = 101)]
        public static void CompileDllActiveBuildTargetDevelopment()
        {
            CompileDll(EditorUserBuildSettings.activeBuildTarget, true);
        }

        [MenuItem("UnityFusion/CompileDll/Win32", priority = 200)]
        public static void CompileDllWin32()
        {
            CompileDll(BuildTarget.StandaloneWindows);
        }

        [MenuItem("UnityFusion/CompileDll/Win64", priority = 201)]
        public static void CompileDllWin64()
        {
            CompileDll(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("UnityFusion/CompileDll/MacOS", priority = 202)]
        public static void CompileDllMacOS()
        {
            CompileDll(BuildTarget.StandaloneOSX);
        }

        [MenuItem("UnityFusion/CompileDll/Linux", priority = 203)]
        public static void CompileDllLinux()
        {
            CompileDll(BuildTarget.StandaloneLinux64);
        }

        [MenuItem("UnityFusion/CompileDll/Android", priority = 210)]
        public static void CompileDllAndroid()
        {
            CompileDll(BuildTarget.Android);
        }

        [MenuItem("UnityFusion/CompileDll/IOS", priority = 220)]
        public static void CompileDllIOS()
        {
            CompileDll(BuildTarget.iOS);
        }

        [MenuItem("UnityFusion/CompileDll/WebGL", priority = 230)]
        public static void CompileDllWebGL()
        {
            CompileDll(BuildTarget.WebGL);
        }
    }
}
