using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders.GameScreens
{
    public class PauseScreen : GameScreen
    {
        private const Keys k_ResumeKey = Keys.R;
        private const float k_BlackTintAlphaValue = 0.4f;
        private TextBlock m_PauseTextBlock;

        public PauseScreen(Game i_Game) : base(i_Game)
        {
            setProperties();
        }

        private void setProperties()
        {
            this.IsModal = true;
            this.IsOverlayed = true;
            this.UseGradientBackground = false;
            this.BlackTintAlpha = k_BlackTintAlphaValue;
            this.UseFadeTransition = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            setText();
            setPosition();
            this.Add(m_PauseTextBlock);
        }

        private void setPosition()
        {
            // Set "position origin" to middle of the text, and set Position of the text to middle of the screen.
            m_PauseTextBlock.Position = this.CenterOfViewport;
        }

        private void setText()
        {
            const bool v_IsTextCentered = true;
            const float k_TextScalesMultiplier = 1.2f;
            string pauseText = $"[Game Paused]{Environment.NewLine}Press R to continue...";
            m_PauseTextBlock = new TextBlock(Game, pauseText, v_IsTextCentered);
            m_PauseTextBlock.Scales = new Vector2(k_TextScalesMultiplier);
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(k_ResumeKey))
            {
                this.ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
