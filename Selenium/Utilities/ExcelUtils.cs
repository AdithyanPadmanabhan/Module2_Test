﻿using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject_JioMart.Utilities
{
    internal class ExcelUtils
    {


        public static List<InputData> ReadSearchDataExcelData(string excelFilePath, string sheetName)
        {
            List<InputData> excelDataList = new List<InputData>();
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true,
                        }
                    });

                    var dataTable = result.Tables[sheetName];

                    if (dataTable != null)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            InputData excelData = new InputData
                            {
                               
                                Username = GetValueOrDefault(row,"username"),
                                Password = GetValueOrDefault(row, "password"),
                                CourseId=GetValueOrDefault(row,"courseid")

                            };

                            excelDataList.Add(excelData);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Sheet '{sheetName}' not found in the Excel file.");
                    }
                }
            }

            return excelDataList;
        }

        static string GetValueOrDefault(DataRow row, string columnName)
        {
            Console.WriteLine(row + "  " + columnName);
            return row.Table.Columns.Contains(columnName) ? row[columnName]?.ToString() : null;
        }
    }
}
