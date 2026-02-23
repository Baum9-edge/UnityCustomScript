#nullable enable
using BaumCustomTemplate.Settings;
using BaumCustomTemplate.Utils;
using System;
using System.Linq;
using System.Text;

// NOTE:
// テンプレート加工処理を一括で扱うクラス。
// 今後カスタム処理の増加・複雑化が生じる場合、各処理を一段深い名前空間に分割し、
// ファイル単位で責務を分離すること。

namespace BaumCustomTemplate.ScriptGeneration
{
    public static class TemplateCustomizer
    {
        public static void ChangeEndOfLine(LineEnding ending, StringBuilder templateText)
        {
            if (ending == LineEnding.CrLf)
            {
                templateText.Replace("\n", "\r\n");
            }
        }

        public static void SetUsingDirective(StringBuilder templateText, TemplateType templateType, TemplateSettings settings)
        {
            string directive = templateType switch
            {
                TemplateType.Interface => settings.UsingForPureCSharp,
                TemplateType.PureCs => settings.UsingForPureCSharp,
                TemplateType.MonoBehaviour => settings.UsingForMonoBehaviour,
                TemplateType.MonoBehaviourDetailInspector => settings.UsingForMonoBehaviour,
                TemplateType.ScriptableObject => settings.UsingForScriptableObject,
                TemplateType.EditorWindow => settings.UsingForEditorWindow,
                _ => ""
            };
            templateText.Replace("#USING_DIRECTIVE#", directive);
        }

        public static void SetNullableFlag(StringBuilder templateText, TemplateType templateType, TemplateSettings settings)
        {
            // MonoBehaviourとMonoBehaviourDetailInspectorもしくはフラグFaleの場合はnullableフラグ無効
            var isMonoBehaviour =
                templateType == TemplateType.EditorWindow ||
                templateType == TemplateType.MonoBehaviour ||
                templateType == TemplateType.MonoBehaviourDetailInspector;
            var isDisable = isMonoBehaviour || settings.EnableNullableFlag == false;

            string nullableFlag = isDisable ? "" : "#nullable enable\n";
            templateText.Replace("#NULLABLE_FLAG#", nullableFlag);
        }

        public static void SetNamespaceHeader(StringBuilder templateText, TemplateType templateType, TemplateSettings settings)
        {
            string namespaceHeader = settings.NamespaceHeader;
            if (string.IsNullOrEmpty(namespaceHeader))
            {
                namespaceHeader = UnityEngine.Application.productName;
            }
            templateText.Replace("#NAMESPACE#", namespaceHeader);
        }
        public static void AppendSubnamespaces(StringBuilder templateText, string outputUnityDir, TemplateSettings settings)
        {
            var replaceText = String.Empty;
            var depth = settings.NamespaceDepthFromScriptFolders;
            if (depth == 0)
            {
                templateText.Replace("#FOLDER_PATH#", replaceText);
                return;
            }

            var suffixDirs = outputUnityDir
                .Split('/')
                .SkipWhile(x => !x.Equals("Scripts", System.StringComparison.OrdinalIgnoreCase))
                .Skip(1)                                       // Scripts の次から
                .Take(depth)                                   // N 個だけ
                .ToArray();

            if (suffixDirs.Length > 0)
                replaceText = $".{string.Join(".", suffixDirs)}";

            templateText.Replace("#FOLDER_PATH#", replaceText);
        }
    }
}
