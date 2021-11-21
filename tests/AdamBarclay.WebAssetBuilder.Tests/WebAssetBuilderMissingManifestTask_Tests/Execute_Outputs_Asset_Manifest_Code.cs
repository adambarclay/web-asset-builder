using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using AdamBarclay.WebAssetBuilder.Tasks;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderMissingManifestTask_Tests;

[ExcludeFromCodeCoverage]
public static class Execute_Outputs_Asset_Manifest_Code
{
	[Fact]
	public static void When_The_Manifest_File_Does_Not_Exist()
	{
		var task = new WebAssetBuilderMissingManifestTask
		{
			AssemblyName = "TestAssembly",
			BuildEngine = BuildEngineHelper.Create().Object
		};

		string expectedOutput;

		using (var resource = typeof(Execute_Outputs_Asset_Manifest_Code).Assembly.GetManifestResourceStream(
			"AdamBarclay.WebAssetBuilder.Tests.Resources.Output.EmptyAssetManifest.cs")!)
		{
			using (var reader = new StreamReader(resource))
			{
				expectedOutput = reader.ReadToEnd();
			}
		}

		bool returnValue;
		string actualOutput;

		using (var memoryStream = new MemoryStream())
		{
			var fileSystem = FileSystemHelper.Create();

			fileSystem.Setup(o => o.File!.Exists("AssetManifest.cs"))!.Returns(false);
			fileSystem.Setup(o => o.File!.OpenWrite("AssetManifest.cs"))!.Returns(memoryStream);

			returnValue = task.Execute(fileSystem.Object!);

			actualOutput = Encoding.UTF8.GetString(memoryStream.ToArray());
		}

		Assert.True(returnValue);
		Assert.Equal(expectedOutput, actualOutput);
	}
}
