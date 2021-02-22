using System;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using SpaceInvaders;

namespace Infrastructure.ObjectModel.UIModel
{
    public class MenuButton : Button
    {
        private const float k_PulseAnimatorTargetScale = 1.2f;
        private const float k_PulseAnimatorPulsesPerSecond = 1f;
        private readonly Menu r_ParentMenu;
        
        public Menu ParentMenu
        {
            get { return r_ParentMenu; }
        }

        public MenuButton(Menu i_ParentMenu) : base(i_ParentMenu.Game)
        {
            r_ParentMenu = i_ParentMenu;
            ParentMenu.Add(this);
        }
        protected override void InitAnimations()
        {
            createPulseAnimator();
        }

        private void createPulseAnimator()
        {
            TimeSpan animationLength = TimeSpan.Zero;
            PulseAnimator pulseAnimator = new PulseAnimator(animationLength, k_PulseAnimatorTargetScale,
                k_PulseAnimatorPulsesPerSecond);
            this.Animations.Add(pulseAnimator);
            this.HighlightChanged += Animations_HighlightChanged;
        }

        private void Animations_HighlightChanged()
        {
            this.Animations.Reset();
            this.Animations.Enabled = !this.Animations.Enabled;
            if ((Game as BaseGame).CurrentActiveScreen != null && (Game as BaseGame).CurrentActiveScreen.Contains(ParentMenu))
            {
                (Game as BaseGame).SoundManager.Play(eGameSounds.MenuMove.ToString());
            }
        }
    }
}
