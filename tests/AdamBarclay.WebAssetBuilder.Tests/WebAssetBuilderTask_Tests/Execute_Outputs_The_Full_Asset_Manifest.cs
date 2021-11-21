using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderTask_Tests;

[ExcludeFromCodeCoverage]
public static class Execute_Outputs_The_Full_Asset_Manifest
{
	[Fact]
	public static void When_The_Build_Succeeds()
	{
		var task = WebAssetBuilderTaskHelper.Create();

		var expectedOutput = FileSystemHelper.ReadAllBytes(
			"AdamBarclay.WebAssetBuilder.Tests.Resources.Output.FullAssetManifest.cs");

		var fileSystem = FileSystemHelper.Create();

		bool returnValue;
		byte[] actualOutput;

		using (var memoryStream = new MemoryStream())
		{
			fileSystem.Setup(o => o.File.OpenWrite("AssetManifest.cs"))!.Returns(memoryStream);

			returnValue = task.Execute(fileSystem.Object!);

			actualOutput = memoryStream.ToArray();
		}

		Assert.True(returnValue);
		Assert.Equal(expectedOutput, actualOutput);
	}
}
