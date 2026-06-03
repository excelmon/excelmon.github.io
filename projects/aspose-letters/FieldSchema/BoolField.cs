namespace AsposeLetters.FieldSchema
{
    public class BoolField : IField
    {
        public string Label { get; }
        public string API { get; }
        public string FieldType => "bool";
        public bool? Value;
        object? IField.Value
        {
            get => Value;
            set => Value = value is bool d ? d : false;
        }

        public BoolField(string label, string api, bool value)
        {
            Label = label;
            API = api;
            Value = value;
        }
    }
}
