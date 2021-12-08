# Web Asset Builder

[![License: MIT](https://img.shields.io/github/license/adambarclay/web-asset-builder?color=blue)](https://github.com/adambarclay/web-asset-builder/blob/main/LICENSE) [![nuget](https://img.shields.io/nuget/v/AdamBarclay.WebAssetBuilder)](https://www.nuget.org/packages/AdamBarclay.WebAssetBuilder/) [![build](https://img.shields.io/github/workflow/status/adambarclay/web-asset-builder/Build/main)](https://github.com/adambarclay/web-asset-builder/actions?query=workflow%3ABuild+branch%3Amain) [![coverage](https://img.shields.io/codecov/c/github/adambarclay/web-asset-builder/main)](https://codecov.io/gh/adambarclay/web-asset-builder/branch/main)

Web Asset Builder minifies .css, .js, and .svg files, outputs files uncompressed and also compressed with brotli and gzip, and adds a hash to file names to facilitate cache-busting. 

## Getting Started

Create a new C# project and install the [AdamBarclay.WebAssetBuilder](https://www.nuget.org/packages/AdamBarclay.WebAssetBuilder/) nuget package.

Add your web asset files to the project.

By default Visual Studio will add the files with a `Build Action` of `None`. The Web Asset Builder will process files with a `Build Action` of `None` so don't change this setting.

When the project is built, the asset files will be processed and a file called `AssetManifest.cs` will be generated.

## AssetManifest.cs

`AssetManifest.cs` contains a mapping between the original file name and the output file name. Some files optionally have a hash appended to the name to facilitate cache-busting.

The manifest will contain a mapping for all of the files even if the file name was not changed.

`AssetManifest` contains a single method:

```c#
    public static class AssetManifest
    {
        /// <summary>Maps the original file name of an asset to the transformed file name used for cache busting.</summary>
        /// <param name="originalFileName">The original file name of the asset.</param>
        /// <returns>The transformed file name of the asset.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="originalFileName"/> is <see langword="null"/>.</exception>
        public static string File(string originalFileName)
    }
```

## Parameters

These parameters can be overwritten in the project file, e.g.

```xml
    <PropertyGroup>
        <AssetOutputPath>../some-other-path</AssetOutputPath>
        <FileTypesToCompress>.html</FileTypesToCompress>
        <FileTypesToMangle>.css;.html</FileTypesToMangle>
    </PropertyGroup>
```

### AssetOutputPath

The output location of the built files relative to the project file.

`AssetOutputPath` defaults to `../../artifacts/assets/`.

### FileTypesToCompress

A list of file extensions to compress with brotli and gzip. An uncompressed version is also output.

- Brotli files are put in `/br`
- Gzip files are put in `/gzip`
- Uncompressed files are put in `/uncompressed`
- File types not included in this list are put in `/static`

`FileTypesToCompress` defaults to `.css;.js;.svg;.txt`.

### FileTypesToMangle

A list of file extensions which will have a hash of the uncompressed file contents appended to their file names.

e.g. `style.css` might become `style-abcdefghijklmnopqrstuvwxyz0123456789`.

`FileTypesToMangle` defaults to `.css;.js`.
