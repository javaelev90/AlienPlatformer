using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Equipment
{
    public enum MAGAZINETYPES { BULLET, ENERGY };

    public interface IMagazine
    {
        int ClipSize();
        MAGAZINETYPES getMagType();
    }
}
