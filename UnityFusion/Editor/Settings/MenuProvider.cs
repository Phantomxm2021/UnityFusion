using UnityFusion.Editor.Installer;
using UnityEditor;
using UnityEngine;

public static class MenuProvider
{

    [MenuItem("UnityFusion/About UnityFusion", priority = 0)]
    public static void OpenAbout() => Application.OpenURL("https://unityfusion.doc.code-philosophy.com/docs/intro");

    [MenuItem("UnityFusion/Installer...", priority = 60)]
    private static void Open()
    {
        InstallerWindow window = EditorWindow.GetWindow<InstallerWindow>("UnityFusion Installer", true);
        window.minSize = new Vector2(800f, 500f);
    }

    [MenuItem("UnityFusion/Settings...", priority = 61)]
    public static void OpenSettings() => SettingsService.OpenProjectSettings("Project/UnityFusion Settings");

    [MenuItem("UnityFusion/Documents/Quick Start")]
    public static void OpenQuickStart() => Application.OpenURL("https://unityfusion.doc.code-philosophy.com/docs/beginner/quickstart");

    [MenuItem("UnityFusion/Documents/Performance")]
    public static void OpenPerformance() => Application.OpenURL("https://unityfusion.doc.code-philosophy.com/docs/basic/performance");

    [MenuItem("UnityFusion/Documents/FAQ")]
    public static void OpenFAQ() => Application.OpenURL("https://unityfusion.doc.code-philosophy.com/docs/help/faq");

    [MenuItem("UnityFusion/Documents/Common Errors")]
    public static void OpenCommonErrors() => Application.OpenURL("https://unityfusion.doc.code-philosophy.com/docs/help/commonerrors");

    [MenuItem("UnityFusion/Documents/Bug Report")]
    public static void OpenBugReport() => Application.OpenURL("https://unityfusion.doc.code-philosophy.com/docs/help/issue");
}

