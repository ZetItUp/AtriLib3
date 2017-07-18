using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AtriLib3.Interfaces
{
    public interface IWorldObject
    {
        Rectangle ObjectRectangle { get; }
        bool HasCollision { get; set; }
    }
}
