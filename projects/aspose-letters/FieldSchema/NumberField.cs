namespace AsposeLetters.FieldSchema
{
    public class NumberField : IField
    {
        public string Label { get; }
        public string API { get; }
        public string FieldType => "number"; // could be currency or number with 2 decimals
        public double Value;
        object? IField.Value
        {
            get => Value;
            set => Value = value is double d ? d : 0.00;
        }
        public NumberField(string label, string api, double value)
        {
            Label = label;
            API = api;
            Value = value;
        }
    }
}
