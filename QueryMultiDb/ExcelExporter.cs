using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

// ReSharper disable PossiblyMistakenUseOfParamsMethod

namespace QueryMultiDb
{
    public static class ExcelExporter
    {
        public static void Generate(ICollection<Table> tables)
        {
            if (tables == null)
            {
                throw new ArgumentNullException(nameof(tables), "Parameter cannot be null.");
            }

            var destination = Parameters.Instance.OutputDirectory + @"\" + Parameters.Instance.OutputFile;

            using (var spreadSheet = SpreadsheetDocument.Create(destination, SpreadsheetDocumentType.Workbook))
            {
                Logger.Instance.Info($"Created excel file '{destination}'");
                var workbookPart = spreadSheet.AddWorkbookPart();

                var wbsp = spreadSheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                wbsp.Stylesheet = CreateStylesheet();
                wbsp.Stylesheet.Save();

                spreadSheet.WorkbookPart.Workbook = new Workbook
                {
                    Sheets = new Sheets()
                };

                foreach (var table in tables)
                {
                    Logger.Instance.Info("Adding new excel sheet.");
                    AddSheet(spreadSheet, table);
                }

                var logTable = LogsToTable(Logger.Instance.Logs);
                AddSheet(spreadSheet, logTable, "Logs");

                var parameterTable = ParametersToTable(Parameters.Instance);
                AddSheet(spreadSheet, parameterTable, "Parameters");
            }

            Logger.Instance.Info("Excel file closed after generation.");
        }

        private static Stylesheet CreateStylesheet()
        {
            var stylesheet = new Stylesheet();

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
                            Value = "FF7F7F7F"
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

        private static Table ParametersToTable(Parameters parameters)
        {
            var parameterColumns = new TableColumn[2];
            parameterColumns[0] = new TableColumn("Parameter", typeof(string));
            parameterColumns[1] = new TableColumn("Value", typeof(string));

            var parameterRows = new List<TableRow>
            {
                CreateParameterRow("OutputDirectory", parameters.OutputDirectory),
                CreateParameterRow("OutputFile", parameters.OutputFile),
                CreateParameterRow("Overwrite", parameters.Overwrite),
                CreateParameterRow("Targets", Parameters.TargetsToJsonString(parameters.Targets)),
                CreateParameterRow("Query", parameters.Query),
                CreateParameterRow("Debug", parameters.Debug.ToString()),
                CreateParameterRow("ConnectionTimeout", parameters.ConnectionTimeout),
                CreateParameterRow("CommandTimeout", parameters.CommandTimeout),
                CreateParameterRow("Sequential", parameters.Sequential),
                CreateParameterRow("Parallelism", parameters.Parallelism),
                CreateParameterRow("IncludeIP", parameters.IncludeIP),
                CreateParameterRow("Quiet", parameters.Quiet),
                CreateParameterRow("StartKeyPress", parameters.StartKeyPress),
                CreateParameterRow("StopKeyPress", parameters.StopKeyPress),
                CreateParameterRow("HideNulls", parameters.HideNulls)
            };

            var parameterTable = new Table(parameterColumns, parameterRows);

            return parameterTable;
        }

        private static TableRow CreateParameterRow(string parameter, object value)
        {
            var items = new object[2];
            items[0] = parameter;
            items[1] = value;
            var tableRow = new TableRow(items);
            return tableRow;
        }

        private static Table LogsToTable(ICollection<Log> logs)
        {
            var logColumns = new TableColumn[8];
            logColumns[0] = new TableColumn("Id", typeof(int));
            logColumns[1] = new TableColumn("Date", typeof(string));
            logColumns[2] = new TableColumn("Server", typeof(string));
            logColumns[3] = new TableColumn("Database", typeof(string));
            logColumns[4] = new TableColumn("ThreadId", typeof(int));
            logColumns[5] = new TableColumn("Level", typeof(string));
            logColumns[6] = new TableColumn("Message", typeof(string));
            logColumns[7] = new TableColumn("Exception", typeof(string));

            var logRows = new List<TableRow>(logs.Count);

            foreach (var log in logs)
            {
                var items = new object[8];
                items[0] = log.Id;
                items[1] = log.Date.ToString("o");
                items[2] = log.Server ?? "";
                items[3] = log.Database ?? "";
                items[4] = log.ThreadId;
                items[5] = log.Level;
                items[6] = log.Message;
                items[7] = log.Exception?.ToString() ?? "";
                var tableRow = new TableRow(items);
                logRows.Add(tableRow);
            }

            var logTable = new Table(logColumns, logRows);

            return logTable;
        }

        private static void AddSheet(SpreadsheetDocument spreadSheet, Table table, string sheetName = null)
        {
            var sheetPart = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            sheetPart.Worksheet = new Worksheet(sheetData);

            var sheets = spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            var relationshipId = spreadSheet.WorkbookPart.GetIdOfPart(sheetPart);

            uint sheetId = 1;

            if (sheets.Elements<Sheet>().Any())
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            var sheet = new Sheet()
            {
                Id = relationshipId,
                SheetId = sheetId,
                Name = sheetName ?? "Sheet" + sheetId
            };
            sheets.Append(sheet);

            var headerRow = new Row();

            foreach (var column in table.Columns)
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

                for (var columnIndex = 0; columnIndex < table.Columns.Length; columnIndex++)
                {
                    var cell = GetExcelCell(tableRow.ItemArray[columnIndex]);
                    newRow.AppendChild(cell);
                }

                sheetData.AppendChild(newRow);
            }
            
            AddTablePart(sheetPart, table.Columns, table.Rows.Count, spreadSheet.WorkbookPart);
        }

        private static void AddTablePart(WorksheetPart sheetPart, TableColumn[] columns, int rowCount, WorkbookPart workBookPart)
        {
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
                                  .Select(y => (uint) y.Table.Id).DefaultIfEmpty(0U).Max()).DefaultIfEmpty(0U).Max() + 1;

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
            return GetXlsRangeReference(0, 1, columnCount - 1, rowCount + 1);
        }

        private static string GetXlsRangeReference(int col1, int row1, int col2, int row2)
        {
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
            var r = string.Empty;

            do
            {
                r = (char) ((byte) 'A' + colIndex % 26) + r;
            } while ((colIndex = colIndex / 26 - 1) >= 0);

            return r;
        }

        private static Cell GetExcelCell(object item)
        {
            var type = item.GetType();

            if (type == typeof(bool))
            {
                return GetExcelCellAsBoolean(item);
            }

            if (type == typeof(int) || type == typeof(long) || type == typeof(short))
            {
                return GetExcelCellAsInteger(item);
            }

            if (type == typeof(DateTime))
            {
                return GetExcelCellAsDateTime(item);
            }

            if (type == typeof(DBNull))
            {
                return GetExcelCellAsNull();
            }

            if (type == typeof(byte[]))
            {
                return GetExcelCellAsByteArray(item);
            }

            return GetExcelCellAsDefault(item);
        }

        private static Cell GetExcelCellAsDefault(object item)
        {
            var cell = new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(item.ToString())
            };

            return cell;
        }

        private static Cell GetExcelCellAsByteArray(object item)
        {
            var base64String = Convert.ToBase64String((byte[])item, Base64FormattingOptions.None);
            var truncatedText = base64String.Length > 32750
                ? base64String.Substring(0, 32750) + "...<TRUNCATED>"
                : base64String;

            var cell = new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(truncatedText)
            };

            return cell;
        }

        private static Cell GetExcelCellAsNull()
        {
            var text = Parameters.Instance.HideNulls ? string.Empty : "NULL";

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
            var dateTime = (DateTime) item;

            var cell = new Cell
            {
                // Works in Office 2010+ with "s" formated datetime.
                DataType = CellValues.Date,
                CellValue = new CellValue(dateTime.ToString("s")),
                StyleIndex = 1
            };

            return cell;
        }

        private static Cell GetExcelCellAsInteger(object item)
        {
            var cell = new Cell
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(item.ToString())
            };

            return cell;
        }

        private static Cell GetExcelCellAsBoolean(object item)
        {
            var cell = new Cell
            {
                // XXX : CellValues.String and not CellValues.Boolean because we ouput the boolean as a string value.
                DataType = CellValues.String,
                CellValue = new CellValue(item.ToString())
            };

            return cell;
        }
    }
}
