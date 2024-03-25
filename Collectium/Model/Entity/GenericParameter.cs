using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity;

[Table("generic_param")]
public abstract class GenericParameter
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int? Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("code")]
    public string? Code { get; set; }
}

public class DocumentRestruktur : GenericParameter
{

}

public class PolaRestruktur : GenericParameter
{

}

public class JenisPengurangan : GenericParameter
{

}


public class PembayaranGp : GenericParameter
{

}

public class AlasanLelang : GenericParameter
{

}

public class BalaiLelang : GenericParameter
{

}

public class JenisLelang : GenericParameter
{

}

public class DocumentAuction : GenericParameter
{

}

public class DocumentAuctionResult : GenericParameter
{

}

public class Asuransi : GenericParameter
{

}

public class AsuransiSisaKlaim : GenericParameter
{

}

public class DocumentInsurance : GenericParameter
{

}

public class RecoveryExecution : GenericParameter
{

}

public class DocumentAyda : GenericParameter
{

}

public class HubunganBank : GenericParameter
{

}

public class RecoveryField : GenericParameter
{

}

public class RuleAction : GenericParameter
{

}

public class RuleActionOption : GenericParameter
{

}

public class RuleValueType : GenericParameter
{

}

public class RuleOperator : GenericParameter
{

}


public class DataSource : GenericParameter
{

}

public class CustomerTag : GenericParameter
{

}