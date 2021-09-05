using System;
using System.Diagnostics.CodeAnalysis;
using AdamBarclay.WebAssetBuilder.Tasks;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderMissingManifestTask_Tests
{
	[ExcludeFromCodeCoverage]
	public static class Execute_Throws_ArgumentNullException
	{
		[Fact]
		public static void When_The_FileSystem_Parameter_Is_Null()
		{
			var task = new WebAssetBuilderMissingManifestTask
			{
				AssemblyName = "TestAssembly",
				BuildEngine = BuildEngineHelper.Create().Object
			};

			Assert.ThrowsAny<ArgumentNullException>(() => task.Execute(null!));
		}
	}
}
