using System.Collections.Generic;
using UnityEditor;

namespace BaumCustomTemplate.Utils
{
    public static class TemplateTexts
    {
        private const string RelativeTemplateTextsPath = "../TemplateTexts~";

        private readonly static Dictionary<TemplateType, string> s_TemplateTypeToFileName = new Dictionary<TemplateType, string>
        {
            { TemplateType.Interface, "Interface.txt" },
            { TemplateType.PureCs, "Pure_Cs.txt" },
            { TemplateType.MonoBehaviour, "MonoBehaviour.txt" },
            { TemplateType.MonoBehaviourDetailInspector, "MonoBehaviour_DetailInspector.txt" },
            { TemplateType.ScriptableObject, "ScriptableObject.txt" },
            { TemplateType.EditorWindow, "EditorWindow.txt" }
        };

        /// <summary>
        /// テンプレートファイルの絶対パスをOS標準区切り文字で取得する。
        /// </summary>
        /// <param name="templateType"></param>
        /// <returns></returns>
        public static string GetTmplateTextOsFullPath(TemplateType templateType)
        {
            string[] guids = AssetDatabase.FindAssets($"t:MonoScript {nameof(TemplateTexts)}");
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            string folder = System.IO.Path.GetDirectoryName(path);

            var scriptTextPath = System.IO.Path.Combine(folder, RelativeTemplateTextsPath, s_TemplateTypeToFileName[templateType]);

            // この時点のパスは区切り文字に\/混在のため、OS標準区切り文字の絶対パスに変換。
            var unityPath = scriptTextPath.Replace('\\', '/');
            string osPath = unityPath.Replace('/', System.IO.Path.DirectorySeparatorChar);
            string osFullPath = System.IO.Path.GetFullPath(osPath);

            return osFullPath;
        }
    }
}
