﻿//-----------------------------------------------------------------------
    public abstract class RealtimeMessage
    {
        public string Type => this.GetType().Name;
    }