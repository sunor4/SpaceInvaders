using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public class Score
    {
        private int m_Value;
        private Color m_ScoreColor;
        public readonly TextBlock r_ScoreTextBlock;
        public event Action ScoreChanged;

        public string ScoreStaticText
        {
            get { return r_ScoreTextBlock.StaticText; }
            set { r_ScoreTextBlock.StaticText = value; }
        }

        public Vector2 ScoreTextBlockPosition
        {
            get { return r_ScoreTextBlock.Position; }
            set { r_ScoreTextBlock.Position = value; }
        }

        public Color ScoreColor
        {
            get { return m_ScoreColor; }
            set
            {
                if (value != m_ScoreColor)
                {
                    m_ScoreColor = value;
                    OnScoreColorChange();
                }
            }
        }

        public int Value
        {
            get { return m_Value; }
            set
            {
                if (value <= 0)
                {
                    m_Value = 0;
                }
                else if (value != m_Value)
                {
                    m_Value = value;
                }

                OnScoreChanged();
            }
        }

        public Score(Game i_Game)
        {
            r_ScoreTextBlock = new TextBlock(i_Game);
        }

        protected void OnScoreColorChange()
        {
            this.r_ScoreTextBlock.TintColor = ScoreColor;
        }

        protected void OnScoreChanged()
        {
            this.r_ScoreTextBlock.Text = this.Value.ToString();

            if (ScoreChanged != null)
            {
                ScoreChanged.Invoke();
            }
        }
    }
}
