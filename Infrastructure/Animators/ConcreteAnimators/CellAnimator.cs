// *** Guy Ronen © 2008-2011 ***//
// We were given permission to use this file as a part of XNA Monogame summer course IDC 2020 //
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class CellAnimator : SpriteAnimator
    {  
        private readonly int r_NumOfCells = 1;
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrCellIdx = 0;
        
        // CTORs
        public CellAnimator(TimeSpan i_CellTime, int i_CurrentCellIndex, int i_NumOfCells, TimeSpan i_AnimationLength)
            : base("Cell Animator", i_AnimationLength)
        {
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            this.r_NumOfCells = i_NumOfCells;
            this.m_CurrCellIdx = i_CurrentCellIndex;

            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        private void goToNextFrame()
        {
            m_CurrCellIdx++;
            if (m_CurrCellIdx >= r_NumOfCells)
            {
                if (m_Loop)
                {
                    m_CurrCellIdx = 0;
                }
                else
                {
                    m_CurrCellIdx = r_NumOfCells - 1; /// lets stop at the last frame
                    this.IsFinished = true;
                }
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
                this.m_OriginalSpriteInfo.SourceRectangle.X +             
                (m_CurrCellIdx * this.BoundSprite.SourceRectangle.Width),
                this.BoundSprite.SourceRectangle.Top,
                this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Height);
        }

        // We use this method in order to change cell time according to change in Jump delay.
        public void SetCellTime(TimeSpan i_CellTime)
        {
            m_CellTime = m_TimeLeftForCell = i_CellTime;
        }
    }
}
