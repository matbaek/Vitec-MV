using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vitec_MV.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        /*The Timestamp attribute specifies that this column is included in the Where clause of Update and Delete commands. 
        The attribute is called Timestamp because previous versions of SQL Server used a SQL timestamp data type before the SQL rowversion type replaced it.*/
    }
}
