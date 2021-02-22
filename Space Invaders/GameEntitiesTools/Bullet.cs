using System;
using System.CodeDom;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpaceInvaders
{
    public class Bullet : Sprite, ICollidable2D
    {
        private const int k_RandomBulletSurviveChance = 5;
        private const float k_MovementSpeedBullet = 140;
        private const string k_SpriteFileName = @"GameAssets\Bullet";
        private readonly Sprite r_BulletShooter;
        private readonly Random r_RandomBulletSurvive = new Random();

        public Sprite BulletShooter
        {
            get { return r_BulletShooter; }
        }

        public Bullet(Game i_Game, Sprite i_BulletShooter) : base(k_SpriteFileName, i_Game)
        {
            // Let Bullet be a standalone entity within the game.
            Game.Components.Add(this);
            r_BulletShooter = i_BulletShooter;
            setBulletByType();
            (Game as BaseGame).CurrentActiveScreen.StateChanged += CurrentScreen_StateChanged;
        }

        private void CurrentScreen_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.CurrentState == eScreenState.Deactivating && e.PrevState == eScreenState.Active ||
                e.CurrentState == eScreenState.Inactive && e.PrevState == eScreenState.Deactivating)
            {
                this.Enabled = false;
            }
            else if (e.CurrentState == eScreenState.Activating && e.PrevState == eScreenState.Inactive ||
                     e.CurrentState == eScreenState.Active && e.PrevState == eScreenState.Activating)
            {
                this.Enabled = true;
            }
            else
            {
                DestroyBullet();
            }
        }

        private void setBulletByType()
        {
            if (BulletShooter is PlayerShip)
            {
                TintColor = Color.Red;
                Velocity = new Vector2(0, -k_MovementSpeedBullet);
            }
            else if (BulletShooter is Enemy)
            {
                TintColor = Color.Blue;
                Velocity = new Vector2(0, k_MovementSpeedBullet);
            }
        }

        // Checks if bullet is out of screen or hit a target, If yes - destroy it, else Update Y direction.
        public override void Update(GameTime i_GameTime)
        {
            if (isBulletOutOfScreen())
            {
                this.DestroyBullet();
            }
            else
            {
                moveInYDirection(i_GameTime);
            }
        }

        // This method is in charge of detecting Bullet going out of screen.
        private bool isBulletOutOfScreen()
        {
            bool isBulletOutOfScreen =
                Position.Y > (Game as BaseGame).GameWindowBounds.Bottom ||
                 Position.Y < (Game as BaseGame).GameWindowBounds.Top;
            return isBulletOutOfScreen;
        }

        protected void moveInYDirection(GameTime i_GameTime)
        {
            float movementDistance =
           (float) i_GameTime.ElapsedGameTime.TotalSeconds * Velocity.Y;

            // Change the bullet's position and hit area together.
            Vector2 newPosition = Position;
            newPosition.Y += movementDistance;
            Position = newPosition;
        }

        public void DestroyBullet()
        {
            Enabled = false;
            Visible = false;
            this.Dispose();
        }

        public override void Collided(ICollidable i_Collidable)
        {
            bool isBulletHitOpponent = (i_Collidable is Enemy && BulletShooter is PlayerShip) ||
                                       (i_Collidable is PlayerShip && BulletShooter is Enemy);
            if (isBulletHitOpponent || (i_Collidable is Barrier)) 
            {
                this.DestroyBullet();
            }

            bool isBulletHitOpponentBullet =
                i_Collidable is Bullet && 
                this.BulletShooter.GetType() != (i_Collidable as Bullet).BulletShooter.GetType();

            if (isBulletHitOpponentBullet)
            {
                if (r_BulletShooter is PlayerShip || !isBulletRandomlySurvive())
                {
                    this.DestroyBullet();
                }
            }
        }

        private bool isBulletRandomlySurvive()
        {
            bool isBulletDestroyed = r_RandomBulletSurvive.Next(k_RandomBulletSurviveChance) < 1;
            return isBulletDestroyed;
        }
    }
}
