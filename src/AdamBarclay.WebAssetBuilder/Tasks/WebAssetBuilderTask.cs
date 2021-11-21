using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AdamBarclay.WebAssetBuilder.Infrastructure;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace AdamBarclay.WebAssetBuilder.Tasks;

public sealed class WebAssetBuilderTask : Task
{
	[Required]
	public string? AssemblyName { get; set; }

	[Required]
	public string? AssetOutputPath { get; set; }

	[Required]
	public string[]? FileTypesToCompress { get; set; }

	[Required]
	public string[]? FileTypesToMangle { get; set; }

	[Required]
	public string[]? ProjectFiles { get; set; }

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

		try
		{
			if (this.AssemblyName is null)
			{
				log.LogError("AssemblyName is null.");

				return false;
			}

			if (this.AssetOutputPath is null)
			{
				log.LogError("AssetOutputPath is null.");

				return false;
			}

			if (this.FileTypesToCompress is null)
			{
				log.LogError("FileTypesToCompress is null.");

				return false;
			}

			if (this.FileTypesToMangle is null)
			{
				log.LogError("FileTypesToMangle is null.");

				return false;
			}

			if (this.ProjectFiles is null)
			{
				log.LogError("ProjectFiles is null.");

				return false;
			}

			if (fileSystem.Directory.Exists(this.AssetOutputPath))
			{
				fileSystem.Directory.Delete(this.AssetOutputPath, true);
			}

			var fileTypesToCompressLookup = new HashSet<string>(this.FileTypesToCompress);
			var fileTypesToMangleLookup = new HashSet<string>(this.FileTypesToMangle);

			var list = new SortedList<string, string>(this.ProjectFiles.Length, StringComparer.Ordinal);

			var fileProcessSuccess = true;

			foreach (var projectFile in this.ProjectFiles)
			{
				(var error, var outputKey) = FileProcessor.ProcessFile(
					log,
					fileSystem,
					projectFile,
					this.AssetOutputPath,
					fileTypesToCompressLookup,
					fileTypesToMangleLookup);

				fileProcessSuccess = fileProcessSuccess && error;

				if (outputKey.Length > 0)
				{
					list.Add("/" + projectFile, "/" + outputKey);
				}
			}

			AssetManifestWriter.OutputAssetManifest(fileSystem, this.AssemblyName, list);

			log.LogMessage(MessageImportance.High, this.AssemblyName + " Assets -> " + this.AssetOutputPath);

			return fileProcessSuccess;
		}
		catch (Exception exception)
		{
			log.LogErrorFromException(exception);

			return false;
		}
	}
}
