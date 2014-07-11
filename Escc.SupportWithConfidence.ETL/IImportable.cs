namespace Escc.SupportWithConfidence.ETL
{
    // All support with confidence tables must have an interface to fill and commit
    interface IImportable
    {
        void Fill();
        void Commit();
    }
}
