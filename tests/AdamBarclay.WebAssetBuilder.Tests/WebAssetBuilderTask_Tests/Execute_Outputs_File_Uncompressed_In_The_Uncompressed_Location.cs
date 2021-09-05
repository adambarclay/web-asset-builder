using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderTask_Tests
{
	[ExcludeFromCodeCoverage]
	public static class Execute_Outputs_File_Uncompressed_In_The_Uncompressed_Location
	{
		[Fact]
		public static void When_The_File_Should_Be_Compressed()
		{
			var task = WebAssetBuilderTaskHelper.Create();

			task.ProjectFiles = new[] { "css/style.css" };

			var expectedOutput = FileSystemHelper.ReadAllBytes(
				"AdamBarclay.WebAssetBuilder.Tests.Resources.Output.style.min.css");

			var fileSystem = FileSystemHelper.Create();

			byte[] actualOutput;

			using (var memoryStream = new MemoryStream())
			{
				fileSystem.Setup(
						o => o.File!.OpenWrite(
							"OutputPath/uncompressed/css/style-1k2pypdtz1f2y2i3ely4f9jwig46xuu5jqvp5w21gdrrs72usu."))!
					.Returns(memoryStream);

				task.Execute(fileSystem.Object!);

				actualOutput = memoryStream.ToArray();
			}

			Assert.Equal(expectedOutput, actualOutput);
		}

		[Fact]
		public static void When_The_File_Should_Not_Be_Compressed()
		{
			var task = WebAssetBuilderTaskHelper.Create();

			task.ProjectFiles = new[] { "f/font.woff2" };

			var expectedOutput = FileSystemHelper.ReadAllBytes(
				"AdamBarclay.WebAssetBuilder.Tests.Resources.Input.font.woff2");

			var fileSystem = FileSystemHelper.Create();

			byte[] actualOutput;

			using (var memoryStream = new MemoryStream())
			{
				fileSystem.Setup(
					o => o.File!.OpenWrite(
						"OutputPath/uncompressed/f/font-1mha83rlvsi6ybp10269l5y8qi5r0fp9jhwycov41495venjes."))!.Returns(
					memoryStream);

				task.Execute(fileSystem.Object!);

				actualOutput = memoryStream.ToArray();
			}

			Assert.Equal(expectedOutput, actualOutput);
		}
	}
}
