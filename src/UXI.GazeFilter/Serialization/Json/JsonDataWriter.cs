using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UXI.GazeFilter.Common;

namespace UXI.GazeFilter.Serialization.Json
{
    public class JsonDataWriter : DisposableBase, IDataWriter, IDisposable
    {
        private readonly JsonSerializer _serializer;
        private readonly JsonTextWriter _writer;
        private bool _isOpen = true;

        public JsonDataWriter(TextWriter writer, JsonSerializer serializer)
        {
            _serializer = serializer;

            _writer = new JsonTextWriter(writer);

            _writer.DateTimeZoneHandling = serializer.DateTimeZoneHandling;
            _writer.DateFormatHandling = serializer.DateFormatHandling;

            _writer.WriteStartArray();
        }


        public bool CanWrite(Type objectType)
        {
            return _isOpen; 
        }


        public void Close()
        {
            if (_isOpen)
            {
                _isOpen = false;
                _writer.WriteEndArray();
                _writer.Flush();
            }
            _writer.Close();
        }


        public void Write(object data)
        {
            if (_isOpen)
            {
                _serializer.Serialize(_writer, data);
            }
        }


        public void WriteRange(IEnumerable<object> data)
        {
            if (_isOpen)
            {
                foreach (var item in data)
                {
                    _serializer.Serialize(_writer, item);
                }
            }
        }


        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
