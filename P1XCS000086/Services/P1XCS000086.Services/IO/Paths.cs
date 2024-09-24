using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.IO
{
    internal static class Paths
    {
        // Extentions

        /// <summary>
        /// テンプレートマニフェストファイル
        /// </summary>
        public const string ExtentionVSTemplateManifest = ".vstman";

        public const string ExtentionVSTemplate = ".vstemplate";


        // Directry Path

        /// <summary>
        /// Visual Studio のテンプレートファイルを格納しているディレクトリ
        /// </summary>
		public const string VSProjectTemplateDirectoryPath = @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\ProjectTemplates";
    }
}
