using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Hisaabkitaab.Components.Model;

public class User
{
    [PrimaryKey, AutoIncrement]

    public int Id { get; set; }


    public string Username { get; set; }

    [Required, Unique]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }


    public string Currency { get; set; }
}