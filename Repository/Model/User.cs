using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class User
{
    private DateTime _createdAtDate;
    private DateTime _updatedAtDate;
    [BsonId]
    public Guid Id { get; set; }

    [BsonElement("username")]
    public string Username { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("password")]
    public string Password { get; set; }

    [BsonElement("firstName")]
    public string FirstName { get; set; }

    [BsonElement("lastName")]
    public string LastName { get; set; }

    [BsonElement("dateOfBirth")]
    public DateOnly DateOfBirth { get; set; }

    [BsonElement("address")]
    public Address Address { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt {
        get
        {
            // Specify the time zone you want to convert to (GMT+7)
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // This is the ID for GMT+7

            // Convert the existing DateTime (_date) to the target time zone (GMT+7)
            DateTime gmtPlus7Time = TimeZoneInfo.ConvertTime(_createdAtDate, targetTimeZone);

            return gmtPlus7Time;
        }
        set
        {
            _createdAtDate = value;
        }
    }

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt
    {
        get
        {
            // Specify the time zone you want to convert to (GMT+7)
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // This is the ID for GMT+7

            // Convert the existing DateTime (_date) to the target time zone (GMT+7)
            DateTime gmtPlus7Time = TimeZoneInfo.ConvertTime(_updatedAtDate, targetTimeZone);

            return gmtPlus7Time;
        }
        set
        {
            _updatedAtDate = value;
        }
    }
}

public class Address
{
    [BsonElement("street")]
    public string Street { get; set; }

    [BsonElement("city")]
    public string City { get; set; }

    [BsonElement("state")]
    public string State { get; set; }

    [BsonElement("zipCode")]
    public string ZipCode { get; set; }
}
