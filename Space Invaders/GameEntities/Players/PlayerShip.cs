using System;
using Infrastructure.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpaceInvaders
{
    public class PlayerShip : Player, ICollidable2D
    {
        private const Keys k_PlayerOneLeftKey = Keys.Left;
        private const Keys k_PlayerOneRightKey = Keys.Right;
        private const Keys k_PlayerOneShootKey = Keys.Up;
        private const Keys k_PlayerTwoLeftKey = Keys.A;
        private const Keys k_PlayerTwoRightKey = Keys.D;
        private const Keys k_PlayerTwoShootKey = Keys.W;
        private const float k_KeyboardMovementSpeedPlayerShip = 140;
        private const int k_MaxNumOfBulletsOnScreen = 2;
        private const string k_SpriteFileNamePlayerOne = @"GameAssets\Ship01_32x32";
        private const string k_SpriteFileNamePlayerTwo = @"GameAssets\Ship02_32x32";
        private const int k_DefaultPlayerLives = 3;
        private const float k_HitAnimatorDuration = 2f;
        private const float k_DeathAnimatorDuration = 2.6f;
        private const int k_NumOfBlinksOnHitPerSecond = 8;
        private const int k_NumOfRotationsOnDeathPerSecond = 6;
        private Color m_PlayerOneScoreColor = Color.LightBlue;
        private Color m_PlayerTwoScoreColor = Color.ForestGreen;
        private readonly ePlayerIndex r_PlayerIdx;
        private readonly Shooter r_Shooter;
        private PlayerControls m_PlayerControls;

        public ePlayerIndex PlayerIdx
        {
            get { return r_PlayerIdx; }
        }

        private PlayerControls PlayerController
        {
            get { return m_PlayerControls; }
        }

        private Shooter PlayerShooter
        {
            get { return r_Shooter; }
        }

        private IInputManager InputManager
        {
            get { return m_InputManager; }
            set { m_InputManager = value; }
        }

        public PlayerShip(Game i_Game, ePlayerIndex i_PlayerIndex, bool i_IsPlayable = true) : base(string.Empty, i_Game)
        {
            setProperties();
            r_Shooter = new Shooter(this, k_MaxNumOfBulletsOnScreen);
            r_PlayerIdx = i_PlayerIndex;
            setUniquePropsByPlayerIdx();
            if (!i_IsPlayable)
            {
                this.Enabled = false;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            Hit += TakeDamage;
            Hit += SoundManager_Hit;
            Hit += Animations_Hit;
        }

        private void SoundManager_Hit()
        {
            (Game as BaseGame).SoundManager.Play(eGameSounds.LifeDie.ToString());
        }

        private void setProperties()
        {
            Lives = k_DefaultPlayerLives;
            Velocity = new Vector2(k_KeyboardMovementSpeedPlayerShip);
            TintColor = Color.White;
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
        }

        protected override void InitAnimations()
        {
            createDeathAnimator();
            createHitAnimator();
        }

        private void createHitAnimator()
        {
            TimeSpan animationLength = TimeSpan.FromSeconds(k_HitAnimatorDuration);
            TimeSpan blinkDuration = TimeSpan.FromSeconds(1f / (float)k_NumOfBlinksOnHitPerSecond);
            BlinkAnimator blinkAnimator = new BlinkAnimator(blinkDuration, animationLength);
            CompositeAnimator hitAnimator = new CompositeAnimator(
            "Hit Animator", animationLength, this, blinkAnimator);
            this.Animations.Add(hitAnimator);
        }

        private void Animations_Hit()
        {
            this.Animations.Resume();
            if (this.Animations["Hit Animator"].IsFinished)
            {
                this.Animations["Hit Animator"].Reset();
            }
        }

        private void createDeathAnimator()
        {
            TimeSpan animationLength = TimeSpan.FromSeconds(k_DeathAnimatorDuration);
            RotateAnimator rotateAnimator = new RotateAnimator(k_NumOfRotationsOnDeathPerSecond, animationLength);
            FadeAnimator fadeAnimator = new FadeAnimator(animationLength);
            CompositeAnimator deathAnimator = new CompositeAnimator(
            "Death Animator", animationLength, this, rotateAnimator, fadeAnimator);
            this.Animations.AddWithoutEnabling(deathAnimator);
            Death += animations_Death;
        }

        private void animations_Death()
        {
            Hit -= Animations_Hit;
            (this.Animations["Hit Animator"] as CompositeAnimator).Enabled = false;
            this.Death -= animations_Death;
            this.Animations["Death Animator"].Resume();
            this.Animations["Death Animator"].Finished += animations_DeathFinish;
        }

        private void animations_DeathFinish(object sender, EventArgs e)
        {
            Die();
        }

        // This method sets the unique properties of each player by its given index.
        private void setUniquePropsByPlayerIdx()
        {
            switch (PlayerIdx)
            {
                case ePlayerIndex.PlayerOne:
                    m_PlayerControls = new PlayerControls(k_PlayerOneLeftKey, k_PlayerOneRightKey, k_PlayerOneShootKey);
                    this.AssetName = k_SpriteFileNamePlayerOne;
                    this.PlayerScore.ScoreColor = m_PlayerOneScoreColor;
                    break;
                case ePlayerIndex.PlayerTwo:
                    m_PlayerControls = new PlayerControls(k_PlayerTwoLeftKey, k_PlayerTwoRightKey, k_PlayerTwoShootKey);
                    this.AssetName = k_SpriteFileNamePlayerTwo;
                    this.PlayerScore.ScoreColor = m_PlayerTwoScoreColor;
                    break;
            }
        }

        protected override void SetInitialPosition()
        {
            // Initialize ship location to bottom right position of the screen.
            float xPos = (float)(Game as BaseGame).GameWindowBounds.Left + ((int)PlayerIdx * Texture.Width);
            float yPos = (float)(Game as BaseGame).GameWindowBounds.Height - Texture.Height;
            Vector2 playerShipInitPosition = new Vector2(xPos, yPos);
            Position = playerShipInitPosition;
        }

        public override void Update(GameTime i_GameTime) 
        {
            if (Enabled && !IsAlreadyDead)
            {
                changePosition(i_GameTime);
            }

            base.Update(i_GameTime);
        }

        private void changePosition(GameTime i_GameTime)
        {
            Vector2 newPosition = Position;
            bool IsMouseTriggered = false;
            
            if (PlayerIdx == ePlayerIndex.PlayerOne)
            {
                newPosition += getMouseInput(out IsMouseTriggered);
            }
            
            if (!IsMouseTriggered)
            {
                newPosition += getKeyboardInput(i_GameTime);
            }

            // clam the position between screen bounds:
            newPosition.X = MathHelper.Clamp(newPosition.X, 0f, (Game as BaseGame).GameWindowBounds.Width - Texture.Width);

            // Change the player's hit area and position together.
            Position = newPosition;
        }

        // Gets the mouse input from the user, if it's movement related - returns the new X-Direction.
        // if left click is pressed, shoot player bullet.
        private Vector2 getMouseInput(out bool o_IsMouseTriggered)
        {
            o_IsMouseTriggered = false;

            // Init vector to unreachable location on screen.
            Vector2 newPosition = Vector2.Zero;

            // If mouse is outside of screen, don't allow movement of the ship in order to keep offset at ship location.
            bool isMouseWithinScreen =
                InputManager.MouseState.Position.X > (Game as BaseGame).GameWindowBounds.Left &&
                InputManager.MouseState.Position.X < (Game as BaseGame).GameWindowBounds.Right;

            if (isMouseWithinScreen && InputManager.MousePositionDelta != Vector2.Zero)
            {
                o_IsMouseTriggered = true;
                newPosition.X = InputManager.MousePositionDelta.X;
            }

            // If a click was detected (switch from pressed to released)
            if (InputManager.ButtonPressed(eInputButtons.Left))
            {
               PlayerShooter.ShootBullet();
            }

            return newPosition;
        }

        // Gets the keyboard input from the user, if it's movement related - returns the new X-Direction.
        // if enter is pressed, shoot player bullet.
        private Vector2 getKeyboardInput(GameTime i_GameTime)
        {
            Vector2 newVelocity = Vector2.Zero;

            // We only want to detect one click of enter to shoot.
            if (InputManager.KeyPressed(PlayerController.ShootKey))
            {
                PlayerShooter.ShootBullet();
            }

            // We keep all if statements in case of multiple key presses. We increment\decrement by one
            // to indicate that a key was pressed.
            if (InputManager.KeyboardState.IsKeyDown(PlayerController.RightKey))
            {
                newVelocity.X++;
            }

            if (InputManager.KeyboardState.IsKeyDown(PlayerController.LeftKey))
            {
                newVelocity.X--;
            }

            Vector2 changeInPosition = newVelocity * (k_KeyboardMovementSpeedPlayerShip * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            return changeInPosition;
        }

        public override void TakeDamage()
        {
            const int k_ScoreLostOnHit = 600;

            // Reduce final score by 600 for each destroyed live + change location to initial position.
            r_Score.Value -= k_ScoreLostOnHit;
            base.TakeDamage();
            if (Lives > 0)
            {
                SetInitialPosition();
            }
        }

        public void Die()
        {
            Enabled = false;
            Visible = false;
            (Game as BaseGame).GameOver();
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                Bullet bullet = i_Collidable as Bullet;
                if (bullet.BulletShooter is Enemy && !this.IsAlreadyDead)
                {
                    this.OnHit();
                }
            }
        }
    }
}
