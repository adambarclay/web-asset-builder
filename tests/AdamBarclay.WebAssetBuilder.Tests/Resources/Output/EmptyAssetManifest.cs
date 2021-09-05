using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace TestAssembly
{
	/// <summary>Contains the manifest of original files and their transformed names.</summary>
	[GeneratedCode("AdamBarclay.WebAssetBuilder", "1.0.0.0")]
	public static class AssetManifest
	{
		private static readonly Dictionary<string, string> Files = new Dictionary<string, string>(0);

		/// <summary>Maps the original file name of an asset to the transformed file name used for cache busting.</summary>
		/// <param name="originalFileName">The original file name of the asset.</param>
		/// <returns>The transformed file name of the asset.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="originalFileName"/> is <see langword="null"/>.</exception>
		public static string File(string originalFileName)
		{
			if (originalFileName is null)
			{
				throw new ArgumentNullException(nameof(originalFileName));
			}

			return AssetManifest.Files[originalFileName];
		}
	}
}
