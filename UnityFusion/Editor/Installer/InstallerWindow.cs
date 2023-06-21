using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace UnityFusion.Editor.Installer
{
    public class InstallerWindow : EditorWindow
    {
        private InstallerController _controller;

        private bool _installFromDir;

        private string _installLibil2cppWithUnityFusionSourceDir;

        private void OnEnable()
        {
            _controller = new InstallerController();
        }

        private void OnGUI()
        {
            var rect = new Rect
            {
                x = EditorGUIUtility.currentViewWidth - 24,
                y = 5,
                width = 24,
                height = 24
            };
            var content = EditorGUIUtility.IconContent("Settings");
            content.tooltip = "点击打开UnityFusion Settings";
            if (GUI.Button(rect, content, GUI.skin.GetStyle("IconButton")))
            {
                SettingsService.OpenProjectSettings("Project/UnityFusion Settings");
            }

            bool hasInstall = _controller.HasInstalledUnityFusion();

            GUILayout.Space(10f);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"安装状态：{(hasInstall ? "已安装" : "未安装")}", EditorStyles.boldLabel);
            GUILayout.Space(10f);


            EditorGUILayout.LabelField($"{SettingsUtil.PackageName} 版本:     v{_controller.PackageVersion}");
            GUILayout.Space(5f);
            EditorGUILayout.LabelField($"UnityFusionCLR 版本:    {_controller.UnityFusionLocalVersion}");
            GUILayout.Space(5f);
            EditorGUILayout.LabelField($"il2cpp_plus 版本:    {_controller.Il2cppPlusLocalVersion}");
            GUILayout.Space(5f);
            
            GUILayout.Space(10f);

            if (_controller.IsComaptibleVersion())
            {
                GUIInstallButton("安装unityfusion+il2cpp_plus代码到本地目录", "安装");
            }
            else
            {
                EditorGUILayout.HelpBox("与当前版本不兼容", MessageType.Error);
            }

            EditorGUILayout.EndVertical();
        }

        private void GUIInstallButton(string content, string button)
        {
            EditorGUILayout.BeginHorizontal();
            _installFromDir = EditorGUILayout.Toggle("从本地复制libil2cpp", _installFromDir);
            EditorGUI.BeginDisabledGroup(!_installFromDir);
            EditorGUILayout.TextField(_installLibil2cppWithUnityFusionSourceDir, GUILayout.Width(400));
            if (GUILayout.Button("选择目录", GUILayout.Width(100)))
            {
                _installLibil2cppWithUnityFusionSourceDir = EditorUtility.OpenFolderPanel("选择libil2cpp目录", Application.dataPath, "libil2cpp");
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(content);
            if (GUILayout.Button(button, GUILayout.Width(100)))
            {
                InstallLocalUnityFusion();
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndHorizontal();

        }

        private void InstallLocalUnityFusion()
        {
            if (_installFromDir)
            {
                if (!Directory.Exists(_installLibil2cppWithUnityFusionSourceDir))
                {
                    Debug.LogError($"本地libil2cpp复制目录不存在. '{_installLibil2cppWithUnityFusionSourceDir}'");
                    return;
                }
                if (!File.Exists($"{_installLibil2cppWithUnityFusionSourceDir}/il2cpp-config.h") || !File.Exists($"{_installLibil2cppWithUnityFusionSourceDir}/unityfusion/RuntimeApi.cpp"))
                {
                    Debug.LogError($"本地libil2cpp不是合法有效的源码目录. '{_installLibil2cppWithUnityFusionSourceDir}'");
                    return;
                }
                _controller.InstallFromLocal(_installLibil2cppWithUnityFusionSourceDir);
            }
            else
            {
                _controller.InstallDefaultUnityFusion();
            }
        }
    }
}
