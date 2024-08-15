using System.Security.Cryptography;
using System.Text;

namespace Data;

public class User
{
    public User(string firstName, string lastName, string salt, string? middleName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }

    private string? _hash;
    public string? Hash => _hash;

    public void SetHash(string salt)
    {
        var inputBytes = Encoding.UTF8.GetBytes($"{LastName}{FirstName}{MiddleName}{salt}");
        var inputHash = SHA256.HashData(inputBytes);
        var generatedHash = Convert.ToHexString(inputHash);
        if (_hash is null)
        {
            _hash = generatedHash;
        }
    }
}