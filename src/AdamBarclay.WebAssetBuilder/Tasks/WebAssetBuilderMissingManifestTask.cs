using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AdamBarclay.WebAssetBuilder.Infrastructure;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace AdamBarclay.WebAssetBuilder.Tasks;

public sealed class WebAssetBuilderMissingManifestTask : Task
{
	[Required]
	public string? AssemblyName { get; set; }

	[ExcludeFromCodeCoverage]
	public override bool Execute()
	{
		return this.Execute(new FileSystemImplementation());
	}

	public bool Execute(FileSystem fileSystem)
	{
		if (fileSystem == null)
		{
			throw new ArgumentNullException(nameof(fileSystem));
		}

		var log = this.Log!;

		if (this.AssemblyName is null)
		{
			log.LogError("AssemblyName is null.");

			return false;
		}

		try
		{
			if (!fileSystem.File.Exists("AssetManifest.cs"))
			{
				AssetManifestWriter.OutputAssetManifest(
					fileSystem,
					this.AssemblyName,
					new SortedList<string, string>(0));
			}

			return true;
		}
		catch (Exception exception)
		{
			log.LogErrorFromException(exception);

			return false;
		}
	}
}
