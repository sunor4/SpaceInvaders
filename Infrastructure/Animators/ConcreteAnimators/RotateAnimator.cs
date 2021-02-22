using System;
using Infrastructure.ObjectModel.Animators;
using Microsoft.Xna.Framework;

namespace Infrastructure.Animators.ConcreteAnimators
{
    public class RotateAnimator : SpriteAnimator
    {
        private readonly float r_NumOfRotationsPerSecond;

        private float NumOfRotationsPerSecond
        {
            get { return r_NumOfRotationsPerSecond; }
        }

        public RotateAnimator(string i_Name, float i_NumOfRotationsPerSecond, TimeSpan i_AnimationLength) :
            base(i_Name, i_AnimationLength)
        {
            r_NumOfRotationsPerSecond = i_NumOfRotationsPerSecond;
        }

        public RotateAnimator(float i_NumOfRotationsPerSecond, TimeSpan i_AnimationLength) :
            this("Rotate", i_NumOfRotationsPerSecond, i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            // We change the rotate origin to the center of our source rectangle, then we scale.
            BoundSprite.RotationOrigin = BoundSprite.SourceRectangleCenter * BoundSprite.Scales;
            BoundSprite.Rotation += (float)(2 * Math.PI) * NumOfRotationsPerSecond *
                                    (float) i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Rotation = m_OriginalSpriteInfo.Rotation;
            this.BoundSprite.RotationOrigin = m_OriginalSpriteInfo.RotationOrigin;
        }
    }
}
