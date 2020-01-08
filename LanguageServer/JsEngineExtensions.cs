using JavaScriptEngineSwitcher.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageServer
{
    public static class JsEngineExtensions
    {
        public static object TimedEvaluate(this IJsEngine engine, TimeSpan timeout,
            string expression)
        {
            if (engine.SupportsScriptInterruption)
            {
                using (var timer = new Timer(state => engine.Interrupt()))
                {
                    try
                    {
                        timer.Change(timeout, TimeSpan.FromMilliseconds(Timeout.Infinite));

                        return engine.Evaluate(expression);
                    }
                    catch (JsInterruptedException e)
                    {
                        throw new TimeoutException("Script execution timed out", e);
                    }
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("JavaScript engine with name " +
                    "'{0}' does not support interruption of the script execution.", engine.Name));
            }
        }

        public static void TimedExecute(this IJsEngine engine, TimeSpan timeout,
            string expression)
        {
            if (engine.SupportsScriptInterruption)
            {
                using (var timer = new Timer(state => engine.Interrupt()))
                {
                    try
                    {
                        timer.Change(timeout, TimeSpan.FromMilliseconds(Timeout.Infinite));

                        engine.Execute(expression);
                    }
                    catch (JsInterruptedException e)
                    {
                        throw new TimeoutException("Script execution timed out", e);
                    }
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("JavaScript engine with name " +
                    "'{0}' does not support interruption of the script execution.", engine.Name));
            }
        }
    }
}
