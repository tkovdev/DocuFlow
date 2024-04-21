namespace Data;

public class Signature
{
    public Signature(User signatory)
    {
        SignatoryName = $"{signatory.LastName}, {signatory.FirstName} {signatory.MiddleName ?? ""}";
    }
    public string SignatoryName { get; set; }
}