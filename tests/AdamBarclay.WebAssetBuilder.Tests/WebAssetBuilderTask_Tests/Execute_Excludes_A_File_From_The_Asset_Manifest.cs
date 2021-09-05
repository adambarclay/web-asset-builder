using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderTask_Tests
{
	[ExcludeFromCodeCoverage]
	public static class Execute_Excludes_A_File_From_The_Asset_Manifest
	{
		[Fact]
		public static void When_Minification_Fails()
		{
			var buildEngine = BuildEngineHelper.Create();

			var task = WebAssetBuilderTaskHelper.Create(buildEngine);

			task.ProjectFiles = ProjectFileHelper.Create(new[] { "js/badscript.js" });

			string expectedOutput;

			using (var resource = typeof(Execute_Outputs_The_Full_Asset_Manifest).Assembly.GetManifestResourceStream(
				"AdamBarclay.WebAssetBuilder.Tests.Resources.Output.FullAssetManifest.cs")!)
			{
				using (var reader = new StreamReader(resource))
				{
					expectedOutput = reader.ReadToEnd();
				}
			}

			var fileSystem = FileSystemHelper.Create();

			string actualOutput;

			using (var memoryStream = new MemoryStream())
			{
				fileSystem.Setup(o => o.File!.OpenWrite("AssetManifest.cs"))!.Returns(memoryStream);

				task.Execute(fileSystem.Object!);

				actualOutput = Encoding.UTF8.GetString(memoryStream.ToArray());
			}

			Assert.Equal(expectedOutput, actualOutput);
		}
	}
}
