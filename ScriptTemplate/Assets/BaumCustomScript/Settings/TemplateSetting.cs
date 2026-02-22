#nullable enable
using UnityEngine;
using UnityEditor;
using System.IO;

namespace BaumCustomTemplate.Settings
{
    public class TemplateSettings : ScriptableObject
    {
        // ScriptableObject の保存先
        public const string AssetPath = "Assets/Baum/CustomScripts/Setting.asset";

        [Header("Using Directive設定")]
        [SerializeField] private string m_UsingForPureCSharp = DefaultPureCsDirective;
        [SerializeField] private string m_UsingForMonoBehaviour = DefaultMonoDirective;
        [SerializeField] private string m_UsingForScriptableObject = DefaultScriptableObjectDirective;
        [SerializeField] private string m_UsingForEditorWindow = DefaultEditorWindowDirective;

        [Header("名前空間の先頭部。空の場合はProdctNameを強制適用")]
        [SerializeField] private string m_NamespaceHeader = "";

        [Header("Scriptsより下のディレクトリ構造を名前空間に追加するか")]
        [SerializeField] private bool m_UseUnderScriptsDirectoryStructureForNamespace = true;

        [Header("nullableフラグを有効にするか（Monovihaviourはフラグに関係なく無効）")]
        [SerializeField] private bool m_EnableNullableFlagWithoutMonobehaviour = true;


        // 各設定の外部アクセス用プロパティ
        public string UsingForPureCSharp => m_UsingForPureCSharp;
        public string UsingForMonoBehaviour => m_UsingForMonoBehaviour;
        public string UsingForScriptableObject => m_UsingForScriptableObject;
        public string UsingForEditorWindow => m_UsingForEditorWindow;
        public bool UseUnderScriptsDirectoryStructureForNamespace => m_UseUnderScriptsDirectoryStructureForNamespace;
        public bool EnableNullableFlag => m_EnableNullableFlagWithoutMonobehaviour;
        public string NamespaceHeader => m_NamespaceHeader;

        public static TemplateSettings Select()
        {
            var settings = AssetDatabase.LoadAssetAtPath<TemplateSettings>(AssetPath);
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