using System;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpaceInvaders
{
    public class Barrier : Sprite, ICollidable2D
    {
        private const float k_BarrierMovementSpeed = 35;
        private const string k_SpriteFileName = @"GameAssets\Barrier_44x32";
        private float m_TotalDistanceMovedBeforeSwitch;

        public Barrier(Game i_Game) : base(k_SpriteFileName, i_Game)
        {
            this.BlendState = BlendState.NonPremultiplied;
            this.SortMode = SpriteSortMode.Immediate;
            TintColor = Color.White;
            m_Velocity.X = k_BarrierMovementSpeed;
            m_TotalDistanceMovedBeforeSwitch = 0;
            Hit += SoundManager_Hit;
        }

        private void SoundManager_Hit()
        {
            (Game as BaseGame).SoundManager.Play(eGameSounds.BarrierHit.ToString());
        }

        public override void Update(GameTime i_GameTime)
        {
            Vector2 newPos = Position;
            float movementDistance = Velocity.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            newPos.X += movementDistance;
            m_TotalDistanceMovedBeforeSwitch += Math.Abs(movementDistance);
            Position = newPos;

            // Change direction each frame:
            if (m_TotalDistanceMovedBeforeSwitch >= this.Width / 2)
            {
                m_TotalDistanceMovedBeforeSwitch -= this.Width / 2;
                m_Velocity.X = -Velocity.X;
            }
        }

        protected override void SetInitialPosition()
        {
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                Bullet bullet = i_Collidable as Bullet;

                // We want the "bite" size to be of this:
                int pixelsToEatInYAxis = (int)(0.35 * bullet.Height);

                // We get the bullet direction in order to know in which direction to "bite"
                float bulletDirection = Math.Sign(bullet.Velocity.Y);
                int changeInYAxis = pixelsToEatInYAxis * (int)bulletDirection;
                Vector2 bulletNewPos = new Vector2(bullet.Position.X, bullet.Position.Y + changeInYAxis);
                
                // We want to avoid triggering the collision manager again.
                bullet.SetPositionWithoutCollision(bulletNewPos);
                ErasePixelCollision(bullet);
                OnHit();
            }
            else
            {
                ErasePixelCollision(i_Collidable);
            }
        }
    }
}
