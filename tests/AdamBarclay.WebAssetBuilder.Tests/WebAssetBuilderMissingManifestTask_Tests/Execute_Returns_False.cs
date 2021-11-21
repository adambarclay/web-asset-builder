using System;
using System.Diagnostics.CodeAnalysis;
using AdamBarclay.WebAssetBuilder.Tasks;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderMissingManifestTask_Tests;

[ExcludeFromCodeCoverage]
public static class Execute_Returns_False
{
	[Fact]
	public static void When_The_AssemblyName_Property_Is_Null()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = new WebAssetBuilderMissingManifestTask
		{
			AssemblyName = null,
			BuildEngine = buildEngine.Object
		};

		var returnValue = task.Execute(FileSystemHelper.Create().Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(It.Is<BuildErrorEventArgs>(e => e.Message == "AssemblyName is null.")!),
			Times.Once);
	}

	[Fact]
	public static void When_The_File_Cannot_Be_Created()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = new WebAssetBuilderMissingManifestTask
		{
			AssemblyName = "TestAssembly",
			BuildEngine = buildEngine.Object
		};

		var fileSystem = FileSystemHelper.Create();

		fileSystem.Setup(o => o.File!.Exists("AssetManifest.cs"))!.Returns(false);
		fileSystem.Setup(o => o.File!.OpenWrite("AssetManifest.cs"))!.Throws<UnauthorizedAccessException>();

		var returnValue = task.Execute(fileSystem.Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(
				It.Is<BuildErrorEventArgs>(e => e.Message == "Attempted to perform an unauthorized operation.")!),
			Times.Once);
	}
}
