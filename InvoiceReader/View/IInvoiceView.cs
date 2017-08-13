using System;
using System.Collections.Generic;

namespace Invoice.View
{
    public interface IInvoiceView
    {
        List<object> Data { get; set; }

        List<object> DataToExport { get; set; }

        string OpenFileName { get; set; }

        string ExportFileName { get; set; }

        event EventHandler OpenFile;

        event EventHandler ExportData;

        void ShowPopupMessage(string s);
    }
}
