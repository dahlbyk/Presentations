using System;

namespace AtoV.Loggers
{
    public abstract class AbstractLogger
    {
        public abstract void Log(string msg);

        public virtual void Log(string msg, string category)
        {
            Log(category + ": " + msg);
        }

        public void LogFormat(string format, params object[] args)
        {
            Log(string.Format(format, args));
        }

        public void LogFormat(string category, string format, params object[] args)
        {
            Log(string.Format(format, args), category);
        }
    }
}
