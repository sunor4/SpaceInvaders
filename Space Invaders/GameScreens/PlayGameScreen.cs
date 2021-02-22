using System;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpaceInvaders.GameScreens
{
    public class PlayGameScreen : GameScreen
    {
        private const Keys k_PauseScreenKey = Keys.P;
        private const float k_IncreaseInBarrierSpeedPerLevel = 1.06f;
        private const int k_LevelDifficultyCycle = 4;
        private PlayGameUI m_PlayGameUI;
        private EnemyMatrix m_EnemyMatrix;
        private MotherShip m_MotherShip;
        public PlayerShip m_PlayerOne;
        public PlayerShip m_PlayerTwo;
        private CompositeBarrier m_CompositeBarrier;
        private PauseScreen m_PauseScreen;
        private int m_InitLevel;
        private int m_CurrentLevel = 0;
        public event Action LevelChanged; 

        public int CurrentLevel
        {
            get { return m_CurrentLevel; }
            private set
            {
                if (m_CurrentLevel != value)
                {
                    m_CurrentLevel = value;
                    OnLevelChanged();
                }
            }
        }

        public PlayGameScreen(Game i_Game) : base(i_Game)
        {
            m_InitLevel = CurrentLevel;
            createGameEntities();
            createPlayers();
            subscribeToEvents();
            m_PlayGameUI = new PlayGameUI(Game, m_PlayerOne, m_PlayerTwo);
            m_CompositeBarrier = new CompositeBarrier(Game);
            m_PauseScreen = new PauseScreen(Game);
            addAllComponents();
        }

        public override void Initialize()
        {
            setCurrentMatrixLogic();
            setCurrentBarrierLogic();
            base.Initialize();
            reinitMatrixIfNeeded();
        }

        private void reinitMatrixIfNeeded()
        {
            if (CurrentLevel != m_InitLevel)
            {
                m_EnemyMatrix.Initialize();
            }
        }

        protected override void AddComponentsToUI()
        {
            GameUI.Add(new Background(Game));
        }

        private void subscribeToEvents()
        {
            m_EnemyMatrix.LevelCleared += EnemyMatrix_LevelCleared;
            this.LevelChanged += PlayGameScreen_LevelChanged;
        }

        private void createGameEntities()
        {
            m_EnemyMatrix = new EnemyMatrix(Game);
            m_MotherShip = new MotherShip(Game);
        }

        private void createPlayers()
        {
            int numOfPlayers = (Game as BaseGame).NumOfPlayers;
            switch (numOfPlayers)
            {
                case 2:
                {
                    m_PlayerOne = new PlayerShip(Game, ePlayerIndex.PlayerOne);
                    m_PlayerTwo = new PlayerShip(Game, ePlayerIndex.PlayerTwo);
                    break;
                }
                default:
                {
                    m_PlayerOne = new PlayerShip(Game, ePlayerIndex.PlayerOne);
                    break;
                }
            }
        }

        private void addAllComponents()
        {
            this.Add(m_PlayerOne);
            this.Add(m_PlayerTwo);
            this.Add(m_PlayGameUI);
            this.Add(m_EnemyMatrix);
            this.Add(m_MotherShip);
            this.Add(m_CompositeBarrier);
        }

        private void EnemyMatrix_LevelCleared()
        {
            CurrentLevel++;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(k_PauseScreenKey))
            {
                ScreensManager.SetCurrentScreen(m_PauseScreen);
            }
        }

        private void OnLevelChanged()
        {
            if (LevelChanged != null)
            {
                LevelChanged.Invoke();
            }
        }

        private void PlayGameScreen_LevelChanged()
        {
            soundIndicationOnLevelChanged();
            logicChangesOnLevelChanged();
            visualChangesOnLevelChanged();
        }

        private void soundIndicationOnLevelChanged()
        {
            (Game as BaseGame).SoundManager.Play(eGameSounds.LevelWin.ToString());
        }

        private void visualChangesOnLevelChanged()
        {
            this.ExitScreen();
        }

        private void logicChangesOnLevelChanged()
        {
            resetMotherShip();
            resetEnemyMatrix();
            resetCompositeBarrier();
        }

        private void resetMotherShip()
        {
            m_MotherShip.Respawn();
        }

        private void resetCompositeBarrier()
        {
            m_CompositeBarrier.Reset();
            m_CompositeBarrier.Initialize();
        }

        private void resetEnemyMatrix()
        {
            m_EnemyMatrix.Reset();
        }

        private void setCurrentMatrixLogic()
        {
            int levelDifficulty = m_CurrentLevel % k_LevelDifficultyCycle; // between 0 to 3

            //Add score to all enemies:
            int extraScoreToAdd = 100 * levelDifficulty;
            m_EnemyMatrix.ExtraScorePerInvader = extraScoreToAdd;

            //Add additional columns:
            int numOfColsToAdd = levelDifficulty;
            m_EnemyMatrix.CurrentNumOfCols = numOfColsToAdd + m_EnemyMatrix.InitNumOfCols;
        }

        private void setCurrentBarrierLogic()
        {
            int levelDifficulty = m_CurrentLevel % k_LevelDifficultyCycle; // between 0 to 3

            //Change barriers velocity:
            Vector2 velocityMultiplier = (levelDifficulty == 0) ? Vector2.Zero : Vector2.One;
            float additionalChangeInSpeed = (float)Math.Pow(k_IncreaseInBarrierSpeedPerLevel, (levelDifficulty - 1));
            additionalChangeInSpeed = MathHelper.Clamp(additionalChangeInSpeed, 1, int.MaxValue);
            Vector2 velocityIncreaseInSpeed = new Vector2(additionalChangeInSpeed);
            
            // Update all barriers through the composite barrier:
            m_CompositeBarrier.VelocityMultiplier = velocityMultiplier * velocityIncreaseInSpeed;
        }
    }
}
