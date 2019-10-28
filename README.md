# Project status

[![License](https://img.shields.io/:license-BSD-blue.svg)](https://opensource.org/licenses/BSD-3-Clause)

[![Build status](https://ci.appveyor.com/api/projects/status/29cusv9r5hu1r2e5?svg=true)](https://ci.appveyor.com/project/alexandre-lecoq/querymultidb)

[![codecov](https://codecov.io/gh/alexandre-lecoq/QueryMultiDb/branch/master/graph/badge.svg)](https://codecov.io/gh/alexandre-lecoq/QueryMultiDb)

[![Coverage Status](https://coveralls.io/repos/github/alexandre-lecoq/QueryMultiDb/badge.svg?branch=master)](https://coveralls.io/github/alexandre-lecoq/QueryMultiDb?branch=master)


# QueryMultiDb

A command-line tool to query multiple SQL Server databases at once and store results in an excel file.

## Command line parameters

The following parameters are supported :

Parameter|Description|Default|Required
---------|-----------|-------|--------
outputdirectory|Indicates output directory for generated file. The default is the current working directory.||
outputfile|Indicates the name of the generated file.||✔
overwrite|Overwrite output file if it already exists.|false|
targets|Indicates the list of databases to query.||
targetsstandardinput|Indicates the list of databases to query is read from standard input.|false|
targetsfile|Indicates the file containing the list of databases to query.||
query|Indicates the query to execute.||
queryfile|Indicates the file containing the SQL query to execute.||
sequential|Perform queries one at a time.|false|
connectiontimeout|The time (in seconds) to wait for a connection to open.|5|
commandtimeout|The time in seconds to wait for the command to execute.|60|
parallelism|The maximum number of queries running in parallel.|4|
startkeypress|Wait for a key press to start.|false|
stopkeypress|Wait for a key press to stop.|false|
progress|Reports progress on standard error output.|false|
nullscolor|Indicates the color of the NULL text in excel files.|7F7F7F|
shownulls|Show NULL values explicitly rather than showing empty value.|true|
showipaddress|Show server's IP address.|true|
showservername|Show server's name.|true|
showdatabasename|Show database's name.|true|
showextracolumns|Show targets' extra columns.|true|
showlogsheet|Show log sheet in excel file.|true|
showparametersheet|Show parameter sheet in excel file.|true|
showinformationmessages|Show parameter sheet in excel file.|true|
sheetlabels|Defines the sheets' labels.||
discardresults|Discard query results and display counts instead|false|
applicationname|Defines the application name for the SQL server connection.||
exporter|Selects the output file format.|excel|
csvdelimiter|Defines the CSV delimiter used to separate fields.|;|
base10threshold|The inclusive maximum number of bytes for which to use decimal representation.|4|
base16threshold|The inclusive maximum number of bytes for which to use hexadecimal representation.|64|
base64threshold|The inclusive maximum number of bytes for which to use base 64 representation.|262144|

### Example

`QueryMultiDb.exe --progress --parallelism 8 --overwrite --queryfile "set001.sql" --outputfile "set001.xlsx" --targetsfile "set001.targets" --shownulls false`

### Binary data

Binary data can be exported using different encoding :
* external reference
* base 64
* base 16
* base 10
* No data

The selected encoding depends on the data size.
In excel files data might be truncated due to the maximum length of a cell which is 32000+ characters.
External references are not supported in excel files.
Content type is assumed to be `application/octet-stream`.

#### External reference

Up to : any size
Format : `data:reference,<filepath>`
`<filepath>` is the path relative to the root of the ZIP file.
`<filepath>` has the format `<byte1>/<byte2>/<byte3>/<byte4>/<filename>` where `<byte1/2/3/4>` are the first 4 bytes of the filename.
`<filename>` has a maximum length of 250 characters. The minimum length is 4 bytes. It has the format `<identifier>-<byte>`.
`<filename>` has no extension as the content type is assumed to be the generic `application/octet-stream`.
`<identifier>` has characters representing lowercase hexadecimal digits.
`<identifier>` could be a GUID, or something else.
`<byte>` is the first byte of the binary data in lowercase hexadecimal.
Example : `data:reference,4d/13/c5/3b/4d13c53bcbab4cf0bf3693c21e39ae7c-3e`

#### Base 64

Up to : 262144 bytes
Format : `data:base64,<base64_encoded_data>`
Example : `data:base64,ZjYzYTJiYzA3NDBlYzlmZTk0ZjJkYzQ3NDNmOTEyNWRkZmY2NjA3N2E2NzA0ZTk5NWY4MDE0MGYyZGVkNDg3ZDExOA==`

#### Base 16

Up to : 64 bytes
Format : `data:base16,<base16_endoded_data>`
Example : `data:base16,3403d8842ceaa2415212771bd971a745808c27cc582`

#### Base 10

Up to : 4 bytes
Format : `data:base10,<base10_endoded_data>`
Example : `data:base10,1311216559`

#### No data

Up to : 0 bytes
Format : `data:none`
Example : `data:none`

## Targets

The utility expects a JSON formatted file for specifying database targets.
The file can be generated with the [DbTargets](#dbtargets) utility.

### Simple example

```javascript
{
	"DatabaseList": [
		{ "ServerName": "localhost", "DatabaseName": "FUNNY_DB" }
	]
}
```

### Advanced example

```javascript
{
	"ExtraValue1Title": "test 1",
	"ExtraValue2Title": "test 2",
	"ExtraValue3Title": "test 3",
	"ExtraValue4Title": "test 4",
	"ExtraValue5Title": "test 5",
	"ExtraValue6Title": "test 6",
	"DatabaseList": [
		{
			"ServerName": "localhost",
			"DatabaseName": "FUNNY_DB",
			"ExtraValue1": "😸😹😺",
			"ExtraValue2": "🙈🙉🙊",
			"ExtraValue3": "😨😰😱",
			"ExtraValue4": "😇😈😉",
			"ExtraValue5": "🙍🙎🙏",
			"ExtraValue6": "😀😁😂"
		},
		{
			"ServerName": "localhost",
			"DatabaseName": "NASTY_DB",
			"ExtraValue1": "jumbo",
			"ExtraValue2": "guitar",
			"ExtraValue3": "kitchen",
			"ExtraValue4": "failure",
			"ExtraValue5": "rocknroll",
			"ExtraValue6": "beer"
		}
	]
}
```

# DbTargets

A command line utility to generate targets JSON-formated database files.

## Command line parameters

`DbTargets.exe <servername> [regexp]`
* `servername` : The server's name to connect to.
* `regexp` : An optional regular expression to filter out database names.

Check [Regular Expression Language - Quick Reference](https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference) for valid regular expressions.

### Example

`DbTargets.exe sqlserver.name.com ".*log.*" > file.targets`

# GUI

A GUI to use QueryMultiDb in a friendlier way.

## How to run

`QueryMultiDbGui.exe`

# Installation

QueryMultiDb can be installed using several methods.

## Download

Chocolatey package and Windows installer are available in [appveyor artifacts](https://ci.appveyor.com/project/alexandre-lecoq/querymultidb/build/artifacts).

## Chocolatey

It can be installed, upgraded and uninstalled using [chocolatey](https://chocolatey.org/packages/QueryMultiDb).
To install QueryMultiDb, run the following command from the command line or from PowerShell:
`choco install querymultidb`
