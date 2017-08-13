using Invoice.Model;
using System;
using System.Collections.Generic;

namespace Invoice.Bll
{
    public class InvoiceEventAggregator
    {
        public event EventHandler<FileChangedEventArgs> FileChanged;

        public event EventHandler<DataChangedEventArgs> DataChanged;

        public event EventHandler<ExportInvokedEventArgs> ExportInvoked;

        public event EventHandler<ExportInvokedEventArgs> ExportInvokedEnd;

        public virtual void OnFileChanged(string fileName)
        {
            FileChangedEventArgs e = new FileChangedEventArgs { FileName = fileName };
            FileChanged?.Invoke(this, e);
        }

        public virtual void OnDataChanged(List<InvoiceItem> data)
        {
            DataChangedEventArgs e = new DataChangedEventArgs { Data = data };
            DataChanged?.Invoke(this, e);
        }

        public virtual void OnExportInvoked(List<InvoiceItem> data, string fileType, string fileName)
        {
            ExportInvokedEventArgs e = new ExportInvokedEventArgs { Data = data, FileType = fileType, FileName = fileName };
            ExportInvoked?.Invoke(this, e);
        }

        public virtual void OnExportInvokedEnd(List<InvoiceItem> data, string fileType, string fileName)
        {
            ExportInvokedEventArgs e = new ExportInvokedEventArgs { Data = data, FileType = fileType, FileName =fileName };
            ExportInvokedEnd?.Invoke(this, e);
        }

        public void AddFileChangedEventHandler(IFileChangedEventHandler h)
        {
            FileChanged += h.HandleFileOpen;
        }

        public void AddDataChangedEventHandler(IDataChangedEventHandler h)
        {
            DataChanged += h.HandleDataChanged;
        }

        public void AddExportInvokedEventHandler(IExportInvokedEventHandler h)
        {
            ExportInvoked += h.HandleExportInvoked;
        }

        public void AddExportInvokedEndEventHandler(IExportInvokedEndEventHandler h)
        {
            ExportInvokedEnd += h.HandleExportInvokedEnd;
        }
    }

    public class FileChangedEventArgs : EventArgs
    {
        public string FileName { get; set; }
    }

    public class DataChangedEventArgs : EventArgs
    {
        public List<InvoiceItem> Data { get; set; }
    }

    public class ExportInvokedEventArgs : DataChangedEventArgs
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
    }

    public interface IFileChangedEventHandler
    {
        void HandleFileOpen(object sender, FileChangedEventArgs e);
    }

    public interface IDataChangedEventHandler
    {
        void HandleDataChanged(object sender, DataChangedEventArgs e);
    }

    public interface IExportInvokedEventHandler
    {
        void HandleExportInvoked(object sender, ExportInvokedEventArgs e);
    }

    public interface IExportInvokedEndEventHandler
    {
        void HandleExportInvokedEnd(object sender, ExportInvokedEventArgs e);
    }
}
