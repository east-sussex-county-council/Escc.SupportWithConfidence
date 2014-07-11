using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace Escc.SupportWithConfidence.ETL
{
    /// <summary>
    /// This class is used to join the provider with associated categories.
    /// The search pages use this table to determine which categories to display
    /// Only categories that have atleast one provider are visiable in search.
    /// This avoids user searching and getting back no results.
    /// </summary>
    public class ProviderCategoryDataTable : IImportable
    {
        #region Fields

        DataTable _dtProviderCategory;
        readonly DataTable _dtImport;
        readonly DataTable _dtCategory;
        #endregion


        #region Properties

        public DataTable Table
        {
            get { return _dtProviderCategory; }
            set { _dtProviderCategory = value; }
        }

        #endregion




        #region Constructor

        public ProviderCategoryDataTable(DataTable import, DataTable category)
        {
            _dtImport = import;
            _dtCategory = category;
            _dtProviderCategory = new DataTable();

            _dtProviderCategory.Columns.Add("FlareId", typeof(Int32));
            _dtProviderCategory.Columns.Add("CategoryId", typeof(Int32));



            Fill();
        }
        #endregion

        #region Interface implemented


        private string FixCatKey(string input)
        {
            var key = input;
            if (input.Contains(" "))
            {
                key = input.Remove(input.IndexOf(" ", System.StringComparison.Ordinal));
            }
            return key;
        }



        /// <summary>
        /// This method fills the provider category datatable if the import table contains category information
        /// that matches the category table
        /// </summary>
        public void Fill()
        {


            var categoryLookup = _dtCategory.Rows.Cast<DataRow>().ToDictionary(item => item["Code"].ToString(), item => (Int32)item["Id"]);


            // Loop over the import table extract providerid and Cat 1 - 8
            // Look up on category for the code
            // Insert id, providerid and catid


            foreach (DataRow item in _dtImport.Rows)
            {
                int providerId = int.Parse(item["UniqueId"].ToString());

                for (int i = 1; i < 10; i++)
                {
                    string cat = item["Cat" + i.ToString(CultureInfo.InvariantCulture)].ToString();
                    if (cat.Length > 0)
                    {
                        cat = FixCatKey(cat);

                        if (item["Cat" + i].ToString().Length > 0)
                        {
                            int value;
                            if (categoryLookup.TryGetValue(cat, out value))
                            {
                                _dtProviderCategory.Rows.Add(providerId, value);
                            }
                        }
                    }

                }


            }


            DataRow[] filtered = _dtProviderCategory.Select("", "");

            DataTable dtProviderCategoryTemp = _dtProviderCategory.Clone();
            dtProviderCategoryTemp.Columns.Add("Id", typeof(Int32));
            dtProviderCategoryTemp.Columns["Id"].AutoIncrement = true;
            dtProviderCategoryTemp.Columns["Id"].AutoIncrementSeed = 1;
            dtProviderCategoryTemp.Columns["Id"].SetOrdinal(0);

            _dtProviderCategory = null;
            _dtProviderCategory = dtProviderCategoryTemp;
            foreach (DataRow item in filtered)
            {
                _dtProviderCategory.ImportRow(item);
            }


        }

        /// <summary>
        /// Save the provider category data to SQL
        /// </summary>
        public void Commit()
        {
            var parameters = new SqlParameter[3];
            foreach (DataRow item in _dtProviderCategory.Rows)
            {


                parameters[0] = new SqlParameter("@Id", SqlDbType.BigInt) { Value = (int)item["Id"] };
                parameters[1] = new SqlParameter("@FlareId", SqlDbType.BigInt) { Value = (int)item["FlareId"] };
                parameters[2] = new SqlParameter("@CategoryId", SqlDbType.BigInt) { Value = (int)item["CategoryId"] };



                DataAccess.Save(ConfigurationManager.AppSettings["Save_ProviderCategory"], parameters);
            }
        }
        #endregion
    }
}
