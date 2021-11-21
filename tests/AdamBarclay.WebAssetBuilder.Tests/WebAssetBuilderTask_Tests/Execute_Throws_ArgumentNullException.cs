using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AdamBarclay.WebAssetBuilder.Tests.WebAssetBuilderTask_Tests;

[ExcludeFromCodeCoverage]
public static class Execute_Throws_ArgumentNullException
{
	[Fact]
	public static void When_The_FileSystem_Parameter_Is_Null()
	{
		var task = WebAssetBuilderTaskHelper.Create();

		Assert.ThrowsAny<ArgumentNullException>(() => task.Execute(null!));
	}
}
