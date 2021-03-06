﻿
using System;

namespace Draughts.Service
{
    public class GameException : Exception
    {
        public GameException() : base() { }

        public GameException(string message) : base(message) { }

        public GameException(string message, Exception innerException) : base(message, innerException) { }
    }
}
