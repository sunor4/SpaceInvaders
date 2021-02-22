using System;
using Infrastructure.Managers;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders;
using SpaceInvaders.GameScreens;

namespace Space_Invaders
{
    public class SpaceInvaders : BaseGame
    {
        private const Keys k_MuteKey = Keys.M;
        private IInputManager m_InputManager;
        private ICollisionsManager m_CollisionsManager;

        public SpaceInvaders()
        {
            GameGraphicsDeviceManager = new GraphicsDeviceManager(this);
            m_ResolutionManager = new ResolutionManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.GameEnd += BaseGame_GameEnd;
            createGameEntities();
        }

        private void createGameEntities()
        {
            m_CollisionsManager = new CollisionsManager(this);
            m_InputManager = new InputManager(this);
            m_ScreensMananger = new ScreensMananger(this);
            m_SoundManager = new XactSoundManager(this, Enum.GetNames(typeof(eGameSounds)));
        }

        private void BaseGame_GameEnd()
        {
            // Close the play screen + level transition screen, then set the game over screen.
            m_ScreensMananger.ActiveScreen.ExitScreen();
            m_ScreensMananger.ActiveScreen.ExitScreen();
            m_ScreensMananger.SetCurrentScreen(new GameOverScreen(this));
            m_SoundManager.Play(eGameSounds.GameOver.ToString());
        }

        protected override void Initialize()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
            m_SoundManager.Play(eGameSounds.BGMusic.ToString());
            (m_ScreensMananger as ScreensMananger).Push(new MainMenuScreen(this));
            m_ScreensMananger.SetCurrentScreen(new WelcomeScreen(this));
        }

        protected override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (m_InputManager.KeyPressed(k_MuteKey))
            {
                (SoundManager as XactSoundManager).ToggleMuteIndirectly();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}