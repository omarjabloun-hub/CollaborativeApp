using System;

namespace CollaborativeApp.User;

public class User
{
    public String Id { get; set; }
    public String Name { get; }

    public User()
    {
        Id = Guid.NewGuid().ToString();
        Name = string.Concat("User ", Id.AsSpan(0, 4));
        Console.WriteLine(Name);
    }
}