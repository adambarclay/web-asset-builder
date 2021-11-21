using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderTask_Tests;

[ExcludeFromCodeCoverage]
public static class Execute_Returns_False_And_Writes_To_The_Error_Log
{
	[Fact]
	public static void When_Minification_Fails()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = WebAssetBuilderTaskHelper.Create(buildEngine);

		task.ProjectFiles = new[] { "js/badscript.js" };

		var fileSystem = FileSystemHelper.Create();

		var returnValue = task.Execute(fileSystem.Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(It.Is<BuildErrorEventArgs>(e => e.Message == "Expected '{': alert")!),
			Times.Once);
	}

	[Fact]
	public static void When_The_AssemblyName_Property_Is_Null()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = WebAssetBuilderTaskHelper.Create(buildEngine);

		task.AssemblyName = null;

		var returnValue = task.Execute(FileSystemHelper.Create().Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(It.Is<BuildErrorEventArgs>(e => e.Message == "AssemblyName is null.")!),
			Times.Once);
	}

	[Fact]
	public static void When_The_Asset_Manifest_Can_Not_Be_Created()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = WebAssetBuilderTaskHelper.Create(buildEngine);

		var fileSystem = FileSystemHelper.Create();

		fileSystem.Setup(o => o.File.OpenWrite("AssetManifest.cs"))!.Throws(new UnauthorizedAccessException());

		var returnValue = task.Execute(fileSystem.Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(
				It.Is<BuildErrorEventArgs>(e => e.Message == "Attempted to perform an unauthorized operation.")!),
			Times.Once);
	}

	[Fact]
	public static void When_The_AssetOutputPath_Property_Is_Null()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = WebAssetBuilderTaskHelper.Create(buildEngine);

		task.AssetOutputPath = null;

		var returnValue = task.Execute(FileSystemHelper.Create().Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(It.Is<BuildErrorEventArgs>(e => e.Message == "AssetOutputPath is null.")!),
			Times.Once);
	}

	[Fact]
	public static void When_The_FileTypesToCompress_Property_Is_Null()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = WebAssetBuilderTaskHelper.Create(buildEngine);

		task.FileTypesToCompress = null;

		var returnValue = task.Execute(FileSystemHelper.Create().Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(It.Is<BuildErrorEventArgs>(e => e.Message == "FileTypesToCompress is null.")!),
			Times.Once);
	}

	[Fact]
	public static void When_The_FileTypesToMangle_Property_Is_Null()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = WebAssetBuilderTaskHelper.Create(buildEngine);

		task.FileTypesToMangle = null;

		var returnValue = task.Execute(FileSystemHelper.Create().Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(It.Is<BuildErrorEventArgs>(e => e.Message == "FileTypesToMangle is null.")!),
			Times.Once);
	}

	[Fact]
	public static void When_The_Output_Directory_Can_Not_Be_Deleted()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = WebAssetBuilderTaskHelper.Create(buildEngine);

		var fileSystem = FileSystemHelper.Create();

		fileSystem.Setup(o => o.Directory.Delete(It.IsAny<string>()!, It.IsAny<bool>()))!.Throws(
			new UnauthorizedAccessException());

		var returnValue = task.Execute(fileSystem.Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(
				It.Is<BuildErrorEventArgs>(e => e.Message == "Attempted to perform an unauthorized operation.")!),
			Times.Once);
	}

	[Fact]
	public static void When_The_ProjectFiles_Property_Is_Null()
	{
		var buildEngine = BuildEngineHelper.Create();

		var task = WebAssetBuilderTaskHelper.Create(buildEngine);

		task.ProjectFiles = null;

		var returnValue = task.Execute(FileSystemHelper.Create().Object!);

		Assert.False(returnValue);

		buildEngine.Verify(
			o => o.LogErrorEvent(It.Is<BuildErrorEventArgs>(e => e.Message == "ProjectFiles is null.")!),
			Times.Once);
	}
}
