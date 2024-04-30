﻿using System.IO;

public sealed partial class DataTableProcessor
{
    private sealed class BooleanProcessor : GenericDataProcessor<bool>
    {
        public override bool IsSystem => true;

        public override string LanguageKeyword => "bool";

        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "bool",
                "boolean",
                "system.boolean"
            };
        }

        public override bool Parse(string value)
        {
            return bool.Parse(value);
        }

        public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter,
            string value)
        {
            binaryWriter.Write(Parse(value));
        }
    }
}