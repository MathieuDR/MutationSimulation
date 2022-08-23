namespace ByteEncoder.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class BytePiece : Attribute {
    public BytePiece() { }

    public BytePiece(int order) => Order = order;

    public int? Order { get; set; }
}
