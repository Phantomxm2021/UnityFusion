using UnityFusion.Editor.Link;
using UnityFusion.Editor.Meta;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityFusion.Editor.Commands
{
    using Analyzer = UnityFusion.Editor.Link.Analyzer;

    public static class LinkGeneratorCommand
    {

        [MenuItem("UnityFusion/Generate/LinkXml", priority = 100)]
        public static void GenerateLinkXml()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            CompileDllCommand.CompileDll(target);
            GenerateLinkXml(target);
        }

        public static void GenerateLinkXml(BuildTarget target)
        {
            var ls = SettingsUtil.UnityFusionSettings;

            List<string> hotfixAssemblies = SettingsUtil.HotUpdateAssemblyNamesExcludePreserved;

            var analyzer = new Analyzer(MetaUtil.CreateHotUpdateAndAOTAssemblyResolver(target, hotfixAssemblies));
            var refTypes = analyzer.CollectRefs(hotfixAssemblies);

            Debug.Log($"[LinkGeneratorCommand] hotfix assembly count:{hotfixAssemblies.Count}, ref type count:{refTypes.Count} output:{Application.dataPath}/{ls.outputLinkFile}");
            var linkXmlWriter = new LinkXmlWriter();
            linkXmlWriter.Write($"{Application.dataPath}/{ls.outputLinkFile}", refTypes);
            AssetDatabase.Refresh();
        }
    }
}
