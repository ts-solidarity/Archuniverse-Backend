using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archuniverse.Characters;
using Archuniverse.Combat;
using Archuniverse.Items;
using Archuniverse.Utilities;
using Archuniverse;

namespace Archuniverse.Items
{
    public class Ware : Item
    {
        public Ware(string name, Grade grade, int worth) 
            : base(name, Type.Ware, grade, worth)
        {
            
        }
    }
}
