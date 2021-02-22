using System;
using Infrastructure.ObjectModel.Animators;
using Microsoft.Xna.Framework;

namespace Infrastructure.Animators.ConcreteAnimators
{
    public class FadeAnimator : SpriteAnimator
    {
        private TimeSpan m_TotalElapsedTime = TimeSpan.Zero;

        // CTORs
        public FadeAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
        }

        public FadeAnimator(TimeSpan i_AnimationLength)
            : this("Fade", i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TotalElapsedTime += i_GameTime.ElapsedGameTime;
            BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity *
                                  (float)((AnimationLength.TotalSeconds - m_TotalElapsedTime.TotalSeconds) /
                                           AnimationLength.TotalSeconds);
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
        }
    }
}
