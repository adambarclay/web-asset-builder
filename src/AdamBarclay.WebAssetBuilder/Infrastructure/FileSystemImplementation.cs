using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace AdamBarclay.WebAssetBuilder.Infrastructure;

[ExcludeFromCodeCoverage]
internal sealed class FileSystemImplementation : FileSystem
{
	internal FileSystemImplementation()
	{
		this.File = new RealFileWrapper();
		this.Directory = new RealDirectoryWrapper();
	}

	public DirectoryWrapper Directory { get; }

	public FileWrapper File { get; }

	private sealed class RealDirectoryWrapper : DirectoryWrapper
	{
		public DirectoryInfo CreateDirectory(string path)
		{
			return System.IO.Directory.CreateDirectory(path);
		}

		public void Delete(string path, bool recursive)
		{
			System.IO.Directory.Delete(path, recursive);
		}

		public bool Exists(string? path)
		{
			return System.IO.Directory.Exists(path);
		}
	}

	private sealed class RealFileWrapper : FileWrapper
	{
		public StreamWriter CreateText(string path)
		{
			return System.IO.File.CreateText(path);
		}

		public bool Exists(string path)
		{
			return System.IO.File.Exists(path);
		}

		public Stream OpenRead(string path)
		{
			return System.IO.File.OpenRead(path);
		}

		public Stream OpenWrite(string path)
		{
			return System.IO.File.OpenWrite(path);
		}

		public byte[] ReadAllBytes(string path)
		{
			return System.IO.File.ReadAllBytes(path);
		}
	}
}
