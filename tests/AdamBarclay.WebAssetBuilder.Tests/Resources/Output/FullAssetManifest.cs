using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace TestAssembly
{
	/// <summary>Contains the manifest of original files and their transformed names.</summary>
	[GeneratedCode("AdamBarclay.WebAssetBuilder", "1.0.0.0")]
	public static class AssetManifest
	{
		private static readonly Dictionary<string, string> Files = new Dictionary<string, string>
		{
			{ "/css/style.css", "/css/style-1k2pypdtz1f2y2i3ely4f9jwig46xuu5jqvp5w21gdrrs72usu" },
			{ "/f/font.woff2", "/f/font-1mha83rlvsi6ybp10269l5y8qi5r0fp9jhwycov41495venjes" },
			{ "/js/script.js", "/js/script-1qvd8xi1x7gicp9hzmn16150ffhqgemfx5eclq79349zpriln6" },
			{ "/svg/image.svg", "/svg/image-1b9j8z50kty1rhk6v25aapbqngqhmcl3o0kmpi62l875l6e9is" }
		};

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
