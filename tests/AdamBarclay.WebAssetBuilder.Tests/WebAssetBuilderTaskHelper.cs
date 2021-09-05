using System.Diagnostics.CodeAnalysis;
using AdamBarclay.WebAssetBuilder.Tasks;
using Microsoft.Build.Framework;
using Moq;

namespace AdamBarclay.WebAssetBuilder.Tests
{
	[ExcludeFromCodeCoverage]
	internal static class WebAssetBuilderTaskHelper
	{
		internal static WebAssetBuilderTask Create()
		{
			return WebAssetBuilderTaskHelper.Create(BuildEngineHelper.Create());
		}

		internal static WebAssetBuilderTask Create(Mock<IBuildEngine> buildEngine)
		{
			return new WebAssetBuilderTask
			{
				AssemblyName = "TestAssembly",
				AssetOutputPath = "OutputPath",
				BuildEngine = buildEngine.Object,
				FileTypesToCompress = new[] { ".css", ".js", ".svg" },
				ProjectFiles = ProjectFileHelper.Create()
			};
		}
	}
}
