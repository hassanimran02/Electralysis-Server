using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Petapoco
{
    public class TableModels
    {
        [TableName("Samples")]
        [PrimaryKey("SampleID")]
        [ExplicitColumns]
        public partial class Sample
        {
            [Column]
            public int SampleID { get; set; }
            [Column]
            public string Name { get; set; } = null!;
        }
    }
}
