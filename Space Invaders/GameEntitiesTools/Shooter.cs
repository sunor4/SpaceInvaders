using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    // This class is a representation of a shooter object to be used in composition with Invader and PlayerShip.
    public class Shooter
    {
        public event Action BulletDisappear;

        public event Action BulletShot;

        private readonly string r_ShootSoundName;
        private readonly Sprite r_BulletOriginObject;
        private readonly int r_MaxCapacity;
        private readonly List<Bullet> r_CurrentShotBulletsList = new List<Bullet>();

        private List<Bullet> CurrentShotBulletsList
        {
            get { return r_CurrentShotBulletsList; }
        }

        private Sprite BulletOriginObject
        {
            get { return r_BulletOriginObject; }
        }

        private int MaxCapacity
        {
            get { return r_MaxCapacity; }
        }

        public Shooter(Sprite i_BulletOriginObject, int i_MaxCapacity)
        {
            r_BulletOriginObject = i_BulletOriginObject;
            r_MaxCapacity = i_MaxCapacity;
            r_ShootSoundName = (i_BulletOriginObject is PlayerShip)
                ? eGameSounds.SSGunShot.ToString()
                : eGameSounds.EnemyGunShot.ToString();
            BulletShot += Shooter_BulletShot;
        }

        private void Shooter_BulletShot()
        {
            (BulletOriginObject.Game as BaseGame).SoundManager.Play(r_ShootSoundName);
        }

        public void ShootBullet()
        {
            if (CurrentShotBulletsList.Count < MaxCapacity)
            {
                OnBulletShot();
                Bullet shotBullet = new Bullet(BulletOriginObject.Game, BulletOriginObject);
                setBulletPosition(shotBullet);
                shotBullet.Disposed += Bullet_Disposed;
                CurrentShotBulletsList.Add(shotBullet);
            }
        }

        private void OnBulletShot()
        {
            if (BulletShot != null)
            {
                BulletShot.Invoke();
            }
        }

        private void setBulletPosition(Bullet i_ShotBullet)
        {
            Vector2 newBulletPosition = BulletOriginObject.Position;
            newBulletPosition.Y += i_ShotBullet.Height * Math.Sign(i_ShotBullet.Velocity.Y);
            newBulletPosition.X += (BulletOriginObject.Width - i_ShotBullet.Width) / 2;
            i_ShotBullet.Position = newBulletPosition;
        }

        // This method subscribes to EnabledChanged event of the bullets that CurrentShotBulletsList holds,
        // In order to "dispose" of unnecessary bullets and make space for new bullets to come.
        private void Bullet_Disposed(object i_Bullet, EventArgs i_EventArgs)
        {
            if (i_Bullet is Bullet)
            {
                CurrentShotBulletsList.Remove((Bullet)i_Bullet);
                OnBulletDisappear();
            }
        }

        private void OnBulletDisappear()
        {
            if (BulletDisappear != null)
            {
                BulletDisappear.Invoke();
            }
        }
    }
}
