using System.Collections.Generic;
using System.IO;
using AdamBarclay.WebAssetBuilder.Infrastructure;

namespace AdamBarclay.WebAssetBuilder;

internal static class AssetManifestWriter
{
	internal static void OutputAssetManifest(
		FileSystem fileSystem,
		string manifestNamespace,
		SortedList<string, string> manifest)
	{
		using (var assetManifestFile = fileSystem.File.OpenWrite("AssetManifest.cs"))
		{
			using (var assetManifest = new StreamWriter(assetManifestFile))
			{
				assetManifest.WriteLine("using System;");
				assetManifest.WriteLine("using System.CodeDom.Compiler;");
				assetManifest.WriteLine("using System.Collections.Generic;");
				assetManifest.WriteLine();
				assetManifest.Write("namespace ");
				assetManifest.Write(manifestNamespace);
				assetManifest.WriteLine(";");
				assetManifest.WriteLine();

				assetManifest.WriteLine(
					"/// <summary>Contains the manifest of original files and their transformed names.</summary>");

				var assemblyName = typeof(AssetManifestWriter).Assembly.GetName();

				assetManifest.WriteLine($"[GeneratedCode(\"{assemblyName.Name}\", \"{assemblyName.Version}\")]");
				assetManifest.WriteLine("public static class AssetManifest");
				assetManifest.WriteLine("{");

				assetManifest.Write(
					"\tprivate static readonly Dictionary<string, string> Files = new Dictionary<string, string>");

				if (manifest.Count > 0)
				{
					assetManifest.WriteLine();
					assetManifest.WriteLine("\t{");

					assetManifest.Write(
						"\t\t{ \"" +
						manifest.Keys[0].Replace('\\', '/') +
						"\", \"" +
						manifest.Values[0].Replace('\\', '/') +
						"\" }");

					for (var i = 1; i < manifest.Count; ++i)
					{
						assetManifest.WriteLine(",");

						assetManifest.Write(
							"\t\t{ \"" +
							manifest.Keys[i].Replace('\\', '/') +
							"\", \"" +
							manifest.Values[i].Replace('\\', '/') +
							"\" }");
					}

					assetManifest.WriteLine();
					assetManifest.WriteLine("\t};");
				}
				else
				{
					assetManifest.WriteLine("(0);");
				}

				assetManifest.WriteLine();

				assetManifest.WriteLine(
					"\t/// <summary>Maps the original file name of an asset to the transformed file name used for cache busting.</summary>");

				assetManifest.WriteLine(
					"\t/// <param name=\"originalFileName\">The original file name of the asset.</param>");

				assetManifest.WriteLine("\t/// <returns>The transformed file name of the asset.</returns>");

				assetManifest.WriteLine(
					"\t/// <exception cref=\"ArgumentNullException\"><paramref name=\"originalFileName\"/> is <see langword=\"null\"/>.</exception>");

				assetManifest.WriteLine("\tpublic static string File(string originalFileName)");
				assetManifest.WriteLine("\t{");
				assetManifest.WriteLine("\t\tif (originalFileName is null)");
				assetManifest.WriteLine("\t\t{");
				assetManifest.WriteLine("\t\t\tthrow new ArgumentNullException(nameof(originalFileName));");
				assetManifest.WriteLine("\t\t}");
				assetManifest.WriteLine();
				assetManifest.WriteLine("\t\treturn AssetManifest.Files[originalFileName];");
				assetManifest.WriteLine("\t}");
				assetManifest.WriteLine("}");
			}
		}
	}
}
