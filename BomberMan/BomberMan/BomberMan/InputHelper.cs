using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BomberMan
{
    /// <summary>Вспомогательный клас по опросу мыши и клавы</summary>
    static class InputHelper
    {
        /// <summary>Текущее состояние мыши</summary>
        public static MouseState СurrentMouseState;

        /// <summary>Предыдущее состояние мыши</summary>
        public static MouseState PreviousMouseState;

        /// <summary>Позиция мыши на экране</summary>
        public static Vector2 MousePosition
        {
            get { return new Vector2(СurrentMouseState.X, СurrentMouseState.Y); }
        }

        /// <summary>Текущее состояние клавиатуры</summary>
        public static KeyboardState CurrentKeyboardState;

        /// <summary>Предыдущее состояние клавиатуры</summary>
        public static KeyboardState PreviousKeyboardState;

        /// <summary>Обновлене данных</summary>
        public static void Update()
        {
            PreviousMouseState = СurrentMouseState;
            PreviousKeyboardState = CurrentKeyboardState;
            СurrentMouseState = Mouse.GetState();
            CurrentKeyboardState = Keyboard.GetState();
        }

        /// <summary>Была ли нажата и отпущена лева клавиша мыши</summary>
        public static bool MouseLeftButtonPressed()
        {
            return СurrentMouseState.LeftButton == ButtonState.Pressed &&
                   PreviousMouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>Проверка тока на нажатие левой клавиши мыши</summary>
        public static bool MouseLeftButtonDown()
        {
            return СurrentMouseState.LeftButton == ButtonState.Pressed;
        }


        /// <summary>Была ли нажата и отпущена клавища</summary>
        public static bool KeyPressed(Keys k)
        {
            return CurrentKeyboardState.IsKeyDown(k) && PreviousKeyboardState.IsKeyUp(k);
        }

        /// <summary>Проверка только на нажатие клавиши</summary>
        public static bool IsKeyDown(Keys k)
        {
            return CurrentKeyboardState.IsKeyDown(k);
        }
    }
}
