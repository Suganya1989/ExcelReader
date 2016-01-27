using BusinessFacade;
using Excel;
using ServiceCodeDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDIAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteScriptsToFile();

        }

        static void WriteScriptsToFile()
        {
            string filePath = @"C:\Mapping results (review LD 20160105).xlsx";
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            StreamWriter outputFile = new StreamWriter("InsertStatements" + DateTime.Now.ToFileTime() + ".txt");

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();

            foreach (DataTable table in result.Tables)
            {

                foreach (DataRow row in table.Rows)
                {
                    ServiceCodes serviceCodes = new ServiceCodes();
                    StringBuilder sb = new StringBuilder();
                    var counter = 1;

                    if (row[7].ToString() == "Add - new")
                    {
                        sb.Append("Insert into service_codes (edi_code,service_code,svce_code_gp_id) values (");
                        for (int x = 0; x < table.Columns.Count; x++)
                        {
                            if ((x == 0 && !String.IsNullOrEmpty(row[x].ToString())) || x == 1)
                            {
                                sb.Append("\"" + (row[x].ToString()) + "\",");
                              
                            }
                            else if (x == 2)
                            {
                                sb.Append("\"" + (row[x].ToString()) + "\")");
                            }
                            serviceCodes.ServiceCodeID = counter;
                            serviceCodes.EDICode = row[0].ToString();
                            serviceCodes.ServiceCode = row[1].ToString();
                            serviceCodes.SvceCodeGPID = Convert.ToInt64(row[2].ToString());
                            InsertSvceCodeToDB(serviceCodes);
                            counter++;
                        }
                    }


                    outputFile.WriteLine(sb + "\n");

                }

            }


            excelReader.Close();
            outputFile.Close();
        }

        static void InsertSvceCodeToDB(ServiceCodes serviceCodes)
        {
            ServiceCodeCommand command = new ServiceCodeCommand();
            command.InsertServiceCodes(serviceCodes);
        }
    }
}
