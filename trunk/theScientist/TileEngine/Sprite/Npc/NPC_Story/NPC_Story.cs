﻿#region Using fält

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

#endregion

namespace TileEngine.Sprite.Npc.NPC_Story
{
    public class NPC_Story : NPC
    {
        protected bool Immortal { get; set; }
        protected float speakingRadius = 25f;
        public bool canTalk = true;
        public Conversation text;
        public Texture2D picture;

        public NPC_Story(Texture2D texture, Script script, Texture2D picture) : base(texture,script)
        {
            this.Immortal = true;
            this.picture = picture;
        }

        public float SpeakingRadius
        {
            get { return speakingRadius; }
            set { speakingRadius = (float) Math.Max(value, CollisionRadius); }
        }

        public bool InSpeakingRange(AnimatedSprite sprite)
        {
            Vector2 d = Origin - sprite.Origin;

            return (d.Length() < SpeakingRadius);
        }

        public void StartConversation(string conversationName)
        {
            if (script == null || canTalk == false)
                return;
            text = script[conversationName];
            
        }

        public void EndConversation()
        {
            if (script == null)
                return;
            canTalk = false;
        }
    }
}