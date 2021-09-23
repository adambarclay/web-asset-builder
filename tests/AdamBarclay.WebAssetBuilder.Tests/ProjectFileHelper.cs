using System.Diagnostics.CodeAnalysis;

namespace AdamBarclay.WebAssetBuilder.Tests
{
	[ExcludeFromCodeCoverage]
	internal static class ProjectFileHelper
	{
		internal static string[] Create()
		{
			return new[] { "css/style.css", "f/font.woff2", "js/script.js", "robots.txt", "svg/image.svg" };
		}

		internal static string[] Create(string[] extra)
		{
			var projectFiles = ProjectFileHelper.Create();

			var array = new string[projectFiles.Length + extra.Length];

			projectFiles.CopyTo(array, 0);
			extra.CopyTo(array, projectFiles.Length);

			return array;
		}
	}
}
