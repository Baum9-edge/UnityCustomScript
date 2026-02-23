using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Connect;
using UnityEngine;

/* 全体の流れとして、まず右クリック > スクリプト生成ボタン押下 > 
 * ファイルをシステム機能で読み込み、カスタム文字列を変換 > 
 * Temporaryディレクトリに保存 > パス取得 > 
 * Unityのスクリプト生成関数を実行（ここでTemporaryに書き出したカスタム文字列修正版テキスト指定） >
 * Unityが空スクリプト欄を用意 > ユーザがファイル名入力 >
 * Unity本来のカスタマイズテンプレート挿入機能が動作 >
 * カスタムテンプレートもUnityのテンプレートも反映されたカスタムテンプレートをもとに展開されたスクリプト生成完了。 */

namespace BaumCustomTemplate.Utils
{
    public static class TemplateTexts
    {
        private const string RelativeTemplateTextsPath = "../TemplateTexts~";

        public enum LineEnding
        {
            Lf,
            CrLf
        }

        public enum TemplateType
        {
            Interface,
            PureCs,
            MonoBehaviour,
            MonoBehaviourDetailInspector,
            ScriptableObject,
            EditorWindow
        }

        private readonly static Dictionary<TemplateType, string> TemplateTypeToFileName = new Dictionary<TemplateType, string>
        {
            { TemplateType.Interface, "Interface.txt" },
            { TemplateType.PureCs, "Pure_Cs.txt" },
            { TemplateType.MonoBehaviour, "MonoBehaviour.txt" },
            { TemplateType.MonoBehaviourDetailInspector, "MonoBehaviour_DetailInspector.txt" },
            { TemplateType.ScriptableObject, "ScriptableObject.txt" },
            { TemplateType.EditorWindow, "EditorWindow.txt" }
        };

        public static string GetTmplateTextAbsoluteOsPath(LineEnding lineEnding, TemplateType templateType)
        {
            string[] guids = AssetDatabase.FindAssets($"t:MonoScript {nameof(TemplateTexts)}");
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            string folder = System.IO.Path.GetDirectoryName(path);

            var scriptTextPath = System.IO.Path.Combine(folder, RelativeTemplateTextsPath, TemplateTypeToFileName[templateType]);

            // この時点のパスは区切り文字に\/混在のため、OS標準区切り文字の絶対パスに変換。
            var unityPath = scriptTextPath.Replace('\\', '/');
            string osPath = unityPath.Replace('/', System.IO.Path.DirectorySeparatorChar);
            string absoluteOsPath = System.IO.Path.GetFullPath(osPath);

            return absoluteOsPath;
        }
    }
}
