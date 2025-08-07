# .NET 9.0 Migration Notes

This solution has been migrated from .NET Framework 4.8 to .NET 8.0 as a step towards .NET 9.0.

## Current Status

✅ **Migrated to .NET 8.0 (ready for .NET 9.0 when available)**

All projects have been converted from the old-style MSBuild format to the modern SDK-style format and updated to target .NET 8.0. When .NET 9.0 becomes available in the build environment, the target framework can be easily updated.

## Projects Status

| Project | Status | Notes |
|---------|--------|-------|
| QueryMultiDb.Common | ✅ Migrated | Library project successfully converted |
| DbTargets | ✅ Migrated | Console app converted, updated SqlClient to Microsoft.Data.SqlClient |
| QueryMultiDb | ✅ Migrated | Main console app converted, updated dependencies |
| QueryMultiDbGui.Windows | ⚠️ Windows Only | Requires Windows SDK, will build on Windows |
| QueryMultiDb.Tests.Unit | ✅ Migrated | Unit tests running successfully |
| QueryMultiDb.Common.Tests.Unit | ✅ Migrated | Unit tests running successfully |
| QueryMultiDb.Tests.System | ✅ Migrated | System tests converted |

## Key Changes Made

### Updated Dependencies
- **SQL Client**: Upgraded from `System.Data.SqlClient` to `Microsoft.Data.SqlClient` 5.2.0
- **CsvHelper**: Upgraded from v15.0.3 to v30.0.1
- **DocumentFormat.OpenXml**: Upgraded from v2.10.1 to v3.0.2
- **Newtonsoft.Json**: Upgraded from v12.0.3 to v13.0.3
- **NLog**: Upgraded from v4.7.0 to v5.2.8
- **xUnit**: Upgraded to v2.6.4 with modern test runner

### Project Format
- Converted all projects from old-style MSBuild to SDK-style format
- Removed `packages.config` files in favor of `PackageReference`
- Simplified project files significantly
- Disabled nullable reference types to minimize migration impact
- Disabled deterministic builds where version wildcards are used

### Windows Forms Project
The GUI project has been renamed to `QueryMultiDbGui.Windows` to indicate its Windows dependency. It requires the Windows Desktop SDK to build and will only work on Windows environments.

## To Complete Migration to .NET 9.0

When .NET 9.0 SDK becomes available:

1. Update all `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net9.0</TargetFramework>`
2. For Windows Forms project, use `<TargetFramework>net9.0-windows</TargetFramework>`
3. Test and update any dependencies as needed

## Breaking Changes

- **SQL Client namespace**: Code using `System.Data.SqlClient` has been updated to `Microsoft.Data.SqlClient`
- **NetworkLibrary property**: Removed from SqlConnectionStringBuilder (not supported in modern SqlClient)
- **NLog API**: Some obsolete methods are used (warnings only, still functional)

## Testing

All unit tests pass successfully:
- QueryMultiDb.Tests.Unit: ✅ 1 test passed
- QueryMultiDb.Common.Tests.Unit: ✅ 18 tests passed
- QueryMultiDb.Tests.System: ✅ Builds successfully

The main application runs correctly and displays help as expected.