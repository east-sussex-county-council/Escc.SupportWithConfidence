using Exceptionless;
using System;

namespace Escc.SupportWithConfidence.ETL
{
    /// <summary>
    /// Console application which performs ETL from Flare (trading standards system) and imports into support with confidence database.
    /// Sponsors Mina O'Brien & Bianca Byrnes
    /// Developer David Hutson
    /// Project manager Sandra South
    /// External consultant Keith Bean
    /// </summary>
    class Program
    {
        static void Main()
        {
            ExceptionlessClient.Current.Startup();

            // Controller is the engine for the ETL process and is the pipe through which all calls and returns are made

            var controller = new Controller();

            // Only after checking that all of the tables have some rows imported and formatted will the program move on to save the flare data into the database
            if (controller.IsReady)
            {
                // Save the imported and formatted data in the SWC database
                controller.Commit();
            }
            else
            {
                // Generic error message to tell developers the import failed. Most likely cause is a data formatting issue in the overnight data csv file.
                // Review resource file in project to see location of CSV files.
                throw new Exception("Support with Confidence ETL failed. Extract of Flare data was not loaded. Check CSV files are not corrupted or may have been renamed. System was unable to map datatables and load the live system. No update has occured.");
            }
        }












    }
}
