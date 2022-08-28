using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Command.Commands
{
    public class ExcelFile<T>
    {
        public readonly List<T> list;
        public string FileName => $"{typeof(T).Name}.xlsx";
        public string FileType => $"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcelFile(List<T> list)
        {
            this.list = list;
        }

        public MemoryStream Create()
        {
            var workbook = new XLWorkbook();
            var dataset = new DataSet();
            dataset.Tables.Add(GetTable());
            workbook.Worksheets.Add(dataset);
            var excelMemory = new MemoryStream();
            workbook.SaveAs(excelMemory);
            //return Task.FromResult(excelMemory); //Task<MemoryStream> dönüşü için yazılan kod.
            return excelMemory;
        }

        private DataTable GetTable()
        {
            var table = new DataTable();
            var type = typeof(T);
            type.GetProperties().ToList().ForEach(x =>
            {
                table.Columns.Add(x.Name,x.PropertyType);
            });

            this.list.ForEach(x =>
            {
                var values = type.GetProperties().Select(m => m.GetValue(x)).ToArray();
                table.Rows.Add(values);
            });
            
            return table;
        }

    }
}
