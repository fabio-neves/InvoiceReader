using Invoice.Model;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Invoice.Bll
{
    public class PDFReader: IFileChangedEventHandler
    {
        public void HandleFileOpen(object sender, FileChangedEventArgs e)
        {
            var resultado = GetFaturaItems(e.FileName);
            ((InvoiceEventAggregator)sender).OnDataChanged(resultado);
        }

        private List<InvoiceItem> GetFaturaItems(string fileName)
        {
            var result = new List<InvoiceItem>();
            try
            {
                PdfReader reader = new PdfReader(fileName);

                var its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();

                String rawTextV = PdfTextExtractor.GetTextFromPage(reader, 1, its);

                var pageVencimento = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(rawTextV)));

                var vencimento = GetVencimento(pageVencimento);


                for (int pageNumber = 1; pageNumber < reader.NumberOfPages + 1; pageNumber++)
                {
                    its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();

                    String rawText = PdfTextExtractor.GetTextFromPage(reader, pageNumber, its);

                    var page = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(rawText)));
                    result.AddRange(ParsePage(page, vencimento));

                }
                reader.Close();
            }
            catch (Exception)
            {
                //TODO: Bug Hidden
            }
            return result;
        }

        private DateTime GetVencimento(string page)
        {
            var lines = page.Split('\n');
            var culture = new CultureInfo("pt-BR");
            var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };
            DateTime vencimento = DateTime.Now;
            for (var idx = 0; idx < lines.Length; idx++)
            {
                var line = lines[idx];
                var isVencimentoField = line.ToLowerInvariant() == "vencimento:" && idx + 1 < lines.Length && DateTime.TryParse(lines[idx + 1], culture, DateTimeStyles.None, out vencimento);
            }
            return vencimento;
        }

        private List<InvoiceItem> ParsePage(string page, DateTime vencimento)
        {
            var result = new List<InvoiceItem>();
            var lines = page.Split('\n');
            var culture = new CultureInfo("pt-BR");
            var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };
            for (var idx = 0; idx < lines.Length; idx++)
            {
                var line = lines[idx];
                DateTime data = DateTime.Now;
                decimal valorReal = 0;
                decimal valorDolar = 0;
                var tokens = line.Split(' ');
                var isItemFatura = tokens.Length > 1 && DateTime.TryParse(tokens[0], culture, DateTimeStyles.None, out data)
                    && decimal.TryParse(tokens[tokens.Length - 2], NumberStyles.Number, numberFormatInfo, out valorReal)
                    && decimal.TryParse(tokens[tokens.Length - 1], NumberStyles.Number, numberFormatInfo, out valorDolar);

                if (isItemFatura)
                {
                    var descricao = string.Empty;
                    for (int jdx = 1; jdx < tokens.Length - 2; jdx++)
                    {
                        descricao += tokens[jdx] + (jdx < tokens.Length - 3 ? " " : string.Empty);
                    }
                    result.Add(new InvoiceItem { Data = data, Vencimento = vencimento, Beneficiario = descricao, ValorReal = valorReal, ValorDolar = valorDolar });
                }
            }
            return result;
        }
    }
}
