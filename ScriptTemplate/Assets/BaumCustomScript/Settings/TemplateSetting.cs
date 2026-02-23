#nullable enable
using UnityEngine;
using UnityEditor;
using System.IO;
using BaumCustomTemplate.Utils;

namespace BaumCustomTemplate.Settings
{
    public class TemplateSettings : ScriptableObject
    {
        [MenuItem("Baum/CustomTemplate/SelectSettings")]
        private static void SelectTemplateSettings() => Selection.activeObject = Select();

        // ScriptableObject の保存先
        public const string AssetPath = "Assets/Baum/CustomScripts/Setting.asset";

        [Header("Using Directive設定")]
        [SerializeField] private string m_UsingForPureCSharp = DefaultPureCsDirective;
        [SerializeField] private string m_UsingForMonoBehaviour = DefaultMonoDirective;
        [SerializeField] private string m_UsingForScriptableObject = DefaultScriptableObjectDirective;
        [SerializeField] private string m_UsingForEditorWindow = DefaultEditorWindowDirective;

        [Header("名前空間の先頭部。空の場合はProdctNameを強制適用")]
        [SerializeField] private string m_NamespaceHeader = "";

        [Header("Scriptsより下のディレクトリ構造を深さNまで名前空間に追加する。0:無効, 1:Scripts直下のディレクトリのみ名前空間に反映")]
        [SerializeField] private int m_NamespaceDepthFromScriptFolders = 0;

        [Header("nullableフラグを有効にするか（Monovihaviourはフラグに関係なく無効）")]
        [SerializeField] private bool m_EnableNullableFlagWithoutMonobehaviour = false;

        [Header("ファイルの改行コード")]
        [SerializeField] private LineEnding m_LineEnding = LineEnding.Lf;


        // 各設定の外部アクセス用プロパティ
        public string UsingForPureCSharp => m_UsingForPureCSharp;
        public string UsingForMonoBehaviour => m_UsingForMonoBehaviour;
        public string UsingForScriptableObject => m_UsingForScriptableObject;
        public string UsingForEditorWindow => m_UsingForEditorWindow;
        public string NamespaceHeader => m_NamespaceHeader;
        public int NamespaceDepthFromScriptFolders => m_NamespaceDepthFromScriptFolders;
        public bool EnableNullableFlag => m_EnableNullableFlagWithoutMonobehaviour;
        public LineEnding LineEnding => m_LineEnding;

        public static TemplateSettings Select()
        {
            TemplateSettings settings = AssetDatabase.LoadAssetAtPath<TemplateSettings>(AssetPath);
            if (settings != null)
                return settings;

            // フォルダ作成
            string folder = Path.GetDirectoryName(AssetPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                AssetDatabase.Refresh();
            }

            // 新規作成
            settings = CreateInstance<TemplateSettings>();

            // Namespace が空ならプロジェクト名を自動設定
            if (string.IsNullOrEmpty(settings.m_NamespaceHeader))
            {
                string projectName = Application.productName.Replace(" ", "");
                settings.m_NamespaceHeader = projectName;
            }

            AssetDatabase.CreateAsset(settings, AssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return settings;
        }

        // ================================
        // Default Using Directives
        // ================================
        private const string DefaultPureCsDirective = @"";
        private const string DefaultMonoDirective =
@"using UnityEngine;
using System.Collections;";
        private const string DefaultScriptableObjectDirective =
@"using UnityEngine;
using UnityEditor;";
        private const string DefaultEditorWindowDirective =
@"using UnityEngine;
using UnityEditor;";
    }
}