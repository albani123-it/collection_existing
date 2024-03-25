using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity;

[Table("status")]
public abstract class Status
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int? Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}

public class StatusGeneral : Status
{

}

public class StatusRequest : Status
{

}

public class StatusCall : Status
{

}

public class StatusRestruktur : Status
{

}

public class StatusLeLang : Status
{

}

public class StatusAsuransi : Status
{

}

