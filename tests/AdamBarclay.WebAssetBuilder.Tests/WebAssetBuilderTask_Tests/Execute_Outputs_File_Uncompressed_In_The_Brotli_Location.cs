using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderTask_Tests
{
	[ExcludeFromCodeCoverage]
	public static class Execute_Outputs_File_Uncompressed_In_The_Brotli_Location
	{
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
				fileSystem.Setup(o => o.File!.OpenWrite("OutputPath/br/f/font."))!.Returns(memoryStream);

				task.Execute(fileSystem.Object!);

				actualOutput = memoryStream.ToArray();
			}

			Assert.Equal(expectedOutput, actualOutput);
		}
	}
}
