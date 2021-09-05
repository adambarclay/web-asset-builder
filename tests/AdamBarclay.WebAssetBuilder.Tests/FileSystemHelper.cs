using System.Diagnostics.CodeAnalysis;
using System.IO;
using AdamBarclay.WebAssetBuilder.Infrastructure;
using Moq;

namespace AdamBarclay.WebAssetBuilder.Tests
{
	[ExcludeFromCodeCoverage]
	internal static class FileSystemHelper
	{
		internal static Mock<FileSystem> Create()
		{
			var fileSystem = new Mock<FileSystem>(MockBehavior.Strict);

			fileSystem.Setup(o => o.Directory.CreateDirectory(It.IsAny<string>()!))!.Returns(new DirectoryInfo("/"));
			fileSystem.Setup(o => o.Directory.Exists("OutputPath"))!.Returns(true);
			fileSystem.Setup(o => o.Directory.Delete("OutputPath", true));

			fileSystem.Setup(o => o.File.Exists("AssetManifest.cs"))!.Returns(false);

			fileSystem.Setup(o => o.File.ReadAllBytes("css/style.css"))!.Returns(
				() => FileSystemHelper.ReadAllBytes("AdamBarclay.WebAssetBuilder.Tests.Resources.Input.style.css"));

			fileSystem.Setup(o => o.File.ReadAllBytes("f/font.woff2"))!.Returns(
				() => FileSystemHelper.ReadAllBytes("AdamBarclay.WebAssetBuilder.Tests.Resources.Input.font.woff2"));

			fileSystem.Setup(o => o.File.ReadAllBytes("js/script.js"))!.Returns(
				() => FileSystemHelper.ReadAllBytes("AdamBarclay.WebAssetBuilder.Tests.Resources.Input.script.js"));

			fileSystem.Setup(o => o.File.ReadAllBytes("svg/image.svg"))!.Returns(
				() => FileSystemHelper.ReadAllBytes("AdamBarclay.WebAssetBuilder.Tests.Resources.Input.image.svg"));

			fileSystem.Setup(o => o.File.ReadAllBytes("js/badscript.js"))!.Returns(
				() => FileSystemHelper.ReadAllBytes("AdamBarclay.WebAssetBuilder.Tests.Resources.Input.badscript.txt"));

			fileSystem.Setup(o => o.File.OpenWrite(It.IsAny<string>()!))!.Returns(() => new MemoryStream());

			return fileSystem;
		}

		internal static byte[] ReadAllBytes(string assetName)
		{
			using (var memoryStream = new MemoryStream())
			{
				using (var resource = typeof(FileSystemHelper).Assembly.GetManifestResourceStream(assetName)!)
				{
					resource.CopyTo(memoryStream);
				}

				return memoryStream.ToArray();
			}
		}
	}
}
