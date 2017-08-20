using Invoice.Bll;
using Invoice.Presenter;
using System;
using System.Windows.Forms;

namespace InvoiceReader.WindowsForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var main = new Main();
            var ea = new InvoiceEventAggregator();
            var faturaPresenter = new InvoicePresenter(main, ea);
            ea.AddDataChangedEventHandler(faturaPresenter);
            ea.AddExportInvokedEndEventHandler(faturaPresenter);
            ea.AddFileChangedEventHandler(new PDFNewFormatReader());
            ea.AddExportInvokedEventHandler(new QifExport());
            Application.Run(main);            
        }
    }
}
