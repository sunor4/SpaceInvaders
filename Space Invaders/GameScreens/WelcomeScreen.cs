using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace SpaceInvaders.GameScreens
{
    public class WelcomeScreen : GameScreen
    {
        private const Keys k_StartGameKey = Keys.Enter;
        private const Keys k_EscapeGameKey = Keys.Escape;
        private const Keys k_MainMenuKey = Keys.K;
        private const float k_SpacingBetweenTextBlocks = 40f; // In pixels
        private TextBlock m_WelcomeTextBlock;
        private TextBlock m_InstructionsTextBlock;

        public WelcomeScreen(Game i_Game) : base(i_Game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            createAndAddTextBlocks();
        }

        protected override void AddComponentsToUI()
        {
            this.GameUI.Add(new Background(Game));
        }

        private void createAndAddTextBlocks()
        {
            createAndAddWelcomeTextBlock();
            createAndAddInstructionsTextBlock();
        }

        private void createAndAddWelcomeTextBlock()
        {
            const string k_WelcomeText = @"Welcome to Space Invaders!";
            Vector2 textPos = CenterOfViewport - new Vector2(0,k_SpacingBetweenTextBlocks);
            m_WelcomeTextBlock = new TextBlock(Game, k_WelcomeText, textPos);
            m_WelcomeTextBlock.Initialize();
            m_WelcomeTextBlock.Scales = new Vector2(2f);
            m_WelcomeTextBlock.PositionOrigin = m_WelcomeTextBlock.TextCenter;
            this.Add(m_WelcomeTextBlock);
        }

        private void createAndAddInstructionsTextBlock()
        {
            string instructionText = $"1. Press {k_StartGameKey.ToString()} to start game.{Environment.NewLine}" +
                                     $"2. Press {k_EscapeGameKey.ToString()} to exit.{Environment.NewLine}" +
                                     $"3. Press {k_MainMenuKey.ToString()} to proceed to Main Menu.";
            Vector2 textPos = CenterOfViewport + new Vector2(0, k_SpacingBetweenTextBlocks);
            m_InstructionsTextBlock = new TextBlock(Game, instructionText, textPos);
            m_InstructionsTextBlock.Initialize();
            m_InstructionsTextBlock.PositionOrigin = m_InstructionsTextBlock.TextCenter;
            this.Add(m_InstructionsTextBlock);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState pressedKey = InputManager.KeyboardState;
            if (pressedKey.IsKeyDown(k_EscapeGameKey))
            {
                exitGame();
            }
            else if (pressedKey.IsKeyDown(k_StartGameKey))
            {
                startGame();
            }
            else if (pressedKey.IsKeyDown(k_MainMenuKey))
            {
                goToMainMenu();
            }
        }

        private void exitGame()
        {
            Game.Exit();
        }

        private void startGame()
        {
            ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
        }

        private void goToMainMenu()
        {
            this.ExitScreen();
        }
    }
}
