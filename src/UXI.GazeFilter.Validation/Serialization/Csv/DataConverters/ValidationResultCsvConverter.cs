﻿using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Serialization.Formats.Csv;
using UXI.GazeToolkit.Serialization.Formats.Csv.Converters;
using UXI.GazeToolkit.Utils;
using UXI.GazeToolkit.Validation;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeFilter.Validation.Serialization.Csv.DataConverters
{
    public class ValidationResultCsvConverter : CsvConverter<ValidationResult>
    {
        public override bool CanRead => false;


        public override bool CanWrite => true;


        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(ValidationPoint.Validation)));
            writer.WriteField(naming.Get(nameof(ValidationPoint.Point)));

            serializer.WriteHeader<Point2>(writer, naming);
            
            serializer.WriteHeader<EyeValidationPointResult>(writer, naming, nameof(ValidationPointResult.LeftEye));
            serializer.WriteHeader<EyeValidationPointResult>(writer, naming, nameof(ValidationPointResult.RightEye));
        }


        protected override void WriteCsv(ValidationResult data, CsvWriter writer, CsvSerializerContext serializer)
        {
            bool isNextRecord = false;

            foreach (var point in data.Points)
            {
                if (isNextRecord)
                {
                    writer.NextRecord();
                }

                isNextRecord = true;

                writer.WriteField(point.Point.Validation);
                writer.WriteField(point.Point.Point);

                serializer.Serialize(writer, point.Point.Position);

                serializer.Serialize(writer, point.LeftEye);
                serializer.Serialize(writer, point.RightEye);

                writer.NextRecord();
            }
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            throw new NotSupportedException();
        }
    }
}
