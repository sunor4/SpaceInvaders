using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    // This struct is a representation of the player controls, customized to this specific game (has references to
    // Left, Right and Shoot only).
    public struct PlayerControls
    {
        private Keys m_LeftKey;
        private Keys m_RightKey;
        private Keys m_ShootKey;

        public Keys LeftKey
        {
            get { return m_LeftKey; }
            set { m_LeftKey = value; }
        }

        public Keys RightKey
        {
            get { return m_RightKey; }
            set { m_RightKey = value; }
        }

        public Keys ShootKey
        {
            get { return m_ShootKey; }
            set { m_ShootKey = value; }
        }

        public PlayerControls(Keys i_LeftKey, Keys i_RightKey, Keys i_ShootKey)
        {
            m_LeftKey = i_LeftKey;
            m_RightKey = i_RightKey;
            m_ShootKey = i_ShootKey;
        }
    }
}
