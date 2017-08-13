using Invoice.Bll;
using Invoice.Model;
using Invoice.View;
using InvoiceReader;
using System;
using System.Linq;

namespace Invoice.Presenter
{
    public class InvoicePresenter : IDataChangedEventHandler, IExportInvokedEndEventHandler
    {
        private IInvoiceView viewer;

        private InvoiceEventAggregator eventAggregator;

        public InvoicePresenter(IInvoiceView v, InvoiceEventAggregator ea)
        {
            viewer = v;
            eventAggregator = ea;
            viewer.ExportData += Viewer_ExportData; ;
            viewer.OpenFile += Viewer_OpenFile;
        }

        private void Viewer_ExportData(object sender, EventArgs e)
        {            
            var v = (IInvoiceView)sender;
            if (v.DataToExport != null && v.DataToExport.Count > 0)
            {
                eventAggregator.OnExportInvoked(v.DataToExport.Cast<InvoiceItem>().ToList(), "qif", v.ExportFileName);
            }
        }

        private void Viewer_OpenFile(object sender, EventArgs e)
        {
            eventAggregator.OnFileChanged(((IInvoiceView)sender).OpenFileName);
        }

        public void HandleDataChanged(object sender, DataChangedEventArgs e)
        {
            viewer.Data = e.Data.Cast<object>().ToList();
            viewer.ShowPopupMessage(string.Format(Messages.DataChangedMessage, e.Data.Count));            
        }

        public void HandleExportInvokedEnd(object sender, ExportInvokedEventArgs e)
        {
            viewer.ShowPopupMessage(string.Format(Messages.ExportInvokedEndMessage, e.FileName));
        }
    }
}
