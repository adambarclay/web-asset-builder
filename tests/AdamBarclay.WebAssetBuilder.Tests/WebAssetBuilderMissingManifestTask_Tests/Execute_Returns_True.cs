using System.Diagnostics.CodeAnalysis;
using System.IO;
using AdamBarclay.WebAssetBuilder.Tasks;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderMissingManifestTask_Tests;

[ExcludeFromCodeCoverage]
public static class Execute_Returns_True
{
	[Fact]
	public static void When_The_Manifest_File_Does_Not_Exist()
	{
		var task = new WebAssetBuilderMissingManifestTask
		{
			AssemblyName = "TestAssembly",
			BuildEngine = BuildEngineHelper.Create().Object
		};

		var fileSystem = FileSystemHelper.Create();

		fileSystem.Setup(o => o.File!.Exists("AssetManifest.cs"))!.Returns(false);

		fileSystem.Setup(o => o.File!.CreateText("AssetManifest.cs"))!.Returns(
			() => new StreamWriter(new MemoryStream()));

		var returnValue = task.Execute(fileSystem.Object!);

		Assert.True(returnValue);
	}

	[Fact]
	public static void When_The_Manifest_File_Exists()
	{
		var task = new WebAssetBuilderMissingManifestTask
		{
			AssemblyName = "TestAssembly",
			BuildEngine = BuildEngineHelper.Create().Object
		};

		var fileSystem = FileSystemHelper.Create();

		fileSystem.Setup(o => o.File!.Exists("AssetManifest.cs"))!.Returns(true);

		fileSystem.Setup(o => o.File!.CreateText("AssetManifest.cs"))!.Returns(
			() => new StreamWriter(new MemoryStream()));

		var returnValue = task.Execute(fileSystem.Object!);

		Assert.True(returnValue);
	}
}
