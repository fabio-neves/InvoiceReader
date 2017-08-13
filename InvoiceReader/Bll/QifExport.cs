using System;
using System.Globalization;

namespace Invoice.Bll
{
    public class QifExport : IExportInvokedEventHandler
    {
        NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = ".", NumberGroupSeparator = "," };

        public void HandleExportInvoked(object sender, ExportInvokedEventArgs e)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(e.FileName, false))
                {
                    file.WriteLine("!Type:CCard");
                    foreach (var r in e.Data)
                    {
                        file.WriteLine("D{0}", r.ValorReal >= 0 ? r.Vencimento.ToString("dd/MM/yyyy") : r.Data.ToString("dd/MM/yyyy"));
                        file.WriteLine("T{0}{1}", r.ValorReal >= 0 ? "-" : "+", Math.Abs(r.ValorReal).ToString(nfi));
                        file.WriteLine("P{0}", r.Beneficiario);
                        file.WriteLine("M{0}", r.Data.ToString("dd/MM/yyyy"));
                        file.WriteLine("^");
                    }
                }

                ((InvoiceEventAggregator)sender).OnExportInvokedEnd(e.Data, e.FileType, e.FileName);
            }
            catch (Exception)
            {
                //TODO: Bug Hidden
            }
        }
    }
}
