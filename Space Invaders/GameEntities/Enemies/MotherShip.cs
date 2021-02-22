using System;
using Infrastructure.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpaceInvaders
{
    public class MotherShip : Enemy
    {
        private const float k_DeathAnimatorDuration = 3f;
        private const float k_DeathBlinkDuration = 0.1f;
        private const float k_MotherShipSpeed = 95f;
        private const string k_SpriteFileName = @"GameAssets\MotherShip_32x120";
        private const int k_ScoreOnKillMothership = 600;
        private Random m_ShipRandom = new Random();

        public MotherShip(Game i_Game) : base(i_Game, k_SpriteFileName)
        {
            ScoreOnKill = k_ScoreOnKillMothership;
            TintColor = Color.Red;
            Visible = false;
            Death += SoundManager_Death;
        }

        private void SoundManager_Death()
        {
            (Game as BaseGame).SoundManager.Play(eGameSounds.MotherShipKill.ToString());
        }

        protected override void InitAnimations()
        {
            createDeathAnimator();
        }

        private void createDeathAnimator()
        {
            TimeSpan animationLength = TimeSpan.FromSeconds(k_DeathAnimatorDuration);
            TimeSpan blinkDuration = TimeSpan.FromSeconds(k_DeathBlinkDuration);
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(animationLength);
            FadeAnimator fadeAnimator = new FadeAnimator(animationLength);
            BlinkAnimator blinkAnimator = new BlinkAnimator(blinkDuration, animationLength);
            CompositeAnimator deathAnimator = new CompositeAnimator(
            "Death Animator", animationLength, this, shrinkAnimator, fadeAnimator, blinkAnimator);
            this.Animations.Add(deathAnimator);
            Death += Animations_Death;
        }

        private void Animations_Death()
        {
            this.Animations.Resume();
            this.Animations.Reset();
            this.Animations["Death Animator"].Resume();
            this.Animations["Death Animator"].Finished += Animations_DeathFinish;
        }

        private void Animations_DeathFinish(object sender, EventArgs e)
        {
            Die();
            this.Animations["Death Animator"].Pause();
            this.Animations.Pause();
        }

        protected override void SetInitialPosition()
        {
            // We want the mothership to be "invisible" to the player at first, so it would create the illusion
            // of entering the screen.
            float xPos = -Texture.Width;

            // We want a space of one mothership from top of the screen.
            float yPos = Texture.Height;
            Position = new Vector2(xPos, yPos);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (Enabled)
            {
                if (!Visible)
                {
                    randomMotherShipAppearance();
                }
                else
                {
                    Vector2 newPosition = Position;
                    newPosition.X += k_MotherShipSpeed * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                    Position = newPosition;
                    isMotherShipVisible();
                }
            }

            base.Update(i_GameTime);
        }

        // This method is in charge of generating a random mothership appearance.
        private void randomMotherShipAppearance()
        {
            const int k_Chance = 3;
            const int k_MaximumChance = 1000;
            float shipChance = m_ShipRandom.Next(k_MaximumChance);
            Visible = shipChance <= k_Chance;
        }

        // This method is in charge of checking whether the mothership is on screen at the moment so
        // we know if we should generate another random appearance.
        private void isMotherShipVisible()
        {
            bool isMotherShipVisible = Bounds.Left <= (Game as BaseGame).GameWindowBounds.Right;

            if (!isMotherShipVisible)
            {
                Die();
            }
        }

        public override void Die()
        {
            Visible = false;
            this.Animations["Death Animator"].Reset();
            this.Animations["Death Animator"].Pause();
            Respawn();
        }
    }
}
