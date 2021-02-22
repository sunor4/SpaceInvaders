using System;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpaceInvaders
{
    public class CompositeBarrier : CompositeDrawableComponent<Barrier>
    {
        private const int k_NumOfBarriers = 4;
        private const float k_DistanceBetweenBarriersMultiplier = 1.3f;
        private Vector2 m_VelocityMultiplier = Vector2.Zero;

        public Vector2 VelocityMultiplier
        {
            get { return m_VelocityMultiplier; }
            set { m_VelocityMultiplier = value; }
        }

        public CompositeBarrier(Game i_Game) : base(i_Game)
        {
            this.BlendState = BlendState.NonPremultiplied;
            this.SpritesSortMode = SpriteSortMode.Immediate;
        }

        public override void Initialize()
        {
            base.Initialize();
            initializeBarriers();
            setBarriersPositions();
        }

        private void initializeBarriers()
        {
            Barrier currentBarrier;
            for (int i = 0; i < k_NumOfBarriers; i++)
            {
                currentBarrier = new Barrier(Game);
                currentBarrier.Velocity *= VelocityMultiplier;
                this.Add(currentBarrier);
            }
        }

        private void setBarriersPositions()
        {
            Barrier defaultBarrier = Components[0];
            int startingYPosOfBarrier = (int)((Game as BaseGame).GameWindowBounds.Height - (3 * defaultBarrier.Height));
            float middleOfScreenX = this.CenterOfViewport.X;
            float distanceBetweenBarriers = defaultBarrier.Width * k_DistanceBetweenBarriersMultiplier;
            float startingXPosOfBarrier = middleOfScreenX - (k_NumOfBarriers / 2 * (int)(distanceBetweenBarriers + defaultBarrier.Width)) -
                                          (int)(distanceBetweenBarriers / 2);
            for (int i = 0; i < this.Count; i++)
            {
                float currentBarrierXPos = startingXPosOfBarrier + (2 * i * (int)distanceBetweenBarriers);
                Components[i].Position = new Vector2(currentBarrierXPos, startingYPosOfBarrier);
            }
        }

        public void Reset()
        {
            this.Clear();
        }
    }
}
