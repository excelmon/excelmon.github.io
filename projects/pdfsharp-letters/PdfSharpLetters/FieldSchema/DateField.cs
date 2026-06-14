namespace PdfSharpLetters.FieldSchema
{
    public class DateField : IField
    {
        public string Label { get; }
        public string API { get; }
        public string FieldType => "date";
        public DateOnly? Value { get; set; }
        object? IField.Value
        {
            get => Value;
            set => Value = value is DateOnly d ? d : null;
        }

        public DateField(string label, string api, DateOnly value)
        {
            Label = label;
            API = api;
            Value = value;
        }
    }
}
