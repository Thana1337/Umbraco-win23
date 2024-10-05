using System.ComponentModel.DataAnnotations;

namespace block_grid_umbraco.Models;

public class ContactFormModel
{

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Service { get; set; } = null!;

    public string Message { get; set; } = null!;
}
