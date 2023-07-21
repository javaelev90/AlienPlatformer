using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Equipment
{
    public class GunMagazine : IMagazine
    {
        public int ClipSize()
        {
            return 5;
        }

        public MAGAZINETYPES getMagType()
        {
            return MAGAZINETYPES.BULLET;
        }
    }
}
