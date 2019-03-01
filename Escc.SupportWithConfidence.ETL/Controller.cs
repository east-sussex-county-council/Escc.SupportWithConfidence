
namespace Escc.SupportWithConfidence.ETL
{
    /// <summary>
    /// Main entry point to ETL process, this class manages the whole process.
    /// </summary>
    public class Controller
    {
        ImportDataTable _import;
        ProviderDataTable _provider;

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

        public Controller()
        {

            _import = new ImportDataTable();
            _provider = new ProviderDataTable(_import.Table);

            // Very simple validation process of checking that all the import tables have atleast one row. If they do the hold some rows then the extract and transformation step is complete and
            // the data is ready for loading e.g. saving to the support with confidence database
            if ((_import.Table.Rows.Count > 0) && (_provider.Table.Rows.Count > 0))
            {
                IsReady = true;
            }
            else
            {
                IsReady = false;
            }



        }

        // Save all the import tables to SQL
        public void Commit()
        {
            DataAccess.Save("usp_Admin_PreLoad", null);
            Provider.Commit();
            DataAccess.Save("usp_Admin_PostLoad_2", null);
        }
    }
}
