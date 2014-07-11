using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;

namespace Escc.SupportWithConfidence.ETL
{
    /// <summary>
    /// Main entry point to ETL process, this class manages the whole process.
    /// </summary>
    public class Controller
    {

        #region Fields

        CategoryDataTable _category;
        ImportDataTable _import;
        ProviderDataTable _provider;
        readonly ProviderCategoryDataTable _providerCategory;

        #endregion

        #region Properties

        public bool IsReady { get; set; }

        public ProviderDataTable Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        public ImportDataTable Import
        {
            get { return _import; }
            set { _import = value; }
        }

        public CategoryDataTable Category
        {
            get { return _category; }
            set { _category = value; }
        }
        #endregion

        #region Constructor

        public Controller()
        {
           
                _category = new CategoryDataTable();
                _import = new ImportDataTable();
                _provider = new ProviderDataTable(_import.Table);
                _providerCategory = new ProviderCategoryDataTable(_import.Table, _category.Table);

                // Very simple validation process of checking that all the import tables have atleast one row. If they do the hold some rows then the extract and transformation step is complete and
                // the data is ready for loading e.g. saving to the support with confidence database
                if ((_category.Table.Rows.Count > 0) & (_import.Table.Rows.Count > 0) & (_provider.Table.Rows.Count > 0) & (_providerCategory.Table.Rows.Count > 0))
                {
                    IsReady = true;
                }
                else
                {
                    IsReady = false;
                }
           


        }
        #endregion

        #region Methods

        // Save all the import tables to SQL
        public void Commit()
        {


            DataAccess.Save("usp_Admin_PreLoad", null);
            Provider.Commit();
            Category.Commit();
            _providerCategory.Commit();
            DataAccess.Save("usp_Admin_PostLoad", null);
        }
        #endregion

    }
}
