using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderTask_Tests;

[ExcludeFromCodeCoverage]
public static class Execute_Does_Not_Modify_The_Name_Of_The_File
{
	[Fact]
	public static void When_The_File_Is_RobotsTxt()
	{
		var task = WebAssetBuilderTaskHelper.Create();

		task.ProjectFiles = new[] { "robots.txt" };

		var brExpectedOutput = FileSystemHelper.ReadAllBytes(
			"AdamBarclay.WebAssetBuilder.Tests.Resources.Output.robots.txt.br");

		var gzipExpectedOutput = FileSystemHelper.ReadAllBytes(
			"AdamBarclay.WebAssetBuilder.Tests.Resources.Output.robots.txt.gzip");

		var uncompressedExpectedOutput = FileSystemHelper.ReadAllBytes(
			"AdamBarclay.WebAssetBuilder.Tests.Resources.Output.robots.txt");

		var fileSystem = FileSystemHelper.Create();

		byte[] brActualOutput;
		byte[] gzipActualOutput;
		byte[] uncompressedActualOutput;

		using (var br = new MemoryStream())
		{
			using (var gzip = new MemoryStream())
			{
				using (var uncompressed = new MemoryStream())
				{
					fileSystem.Setup(o => o.File.OpenWrite("OutputPath/br/robots.txt"))!.Returns(br);
					fileSystem.Setup(o => o.File.OpenWrite("OutputPath/gzip/robots.txt"))!.Returns(gzip);

					fileSystem.Setup(o => o.File.OpenWrite("OutputPath/uncompressed/robots.txt"))!.Returns(
						uncompressed);

					task.Execute(fileSystem.Object!);

					brActualOutput = br.ToArray();
					gzipActualOutput = gzip.ToArray();
					uncompressedActualOutput = uncompressed.ToArray();
				}
			}
		}

		Assert.Equal(brExpectedOutput, brActualOutput);
		Assert.Equal(gzipExpectedOutput, gzipActualOutput);
		Assert.Equal(uncompressedExpectedOutput, uncompressedActualOutput);
	}
}
