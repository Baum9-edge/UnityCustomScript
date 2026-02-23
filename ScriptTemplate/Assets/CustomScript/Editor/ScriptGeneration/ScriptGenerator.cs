#nullable enable
using BaumCustomTemplate.Settings;
using BaumCustomTemplate.Utils;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;


namespace BaumCustomTemplate.ScriptGeneration
{
    public class ScriptGenerator
    {
        private const int NewAssetInstanceId = 0; // 新規アセットのインスタンスIDは常に0

        private static readonly string s_TempPath = Path.Combine(Application.temporaryCachePath,"baumTempScript.txt");
        public static bool Generate(string outputUnityDir, TemplateType templateType)
        {
            var templateOsFullPath = TemplateTexts.GetTmplateTextOsFullPath(templateType);
            if (!File.Exists(templateOsFullPath))
            {
                Debug.LogError($"テンプレートファイル {templateOsFullPath} が見つかりません。");
                return false;
            }

            // テンプレートテキストを読み込み、カスタム設定に従って調整する。
            var templateText = new StringBuilder(File.ReadAllText(templateOsFullPath));
            EndNameEditAction endNameEditAction = CustomizeTemplateWithSettings(templateText, templateType, outputUnityDir);

            // カスタムテンプレートの調整完了。一時領域にテンプレートファイルを上書き保存
            File.WriteAllText(s_TempPath, templateText.ToString(), Encoding.UTF8);

            var icon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                NewAssetInstanceId,
                endNameEditAction,
                $"{templateType.ToString()}.cs",
                icon,
                s_TempPath
            );
            return true;
        }

        private static EndNameEditAction CustomizeTemplateWithSettings(StringBuilder templateText, TemplateType templateType, string outputUnityDir)
        {
            var generateSettings = TemplateSettings.Select();

            // カスタムテンプレートのusingディレクティブを置換
            TemplateCustomizer.SetUsingDirective(templateText, templateType, generateSettings);
            // nullableフラグ設定
            TemplateCustomizer.SetNullableFlag(templateText, templateType, generateSettings);
            // 名前空間を設定
            TemplateCustomizer.SetNamespaceHeader(templateText, templateType, generateSettings);
            TemplateCustomizer.AppendSubnamespaces(templateText, outputUnityDir, generateSettings);

            // 改行コードに応じたEndNameEditActionを返す
            return generateSettings.LineEnding switch
            {
                LineEnding.Lf => ScriptableObject.CreateInstance<CreateLFScript>(),
                LineEnding.CrLf => ScriptableObject.CreateInstance<CreateCRLFScript>(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static void WriteUnityScript(int instanceId, string pathName, string text)
        {
            // クラス名を置換
            var className = Path.GetFileNameWithoutExtension(pathName);
            text = text.Replace("#SCRIPTNAME#", className);

            // LF のまま書き込み
            File.WriteAllText(pathName, text, new UTF8Encoding(false));

            // Unity に認識させる
            AssetDatabase.ImportAsset(pathName);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pathName));
        }
    }

    public class CreateLFScript : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            // テンプレート読み込み（LF に統一）
            var text = File.ReadAllText(resourceFile)
                .Replace("\r\n", "\n")
                .Replace("\r", "\n");

            ScriptGenerator.WriteUnityScript(instanceId, pathName, text);
        }
    }

    public class CreateCRLFScript : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            // テンプレート読み込み（CRLF に統一）
            var text = File.ReadAllText(resourceFile)
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", "\r\n");

            ScriptGenerator.WriteUnityScript(instanceId, pathName, text);
        }
    }
}