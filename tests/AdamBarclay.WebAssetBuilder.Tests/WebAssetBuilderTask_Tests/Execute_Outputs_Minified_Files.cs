using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderTask_Tests
{
	[ExcludeFromCodeCoverage]
	public static class Execute_Outputs_Minified_Files
	{
		[Fact]
		public static void When_The_File_Extension_Is_Css()
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
		public static void When_The_File_Extension_Is_Js()
		{
			var task = WebAssetBuilderTaskHelper.Create();

			task.ProjectFiles = new[] { "js/script.js" };

			var expectedOutput = FileSystemHelper.ReadAllBytes(
				"AdamBarclay.WebAssetBuilder.Tests.Resources.Output.script.min.js");

			var fileSystem = FileSystemHelper.Create();

			byte[] actualOutput;

			using (var memoryStream = new MemoryStream())
			{
				fileSystem.Setup(
						o => o.File!.OpenWrite(
							"OutputPath/uncompressed/js/script-1qvd8xi1x7gicp9hzmn16150ffhqgemfx5eclq79349zpriln6."))!
					.Returns(memoryStream);

				task.Execute(fileSystem.Object!);

				actualOutput = memoryStream.ToArray();
			}

			Assert.Equal(expectedOutput, actualOutput);
		}

		[Fact]
		public static void When_The_File_Extension_Is_Svg()
		{
			var task = WebAssetBuilderTaskHelper.Create();

			task.ProjectFiles = new[] { "svg/image.svg" };

			var expectedOutput = FileSystemHelper.ReadAllBytes(
				"AdamBarclay.WebAssetBuilder.Tests.Resources.Output.image.min.svg");

			var fileSystem = FileSystemHelper.Create();

			byte[] actualOutput;

			using (var memoryStream = new MemoryStream())
			{
				fileSystem.Setup(
						o => o.File!.OpenWrite(
							"OutputPath/uncompressed/svg/image-1b9j8z50kty1rhk6v25aapbqngqhmcl3o0kmpi62l875l6e9is."))!
					.Returns(memoryStream);

				task.Execute(fileSystem.Object!);

				actualOutput = memoryStream.ToArray();
			}

			Assert.Equal(expectedOutput, actualOutput);
		}
	}
}
