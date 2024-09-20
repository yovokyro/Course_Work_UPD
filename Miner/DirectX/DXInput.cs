using System;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Windows;

namespace Miner.DirectX
{
    /// <summary>
    /// Класс для захвата клавиатуры
    /// </summary>
    public class DXInput : IDisposable
    {
        //DirectInput
        private DirectInput _directInput;
        private Keyboard _keyboard;
        private KeyboardState _keyboardState;
        public KeyboardState KeyboardState { get => _keyboardState; }
        //состояние подключения клавиатуры
        private bool _keyboardConnect;
        public bool KeyboardConnect { get => _keyboardConnect; }

        public DXInput(RenderForm renderForm)
        {
            _directInput = new DirectInput();

            _keyboard = new Keyboard(_directInput);
            _keyboard.Properties.BufferSize = 16;
            _keyboard.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            ConnectKeyboard();
            _keyboardState = new KeyboardState();
        }

        /// <summary>
        /// Подключает клавиатуру
        /// </summary>
        private void ConnectKeyboard()
        {
            try
            {
                _keyboard.Acquire();
                _keyboardConnect = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    _keyboardConnect = false;
            }
        }

        /// <summary>
        /// Обновляет клавиатуру (подключает, если не было установлено соединения)
        /// </summary>
        public void UpdateKeyboard()
        {
            if (!_keyboardConnect) ConnectKeyboard();

            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                _keyboard.GetCurrentState(ref _keyboardState);
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
            }

            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _keyboardConnect = false;
        }

        /// <summary>
        /// Освобождение неуправляемых ресурсов
        /// </summary>
        public void Dispose()
        {
            Utilities.Dispose(ref _keyboard);
            Utilities.Dispose(ref _directInput);
        }
    }
}
