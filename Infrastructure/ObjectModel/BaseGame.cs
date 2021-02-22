using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    // This class is a reusable base class for Game.
    public class BaseGame : Game
    {
        private const int k_MinNumOfPlayers = 1;
        private const int k_MaxNumOfPlayers = 2;
        private List<Player> m_PlayersList = new List<Player>();
        private GraphicsDeviceManager m_GraphicsDevice;
        protected SpriteBatch m_SpriteBatch;
        protected ISoundManager m_SoundManager;
        protected IScreensMananger m_ScreensMananger;
        protected IResolutionManager m_ResolutionManager;
        protected GameTime m_GameTime;
        private int m_PlayerDeathCounter = 0;
        private int m_NumOfPlayers = 1;
        public event Action GameEnd;

        public GameScreen CurrentActiveScreen
        {
            get { return m_ScreensMananger.ActiveScreen; }
        }

        public IResolutionManager ResolutionManager
        {
            get { return m_ResolutionManager; }
        }

        public ISoundManager SoundManager
        {
            get { return m_SoundManager; }
        }

        public GraphicsDeviceManager GameGraphicsDeviceManager
        {
            get { return m_GraphicsDevice; }
            protected set { m_GraphicsDevice = value; }
        }

        public int NumOfPlayers
        {
            get { return m_NumOfPlayers; }
            set
            {
                m_NumOfPlayers = MathHelper.Clamp(value, k_MinNumOfPlayers, k_MaxNumOfPlayers);
            }
        }

        public List<Player> PlayersList
        {
            get { return m_PlayersList; }
        }

        public GameTime BaseGameTime
        {
            get { return m_GameTime; }
        }

        public SpriteBatch MainSpriteBatch
        {
            get { return m_SpriteBatch; }
        }

        // this.GraphicsDevice.Viewport.Bounds
        // this.Window.ClientBounds
        public Rectangle GameWindowBounds
        {
            get
            {
                return this.GraphicsDevice.Viewport.Bounds;
            }
        }

        protected override void Update(GameTime i_GameTime)
        {
            m_GameTime = i_GameTime;
            base.Update(i_GameTime);
        }

        // This method handles the game over process.
        // if ForceGameOver = true, we terminate the game immediately, else we check if all players are dead before terminating. 
        public void GameOver(bool i_ForceGameOver = false)
        {
            m_PlayerDeathCounter++;
            if (m_PlayerDeathCounter == PlayersList.Count || i_ForceGameOver)
            {
                OnGameEnd();
            }
        }

        private void OnGameEnd()
        {
            if (GameEnd != null)
            {
                GameEnd.Invoke();
            }
        }

        public void ResetGame()
        {
            for (int i = m_PlayersList.Count - 1; i >= 0; i--)
            {
                m_PlayersList.RemoveAt(i);
            }

            m_PlayerDeathCounter = 0;
        }
    }
}
