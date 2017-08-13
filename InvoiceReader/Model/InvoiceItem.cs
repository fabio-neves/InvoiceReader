using System;
using System.Globalization;

namespace Invoice.Model
{
    public class InvoiceItem
    {
        public DateTime Data { get; set; }

        public DateTime Vencimento { get; set; }

        public string Beneficiario { get; set; }

        public decimal ValorReal { get; set; }

        public decimal ValorDolar { get; set; }

        public string Qif
        {
            get
            {
                NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = ".", NumberGroupSeparator = "," };
                return string.Format("D{0}\nT{1}{2}\nP{3}\nM{4}\n^", ValorReal >= 0 ? Vencimento.ToString("dd/MM/yyyy") : Data.ToString("dd/MM/yyyy"),
                    ValorReal >= 0 ? "-" : "+", Math.Abs(ValorReal).ToString(nfi), Beneficiario, Data.ToString("dd/MM/yyyy"));
            }
        }
    }
}
