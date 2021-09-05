using System.IO;

namespace AdamBarclay.WebAssetBuilder.Infrastructure
{
	public interface FileWrapper
	{
		StreamWriter CreateText(string path);

		bool Exists(string path);

		Stream OpenRead(string path);

		Stream OpenWrite(string path);

		byte[] ReadAllBytes(string path);
	}
}
