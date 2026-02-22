
using BaumCustomTemplate.Utils;
using System.IO;
using UnityEditor;

namespace BaumCustomTemplate.UI
{
    public class RightClickMenu
    {
        // 右クリック展開メニューの位置調整用定数
        private const int TopOrder = -512;

        // 右クリック展開メニューにおける水平線挟み込み定数。
        // 一つ上のメニューとの差が10 より お お き い 場合、水平線が挟み込まれる（すなわち11以上）。
        private const int SeparatorBoundary = 10;

        /// <summary>
        /// 右クリックメニューが展開されたときの、選択フォルダパス
        /// </summary>
        private static string GetSelectedFolder()
        {
            UnityEngine.Object obj = Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path))
                return "Assets";

            if (!AssetDatabase.IsValidFolder(path))
                path = Path.GetDirectoryName(path);

            // Unityのパス区切りは常にスラッシュであるため、バックスラッシュをスラッシュに置換。
            return path.Replace('\\', '/');
        }

        [MenuItem("Assets/Create/Script/Interface", false, TopOrder + SeparatorBoundary * 0 + 1)]
        private static void CreateInterface()
        {
            var folderPath = GetSelectedFolder();
            CreateScript(folderPath);
        }
        [MenuItem("Assets/Create/Script/Pure_C#", false, TopOrder + SeparatorBoundary * 0 + 2)]
        private static void CreatePureCSharp()
        {
            var folderPath = GetSelectedFolder();
            CreateScript(folderPath);
        }

        [MenuItem("Assets/Create/Script/MonoBehaviour", false, TopOrder + SeparatorBoundary * 2 + 1)]
        private static void CreateMonoBehaviour()
        {
            var folderPath = GetSelectedFolder();
            CreateScript(folderPath);
        }
        [MenuItem("Assets/Create/Script/MonoBehaviour_DetailInspector", false, TopOrder + SeparatorBoundary * 2 + 2)]
        private static void CreateMonoBehaviourDetail()
        {
            var folderPath = GetSelectedFolder();
            CreateScript(folderPath);
        }

        [MenuItem("Assets/Create/Script/ScriptableObject", false, TopOrder + SeparatorBoundary * 4 + 1)]
        private static void CreateScriptableObject()
        {
            var folderPath = GetSelectedFolder();
            CreateScript(folderPath);
        }
        [MenuItem("Assets/Create/Script/EditorWindow", false, TopOrder + SeparatorBoundary * 4 + 2)]
        private static void CreateEditorWindow()
        {
            var folderPath = GetSelectedFolder();
            CreateScript(folderPath);
        }

        private static void CreateScript(string folderPath)
        {
            var path = TemplateTexts.GetTmplateTextAbsoluteOsPath(TemplateTexts.LineEnding.Lf, TemplateTexts.TemplateType.Interface);
            UnityEngine.Debug.Log($"Selected folder: {folderPath}");
            UnityEngine.Debug.Log($"Use script: {path}");

            if(System.IO.File.Exists(path))
            {
                UnityEngine.Debug.Log("File exists.");
            }
        }
    }
}
