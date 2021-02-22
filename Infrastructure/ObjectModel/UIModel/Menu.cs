using System;
using System.Collections.Generic;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Infrastructure.ObjectModel.UIModel
{
    public class Menu : CompositeDrawableComponent<Button>
    {
        private const Keys k_NavigationUpKey = Keys.Up;
        private const Keys k_NavigationDownKey = Keys.Down;
        private const float k_HeadlineScaleMultiplier = 2f;
        private const float k_ButtonsTextScaleMultiplier = 1f;
        private const float k_SpaceBetweenButtonsInYAxis = 20f;
        private IInputManager m_InputManager;
        private Vector2 m_InitialPosition;
        private TextBlock m_HeadlineTextBlock;
        private readonly List<Button> r_ButtonsCollection = new List<Button>();
        private int m_CurrentHighlightedButtonIndex = 0;

        public int CurrentHighlightedButtonIndex
        {
            get { return m_CurrentHighlightedButtonIndex; }
            set
            {
                if (value < 0)
                {
                    value = ButtonsCollection.Count - 1;
                }
                else
                {
                    value = value % ButtonsCollection.Count;
                }

                if (value != m_CurrentHighlightedButtonIndex)
                {
                    ButtonsCollection[m_CurrentHighlightedButtonIndex].IsHighlighted = false;
                    m_CurrentHighlightedButtonIndex = value;
                    ButtonsCollection[m_CurrentHighlightedButtonIndex].IsHighlighted = true;
                }
            }
        }

        public List<Button> ButtonsCollection
        {
            get { return r_ButtonsCollection; }
        }

        public string HeadlineText
        {
            get { return m_HeadlineTextBlock.Text; }
            set { m_HeadlineTextBlock.Text = value; }
        }

        public Vector2 InitialPosition
        {
            get { return m_InitialPosition; }
            set { m_InitialPosition = value; }
        }
        
        public Menu(Game i_Game, string i_HeadlineText, Vector2 i_InitialPosition, params Button[] i_Buttons) : base(i_Game)
        {
            m_HeadlineTextBlock = new TextBlock(Game, i_HeadlineText);
            this.m_Sprites.Add(m_HeadlineTextBlock);
            m_InitialPosition = i_InitialPosition;
        }

        public Menu(Game i_Game, string i_HeadlineText, params Button[] i_Buttons) : 
            this(i_Game,  i_HeadlineText, Vector2.Zero, i_Buttons)
        {

        }

        public Menu(Game i_Game, params Button[] i_Buttons) : 
            this(i_Game, string.Empty, Vector2.Zero, i_Buttons)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            initHeadline();
            setButtonsPositions();
            initButtonsList();
        }

        private void initButtonsList()
        {
            foreach (Button button in Components)
            {
                ButtonsCollection.Add(button);
                button.Highlighted += Button_Highlighted;
            }

            if (ButtonsCollection.Count > 0)
            {
                ButtonsCollection[m_CurrentHighlightedButtonIndex].IsHighlighted = true;
            }
        }

        private void Button_Highlighted(object sender, EventArgs e)
        {
            int indexOfButton = ButtonsCollection.IndexOf(sender as Button);
            CurrentHighlightedButtonIndex = indexOfButton;
        }

        private void initHeadline()
        {
            m_HeadlineTextBlock.Initialize();
            m_HeadlineTextBlock.Scales = new Vector2(k_HeadlineScaleMultiplier);
            m_HeadlineTextBlock.PositionOrigin = m_HeadlineTextBlock.TextCenter;
        }

        private void setButtonsPositions()
        {
            bool isHeadlinePresent = (HeadlineText != null);
            m_HeadlineTextBlock.Position = m_InitialPosition;
            Vector2 currentButtonsPosition = isHeadlinePresent
                ? new Vector2(m_InitialPosition.X,
                    m_InitialPosition.Y + m_HeadlineTextBlock.Bounds.Height)
                : m_InitialPosition;
            foreach (Button button in this.Components)
            {
                button.Position = currentButtonsPosition;
                float currentButtonHeight = button.Bounds.Height;
                currentButtonsPosition.Y += currentButtonHeight + k_SpaceBetweenButtonsInYAxis;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_InputManager.KeyPressed(k_NavigationDownKey))
            {
                CurrentHighlightedButtonIndex++;
            }
            else if (m_InputManager.KeyPressed(k_NavigationUpKey))
            {
                CurrentHighlightedButtonIndex--;
            }
        }
    }
}
