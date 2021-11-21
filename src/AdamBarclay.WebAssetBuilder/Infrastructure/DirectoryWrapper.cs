using System.IO;

namespace AdamBarclay.WebAssetBuilder.Infrastructure;

public interface DirectoryWrapper
{
	DirectoryInfo CreateDirectory(string path);

	void Delete(string path, bool recursive);

	bool Exists(string path);
}
