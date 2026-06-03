namespace AsposeLetters.FieldSchema
{
    public interface IField
    {
        string Label { get; }
        string API { get; } 
        string FieldType { get; }  // Example: "text", "dropdown", "checkbox"
        object? Value { get; set; }
    }
}
