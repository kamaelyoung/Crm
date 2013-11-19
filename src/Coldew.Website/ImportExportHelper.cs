using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.POIFS;
using NPOI.Util;
using System.Reflection;
using Coldew.Api;
using System.Web.Mvc;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Coldew.Website.Models;
using Coldew.Api.Organization;

namespace Coldew.Website
{
	public class ImportExportHelper
	{
		public static Stream RenderDataTableToExcel(DataTable SourceTable)
		{
			HSSFWorkbook workbook = new HSSFWorkbook();
			MemoryStream ms = new MemoryStream();
			HSSFSheet sheet = workbook.CreateSheet();
			HSSFRow headerRow = sheet.CreateRow(0);

			// handling header.   
			foreach (DataColumn column in SourceTable.Columns)
				headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

			// handling value.   
			int rowIndex = 1;

			foreach (DataRow row in SourceTable.Rows)
			{
				HSSFRow dataRow = sheet.CreateRow(rowIndex);

				foreach (DataColumn column in SourceTable.Columns)
				{
					object objValue = row[column];
					if (objValue.GetType() == typeof(DateTime))
					{
						objValue = DateTime.Parse(objValue.ToString()).ToString("yyyy-MM-dd");
					}
					dataRow.CreateCell(column.Ordinal).SetCellValue(objValue.ToString());
				}

				rowIndex++;
			}

			workbook.Write(ms);
			ms.Flush();
			ms.Position = 0;

			sheet = null;
			headerRow = null;
			workbook = null;

			return ms;
		}

		public static void RenderDataTableToExcel(DataTable SourceTable, string FileName)
		{
			MemoryStream ms = RenderDataTableToExcel(SourceTable) as MemoryStream;
			FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
			byte[] data = ms.ToArray();

			fs.Write(data, 0, data.Length);
			fs.Flush();
			fs.Close();

			data = null;
			ms = null;
			fs = null;
		}

		public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
		{
			HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
			HSSFSheet sheet = workbook.GetSheet(SheetName);

			DataTable table = new DataTable();

			HSSFRow headerRow = sheet.GetRow(HeaderRowIndex);
			int cellCount = headerRow.LastCellNum;

			for (int i = headerRow.FirstCellNum; i < cellCount; i++)
			{
				DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
				table.Columns.Add(column);
			}

			int rowCount = sheet.LastRowNum;

			for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
			{
				HSSFRow row = sheet.GetRow(i);
				DataRow dataRow = table.NewRow();

				for (int j = row.FirstCellNum; j < cellCount; j++)
				{
					HSSFCell cell = row.GetCell(j);
					string objValue = "";
					if (cell.GetType() == typeof(DateTime))
					{
						objValue = DateTime.Parse(cell.ToString()).ToString("yyyy-MM-dd");
					}
					dataRow[j] = objValue.ToString();
				}
			}

			ExcelFileStream.Close();
			workbook = null;
			sheet = null;
			return table;
		}

		public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
		{
			HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
			HSSFSheet sheet = workbook.GetSheetAt(SheetIndex);

			DataTable table = new DataTable();

			HSSFRow headerRow = sheet.GetRow(HeaderRowIndex);
			int cellCount = headerRow.LastCellNum;

			for (int i = headerRow.FirstCellNum; i < cellCount; i++)
			{
				DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
				table.Columns.Add(column);
			}

			int rowCount = sheet.LastRowNum;

			for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
			{
				HSSFRow row = sheet.GetRow(i);
				DataRow dataRow = table.NewRow();

				for (int j = row.FirstCellNum; j < cellCount; j++)
				{
					if (row.GetCell(j) != null)
						dataRow[j] = row.GetCell(j).ToString();
				}

				table.Rows.Add(dataRow);
			}

			ExcelFileStream.Close();
			workbook = null;
			sheet = null;
			return table;
		}

		/// <summary>读取excel   
		/// 默认第一行为标头   
		/// </summary>   
		/// <param name="path">excel文档路径</param>   
		/// <returns></returns>   
		public static DataTable RenderDataTableFromExcel(string path)
		{
			DataTable dt = new DataTable();

			HSSFWorkbook hssfworkbook;
			using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				hssfworkbook = new HSSFWorkbook(file);
			}
			HSSFSheet sheet = hssfworkbook.GetSheetAt(0);
			System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

			HSSFRow headerRow = sheet.GetRow(0);
			int cellCount = headerRow.LastCellNum;

			for (int j = 0; j < cellCount; j++)
			{
				HSSFCell cell = headerRow.GetCell(j);
				dt.Columns.Add(cell.ToString());
			}

			for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
			{
				HSSFRow row = sheet.GetRow(i);
				DataRow dataRow = dt.NewRow();

				for (int j = row.FirstCellNum; j < cellCount; j++)
				{
					HSSFCell cell = row.GetCell(j);
					if (cell != null)
					{
						//string format = this.CellStyle.GetDataFormatString();
						//
						//return formatter.FormatCellValue(this);
						HSSFDataFormatter formatter = new HSSFDataFormatter();
						if (cell.CellType == HSSFCell.CELL_TYPE_NUMERIC && HSSFDateUtil.IsCellDateFormatted(cell))
						{
							if (cell.DateCellValue.Hour == 0 && cell.DateCellValue.Minute == 0 && cell.DateCellValue.Second == 0)
							{
								dataRow[j] = cell.DateCellValue.ToString("yyyy-MM-dd");
							}
							else
							{
								dataRow[j] = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
							}
						}
						else
						{
							dataRow[j] = cell.ToString();
						}
					}
				}

				dt.Rows.Add(dataRow);
			}

			//while (rows.MoveNext())   
			//{   
			//    HSSFRow row = (HSSFRow)rows.Current;   
			//    DataRow dr = dt.NewRow();   

			//    for (int i = 0; i < row.LastCellNum; i++)   
			//    {   
			//        HSSFCell cell = row.GetCell(i);   


			//        if (cell == null)   
			//        {   
			//            dr[i] = null;   
			//        }   
			//        else   
			//        {   
			//            dr[i] = cell.ToString();   
			//        }   
			//    }   
			//    dt.Rows.Add(dr);   
			//}   

			return dt;
		}

        public static string GetImportTemplate(string userAccount, string objectId, Controller controller)
        {
            string path = controller.Server.MapPath("~/Template.xls");
            FileStream stream = System.IO.File.OpenRead(path);
            HSSFWorkbook workbook = new HSSFWorkbook(stream);
            HSSFSheet sheet = workbook.GetSheetAt(0);

            HSSFRow row = sheet.CreateRow(0);

            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(userAccount, objectId);
            int i = 0;
            foreach (Coldew.Api.FieldInfo filed in coldewObject.Fields)
            {
                if (filed.Type != FieldType.CreatedTime &&
                filed.Type != FieldType.CreatedUser &&
                filed.Type != FieldType.ModifiedTime &&
                filed.Type != FieldType.ModifiedUser)
                {
                    row.CreateCell(i).SetCellValue(filed.Name);
                    i++;
                }
            }

            string tempPath = controller.Server.MapPath(string.Format("~/Temp/{0}.xls", Guid.NewGuid().ToString()));
            if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
            }
            Stream newStream = System.IO.File.Open(tempPath, FileMode.Create);
            workbook.Write(newStream);
            newStream.Close();

            stream.Close();
            workbook = null;
            sheet = null;
            return tempPath;
        }

        public static string GetUploadImportFileJsonFile(string userAccount, string objectId, Controller controller)
        {
            HttpPostedFileBase importFile = controller.Request.Files["importFile"];
            string tempPath = controller.Server.MapPath("~/Temp");
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            string tempFileName = Guid.NewGuid().ToString() + Path.GetExtension(importFile.FileName);
            string tempFilePath = Path.Combine(tempPath, tempFileName);
            importFile.SaveAs(tempFilePath);

            DataTable customerTable = ImportExportHelper.RenderDataTableFromExcel(tempFilePath);
            List<JObject> importModels = new List<JObject>();
            if (customerTable != null)
            {
                ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(userAccount, objectId);
                foreach (DataRow customerRow in customerTable.Rows)
                {
                    JObject importModel = new JObject();
                    foreach (Coldew.Api.FieldInfo filed in coldewObject.Fields)
                    {
                        if (customerTable.Columns.Contains(filed.Name))
                        {
                            if (customerRow[filed.Name] != null)
                            {
                                importModel.Add(filed.Code, customerRow[filed.Name].ToString());
                            }
                        }
                    }
                    importModels.Add(importModel);
                }
            }
            string jsonFilePath = controller.Server.MapPath("~/Temp/" + Guid.NewGuid().ToString());
            StreamWriter jsonStreamWriter = new StreamWriter(jsonFilePath);
            jsonStreamWriter.Write(JsonConvert.SerializeObject(importModels));
            jsonStreamWriter.Close();
            return jsonFilePath;
        }

        public static List<DataGridColumnModel> GetImportColumns(string userAccount, string objectId)
        {
            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(userAccount, objectId);
            List<Coldew.Api.FieldInfo> fields = coldewObject.Fields.ToList();

            List<DataGridColumnModel> columns = new List<DataGridColumnModel>();
            columns.Add(new DataGridColumnModel { field = "importMessage", title = "导入结果", width = 80 });
            columns.AddRange(fields.Where(x => x.Type != FieldType.CreatedTime && 
                x.Type != FieldType.CreatedUser && 
                x.Type != FieldType.ModifiedTime && 
                x.Type != FieldType.ModifiedUser)
                .Select(x => new DataGridColumnModel { field = x.Code, title = x.Name, width = 80 }));
            return columns;
        }

        public static string Export(string userAccount, List<JObject> models, string objectId)
        {
            string path = HttpContext.Current.Server.MapPath("~/Template.xls");
            FileStream stream = System.IO.File.OpenRead(path);
            HSSFWorkbook workbook = new HSSFWorkbook(stream);
            HSSFSheet sheet = workbook.GetSheetAt(0);

            ColdewObjectInfo coldewObject = WebHelper.ColdewObjectService.GetFormById(userAccount, objectId);
            HSSFRow nameRow = sheet.CreateRow(0);
            int nameCellIndex = 0;
            foreach (Coldew.Api.FieldInfo filed in coldewObject.Fields)
            {
                nameRow.CreateCell(nameCellIndex).SetCellValue(filed.Name);
                nameCellIndex++;
            }

            int dataRowIndex = 1;
            foreach (JObject model in models)
            {
                HSSFRow dataRow = sheet.CreateRow(dataRowIndex);

                int dataCellIndex = 0;
                foreach (Coldew.Api.FieldInfo filed in coldewObject.Fields)
                {
                    JToken value = model[filed.Code];
                    var cell = dataRow.CreateCell(dataCellIndex);
                    if (value != null)
                    {
                        cell.SetCellValue(value.ToString());
                    }
                    dataCellIndex++;
                }
                dataRowIndex++;
            }

            string tempPath = HttpContext.Current.Server.MapPath(string.Format("~/Temp/{0}.xls", Guid.NewGuid().ToString()));
            if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
            }
            Stream newStream = System.IO.File.Open(tempPath, FileMode.Create);
            workbook.Write(newStream);
            newStream.Close();

            stream.Close();
            workbook = null;
            sheet = null;

            return tempPath;
        }

        public static List<string> GetUserAccounts(string[] nameOrAccounts, Dictionary<string, UserInfo> userAccountDic, Dictionary<string, UserInfo> userNameDic)
        {
            List<string> salesUserAccounts = new List<string>();
            foreach (string nameOrAccount in nameOrAccounts)
            {
                if (userAccountDic.ContainsKey(nameOrAccount.Trim()))
                {
                    salesUserAccounts.Add(userAccountDic[nameOrAccount.Trim()].Account);
                }
                else if (userNameDic.ContainsKey(nameOrAccount.Trim()))
                {
                    salesUserAccounts.Add(userNameDic[nameOrAccount.Trim()].Account);
                }
                else
                {
                    throw new Exception("找不到销售员");
                }
            }
            return salesUserAccounts;
        }
	}
}
