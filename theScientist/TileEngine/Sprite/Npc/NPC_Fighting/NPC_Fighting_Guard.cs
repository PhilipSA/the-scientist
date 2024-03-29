﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.AI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace TileEngine.Sprite.Npc.NPC_Fighting
{
    public class NPC_Fighting_Guard : NPC_Fighting
    {
        protected Vector2 vectorTowardsTarget;
        protected Vector2 vectorTowardsStart;
        protected float delaySeek;
        protected float elapsedSeek; 
        
        protected bool startingFlag;
        protected bool goingHome;
        protected float aggroRange;
        protected float aggroCircle;
        private Random random;
        private float aggroSpeed;
        private Vector2 motion;
        private float elapsedHitByMelee;
        private float delayHitByMelee;
        private bool meleeHit;
        private bool dirtPileCreated;
        private float patrollingCircle;
        private float delayDirection;
        private float elapsedDirection;
        private int direction;
        private float strikeForce;
        private bool strikeMode;

        private bool timeToStrike;
        private float delayTimeToStrike;
        private float elapsedTimeToStrike;
        private float delayStrike;
        private float delayStruck;
        private float elapsedStruck;
        private float elapsedStrike;

        private Vector2 playerPosition;
        private Vector2 oldPosition;
        private bool collided;
        public float ElapsedTimeToStrike
        {
            get { return elapsedTimeToStrike; }
            set { elapsedTimeToStrike = value; }
        }
        public float DelayTimeToStrike
        {
            get { return delayTimeToStrike; }
            set { delayTimeToStrike = value; }
        }
        public bool Collided
        {
            get { return collided; }
            set { collided = value; }
        }
        public bool TimeToStrike
        {
            get { return timeToStrike; }
            set { timeToStrike = value; }
        }
        public Random Random
        {
            get { return random; }
            set { random = value; }
        }
        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public Vector2 Motion
        {
            get { return motion; }
            set { motion = value; }
        }
        public float DelayDirection
        {
            get { return delayDirection; }
            set { delayDirection = value; }
        }
        public float ElapsedDirection
        {
            get { return elapsedDirection; }
            set { elapsedDirection = value; }
        }
        public bool DirtPileCreated
        {
            get { return dirtPileCreated; }
            set { dirtPileCreated = value; }
        }
        public float AggroSpeed
        {
            get { return aggroSpeed; }
            set { aggroSpeed = value; }
        }
        public float PatrollingCircle
        {
            get { return patrollingCircle; }
            set { patrollingCircle = value; }
        }
       
        public bool MeleeHit
        {
            get { return meleeHit; }
            set { meleeHit = value; }
        }
        public float ElapsedHitByMelee
        {
            get { return elapsedHitByMelee; }
            set { elapsedHitByMelee = value; }
        }
        public float DelayHitByMelee
        {
            get { return delayHitByMelee; }
            set { delayHitByMelee = value; }
        }
        public float StrikeForce
        {
            get { return strikeForce; }
            set { strikeForce = value; }
        }
        public float DelayStruck
        {
            get { return delayStruck; }
            set { delayStruck = value; }
        }
        public float ElapsedStruck
        {
            get { return elapsedStruck; }
            set { elapsedStruck = value; }
        }
        public bool StrikeMode
        {
            get { return strikeMode; }
            set { strikeMode = value; }
        }
        public float DelayStrike
        {
            get { return delayStrike; }
            set { delayStrike = value; }
        }
        public float ElapsedStrike
        {
            get { return elapsedStrike; }
            set { elapsedStrike = value; }
        }
        public Vector2 PlayerPosition
        {
            get { return playerPosition; }
            set { playerPosition = value; }
        }
        public Vector2 OldPosition
        {
            get { return oldPosition; }
            set { oldPosition = value; }
        }

        public float AggroCircle
        {
            get { return aggroCircle; }
            set { aggroCircle = value; }
        }
        public float AggroRange
        {
            get { return aggroRange; }
            set { aggroRange = value; }
        }
        public bool StartingFlag
        {
            get { return startingFlag; }
            set { startingFlag = value; }
        }
        public bool GoingHome
        {
            get { return goingHome; }
            set { goingHome = value; }
        }
        public Vector2 VectorTowardsStart
        {
            get
            {
                vectorTowardsStart.Normalize();
                return vectorTowardsStart;
            }
            set
            {
                vectorTowardsStart = value;
            }
        }
        public Vector2 VectorTowardsTarget
        {
            get
            {
                vectorTowardsTarget.Normalize();
                return vectorTowardsTarget;
            }
            set { vectorTowardsTarget = value; }

        }

        public NPC_Fighting_Guard(Texture2D texture, Script script)
            : base(texture, script)
        {
            this.Dead = false;
            this.strikeMode = false;
            this.startingFlag = true;
            this.vectorTowardsTarget = Vector2.Zero;
            this.vectorTowardsStart = Vector2.Zero;
            this.AggroStartingPosition = Vector2.Zero;
            this.DelayStrike = 1000f;
            this.ElapsedStrike = 0.0f;
            this.Aggro = false;
            this.GoingHome = false;
            this.speed = 1.0f;
            this.aggroRange = 150;
            this.delayStruck = 500f;
            this.elapsedStruck = 0.0f;
            this.aggroCircle = 500;
        }
        public void SetVectorTowardsTargetAndStartAndCheckAggroMelee(GameTime gameTime, AnimatedSprite player)
        {
            elapsedSeek += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(elapsedSeek > delaySeek)
            {
                elapsedSeek = 0.0f;
                vectorTowardsTarget = player.Origin - this.Origin;
            }
            vectorTowardsStart = startingPosition - Position;

            if (HitByArrow)
            {
                ElapsedHitByArrow += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                this.Position += this.ArrowDirection * 3;
                if (ElapsedHitByArrow > DelayHitByArrow)
                {
                    HitByArrow = false;
                    ElapsedHitByArrow = 0.0f;
                }
            }
            if (MeleeHit)
            {
                ElapsedHitByMelee += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


                if (ElapsedHitByMelee > DelayHitByMelee)
                {
                    //Vector2 HitVector = this.Origin - player.Origin;
                    //HitVector.Normalize();
                    //player.Position -= HitVector;

                    this.TimeToStrike = true;

                    MeleeHit = false;
                    ElapsedHitByMelee = 0.0f;
                }
            }
            if (TimeToStrike)
            {
                ElapsedTimeToStrike += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (ElapsedTimeToStrike > DelayTimeToStrike)
                {
                    player.Life -= StrikeForce;
                    TimeToStrike = false;
                    ElapsedTimeToStrike = 0.0f;
                }
            }
            if (Vector2.Distance(this.Position, player.Position) < 50)
            {
                strikeMode = true;
                ElapsedStrike += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                //UpdateSpriteAnimation(player.Position - this.Position);
                if (ElapsedStrike > DelayStrike)
                {
                    this.MeleeHit = true;
                    ElapsedStrike = 0.0f;
                }

            }
            else
            {
                this.ElapsedStrike = 0.0f;
                strikeMode = false;
            }
            if (Vector2.Distance(Position, AggroStartingPosition) > AggroCircle && AggroStartingPosition != Vector2.Zero && !StrikeMode)
            {
                Aggro = false;
                GoingHome = true;
                AggroStartingPosition = Vector2.Zero;
            }
            else if (Vector2.Distance(player.Position, Position) < AggroRange && !GoingHome && !Aggro)
            {
                AggroStartingPosition = Position;
                Aggro = true;
            }
            else if (Vector2.Distance(startingPosition, Position) < 10 && GoingHome)
            {
                //this.Life = 100;
                GoingHome = false;
            }


        }
        public void GetRandomDirection(GameTime gameTime)
        {
            ElapsedDirection += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Vector2.Distance(StartingPosition, Position) > PatrollingCircle && !Aggro && !GoingHome)
            {
                Direction += 180;
                Direction = direction % 360;
            }
            if (ElapsedDirection > DelayDirection && !GoingHome)
            {
                Direction = Random.Next(0, 360);
                ElapsedDirection = 0;
            }


        }
        public override void Update(GameTime gameTime)
        {
            //if (startingFlag)
            //{
            //    this.startingPosition = Position;
            //    startingFlag = false;
            //}

            //if (Aggro)
            //{
            //    Position += VectorTowardsTarget * speed;
            //}
            //else if (goingHome)
            //{
            //    Position += VectorTowardsStart * speed;
            //}
            //else
            //    Position = startingPosition;
            base.Update(gameTime);
        }
        public void UpdateSpriteAnimation(Vector2 motion)
        {

            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                    CurrentAnimationName = "WalkRight"; //Right
                //motion = new Vector2(1f, 0f);
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {
                
                    CurrentAnimationName = "WalkDown"; //Down
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {
                
                    CurrentAnimationName = "WalkUp"; // Up
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                
                    CurrentAnimationName = "WalkLeft"; //Left
                //motion = new Vector2(-1f, 0f);
            }
        }
    }
}
