using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders.GameScreens
{
    public class GameOverScreen : GameScreen
    {
        private const bool v_IsTextCenterized = true;
        private const Keys k_StartGameKey = Keys.Home;
        private const Keys k_EscapeGameKey = Keys.Escape;
        private const Keys k_MainMenuKey = Keys.K;
        private const float k_SpacingBetweenTextBlocks = 40f; // In pixels
        private TextBlock m_GameOverTextBlock;
        private TextBlock m_ScoreTextBlock;
        private TextBlock m_InstructionsTextBlock;

        public GameOverScreen(Game i_Game) : base(i_Game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            createAndAddTextBlocks();
            (Game as BaseGame).ResetGame();
        }

        protected override void AddComponentsToUI()
        {
            this.GameUI.Add(new Background(Game));
        }

        private void createAndAddTextBlocks()
        {
            createAndAddGameOverTextBlock();
            createAndAddScoreTextBlock();
            createAndAddInstructionsTextBlock();
        }

        private void createAndAddGameOverTextBlock()
        {
            const string k_GameOverText = @"Game Over";
            Vector2 textPos = CenterOfViewport - new Vector2(0, 2f * k_SpacingBetweenTextBlocks);
            m_GameOverTextBlock = new TextBlock(Game, k_GameOverText, textPos, v_IsTextCenterized);
            m_GameOverTextBlock.Scales = new Vector2(2f);
            this.Add(m_GameOverTextBlock);
        }

        private void createAndAddScoreTextBlock()
        {
            m_ScoreTextBlock = new TextBlock(Game, v_IsTextCenterized);
            buildEndGameText();
            setScoreTextBlockPosition();
            this.Add(m_ScoreTextBlock);
        }

        private void setScoreTextBlockPosition()
        {
            Vector2 textPos = CenterOfViewport - new Vector2(0, 0.5f * k_SpacingBetweenTextBlocks);
            m_ScoreTextBlock.Position = textPos;
        }

        private void buildEndGameText()
        {
            StringBuilder gameOverMessage = new StringBuilder();
            for (int i = 0; i < (Game as BaseGame).PlayersList.Count; i++)
            {
                gameOverMessage.AppendLine($"Player {i + 1} score: {(Game as BaseGame).PlayersList[i].PlayerScore.Value}");
            }

            m_ScoreTextBlock.Text = gameOverMessage.ToString();
        }

        private void createAndAddInstructionsTextBlock()
        {
            string instructionText = $"1. Press {k_EscapeGameKey.ToString()} to exit.{Environment.NewLine}" +
                                     $"2. Press {k_StartGameKey.ToString()} to start game.{Environment.NewLine}" +
                                     $"3. Press {k_MainMenuKey.ToString()} to proceed to Main Menu.";
            Vector2 textPos = CenterOfViewport + new Vector2(0, 1f * k_SpacingBetweenTextBlocks);
            m_InstructionsTextBlock = new TextBlock(Game, instructionText, textPos, v_IsTextCenterized);
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
