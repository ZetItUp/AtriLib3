using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace AtriLib3
{
    public static class Constants
    {
        internal const int QUADTREE_MAX_OBJECTS = 10;
        internal const int QUADTREE_MAX_LEVELS = 5;

        public static ContentManager ContentMgr { get; set; }

        public const float ZORDER_MIN = 0.0f;
        public const float ZORDER_DEFAULT = 0.5f;
        public const float ZORDER_MAX = 1.0f;

        public const int DEFAULT_WIDTH = 32;
        public const int DEFAULT_HEIGHT = 32;

        public const float ROTATION_0 = 0.0f;   // Most useless but w/e :P
        public const float ROTATION_45 = 45.0f;
        public const float ROTATION_90 = 90.0f;
        public const float ROTATION_135 = 135.0f;
        public const float ROTATION_180 = 180.0f;
        public const float ROTATION_225 = 225.0f;
        public const float ROTATION_270 = 270.0f;
        public const float ROTATION_315 = 315.0f;
        public const float ROTATION_360 = 360.0f;

        public const float SCALE_PROPORTIONAL = 1.0f;
    }
}
