# Updating the documentation

[DocFX](https://dotnet.github.io/docfx/index.html) was used to generate the documentation for this package.

This [guide](https://normanderwan.github.io/DocFxForUnity/index.html) was used as reference for the steps to generate documentation from scratch.

## Common Issues

`error CS0246: The type or namespace name 'UnityEditor' could not be found (are you missing a using directive or an assembly reference?)`

- Updated versions of DocFX had such  errors and was unable generate the scripting API pages.
- DocFX version **2.61.0** was used to successfully generated documentation.
- Run `dotnet tool uninstall -g docfx` then `dotnet tool install -g docfx --version 2.61.0`
- Amend this section if new versions of DocFX no longer generate these errors.
