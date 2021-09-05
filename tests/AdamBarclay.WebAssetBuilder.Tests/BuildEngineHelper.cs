using System.Diagnostics.CodeAnalysis;
using Microsoft.Build.Framework;
using Moq;

namespace AdamBarclay.WebAssetBuilder.Tests
{
	[ExcludeFromCodeCoverage]
	internal static class BuildEngineHelper
	{
		internal static Mock<IBuildEngine> Create()
		{
			var buildEngine = new Mock<IBuildEngine>(MockBehavior.Strict);

			buildEngine.Setup(o => o.ProjectFileOfTaskNode)!.Returns(string.Empty);
			buildEngine.Setup(o => o.LineNumberOfTaskNode)!.Returns(0);
			buildEngine.Setup(o => o.ColumnNumberOfTaskNode)!.Returns(0);
			buildEngine.Setup(o => o.LogMessageEvent(It.IsAny<BuildMessageEventArgs>()!));
			buildEngine.Setup(o => o.LogErrorEvent(It.IsAny<BuildErrorEventArgs>()!));

			return buildEngine;
		}
	}
}
