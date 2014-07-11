using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Escc.SupportWithConfidence.ETL
{
    /// <summary>
    /// This class creates the Provider data table which is at the core of the support with confidence system
    /// </summary>
    public class ProviderDataTable : IImportable
    {
        readonly DataTable _dtImport;
        DataTable dtProvider;

        public DataTable Table
        {
            get { return dtProvider; }
            set { dtProvider = value; }
        }

        /// <summary>
        /// This methods creates the table and calls the fill method
        /// The CSV files has already been extracted and transformed into the import table
        /// before this method gets called.
        /// </summary>
        /// <param name="import"></param>
        public ProviderDataTable(DataTable import)
        {
            _dtImport = import;
            dtProvider = new DataTable();
            dtProvider.Columns.Add("Id", typeof(Int32));
            dtProvider.Columns.Add("FlareId", typeof(Int32));
            dtProvider.Columns.Add("Name", typeof(string));
            dtProvider.Columns.Add("Address1", typeof(string));
            dtProvider.Columns.Add("Address2", typeof(string));
            dtProvider.Columns.Add("Address3", typeof(string));
            dtProvider.Columns.Add("Address4", typeof(string));
            dtProvider.Columns.Add("Postcode", typeof(string));
            dtProvider.Columns.Add("PublishToWeb", typeof(bool));
            dtProvider.Columns.Add("PublishAddress", typeof(bool));
            dtProvider.Columns.Add("Telephone", typeof(string));
            dtProvider.Columns.Add("Mobile", typeof(string));
            dtProvider.Columns.Add("Email", typeof(string));
            dtProvider.Columns.Add("Website", typeof(string));
            dtProvider.Columns.Add("Fax", typeof(string));
            dtProvider.Columns.Add("Easting", typeof(string));
            dtProvider.Columns.Add("Northing", typeof(string));
            dtProvider.Columns.Add("Availability1", typeof(string));
            dtProvider.Columns.Add("Availability2", typeof(string));
            dtProvider.Columns.Add("Availability3", typeof(string));
            dtProvider.Columns.Add("Coverage", typeof(string));
            dtProvider.Columns.Add("Coverage2", typeof(string));
            dtProvider.Columns.Add("ContactName", typeof(string));
            dtProvider.Columns.Add("CrbChecked", typeof(string));
            dtProvider.Columns.Add("CqcChecked", typeof(string));
            dtProvider.Columns.Add("IsBwcMember", typeof(bool));

            
                Fill();
          
        }

        /// <summary>
        /// This methods fills the data table to hold provider information
        /// </summary>
        public void Fill()
        {
            var list = new ArrayList();
            foreach (DataRow item in _dtImport.Rows)
            {
                list.Clear();

                list.Add(item["Id"]);
                list.Add(item["UniqueId"]);
                list.Add(item["Name"]);
                list.Add(item["Address1"]);
                list.Add(item["Address2"]);
                list.Add(item["Address3"]);
                list.Add(item["Address4"]);
                list.Add(item["Postcode5"]);
                list.Add(item["PublishToWeb"].ToString() == "Yes");
                list.Add(item["PublishAddress"].ToString() == "Yes");
                list.Add(item["Telephone"]);
                list.Add(item["Mobile"]);
                list.Add(item["Email"]);

                // Fix to website address which are missing protocol
                if (item["Website"].ToString().Length > 0)
                {
                    string website = item["Website"].ToString().StartsWith("http://") ? item["Website"].ToString() : "http://" + item["Website"];
                    list.Add(website);
                }
                else
                {
                    list.Add(string.Empty);
                }
                list.Add(item["Fax"]);
                list.Add(item["Easting"]);
                list.Add(item["Northing"]);
                list.Add(item["Availability1"]);
                list.Add(item["Availability2"]);
                list.Add(item["Availability3"]);
                list.Add(item["Coverage"]);
                list.Add(item["Coverage2"]);
                list.Add(item["ContactName"]);
                list.Add(item["CrbChecked"]);
                list.Add(item["CqcChecked"]);
                list.Add(item["IsBwcMember"].ToString() == "Yes");

                dtProvider.Rows.Add(list.ToArray());
            }



        }

        /// <summary>
        /// Saves the provider data into SQL
        /// </summary>
        public void Commit()
        {
            foreach (DataRow item in dtProvider.Rows)
            {

                var parameters = new SqlParameter[26];



                parameters[0] = new SqlParameter("@Id", SqlDbType.BigInt) { Value = (int)item["Id"] };
                parameters[1] = new SqlParameter("@FlareId", SqlDbType.BigInt) { Value = (int)item["FlareId"] };
                parameters[2] = new SqlParameter("@ProviderName", SqlDbType.VarChar) { Value = item["Name"] };
                parameters[3] = new SqlParameter("@Address1", SqlDbType.VarChar) { Value = item["Address1"] };
                parameters[4] = new SqlParameter("@Address2", SqlDbType.VarChar) { Value = item["Address2"] };
                parameters[5] = new SqlParameter("@Address3", SqlDbType.VarChar) { Value = item["Address3"] };
                parameters[6] = new SqlParameter("@Address4", SqlDbType.VarChar) { Value = item["Address4"] };
                parameters[7] = new SqlParameter("@Postcode", SqlDbType.VarChar) { Value = item["Postcode"] };
                parameters[8] = new SqlParameter("@PublishAddress", SqlDbType.Bit) { Value = (bool)item["PublishAddress"] };
                parameters[9] = new SqlParameter("@Telephone", SqlDbType.VarChar) { Value = item["Telephone"] };
                parameters[10] = new SqlParameter("@Mobile", SqlDbType.VarChar) { Value = item["Mobile"] };
                parameters[11] = new SqlParameter("@Email", SqlDbType.VarChar) { Value = item["Email"] };
                parameters[12] = new SqlParameter("@Website", SqlDbType.VarChar) { Value = item["Website"] };
                parameters[13] = new SqlParameter("@Fax", SqlDbType.VarChar) { Value = item["Fax"] };
                parameters[14] = new SqlParameter("@Easting", SqlDbType.VarChar) { Value = item["Easting"] };
                parameters[15] = new SqlParameter("@Northing", SqlDbType.VarChar) { Value = item["Northing"] };
                parameters[16] = new SqlParameter("@PublishToWeb", SqlDbType.Bit) { Value = (bool)item["PublishToWeb"] };
                parameters[17] = new SqlParameter("@Availability1", SqlDbType.VarChar) { Value = item["Availability1"] };
                parameters[18] = new SqlParameter("@Availability2", SqlDbType.VarChar) { Value = item["Availability2"] };
                parameters[19] = new SqlParameter("@Availability3", SqlDbType.VarChar) { Value = item["Availability3"] };
                parameters[20] = new SqlParameter("@Coverage", SqlDbType.VarChar) { Value = item["Coverage"] };
                parameters[21] = new SqlParameter("@Coverage2", SqlDbType.VarChar) { Value = item["Coverage2"] };
                parameters[22] = new SqlParameter("@ContactName", SqlDbType.VarChar) { Value = item["ContactName"] };
                parameters[23] = new SqlParameter("@CRBCheckDate", SqlDbType.VarChar) { Value = item["CrbChecked"] };
                parameters[24] = new SqlParameter("@CQCCheckDate", SqlDbType.VarChar) { Value = item["CqcChecked"] };
                parameters[25] = new SqlParameter("@BWCFlag", SqlDbType.Bit) { Value = (bool)item["IsBwcMember"] };





                DataAccess.Save(ConfigurationManager.AppSettings["Save_Provider"], parameters);
            }
        }
    }
}
