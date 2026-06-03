namespace AsposeLetters.FieldSchema
{
    public class TextField : IField
    {
        public string Label { get; }
        public string API { get; }
        public string FieldType => "text";
        public string? Value { get; set; }
        object? IField.Value
        {
            get => Value;
            set => Value = value as string;
        }

        public TextField(string label, string api, string value)
        {
            Label = label;
            API = api;
            Value = value;
        }
    }
}
