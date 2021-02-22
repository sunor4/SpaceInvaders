using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    // Abstract class which is a representation of any enemy in the game.
    public abstract class Enemy : Sprite, ICollidable2D
    {
        private const int k_DefaultEnemyLives = 1;
        private int m_ScoreOnKill;    

        public int ScoreOnKill
        {
            get { return m_ScoreOnKill; }
            set { m_ScoreOnKill = value; }
        }

        public Enemy(Game i_Game, string i_SpriteFileName) : base(i_SpriteFileName, i_Game)
        {
            Lives = k_DefaultEnemyLives;
            Hit += TakeDamage;
        }

        public virtual void Die()
        {
            // Make enemy dead and also eliminate its hit area so it won't collide with anything.
            Enabled = false;
            Visible = false;
            this.Dispose();
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                Bullet bullet = i_Collidable as Bullet;
                if (bullet.BulletShooter is PlayerShip && !IsAlreadyDead)
                {
                    OnHit();
                    AddScore(bullet.BulletShooter as PlayerShip);
                }
            }
        }

        private void AddScore(PlayerShip i_PlayerShip)
        {
            if (!i_PlayerShip.IsAlreadyDead)
            {
                i_PlayerShip.PlayerScore.Value += this.ScoreOnKill;
            }
        }
    }
}
