using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AdamBarclay.WebAssetBuilder.Infrastructure;
using ICSharpCode.SharpZipLib.GZip;
using Microsoft.Build.Utilities;
using NUglify;
using Brotli = BrotliSharpLib.BrotliStream;

namespace AdamBarclay.WebAssetBuilder
{
	internal static class FileProcessor
	{
		private static readonly BigInteger BigInt36 = new BigInteger(36);

		internal static (bool Success, string OutputKey) ProcessFile(
			TaskLoggingHelper log,
			FileSystem fileSystem,
			string fileName,
			string assetOutputPath,
			HashSet<string> fileTypesToCompress)
		{
			var fileExtension = Path.GetExtension(fileName);

			(var error, var fileContents) = fileExtension switch
			{
				".css" => FileProcessor.MinifyFile(log, fileSystem, fileName, s => Uglify.Css(s)),
				".js" => FileProcessor.MinifyFile(log, fileSystem, fileName, s => Uglify.Js(s)),
				".svg" => FileProcessor.MinifyFile(log, fileSystem, fileName, FileProcessor.MinifyXml),
				var _ => (false, fileSystem.File.ReadAllBytes(fileName)!)
			};

			if (error)
			{
				return (false, string.Empty);
			}

			var compressFile = fileTypesToCompress.Contains(fileExtension);

			var outputKey = FileProcessor.GenerateOutputKey(fileName, fileContents);

			FileProcessor.OutputUncompressed(fileSystem, assetOutputPath, outputKey, fileContents);
			FileProcessor.OutputGzipFile(fileSystem, assetOutputPath, outputKey, fileContents, compressFile);
			FileProcessor.OutputBrotliFile(fileSystem, assetOutputPath, outputKey, fileContents, compressFile);

			return (true, outputKey);
		}

		private static string GenerateOutputKey(string fileName, byte[] fileContents)
		{
			byte[] hash;

			using (var hashAlgorithm = SHA256.Create()!)
			{
				hash = hashAlgorithm.ComputeHash(fileContents);
			}

			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

			return Path.Combine(
				Path.GetDirectoryName(fileName) ?? string.Empty,
				fileNameWithoutExtension + "-" + FileProcessor.ToBase36String(hash));
		}

		private static (bool Error, byte[] FileContents) MinifyFile(
			TaskLoggingHelper log,
			FileSystem fileSystem,
			string fileName,
			Func<string, UglifyResult> uglifyFunction)
		{
			var fileContents = Encoding.UTF8.GetString(fileSystem.File.ReadAllBytes(fileName));

			var result = uglifyFunction(fileContents);

			if (result.HasErrors)
			{
				foreach (var resultError in result.Errors)
				{
					log.LogError(resultError.Message!);
				}

				return (true, Array.Empty<byte>());
			}

			return (false, Encoding.UTF8.GetBytes(result.Code));
		}

		private static UglifyResult MinifyXml(string source)
		{
			string result;
			var messages = new List<UglifyError>();

			var xml = XDocument.Parse(source);

			using (var memoryStream = new MemoryStream())
			{
				using (var xmlWriter = XmlWriter.Create(
					memoryStream,
					new XmlWriterSettings
					{
						OmitXmlDeclaration = true,
						Encoding = new UTF8Encoding(false)
					}))
				{
					xml.WriteTo(xmlWriter);
				}

				result = Encoding.UTF8.GetString(memoryStream.ToArray());
			}

			return new UglifyResult(result, messages);
		}

		private static void OutputBrotliFile(
			FileSystem fileSystem,
			string assetOutputPath,
			string outputKey,
			byte[] fileContents,
			bool compressFile)
		{
			var brotliOutputPath = Path.Combine(assetOutputPath, "br", outputKey + ".").Replace('\\', '/');

			fileSystem.Directory.CreateDirectory(Path.GetDirectoryName(brotliOutputPath)!);

			using (var outputFile = fileSystem.File.OpenWrite(brotliOutputPath)!)
			{
				if (compressFile)
				{
					using (var brotli = new Brotli(outputFile, CompressionMode.Compress, true))
					{
						brotli.SetQuality(11);

						brotli.Write(fileContents, 0, fileContents.Length);
					}
				}
				else
				{
					outputFile.Write(fileContents, 0, fileContents.Length);
				}
			}
		}

		private static void OutputGzipFile(
			FileSystem fileSystem,
			string assetOutputPath,
			string outputKey,
			byte[] fileContents,
			bool compressFile)
		{
			var gzipOutputPath = Path.Combine(assetOutputPath, "gzip", outputKey + ".").Replace('\\', '/');

			fileSystem.Directory!.CreateDirectory(Path.GetDirectoryName(gzipOutputPath)!);

			using (var outputFile = fileSystem.File!.OpenWrite(gzipOutputPath)!)
			{
				if (compressFile)
				{
					using (var memoryStream = new MemoryStream())
					{
						using (var gzip = new GZipOutputStream(memoryStream))
						{
							gzip.IsStreamOwner = false;
							gzip.SetLevel(9);

							gzip.Write(fileContents, 0, fileContents.Length);
						}

						var buffer = memoryStream.GetBuffer();

						buffer[4] = buffer[5] = buffer[6] = buffer[7] = 0;

						outputFile.Write(buffer, 0, (int)memoryStream.Length);
					}
				}
				else
				{
					outputFile.Write(fileContents, 0, fileContents.Length);
				}
			}
		}

		private static void OutputUncompressed(
			FileSystem fileSystem,
			string assetOutputPath,
			string outputKey,
			byte[] fileContents)
		{
			var outputPath = Path.Combine(assetOutputPath, "uncompressed", outputKey + ".").Replace('\\', '/');

			fileSystem.Directory!.CreateDirectory(Path.GetDirectoryName(outputPath)!);

			using (var outputFile = fileSystem.File!.OpenWrite(outputPath)!)
			{
				outputFile.Write(fileContents, 0, fileContents.Length);
			}
		}

		private static string ToBase36String(byte[] bytes)
		{
			var list = new List<char>((int)Math.Ceiling(bytes.Length * 8 / Math.Log(36, 2)));

			var dividend = new BigInteger(bytes);

			while (!dividend.IsZero)
			{
				dividend = BigInteger.DivRem(dividend, FileProcessor.BigInt36, out var remainder);

				list.Add("0123456789abcdefghijklmnopqrstuvwxyz"[Math.Abs((int)remainder)]);
			}

			list.Reverse();

			return new string(list.ToArray());
		}
	}
}