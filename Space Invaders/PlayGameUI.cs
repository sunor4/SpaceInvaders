using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.UIModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class PlayGameUI : CompositeDrawableComponent<IGameComponent>
    {
        private const float k_YSpacingBetweenElements = 20f;
        private Vector2 m_InitScorebarPosition = new Vector2(10f);
        private readonly List<LivesBar> r_LivesBarList = new List<LivesBar>();
        private readonly List<TextBlock> r_ScoreTextBlocksList = new List<TextBlock>();

        public List<Player> PlayersList
        {
            get { return (Game as BaseGame).PlayersList; }
        }

        public PlayGameUI(Game i_Game, params PlayerShip[] i_Players) : base(i_Game)
        {
            for (int i = 0; i <i_Players.Length; i++)
            {
                if (i_Players[i] != null)
                {
                    PlayersList.Add(i_Players[i]);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            initScoreBars();
            initLivesBars();
            addComponents();
        }

        protected override void CompositeDrawableComponent_ResolutionChanged()
        {
            resetLivesPositions();
        }

        private void initScoreBars()
        {
            for (int i = 0; i < PlayersList.Count; i++)
            {
                PlayerShip currentPlayer = PlayersList[i] as PlayerShip;
                currentPlayer.PlayerScore.ScoreStaticText = $"P{i+1} Score: ";
                currentPlayer.PlayerScore.Value = 0;
                currentPlayer.PlayerScore.ScoreTextBlockPosition =
                    m_InitScorebarPosition + new Vector2(0, i * k_YSpacingBetweenElements);
                r_ScoreTextBlocksList.Add(currentPlayer.PlayerScore.r_ScoreTextBlock);
            }
        }

        private void initLivesBars()
        {
            for (int i = 0; i < PlayersList.Count; i++)
            {
                PlayerShip currentPlayer = PlayersList[i] as PlayerShip;
                Vector2 currentLifeBarPosition = new Vector2(
                    (Game as BaseGame).GameWindowBounds.Width - currentPlayer.Bounds.Width,
                    (Game as BaseGame).GameWindowBounds.Y + 10 + (i * currentPlayer.Bounds.Height));
                currentPlayer.PlayerLivesBar.Position = currentLifeBarPosition;
                r_LivesBarList.Add(currentPlayer.PlayerLivesBar);
            }
        }

        private void resetLivesPositions()
        {
            for (int i = 0; i < PlayersList.Count; i++)
            {
                Vector2 currentLifeBarPosition = new Vector2(
                    (Game as BaseGame).GameWindowBounds.Width - PlayersList[i].Bounds.Width,
                    (Game as BaseGame).GameWindowBounds.Y + 10 + (i * PlayersList[i].Bounds.Height));
                PlayersList[i].PlayerLivesBar.Position = currentLifeBarPosition;
                PlayersList[i].PlayerLivesBar.ResetLivesPosition();
            }
        }

        private void addComponents()
        {
            foreach (TextBlock score in r_ScoreTextBlocksList)
            {
                this.Add(score);
            }

            foreach (LivesBar livesBar in r_LivesBarList)
            {
                this.Add(livesBar);
            }
        }
    }
}


/*
 *  SCORE SHOULD HOLD VALUE && COLOR.
 */