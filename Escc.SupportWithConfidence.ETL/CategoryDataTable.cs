using LumenWorks.Framework.IO.Csv;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Escc.SupportWithConfidence.ETL
{
    /// <summary>
    /// This class creates a data table that holds the category list for provider services
    /// The Flare data has been extended to accept parentid, depth, providertype and isactive to
    /// all for further filtering options on what and how to display the categories on the search page
    /// </summary>
    public class CategoryDataTable : IImportable
    {
        #region Fields

        DataTable _dtCategory = new DataTable("Categories");
        #endregion

        #region Properties


        public DataTable Table
        {
            get { return _dtCategory; }
            set { _dtCategory = value; }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Setup category table and fill it with data from the header and categories csv files
        /// </summary>
        public CategoryDataTable()
        {
            _dtCategory.Columns.Add("Code", typeof(string));
            _dtCategory.Columns.Add("Description", typeof(string));
            _dtCategory.Columns.Add("ParentId", typeof(Int32));
            _dtCategory.Columns.Add("Depth", typeof(Int32));
            _dtCategory.Columns.Add("ProviderTypeId", typeof(Int32));
            _dtCategory.Columns.Add("IsActive", typeof(bool));


            Fill();


        }
        #endregion

        #region Implemented Interface

        /// <summary>
        /// Create a table of categories from the header and categories csv files
        /// The flare data is extended to includes extra fields to support future filtering and ordering options
        /// </summary>
        public void Fill()
        {

            // Extract the parent categories from the header.csv file
            DataTable dtRawParentCategories = Extract(ConfigurationManager.AppSettings["ParentCategories"]);
            // Extract the  child categories from the categories csv file and combine with parent categories
            dtRawParentCategories.Merge(Extract(ConfigurationManager.AppSettings["ChildCategories"]));
            const string expression = "";
            const string sortOrder = "Code ASC";

            // Apply filter to categories to reorder so the they ordered by Code in ascending order.
            DataRow[] filtered = dtRawParentCategories.Select(expression, sortOrder);

            // Duplicate ordered table and add ID and sequence numbers
            DataTable cat = dtRawParentCategories.Clone();
            cat.Columns.Add("Id", typeof(Int32));
            cat.Columns["Id"].AutoIncrement = true;
            cat.Columns["Id"].AutoIncrementSeed = 1;
            cat.Columns["Id"].SetOrdinal(0);
            cat.Columns.Add("Sequence", typeof(Int32));

            // Import each row with new fields into duplicate table
            foreach (var item in filtered)
            {
                cat.ImportRow(item);
            }

            int parentId = 0;
            string code = string.Empty;

            // Setup the sequence and which category is a parent and which is a child
            foreach (DataRow item in cat.Rows)
            {
                item["Sequence"] = item["Id"];
                if (item["Code"].ToString().Length == 1)
                {
                    // We have a parent
                    item["Depth"] = 1;
                    code = item["Code"].ToString();
                    parentId = (int)item["Id"];
                }
                else
                {
                    if (item["Code"].ToString().StartsWith(code))
                        item["ParentId"] = parentId;
                    item["Depth"] = 2;
                }
            }

            _dtCategory = null;
            _dtCategory = cat;

        }

        /// <summary>
        /// Extracts each row from the CSV file (passed in as a csv path)
        /// Adds the content into a new row before returning the datatable
        /// to the calling method in this case Fill()
        /// </summary>
        /// <param name="csvPath"></param>
        /// <returns>Datatable</returns>
        private DataTable Extract(string csvPath)
        {
            using (var csv = new CsvReader(new StreamReader(csvPath), true))
            {
                int fieldCount = 1;//csv.FieldCount;

                while (csv.ReadNextRecord())
                {
                    for (int i = 0; i < fieldCount; i++)
                    {
                        // The CSV is delimited by a comma
                        string[] content = csv[i].Split(',');

                        var array = new object[6];
                        array[0] = content[1];
                        array[1] = content[2];
                        array[2] = DBNull.Value;
                        array[3] = DBNull.Value;
                        array[4] = 1;
                        array[5] = false;



                        _dtCategory.Rows.Add(array);

                    }

                }

                return _dtCategory;
            }
        }

        /// <summary>
        /// This method saves the data into SupportWithConfidence on essqlpub01s via the proxy database on esdbcontent01s
        /// Commit is only called once the controller.IsReady returns true. This only happens if all of the datatables
        /// that make up supportwithconfidence have data to import. Emtpy tables indicate that the extract  / fill process
        /// failed and there is a data issue in the csv files exported from Flare
        /// </summary>
        public void Commit()
        {
            var parameters = new SqlParameter[8];

            foreach (DataRow item in _dtCategory.Rows)
            {

                parameters[0] = new SqlParameter("@Id", SqlDbType.BigInt) { Value = (int)item["Id"] };
                parameters[1] = new SqlParameter("@Sequence", SqlDbType.BigInt) { Value = (int)item["Sequence"] };
                parameters[2] = new SqlParameter("@Code", SqlDbType.VarChar) { Value = item["Code"] };
                parameters[3] = new SqlParameter("@Description", SqlDbType.VarChar) { Value = item["Description"] };
                if (item["ParentId"] == DBNull.Value)
                {
                    parameters[4] = new SqlParameter("@ParentId", SqlDbType.BigInt) { Value = DBNull.Value };
                }
                else
                {
                    parameters[4] = new SqlParameter("@ParentId", SqlDbType.BigInt) { Value = (int)item["ParentId"] };
                }

                parameters[5] = new SqlParameter("@Depth", SqlDbType.Int) { Value = (int)item["Depth"] };
                parameters[6] = new SqlParameter("@ProviderTypeId", SqlDbType.BigInt) { Value = (int)item["ProviderTypeId"] };
                parameters[7] = new SqlParameter("@IsActive", SqlDbType.Bit) { Value = (bool)item["IsActive"] };


                if (ConfigurationManager.AppSettings["CategoryTransformationEnabled"] == "true")
                {
                    if (CategoryTransformation.ExcludeCategory(parameters[2].Value.ToString()))
                    {
                        continue;
                    }

                    var transformedCategory = parameters[3].Value.ToString();
                    
                    transformedCategory = CategoryTransformation.ProperCase(transformedCategory);
                    transformedCategory = CategoryTransformation.ReplaceAmpersand(transformedCategory);
                    transformedCategory = CategoryTransformation.SubsituteCategory(transformedCategory);

                    parameters[3].Value = transformedCategory;

                    DataAccess.Save(ConfigurationManager.AppSettings["Save_Categories"], parameters);
                }
                else
                {
                    
                    DataAccess.Save(ConfigurationManager.AppSettings["Save_Categories"], parameters);
                }
            }




        }
        #endregion
    }
}
