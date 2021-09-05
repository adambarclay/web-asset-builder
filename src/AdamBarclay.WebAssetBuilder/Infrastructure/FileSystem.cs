namespace AdamBarclay.WebAssetBuilder.Infrastructure
{
	public interface FileSystem
	{
		DirectoryWrapper Directory { get; }

		FileWrapper File { get; }
	}
}
