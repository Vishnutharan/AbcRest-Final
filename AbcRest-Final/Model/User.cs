using System.ComponentModel.DataAnnotations;  // Include the DataAnnotations namespace for validation attributes

public class User
{
    [Key]  // Specifies that 'id' is the primary key in the database
    public int id { get; set; }

    public string FirstName { get; set; }  // Holds the first name of the user // these are the thing need to remove becuse we set manully in the db its null. if login new thats make the error
    public string LastName { get; set; }  // Holds the last name of the user
    public string Email { get; set; }  // Holds the email address of the user
    public string UserName { get; set; }  // Holds the username for the user's account
    public string Password { get; set; }  // Holds the password for the user's account

}
