using System;

namespace Space_Invaders
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new SpaceInvaders())
            {
                game.Run();
            }
        }
    }
}


/*
 * 1. Change implementation of CompositeObjects to inherit from Guy Ronen.
 * 2. Debug and proceed if everything works.
 * 3. Lives bar under BG.
 * 4. Barrier non-premultiplied.
 * 5. Bullet logic - can get to state where we shoot only 1 bullet.
 * 6. Logic changes - creating matrix/barriers again causes problems.
 * 7. Shooter - add BulletType field to differentiate collision.
 * 8. SpriteBatch - if sprite not connected to composite component, we may encounter problems.
 * 9. BlendState.NPM is needed for opacity.
 * 10. TODO: Add implementation to PlayGameScreen such that it creates players according to num of players.
 * 11. GameUI - green/red.
 */