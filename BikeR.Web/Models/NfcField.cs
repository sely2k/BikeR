namespace BikeR.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("bikermobilews.NfcField")]
    public partial class NfcField
    {
        [StringLength(255)]
        public string id { get; set; }

        [Column("__createdAt")]
        public DateTimeOffset C__createdAt { get; set; }

        [Column("__updatedAt")]
        public DateTimeOffset? C__updatedAt { get; set; }

        [Column("__version", TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] C__version { get; set; }

        public string nfctagid { get; set; }

        public string note { get; set; }

        public string userid { get; set; }

        public string qualifiedtag { get; set; }

        public string kindoftag { get; set; }
    }
}
