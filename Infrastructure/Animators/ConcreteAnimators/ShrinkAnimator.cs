using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Animators;

namespace Infrastructure.Animators.ConcreteAnimators
{
    public class ShrinkAnimator : SpriteAnimator
    {
        private readonly Vector2 r_ShrinkSpeedPerSecond;

        private Vector2 ShrinkSpeedPerSecond
        {
            get { return r_ShrinkSpeedPerSecond; }
        }

        public ShrinkAnimator(string i_Name, TimeSpan i_AnimationLength) : 
            base(i_Name, i_AnimationLength)
        {
            r_ShrinkSpeedPerSecond = new Vector2(1f / (float)i_AnimationLength.TotalSeconds);
        }

        public ShrinkAnimator(TimeSpan i_AnimationLength) :
            this("Shrink", i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            // We change our position origin towards the center in order to give an illusion of shrink to center.
            BoundSprite.PositionOrigin -= ShrinkSpeedPerSecond * (float) i_GameTime.ElapsedGameTime.TotalSeconds *
                                          BoundSprite.SourceRectangleCenter;
            BoundSprite.Scales -= ShrinkSpeedPerSecond * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = m_OriginalSpriteInfo.Scales;
            this.BoundSprite.PositionOrigin = m_OriginalSpriteInfo.PositionOrigin;
        }
    }
}
