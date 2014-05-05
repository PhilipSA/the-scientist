﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.Sprite.Npc.NPC_Fighting
{
    public class NPC_Fighting_Patrolling: NPC_Fighting_Guard
    {
        
        
        
        
        private bool collided;
        public bool Collided
        {
            get { return collided; }
            set { collided = value; }
        }
      
        public NPC_Fighting_Patrolling(Texture2D texture, Script script, Random random) :base(texture,script)
        {
            this.ElapsedHitByMelee = 0.0f;
            this.DelayHitByMelee = 300f;
            this.DirtPileCreated = false;
            this.AggroSpeed = 1.5f;
            this.PatrollingCircle = 200f;
            this.DelayHitByArrow = 300f;
            this.ElapsedHitByArrow = 0.0f;
            this.DelayRespawn = 25000f;
            this.ElapsedRespawn = 0.0f;
            this.Dead = false;
            this.OldPosition = Vector2.Zero;
            this.StrikeForce = 2.5f;
            this.collided = false;
            this.Random = random;
            this.Direction = 0;
            this.speed = 0.5f;
            this.StartingFlag = true;
            this.VectorTowardsStart = Vector2.Zero;
            this.VectorTowardsTarget = Vector2.Zero;
            this.AggroStartingPosition = Vector2.Zero;
            this.Aggro = false;
            this.AggroRange = 100;
            this.AggroCircle = 500;
            this.GoingHome = false;
            this.ElapsedDirection = 0.0f;
            this.DelayDirection = 3000f;
            

            FrameAnimation down = new FrameAnimation(1, 50, 80, 0, 0);
            FrameAnimation left = new FrameAnimation(1, 50, 80, 0, 80);
            FrameAnimation right = new FrameAnimation(1, 50, 80, 0, 160);
            FrameAnimation up = new FrameAnimation(1, 50, 80, 0, 240);
            FrameAnimation nothing = new FrameAnimation(1, 0, 0, 0, 0);

            FrameAnimation walkDown = new FrameAnimation(2, 50, 80, 50, 0);
            FrameAnimation walkLeft = new FrameAnimation(2, 50, 80, 50, 80);
            FrameAnimation walkRight = new FrameAnimation(2, 50, 80, 50, 160);
            FrameAnimation walkUp = new FrameAnimation(2, 50, 80, 50, 240);

            FrameAnimation attackDown = new FrameAnimation(2, 65, 80, 205, 0);
            FrameAnimation attackLeft = new FrameAnimation(2, 65, 80, 205, 80);
            FrameAnimation attackRight = new FrameAnimation(2, 65, 80, 205, 160);
            FrameAnimation attackUp = new FrameAnimation(2, 65, 80, 205, 240);

            this.Animations.Add("AttackRight", attackRight);
            this.Animations.Add("AttackLeft", attackLeft);
            this.Animations.Add("AttackUp", attackUp);
            this.Animations.Add("AttackDown", attackDown);
            

            this.Animations.Add("Nothing", nothing);

            this.Animations.Add("Right", right);
            this.Animations.Add("Left", left);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
            this.Animations.Add("WalkRight", walkRight);
            this.Animations.Add("WalkLeft", walkLeft);
            this.Animations.Add("WalkUp", walkUp);
            this.Animations.Add("WalkDown", walkDown);
            
            


        }
        public override void Update(GameTime gameTime)
        {
            if (this.Life <= 0)
                Dead = true;
            if (!Dead)
            {
                if (StartingFlag)
                {
                    this.StartingPosition = Position;
                    StartingFlag = false;
                }

                GetRandomDirection(gameTime);

                this.Motion = new Vector2(
                   (float)Math.Cos(MathHelper.ToRadians(Direction)),
                   (float)Math.Sin(MathHelper.ToRadians(-Direction)));

                UpdateSpriteAnimation(Motion);


                if (Aggro && !StrikeMode)
                {
                    this.speed = AggroSpeed;
                    this.Position += VectorTowardsTarget * speed;

                    UpdateSpriteAnimation(VectorTowardsTarget);
                }
                else if (StrikeMode)
                {
                    UpdateSpriteAttackAnimation(VectorTowardsTarget);
                }
                else if (GoingHome)
                {
                    this.speed = 3.0f;
                    Position += VectorTowardsStart * speed;
                    UpdateSpriteAnimation(VectorTowardsStart);
                }
                else
                {
                    this.speed = 0.5f;
                    Position += Motion * speed;
                }
                if (!Aggro && !GoingHome && Collided)
                {
                    Direction += 180;
                    Direction = Direction % 360;
                    Collided = false;
                }
            }
            else
            {
                CurrentAnimationName = "Nothing";
                ElapsedRespawn += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                this.HitByArrow = false;
                if(ElapsedRespawn > DelayRespawn)
                {
                    this.Life = this.FullHp;
                    this.Dead = false;
                    this.Aggro = false;                    
                    this.ElapsedRespawn = 0.0f;
                    DirtPileCreated = false;
                    this.Position = StartingPosition;
                }
            }
            base.Update(gameTime);
            
        }
        //public void GetRandomDirection(GameTime gameTime)
        //{
        //    ElapsedDirection += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        //    if (Vector2.Distance(StartingPosition, Position) > PatrollingCircle && !Aggro && !GoingHome)
        //    {
        //        Direction += 180;
        //        Direction = Direction % 360;
        //    }
        //    if (ElapsedDirection > DelayDirection && !GoingHome)
        //    {
        //        Direction = random.Next(0, 360);
        //        ElapsedDirection = 0;
        //    }


        //}
        private void UpdateSpriteAttackAnimation(Vector2 motion)
        {

            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                CurrentAnimationName = "AttackRight"; //Right
                //motion = new Vector2(1f, 0f);
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {
                CurrentAnimationName = "AttackDown"; //Down
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {
                CurrentAnimationName = "AttackUp"; // Up
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                CurrentAnimationName = "AttackLeft"; //Left
                //motion = new Vector2(-1f, 0f);
            }
        }
    }
}
