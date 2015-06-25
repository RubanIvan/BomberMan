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

        /// <summary>список нажатых клавиш в порядке их нажатия</summary>
        public static List<Keys> KeyStatePrioryti=new List<Keys>(); 

        /// <summary>Обновлене данных</summary>
        public static void Update()
        {
            PreviousMouseState = СurrentMouseState;
            PreviousKeyboardState = CurrentKeyboardState;
            СurrentMouseState = Mouse.GetState();
            CurrentKeyboardState = Keyboard.GetState();

            if (CurrentKeyboardState.IsKeyDown(Keys.Left)){ AddKeyToList(Keys.Left);}
            if (CurrentKeyboardState.IsKeyDown(Keys.Right)) { AddKeyToList(Keys.Right); }
            if (CurrentKeyboardState.IsKeyDown(Keys.Up)) { AddKeyToList(Keys.Up); }
            if (CurrentKeyboardState.IsKeyDown(Keys.Down)) { AddKeyToList(Keys.Down); }

            if (CurrentKeyboardState.IsKeyUp(Keys.Left)) { RemoveKeyFromList(Keys.Left); }
            if (CurrentKeyboardState.IsKeyUp(Keys.Right)) { RemoveKeyFromList(Keys.Right); }
            if (CurrentKeyboardState.IsKeyUp(Keys.Up)) { RemoveKeyFromList(Keys.Up); }
            if (CurrentKeyboardState.IsKeyUp(Keys.Down)) { RemoveKeyFromList(Keys.Down); }

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


        private static void AddKeyToList(Keys k)
        {
            if(!KeyStatePrioryti.Contains(k)) KeyStatePrioryti.Add(k);
        }

        private static void RemoveKeyFromList(Keys k)
        {
            if (KeyStatePrioryti.Contains(k)) KeyStatePrioryti.Remove(k);
        }

    }
}
