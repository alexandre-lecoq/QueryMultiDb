using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

// ReSharper disable PossiblyMistakenUseOfParamsMethod

namespace QueryMultiDb.Exporter
{
    public class ExcelExporter : Exporter
    {
        /// <summary>
        /// Total number of rows Maximum limit
        /// </summary>
        /// <remarks>
        /// From "Excel specifications and limits" : "Excel for Office 365 Excel 2019 Excel 2016 Excel 2013 Excel 2010 Excel 2007"
        /// At https://support.office.com/en-us/article/excel-specifications-and-limits-1672b34d-7043-467e-8e27-269d656771c3
        /// </remarks>
        private const int ExcelTotalNumberRowsMaximumLimit = 1_048_576;
        
        private const int MaximumExcelCellStringLength = 32750;

        private const int MaximumExcelSheetNameLength = 31;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override string Name => "Excel";

        public override void Generate(ICollection<Table> inputTables, Stream outputStream)
        {
            using (var spreadSheet = SpreadsheetDocument.Create(outputStream, SpreadsheetDocumentType.Workbook))
            {
                Logger.Info("Created excel file.");
                spreadSheet.AddWorkbookPart();

                var wbsp = spreadSheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                wbsp.Stylesheet = CreateStylesheet();
                wbsp.Stylesheet.Save();

                spreadSheet.WorkbookPart.Workbook = new Workbook
                {
                    Sheets = new Sheets()
                };

                var progressReporter = new ProgressReporter("ExcelExporter", Parameters.Instance.Targets.Databases.Count(), s => Console.Error.WriteLine(s));

                var tableIndex = 0;
                foreach (var table in inputTables)
                {
                    if (!Parameters.Instance.ShowInformationMessages && table.Id == Table.InformationMessagesId && inputTables.Count > 1)
                    {
                        continue;
                    }

                    Logger.Info("Adding new excel sheet.");

                    if (table.Rows.Count > ExcelTotalNumberRowsMaximumLimit - 1)
                    {
                        Logger.Error($"Table '{table.Id}' not exported in excel file because it had too many rows. The table has {table.Rows.Count} rows and {table.Columns.Length} columns. The maximum number of rows supported by excel is {ExcelTotalNumberRowsMaximumLimit}.");
                        continue;
                    }

                    var sheetName = GetPartName(table, tableIndex);
                    AddSheet(spreadSheet, table, sheetName);
                    progressReporter.Increment();
                    tableIndex++;
                }

                progressReporter.Done();

                MemoryManager.Clean();

                var target = GetLogTableTarget();

                // If no data sheets were added, we must force adding built-in sheets because an excel file without sheet would be invalid.
                // Showing built-in sheets also helps with the diagnostic.
                var forceBuiltInSheets = tableIndex == 0;

                if (forceBuiltInSheets)
                {
                    Logger.Warn("No data sets to export. Forcing built-in sheets.");
                }

                Logger.Info("Excel file logging horizon. Check console output to see beyond horizon.");

                if (Parameters.Instance.ShowLogSheet || forceBuiltInSheets)
                {
                    var logTable = target.Logs;
                    var partName = GetPartName(logTable, tableIndex++);
                    AddSheet(spreadSheet, logTable, partName);
                }

                if (Parameters.Instance.ShowParameterSheet || forceBuiltInSheets)
                {
                    var parameterTable = ParametersToTable(Parameters.Instance);
                    var partName = GetPartName(parameterTable, tableIndex++);
                    AddSheet(spreadSheet, parameterTable, partName);
                }

                MemoryManager.Clean();

                Logger.Info("Finalizing excel file writing.");
            }
        }

        private new static string GetPartName(Table table, int tableIndex)
        {
            var basePartName = Exporter.GetPartName(table, tableIndex);
            var sheetPartName = $"Sheet{tableIndex + 1}";

            return basePartName ?? sheetPartName;
        }

        private static Stylesheet CreateStylesheet()
        {
            var stylesheet = new Stylesheet();

            var nullsColorValue = "FF" + Parameters.Instance.NullsColor;

            var fonts = new Fonts(
                new Font
                {
                    FontName = new FontName
                    {
                        Val = "Calibri"
                    },
                    FontSize = new FontSize
                    {
                        Val = 11
                    }
                },
                new Font
                {
                    FontName = new FontName
                    {
                        Val = "Calibri"
                    },
                    FontSize = new FontSize
                    {
                        Val = 11
                    },
                    Italic = new Italic(),
                    Color = new Color
                    {
                        Rgb = new HexBinaryValue()
                        {
                            Value = nullsColorValue
                        }
                    }
                }
            );
            
            fonts.Count = (uint) fonts.ChildElements.Count;

            var fills = new Fills(
                new Fill
                {
                    PatternFill = new PatternFill
                    {
                        PatternType = PatternValues.None
                    }
                },

                new Fill
                {
                    PatternFill = new PatternFill
                    {
                        PatternType = PatternValues.Gray125
                    }
                }
            );

            fills.Count = (uint) fills.ChildElements.Count;

            var borders = new Borders(
                new Border
                {
                    LeftBorder = new LeftBorder(),
                    RightBorder = new RightBorder(),
                    TopBorder = new TopBorder(),
                    BottomBorder = new BottomBorder(),
                    DiagonalBorder = new DiagonalBorder()
                }
            );

            borders.Count = (uint) borders.ChildElements.Count;

            var cellStyleFormats = new CellStyleFormats(
                new CellFormat
                {
                    NumberFormatId = 0,
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0
                }
            );

            cellStyleFormats.Count = (uint) cellStyleFormats.ChildElements.Count;

            var numberingFormats = new NumberingFormats(
                new NumberingFormat
                {
                    NumberFormatId = 164u,
                    FormatCode = "yyyy/mm/dd hh:mm:ss.000"
                }
            );

            numberingFormats.Count = (uint) numberingFormats.ChildElements.Count;

            var cellFormats = new CellFormats(
                new CellFormat
                {
                    NumberFormatId = 0,
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0,
                    FormatId = 0
                },
                new CellFormat
                {
                    NumberFormatId = 164u,
                    FontId = 0,
                    FillId = 0,
                    BorderId = 0,
                    FormatId = 0,
                    ApplyNumberFormat = true
                },
                new CellFormat
                {
                    NumberFormatId = 0,
                    FontId = 1,
                    FillId = 0,
                    BorderId = 0,
                    FormatId = 0
                }
            );

            cellFormats.Count = (uint) cellFormats.ChildElements.Count;

            stylesheet.Append(numberingFormats);
            stylesheet.Append(fonts);
            stylesheet.Append(fills);
            stylesheet.Append(borders);
            stylesheet.Append(cellStyleFormats);
            stylesheet.Append(cellFormats);

            var cellStyles = new CellStyles(
                new CellStyle
                {
                    Name = "Normal",
                    FormatId = 0,
                    BuiltinId = 0
                }
            );

            cellStyles.Count = (uint) cellStyles.ChildElements.Count;
            stylesheet.Append(cellStyles);

            var differentialFormats = new DifferentialFormats
            {
                Count = 0
            };

            stylesheet.Append(differentialFormats);

            var tableStyles = new TableStyles
            {
                Count = 0,
                DefaultTableStyle = "TableStyleMedium2",
                DefaultPivotStyle = "PivotStyleLight16"
            };

            stylesheet.Append(tableStyles);

            return stylesheet;
        }

        private static void AddSheet(SpreadsheetDocument spreadSheet, Table table, string sheetName)
        {
            if (spreadSheet == null)
            {
                throw new ArgumentNullException(nameof(spreadSheet));
            }

            if (sheetName == null)
            {
                throw new ArgumentNullException(nameof(sheetName));
            }

            var sheetPart = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            sheetPart.Worksheet = new Worksheet(sheetData);

            var sheets = spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            var relationshipId = spreadSheet.WorkbookPart.GetIdOfPart(sheetPart);

            string truncatedSheetName;

            if (sheetName.Length > MaximumExcelSheetNameLength)
            {
                truncatedSheetName = sheetName.Substring(0, MaximumExcelSheetNameLength);
                Logger.Warn($"Sheet name was truncated. Full name was '{sheetName}', truncated name is '{truncatedSheetName}'.");
            }
            else
            {
                truncatedSheetName = sheetName;
            }

            uint sheetId = 1;

            if (sheets.Elements<Sheet>().Any())
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            var sheet = new Sheet()
            {
                Id = relationshipId,
                SheetId = sheetId,
                Name = truncatedSheetName
            };

            sheets.Append(sheet);

            var headerRow = new Row();

            var columnSet = GenerateValidAndUniqueColumnNames(table.Columns);

            foreach (var column in columnSet)
            {
                var cell = new Cell
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(column.ColumnName)
                };
                headerRow.AppendChild(cell);
            }

            sheetData.AppendChild(headerRow);

            foreach (var tableRow in table.Rows)
            {
                var newRow = new Row();

                for (var columnIndex = 0; columnIndex < columnSet.Length; columnIndex++)
                {
                    var cell = GetExcelCell(tableRow.ItemArray[columnIndex]);
                    newRow.AppendChild(cell);
                }

                sheetData.AppendChild(newRow);
            }

            if (table.Rows.Count > 0)
            {
                // The sheet cannot have a valid table definition if the table has no data rows at all.
                AddTablePart(sheetPart, columnSet, table.Rows.Count, spreadSheet.WorkbookPart);
            }
        }

        private static void AddTablePart(WorksheetPart sheetPart, TableColumn[] columns, int rowCount,
            WorkbookPart workBookPart)
        {
            if (sheetPart == null)
            {
                throw new ArgumentNullException(nameof(sheetPart));
            }

            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (rowCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }

            if (workBookPart == null)
            {
                throw new ArgumentNullException(nameof(workBookPart));
            }

            var rangeReference = GetXlsTableRangeReference(columns.Length, rowCount);

            var ignoredErrors = new IgnoredErrors(
                new IgnoredError
                {
                    NumberStoredAsText = true,
                    SequenceOfReferences = new ListValue<StringValue>
                    {
                        InnerText = rangeReference
                    }
                }
            );

            // Ignored errors must be added before table parts.
            sheetPart.Worksheet.Append(ignoredErrors);

            var tableDefinitionPart = sheetPart.AddNewPart<TableDefinitionPart>();
            var autoFilter = new AutoFilter {Reference = rangeReference};
            var tableColumns = new TableColumns {Count = (uint) columns.Length};
            var styleInfo = new TableStyleInfo
            {
                Name = "TableStyleMedium2",
                ShowFirstColumn = false,
                ShowLastColumn = false,
                ShowRowStripes = true,
                ShowColumnStripes = false
            };

            var tableId = workBookPart.WorksheetParts
                              .Select(x => x.TableDefinitionParts.Where(y => y.Table != null)
                                  .Select(y => (uint) y.Table.Id).DefaultIfEmpty(0U).Max()).DefaultIfEmpty(0U).Max() +
                          1;

            var table =
                new DocumentFormat.OpenXml.Spreadsheet.Table(autoFilter, tableColumns, styleInfo)
                {
                    Id = tableId,
                    Name = "Table" + tableId,
                    DisplayName = "Table" + tableId,
                    Reference = rangeReference,
                    TotalsRowShown = false
                };

            for (var i = 0; i < columns.Length; i++)
            {
                table.TableColumns.Append(
                    new DocumentFormat.OpenXml.Spreadsheet.TableColumn
                    {
                        Id = (uint) (i + 1),
                        Name = columns[i].ColumnName
                    });
            }

            tableDefinitionPart.Table = table;

            var tableParts = new TableParts(
                new TablePart
                {
                    Id = sheetPart.GetIdOfPart(tableDefinitionPart)
                }
            )
            {
                Count = 1U
            };

            sheetPart.Worksheet.Append(tableParts);
        }

        private static string GetXlsTableRangeReference(int columnCount, int rowCount)
        {
            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount));
            }

            if (rowCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            }

            return GetXlsRangeReference(0, 1, columnCount - 1, rowCount + 1);
        }

        private static string GetXlsRangeReference(int col1, int row1, int col2, int row2)
        {
            if (col1 < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(col1));
            }

            if (row1 < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row1));
            }

            if (col2 < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(col2));
            }

            if (row2 < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row2));
            }

            var x = GetXlsCellReference(Math.Min(col1, col2), Math.Min(row1, row2));
            var y = GetXlsCellReference(Math.Max(col1, col2), Math.Max(row1, row2));

            return $"{x}:{y}";
        }

        private static string GetXlsCellReference(int col, int row)
        {
            return $"{GetXlsColumnReference(col)}{row}";
        }

        private static string GetXlsColumnReference(int colIndex)
        {
            if (colIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(colIndex));
            }

            var r = string.Empty;

            do
            {
                r = (char) ((byte) 'A' + colIndex % 26) + r;
            } while ((colIndex = colIndex / 26 - 1) >= 0);

            return r;
        }

        private static Cell GetExcelCell(object item)
        {
            switch (item)
            {
                case null:
                    throw new ArgumentNullException(nameof(item), "Parameter cannot be null.");
                case bool _:
                    return GetExcelCellAsBoolean(item);
                case int _:
                case long _:
                case short _:
                    return GetExcelCellAsInteger(item);
                case DateTime _:
                    return GetExcelCellAsDateTime(item);
                case DBNull _:
                    return GetExcelCellAsNull();
                case byte[] _:
                    return GetExcelCellAsByteArray(item);
                default:
                    return GetExcelCellAsDefault(item);
            }
        }

        private static Cell GetExcelCellAsDefault(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var truncatedText = TruncateTextForExcelCell(item.ToString());

            var cell = new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(truncatedText)
            };

            return cell;
        }

        private static Cell GetExcelCellAsByteArray(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var encodedBytes = ByteArrayToString((byte[]) item, false);
            var truncatedText = TruncateTextForExcelCell(encodedBytes);

            var cell = new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(truncatedText)
            };

            return cell;
        }

        private static Cell GetExcelCellAsNull()
        {
            var text = Parameters.Instance.ShowNulls ? "NULL" : string.Empty;

            var cell = new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(text),
                StyleIndex = 2
            };

            return cell;
        }

        private static Cell GetExcelCellAsDateTime(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var dateTime = (DateTime) item;

#if false
            // This code uses the CellValues.Date data type, but doesn't support milliseconds.
            var cell = new Cell
            {
                DataType = CellValues.Date,
                CellValue = new CellValue(dateTime.ToString("s")),
                StyleIndex = 1
            };
#else
            // This code uses the CellValues.Number data type, but supports milliseconds.
            var cell = new Cell
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(dateTime.ToOADate().ToString(CultureInfo.InvariantCulture)),
                StyleIndex = 1
            };
#endif

            return cell;
        }

        private static Cell GetExcelCellAsInteger(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var cell = new Cell
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(item.ToString())
            };

            return cell;
        }

        private static Cell GetExcelCellAsBoolean(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var cell = new Cell
            {
                // XXX : CellValues.String and not CellValues.Boolean because we output the boolean as a string value.
                DataType = CellValues.String,
                CellValue = new CellValue(item.ToString())
            };

            return cell;
        }

        private static string TruncateTextForExcelCell(string inputString)
        {
            if (inputString == null)
            {
                throw new ArgumentNullException(nameof(inputString));
            }

            var truncateLength = Math.Min(inputString.Length, MaximumExcelCellStringLength);
            var builder = new StringBuilder(inputString, 0, truncateLength, truncateLength + 32);

            if (inputString.Length > builder.Length)
            {
                builder.Append("...<TRUNCATED>");
            }

            // Also remove invalid XML characters
            // (lower codes control characters,
            // higher control characters are not removed,
            // because of suspected uselessness).
            builder
                .Replace("\x00", "").Replace("\x01", "").Replace("\x02", "").Replace("\x03", "").Replace("\x04", "")
                .Replace("\x05", "").Replace("\x06", "").Replace("\x07", "").Replace("\x08", "").Replace("\x09", "\t")
                .Replace("\x0A", "\n").Replace("\x0B", "").Replace("\x0C", "").Replace("\x0D", "\r").Replace("\x0E", "")
                .Replace("\x0F", "").Replace("\x10", "").Replace("\x11", "").Replace("\x12", "").Replace("\x13", "")
                .Replace("\x14", "").Replace("\x15", "").Replace("\x16", "").Replace("\x17", "").Replace("\x18", "")
                .Replace("\x19", "").Replace("\x1A", "").Replace("\x1B", "").Replace("\x1C", "").Replace("\x1D", "")
                .Replace("\x1E", "").Replace("\x1F", "");

            return builder.ToString();
        }
    }
}
