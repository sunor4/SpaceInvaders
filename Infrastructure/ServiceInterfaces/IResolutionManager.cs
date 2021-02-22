using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ServiceInterfaces
{
    public interface IResolutionManager
    {
        public event Action ResolutionChanged;

        public Vector2 ScaledResolutionMultiplier { get; }

        public Matrix TransformMatrix { get; }
        public Vector2 MouseScale { get; }
        public void ToggleFullScreen();
        public void ToggleResizeScreen();
    }
}
