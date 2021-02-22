using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel.UIModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.ObjectModel
{
    public abstract class InputGameButton<T> : MenuButton
    {
        private Keys m_KeyboardInputForwardButton = Keys.PageUp;
        private Keys m_KeyboardInputBackButton = Keys.PageDown;
        private eInputButtons m_MouseInputForwardButton = eInputButtons.Right;
        private eInputButtons m_MouseInputBackButton = eInputButtons.Middle;
        private int m_CurrentInputIndex = 0;
        private readonly List<T> r_InputsList = new List<T>();
        public event Action InputChanged;

        public int MouseWheelValue
        {
            get { return InputManager.MouseWheelValue; }
        }

        public int CurrentInputIndex
        {
            get { return m_CurrentInputIndex; }
            set
            {
                value = (value < 0) ? this.InputList.Count + value : value % InputList.Count;
                if (value != m_CurrentInputIndex)
                {
                    m_CurrentInputIndex = value;
                    Text = r_InputsList[m_CurrentInputIndex].ToString();
                    OnInputChanged();
                }
            }
        }
        public List<T> InputList
        {
            get { return r_InputsList; }
        }

        public InputGameButton(Menu i_ParentMenu) : base (i_ParentMenu)
        {
            InputChanged += InputGameButton_InputChanged;
            SetValuesInList();
            SetInitialValue();
        }

        protected virtual void SetInitialValue()
        {
            if (r_InputsList.Count > 0)
            {
                Text = r_InputsList[0].ToString();
            }
        }

        protected abstract void SetValuesInList();

        protected virtual void InputGameButton_InputChanged()
        {
            
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (IsHighlighted)
            {
                if (InputManager.KeyPressed(m_KeyboardInputForwardButton) ||
                    InputManager.ButtonPressed(m_MouseInputForwardButton) ||
                    MouseWheelValue > 0)
                {
                    CurrentInputIndex++;
                }
                else if (InputManager.KeyPressed(m_KeyboardInputBackButton) ||
                         MouseWheelValue < 0)
                {
                    CurrentInputIndex--;
                }
            }
        }

        protected void OnInputChanged()
        {
            if (InputChanged != null)
            {
                InputChanged.Invoke();
            }
        }
    }
}
