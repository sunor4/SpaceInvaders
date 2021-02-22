using System;
using Infrastructure.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{    
    public class Invader : Enemy
    {
        private const string k_SpriteFileName = @"GameAssets\Invaders_192x32";
        private const int k_SpriteWidthPerInvader = 32;
        private const int k_SpriteCellsPerInvader = 2;
        private const int k_InitialMovementXDirection = 1;
        private const int k_InitialMovementYDirection = 0;
        private const int k_ScoreOnKillPinkEnemy = 300;
        private const int k_ScoreOnKillBlueEnemy = 200;
        private const int k_ScoreOnKillYellowEnemy = 70;
        private const int k_MaxNumOfBulletsOnScreen = 1;
        private const int k_MaxRandomShotValue = 1000;
        private const int k_NumOfJumpsInXDirectionPerSecond = 2;
        private const float k_DecreaseTimeBetweenJumpsOnJumpDown = 0.95f;
        private const float k_InvaderShrinkAndRotateAnimationTime = 1.7f;
        private const float k_NumOfRotationsPerSecond = 5f;

        public event Action ReachBottom;

        public event Action BulletDisappear;

        public event Action BulletShot;

        public event Action<float> ReachBoundsOnNextFrame;

        public event Action TimeUntilJumpChange;

        public event Action MatrixChange;

        private readonly eInvaderType r_InvaderType;
        private float m_TimeUntilJump = 1f / k_NumOfJumpsInXDirectionPerSecond;
        private TimeSpan m_CurrentTimeUntilJump = TimeSpan.Zero;
        private bool m_JumpDownEnabled = false;
        private bool m_IsCompensateOnNextFrame = false;
        private float m_DistanceToCompensateOnNextFrame = 0;
        private readonly Shooter r_InvaderShooter;
        private bool m_IsAllowedToShoot = true;
        private readonly Random r_BulletEnemyRandomizer = new Random();

        private float TimeUntilJump
        {
            get { return m_TimeUntilJump; }
            set
            {
                if (value != m_TimeUntilJump)
                {
                    m_TimeUntilJump = value;
                    OnTimeUntilJumpChange();
                }
            }
        }

        public eInvaderType RInvaderType
        {
            get { return r_InvaderType; }
        }

        private TimeSpan CurrentTimeUntilJump
        {
            get { return m_CurrentTimeUntilJump; }
            set { m_CurrentTimeUntilJump = value; }
        }

        public bool JumpDownEnabled
        {
            get { return m_JumpDownEnabled; }
            set { m_JumpDownEnabled = value; }
        }

        public bool IsCompensateOnNextFrame
        {
            get { return m_IsCompensateOnNextFrame; }
            set { m_IsCompensateOnNextFrame = value; }
        }

        public float DistanceToCompensateOnNextFrame
        {
            get { return m_DistanceToCompensateOnNextFrame; }
            set { m_DistanceToCompensateOnNextFrame = value; }
        }

        private Shooter InvaderShooter
        {
            get { return r_InvaderShooter; }
        }

        public bool IsAllowedToShoot
        {
            get { return m_IsAllowedToShoot; }
            set { m_IsAllowedToShoot = value; }
        }

        public Invader(Game i_Game, eInvaderType i_RInvaderType) : base(i_Game, k_SpriteFileName)
        {
            m_MaxTextureCells = k_SpriteCellsPerInvader;
            r_InvaderType = i_RInvaderType;
            setInvaderByType();
            r_InvaderShooter = new Shooter(this, k_MaxNumOfBulletsOnScreen);
            InvaderShooter.BulletShot += InvaderShooter_BulletShot;
            InvaderShooter.BulletDisappear += InvaderShooter_BulletDisappear;
            Hit += SoundManager_Hit;
        }

        private void SoundManager_Hit()
        {
            (Game as BaseGame).SoundManager.Play(eGameSounds.EnemyKill.ToString());
        }

        protected override void InitSourceRectangle()
        {
            int xPosSourceRectangle = k_SpriteWidthPerInvader *
                                      (m_MaxTextureCells * (int) RInvaderType);
            SourceRectangle = new Rectangle(xPosSourceRectangle, 0, k_SpriteWidthPerInvader, Texture.Height);
            m_WidthBeforeScale = SourceRectangle.Width;
            m_HeightBeforeScale = SourceRectangle.Height;
            Velocity = new Vector2(k_InitialMovementXDirection * SourceRectangle.Width / 2, k_InitialMovementYDirection);
        }

        protected override void InitAnimations()
        {
            createDeathAnimator();
            createCellAnimator();
            this.Animations.Resume();
        }

        private void createCellAnimator()
        {
            TimeSpan cellLength = TimeSpan.FromSeconds(TimeUntilJump);
            CellAnimator cellAnimator =
                new CellAnimator(cellLength, CurrentCellIndex, m_MaxTextureCells, TimeSpan.Zero);
            this.Animations.Add(cellAnimator);
            TimeUntilJumpChange += Invader_TimeUntilJumpChange;
        }

        private void createDeathAnimator()
        {
            TimeSpan animationLength = TimeSpan.FromSeconds(k_InvaderShrinkAndRotateAnimationTime);
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(animationLength);
            RotateAnimator rotateAnimator = new RotateAnimator(k_NumOfRotationsPerSecond, animationLength);
            CompositeAnimator deathAnimator =
                new CompositeAnimator("Death Animator", animationLength, this, shrinkAnimator, rotateAnimator);
            this.Animations.AddWithoutEnabling(deathAnimator);
            Hit += Animations_Death;
        }

        private void Animations_Death()
        {
            this.Animations.PauseAllAnimations();
            this.Animations.Reset();
            this.Animations["Death Animator"].Resume();
            this.Animations["Death Animator"].Finished += Animations_DeathFinish;
        }

        private void Animations_DeathFinish(object sender, EventArgs e)
        {
            Die();
            this.Animations.Pause();
            OnMatrixChange();
        }

        // We don't want to reveal shooter to enemy matrix directly (or to anyone except for Invader for that matter)
        // So we use 2 sets of 2 public events, one for Invader and one for InvaderShooter.
        private void InvaderShooter_BulletShot()
        {
            OnBulletShot();
        }

        private void InvaderShooter_BulletDisappear()
        {
            OnBulletDisappear();
        }

        private void setInvaderByType()
        {
            switch (RInvaderType)
            {
                case eInvaderType.Pink:
                    TintColor = Color.LightPink;
                    ScoreOnKill = k_ScoreOnKillPinkEnemy;
                    break;
                case eInvaderType.Blue:
                    TintColor = Color.LightBlue;
                    ScoreOnKill = k_ScoreOnKillBlueEnemy;
                    break;
                case eInvaderType.Yellow:
                    TintColor = Color.LightYellow;
                    ScoreOnKill = k_ScoreOnKillYellowEnemy;
                    break;
            }
        }

        // Update enemy movement, if jump down is enabled then the enemy will jump down (in formation with matrix)
        public override void Update(GameTime i_GameTime)
        {
            if (Enabled)
            {
                if (m_CurrentTimeUntilJump.TotalSeconds >= TimeUntilJump)
                {
                    if (JumpDownEnabled)
                    {
                        jumpDown();
                    }
                    else if (IsCompensateOnNextFrame)
                    {
                       compensateInXDirection();
                    }
                    else
                    {
                        moveInXDirection();
                    }

                    // Decrement current timespan accordingly.
                    CurrentTimeUntilJump = CurrentTimeUntilJump.Subtract(TimeSpan.FromSeconds(TimeUntilJump));
                }
                else
                {
                    // Increment current timespan accordingly.
                    CurrentTimeUntilJump = CurrentTimeUntilJump.Add(i_GameTime.ElapsedGameTime);
                }

                if (!IsAlreadyDead)
                {
                    shootBullet();
                }
            }

            base.Update(i_GameTime);
        }

        private void shootBullet()
        {
            if (m_IsAllowedToShoot)
            {
                bool isShotFired = r_BulletEnemyRandomizer.Next(k_MaxRandomShotValue) < 1;
                if (isShotFired)
                {
                    InvaderShooter.ShootBullet();
                }
            }
        }

        private void jumpDown()
        {
            float jumpDistance = 0.5f * SourceRectangle.Height;

            // Decrease time between jumps
            TimeUntilJump *= k_DecreaseTimeBetweenJumpsOnJumpDown;

            // Change movement direction of enemy.
            m_Velocity.X *= -1;
            m_Position.Y += jumpDistance;
            
            // Checks whether we've reached the bottom of the screen after the jump in order to trigger the event.
            bool isReachedBottom = Position.Y >= (Game as BaseGame).GameWindowBounds.Bottom - (2 * this.Height);
            if (isReachedBottom)
            {
                OnReachBottom();
            }

            JumpDownEnabled = false;
        }

        private void moveInXDirection()
        {
            float movementDistance = Velocity.X;
            
            // The position after the current update:
            Vector2 newPos = new Vector2(Position.X + movementDistance, Position.Y);
            float compensateOnNextFrameDistance = 0;

            // The position after the next update:
            float xPosOnNextFrame = newPos.X + movementDistance;
            
            // We check if our next frame position is going to exceed the screen bounds, in order to "compensate" by
            // jumping only part of the jump. (we know that only the extreme right/left invader is going to trigger
            // the "compensation" event.
            if (xPosOnNextFrame < (Game as BaseGame).GameWindowBounds.Left)
            {
                compensateOnNextFrameDistance = (Game as BaseGame).GameWindowBounds.Left - newPos.X;
            }
            else if (xPosOnNextFrame > (Game as BaseGame).GameWindowBounds.Right - SourceRectangle.Width)
            {
                compensateOnNextFrameDistance = (Game as BaseGame).GameWindowBounds.Right - SourceRectangle.Width - newPos.X;
            }

            // If we need to compensate, trigger the event.
            bool arrivedExactlyToLeftBound = xPosOnNextFrame == (Game as BaseGame).GameWindowBounds.Left &&
                                             compensateOnNextFrameDistance == 0;
            bool arrivedExactlyToRightBound = xPosOnNextFrame == (Game as BaseGame).GameWindowBounds.Right &&
                                              compensateOnNextFrameDistance == 0;

            if (compensateOnNextFrameDistance != 0 || arrivedExactlyToLeftBound || arrivedExactlyToRightBound)
            {
                OnReachBoundsOnNextFrame(compensateOnNextFrameDistance);
            }

            Position = newPos;
        }

        private void compensateInXDirection()
        {
            Position = new Vector2(Position.X + m_DistanceToCompensateOnNextFrame, Position.Y);
            DistanceToCompensateOnNextFrame = 0;
            IsCompensateOnNextFrame = false;
            JumpDownEnabled = true;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            base.Collided(i_Collidable);
            if (i_Collidable is PlayerShip)
            {
                (Game as BaseGame).GameOver();
            }
        }

        private void OnReachBoundsOnNextFrame(float i_DistanceToCompensate)
        {
            if (ReachBoundsOnNextFrame != null && !IsAlreadyDead)
            {
                ReachBoundsOnNextFrame.Invoke(i_DistanceToCompensate);
            }
        }

        private void OnReachBottom()
        {
            if (ReachBottom != null)
            {
                ReachBottom.Invoke();
            }
        }

        private void OnBulletDisappear()
        {
            if (BulletDisappear != null)
            {
                BulletDisappear.Invoke();
            }
        }

        private void OnBulletShot()
        {
            if (BulletShot != null)
            {
                BulletShot.Invoke();
            }
        }

        private void OnTimeUntilJumpChange()
        {
            if (TimeUntilJumpChange != null)
            {
                TimeUntilJumpChange.Invoke();
            }
        }

        private void OnMatrixChange()
        {
            if (MatrixChange != null)
            {
                MatrixChange.Invoke();
            }
        }

        private void Invader_TimeUntilJumpChange()
        {
            TimeSpan newLength = TimeSpan.FromSeconds(TimeUntilJump);
            (this.Animations["Cell Animator"] as CellAnimator).SetCellTime(newLength);
        }
    }
}