using System.Diagnostics.CodeAnalysis;
using Moq;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderTask_Tests
{
	[ExcludeFromCodeCoverage]
	public static class Execute_Deletes_The_Output_Directory
	{
		[Fact]
		public static void If_It_Already_Exists_Before_Processing_Starts()
		{
			var task = WebAssetBuilderTaskHelper.Create();

			var fileSystem = FileSystemHelper.Create();

			var returnValue = task.Execute(fileSystem.Object!);

			Assert.True(returnValue);

			fileSystem.Verify(o => o.Directory!.Delete("OutputPath", true), Times.Once);
		}
	}
}
