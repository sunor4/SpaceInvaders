using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpaceInvaders.GameScreens
{
    public class LevelTransitionScreen : GameScreen
    {
        private const bool v_IsTextCentered = true;
        private const string k_LevelTextBlockStaticString = @"Level: ";
        private const float k_SpacingBetweenTextBlocks = 40f;
        private const int k_NumOfSecondsUntilGameStart = 3;
        private PlayGameScreen m_MainGameScreen; // Holds the level index.
        private TextBlock m_SecondsCounterTextBlock;
        private TextBlock m_LevelTextBlock;
        private TimeSpan m_TotalScreenTime = TimeSpan.FromSeconds(k_NumOfSecondsUntilGameStart);
        private TimeSpan m_TimeToChangeDigit = TimeSpan.FromSeconds(1);
        private TimeSpan m_CurrentTimeToChangeDigit = TimeSpan.Zero;
        private int m_DigitToShow = k_NumOfSecondsUntilGameStart;

        public PlayGameScreen MainGameScreen
        {
            get { return m_MainGameScreen; }
            private set { m_MainGameScreen = value; }
        }

        public LevelTransitionScreen(Game i_Game) : base(i_Game)
        {
            m_MainGameScreen = new PlayGameScreen(Game);
            m_SecondsCounterTextBlock = new TextBlock(Game, m_DigitToShow.ToString(), v_IsTextCentered);
            m_LevelTextBlock = new TextBlock(Game, v_IsTextCentered);
            this.Add(m_SecondsCounterTextBlock);
            this.Add(m_LevelTextBlock);
        }

        public override void Initialize()
        {
            base.Initialize();
            setTextBlocksProperties();
        }

        protected override void CompositeDrawableComponent_ResolutionChanged()
        {
            Vector2 spacingBetweenBlocks = new Vector2(0, k_SpacingBetweenTextBlocks);
            m_SecondsCounterTextBlock.Position = CenterOfViewport + spacingBetweenBlocks;
            m_LevelTextBlock.Position = CenterOfViewport - spacingBetweenBlocks;
        }

        protected override void AddComponentsToUI()
        {
            this.GameUI.Add(new Background(Game));
        }

        private void setTextBlocksProperties()
        {
            Vector2 spacingBetweenBlocks = new Vector2(0, k_SpacingBetweenTextBlocks);

            // Set SecondsCounterTextBlock's properties:
            m_SecondsCounterTextBlock.Scales = new Vector2(1.72f);
            m_SecondsCounterTextBlock.Position = CenterOfViewport + spacingBetweenBlocks;

            // Set LevelTextBlock's properties:
            m_LevelTextBlock.Position = CenterOfViewport - spacingBetweenBlocks;
            m_LevelTextBlock.StaticText = k_LevelTextBlockStaticString;
            m_LevelTextBlock.Scales = new Vector2(2f);
            m_LevelTextBlock.Text = (MainGameScreen.CurrentLevel + 1).ToString();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_CurrentTimeToChangeDigit += i_GameTime.ElapsedGameTime;
            m_TotalScreenTime -= i_GameTime.ElapsedGameTime;
            if (m_CurrentTimeToChangeDigit >= m_TimeToChangeDigit &&
                m_TotalScreenTime > TimeSpan.Zero)
            {
                tickCountdownClock();
            }
            else if (m_TotalScreenTime <= TimeSpan.Zero)
            {
                goForNextScreen();
            }
        }

        private void tickCountdownClock()
        {
            m_CurrentTimeToChangeDigit -= m_TimeToChangeDigit;
            m_DigitToShow -= m_TimeToChangeDigit.Seconds;
            m_SecondsCounterTextBlock.Text = m_DigitToShow.ToString();
        }

        private void goForNextScreen()
        {
            this.ScreensManager.SetCurrentScreen(MainGameScreen);
            resetUI();
        }

        private void resetUI()
        {
            // Update Timespans for next countdown:
            m_TotalScreenTime = TimeSpan.FromSeconds(k_NumOfSecondsUntilGameStart);
            m_CurrentTimeToChangeDigit = TimeSpan.Zero;

            // Update text of new level: (the reset happens at the END of level transition, BEFORE update of new level)
            int nextLevelTextRepresentation = (MainGameScreen.CurrentLevel + 1) + 1;
            m_LevelTextBlock.Text = nextLevelTextRepresentation.ToString();

            // Reset current time text for next countdown:
            m_DigitToShow = k_NumOfSecondsUntilGameStart;
            m_SecondsCounterTextBlock.Text = m_DigitToShow.ToString();
        }
    }
}
