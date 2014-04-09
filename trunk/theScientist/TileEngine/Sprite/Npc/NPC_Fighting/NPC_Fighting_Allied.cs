﻿#region Using fält

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Sprite.Npc;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc.Npc_Fighting
{
    public class NPC_Fighting_Allied : NPC_Fighting
    {
        protected int AggresionLevel { get; set; }
            
        protected NPC_Fighting_Allied(Texture2D texture, Script script) : base(texture,script)
        {
            
        }

    }
}
#endregion