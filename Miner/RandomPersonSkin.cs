using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Miner
{
    /// <summary>
    /// Класс случайно возвращает директорию со спрайтом персонажа
    /// </summary>
    public class RandomPersonSkin
    {
        private Regex _regex;
        private string[] _directory;
        private Random _rd;
        private List<int> _busy;
        private bool _check;
        public RandomPersonSkin()
        {
            _rd = new Random();
            _busy = new List<int>();
            _regex = new Regex(@"player\d");
            _directory = Directory.GetDirectories(@"..\..\..\Resources\players");
            _check = GetCheckDirectory();
        }

        /// <summary>
        /// Возвращает статус наличия двух директорий со спрайтами персонажа
        /// </summary>
        /// <returns>Возвращает статус наличия двух директорий со спрайтами персонажа</returns>
        private bool GetCheckDirectory()
        {
            List<string> directoryValid = new List<string>();

            if (_directory.Length >= 2)
            {
                for (int i = 0; i < _directory.Length; i++)
                    if (_regex.IsMatch(_directory[i]))
                        directoryValid.Add(_directory[i]);

                if (directoryValid.Count >= 2)
                {
                    _directory = new string[directoryValid.Count];
                    directoryValid.CopyTo(_directory, 0);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Возвращает случайную уникальную директорию со спрайтом персонажа
        /// </summary>
        /// <returns>Возвращает случайную уникальную директорию со спрайтом персонажа</returns>
        public string GetDirectory()
        {
            if (_check)
            {
                int index = _rd.Next(_directory.Length);

                for (int i = 0; i < _busy.Count; i++)
                    if (index == _busy[i])
                        return GetDirectory();

                _busy.Add(index);
                return _directory[index];
            }

            return "";
        }

        /// <summary>
        /// Возвращает случайную уникальную директорию со спрайтом персонажа
        /// </summary>
        /// <returns>Возвращает случайную уникальную директорию со спрайтом персонажа</returns>
        public string GetDirectory(int min, int max)
        {
            if (_check)
            {
                int index = _rd.Next(min, max);

                for (int i = 0; i < _busy.Count; i++)
                    if (index == _busy[i])
                        return GetDirectory();

                _busy.Add(index);
                return _directory[index];
            }

            return "";
        }
    }
}
