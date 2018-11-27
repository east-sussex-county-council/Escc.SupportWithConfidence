using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Escc.SupportWithConfidence.ETL
{
    /// <summary>
    /// This class creates a datatable that represents the raw data from the overnight data csv
    /// It is the core provider information and used later in the ETL process by the ProviderDataTable class
    /// </summary>
    public class ImportDataTable : IImportable
    {
        #region Fields

        DataTable _dtImport;
        #endregion

        #region Properties

        public DataTable Table
        {
            get { return _dtImport; }
            set { _dtImport = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Setup the Import datatable and fills it with data from the overnight csv file
        /// </summary>
        public ImportDataTable()
        {
            _dtImport = new DataTable();
            _dtImport.Columns.Add("UniqueId", typeof(string));
            _dtImport.Columns.Add("Name", typeof(string));
            _dtImport.Columns.Add("Address1", typeof(string));
            _dtImport.Columns.Add("Address2", typeof(string));
            _dtImport.Columns.Add("Address3", typeof(string));
            _dtImport.Columns.Add("Address4", typeof(string));
            _dtImport.Columns.Add("Postcode5", typeof(string));
            _dtImport.Columns.Add("Cat1", typeof(string));
            _dtImport.Columns.Add("Cat2", typeof(string));
            _dtImport.Columns.Add("Cat3", typeof(string));
            _dtImport.Columns.Add("Cat4", typeof(string));
            _dtImport.Columns.Add("Cat5", typeof(string));
            _dtImport.Columns.Add("Cat6", typeof(string));
            _dtImport.Columns.Add("Cat7", typeof(string));
            _dtImport.Columns.Add("Cat8", typeof(string));
            _dtImport.Columns.Add("Cat9", typeof(string));
            _dtImport.Columns.Add("PublishToWeb", typeof(string));
            _dtImport.Columns.Add("PublishAddress", typeof(string));
            _dtImport.Columns.Add("Telephone", typeof(string));
            _dtImport.Columns.Add("Mobile", typeof(string));
            _dtImport.Columns.Add("Email", typeof(string));
            _dtImport.Columns.Add("AddressValidated", typeof(string));
            _dtImport.Columns.Add("Website", typeof(string));
            _dtImport.Columns.Add("Fax", typeof(string));
            _dtImport.Columns.Add("Easting", typeof(string));
            _dtImport.Columns.Add("Northing", typeof(string));
            _dtImport.Columns.Add("PublishPdf", typeof(string));
            _dtImport.Columns.Add("Availability1", typeof(string));
            _dtImport.Columns.Add("Availability2", typeof(string));
            _dtImport.Columns.Add("Availability3", typeof(string));
            _dtImport.Columns.Add("Coverage", typeof(string));
            _dtImport.Columns.Add("Coverage2", typeof(string));
            _dtImport.Columns.Add("ContactName", typeof(string));
            _dtImport.Columns.Add("CrbChecked", typeof(string));
            _dtImport.Columns.Add("CqcChecked", typeof(string));
            _dtImport.Columns.Add("IsBwcMember", typeof(string));

            Fill();

        }
        #endregion


        #region Interface Implementation
        /// <summary>
        /// Fill the data table with the rows from the csv file
        /// </summary>
        public void Fill()
        {
            DataTable dtImportRaw = Extract(ConfigurationManager.AppSettings["ProviderData"]);
            if (dtImportRaw != null)
            {
                DataRow[] filtered = dtImportRaw.Select("", "");
                _dtImport = null;
                _dtImport = dtImportRaw.Clone();
                _dtImport.Columns.Add("Id", typeof(Int32));
                _dtImport.Columns["Id"].AutoIncrement = true;
                _dtImport.Columns["Id"].AutoIncrementSeed = 1;
                _dtImport.Columns["Id"].SetOrdinal(0);

                foreach (var item in filtered)
                {
                    _dtImport.ImportRow(item);
                }
            }

        }

        /// <summary>
        /// Extract the data from the overnight data csv file
        /// 
        /// </summary>
        /// <param name="csvPath"></param>
        /// <returns>DataTable</returns>
        DataTable Extract(string csvPath)
        {

            if (File.GetLastWriteTime(csvPath) < DateTime.Now.AddDays(-2))
            {
                throw new Exception(
                    "Support with Confidence ETL failed. Extract of Flare data is out of date. Flare data will not be imported until this matter is resolved. Overnight csv export has not occurred.");
              
            }




            // The addition of the third  parameter (the delimeter field) is used to split the  csv up. 
            using (var csv = new CsvReader(new StreamReader(csvPath, System.Text.Encoding.GetEncoding("Windows-1252")), true, '|'))
            {
                int fieldCount = csv.FieldCount;

                if (csv.FieldCount == 36)
                {

                    while (csv.ReadNextRecord())
                    {
                        var list = new ArrayList();
                        for (int i = 0; i < fieldCount; i++)
                        {
                            list.Add(csv[i]);
                        }

                        _dtImport.Rows.Add(list.ToArray());

                    }
                }
                else
                {
                   throw new Exception(@"Support with Confidence import failed.  Either a new fields has been added or an old one removed from the export. The import can not happen until this is resolved. Please review the file 'SwC overnight data.csv'");
                   
                }





                return _dtImport;


            }
        }
        #endregion

        /// <summary>
        /// Save the data table to SQL 
        /// </summary>
        public void Commit()
        {

            foreach (DataRow item in _dtImport.Rows)
            {
                var parameters = new SqlParameter[38];

                parameters[0] = new SqlParameter("@UniqueId", SqlDbType.VarChar) { Value = item["UniqueId"] };
                parameters[1] = new SqlParameter("@Name", SqlDbType.VarChar) { Value = item["Name"] };
                parameters[2] = new SqlParameter("@Address1", SqlDbType.VarChar) { Value = item["Address1"] };
                parameters[3] = new SqlParameter("@Address2", SqlDbType.VarChar) { Value = item["Address2"] };
                parameters[4] = new SqlParameter("@Address3", SqlDbType.VarChar) { Value = item["Address3"] };
                parameters[5] = new SqlParameter("@Address4", SqlDbType.VarChar) { Value = item["Address4"] };
                parameters[6] = new SqlParameter("@Postcode5", SqlDbType.VarChar) { Value = item["Postcode5"] };
                parameters[7] = new SqlParameter("@Cat1", SqlDbType.VarChar) { Value = item["Cat1"] };
                parameters[8] = new SqlParameter("@Cat2", SqlDbType.VarChar) { Value = item["Cat2"] };
                parameters[9] = new SqlParameter("@Cat3", SqlDbType.VarChar) { Value = item["Cat3"] };
                parameters[10] = new SqlParameter("@Cat4", SqlDbType.VarChar) { Value = item["Cat4"] };
                parameters[11] = new SqlParameter("@Cat5", SqlDbType.VarChar) { Value = item["Cat5"] };
                parameters[12] = new SqlParameter("@Cat6", SqlDbType.VarChar) { Value = item["Cat6"] };
                parameters[13] = new SqlParameter("@Cat7", SqlDbType.VarChar) { Value = item["Cat7"] };
                parameters[14] = new SqlParameter("@Cat8", SqlDbType.VarChar) { Value = item["Cat8"] };
                parameters[15] = new SqlParameter("@Cat8", SqlDbType.VarChar) { Value = item["Cat8"] };
                parameters[16] = new SqlParameter("@Cat9", SqlDbType.VarChar) { Value = item["Cat9"] };
                parameters[17] = new SqlParameter("@PublishToWeb", SqlDbType.VarChar) { Value = item["PublishToWeb"] };
                parameters[18] = new SqlParameter("@PublishAddress", SqlDbType.VarChar) { Value = item["PublishAddress"] };
                parameters[19] = new SqlParameter("@Telephone", SqlDbType.VarChar) { Value = item["Telephone"] };
                parameters[20] = new SqlParameter("@Mobile", SqlDbType.VarChar) { Value = item["Mobile"] };
                parameters[21] = new SqlParameter("@Email", SqlDbType.VarChar) { Value = item["Email"] };
                parameters[22] = new SqlParameter("@AddressVerfied", SqlDbType.VarChar) { Value = item["AddressValidated"] };
                parameters[23] = new SqlParameter("@Website", SqlDbType.VarChar) { Value = item["Website"] };
                parameters[24] = new SqlParameter("@Fax", SqlDbType.VarChar) { Value = item["Fax"] };
                parameters[25] = new SqlParameter("@Easting", SqlDbType.VarChar) { Value = item["Easting"] };
                parameters[26] = new SqlParameter("@Northing", SqlDbType.VarChar) { Value = item["Northing"] };
                parameters[27] = new SqlParameter("@PublishPdf", SqlDbType.VarChar) { Value = item["PublishPdf"] };
                parameters[28] = new SqlParameter("@Availability1", SqlDbType.VarChar) { Value = item["Availability1"] };
                parameters[29] = new SqlParameter("@Availability2", SqlDbType.VarChar) { Value = item["Availability2"] };
                parameters[30] = new SqlParameter("@Availability3", SqlDbType.VarChar) { Value = item["Availability3"] };
                parameters[31] = new SqlParameter("@Coverage", SqlDbType.VarChar) { Value = item["Coverage"] };
                parameters[32] = new SqlParameter("@Coverage2", SqlDbType.VarChar) { Value = item["Coverage2"] };
                parameters[33] = new SqlParameter("@ContactName", SqlDbType.VarChar) { Value = item["ContactName"] };
                parameters[34] = new SqlParameter("@CrbChecked", SqlDbType.VarChar) { Value = item["CrbChecked"] };
                parameters[35] = new SqlParameter("@CqcChecked", SqlDbType.VarChar) { Value = item["CqcChecked"] };
                parameters[36] = new SqlParameter("@IsBwcMember", SqlDbType.VarChar) { Value = item["IsBwcMember"] };
                parameters[37] = new SqlParameter("@Id", SqlDbType.BigInt) { Value = (int)item["Id"] };

                DataAccess.Save(ConfigurationManager.AppSettings["Save_Import"], parameters);


            }

        }
    }
}
