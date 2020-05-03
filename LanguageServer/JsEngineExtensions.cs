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

        public static void TimedFn<T>(this T engine, TimeSpan timeout, Action<T> timedEngineAction) where T : IJsEngine
        {
            if (engine.SupportsScriptInterruption)
            {
                using (var timer = new Timer(state => engine.Interrupt()))
                {
                    try
                    {
                        timer.Change(timeout, TimeSpan.FromMilliseconds(Timeout.Infinite));

                        timedEngineAction(engine);
                    }
                    catch (JsInterruptedException e)
                    {
                        throw new TimeoutException("Engine execution timed out", e);
                    }
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("JavaScript engine with name " +
                    "'{0}' does not support interruption of the script execution.", engine.Name));
            }
        }

        public static R TimedFn<T, R>(this T engine, TimeSpan timeout, Func<T, R> timedEngineAction) where T : IJsEngine
        {
            if (engine.SupportsScriptInterruption)
            {
                using (var timer = new Timer(state => engine.Interrupt()))
                {
                    try
                    {
                        timer.Change(timeout, TimeSpan.FromMilliseconds(Timeout.Infinite));

                        return timedEngineAction(engine);
                    }
                    catch (JsInterruptedException e)
                    {
                        throw new TimeoutException("Engine execution timed out", e);
                    }
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("JavaScript engine with name " +
                    "'{0}' does not support interruption of the script execution.", engine.Name));
            }
        }

        public static object TimedEvaluate<T>(this T engine, TimeSpan timeout, string expression) where T : IJsEngine => engine.TimedFn(timeout, (e) => e.Evaluate(expression));

        public static void TimedExecute<T>(this T engine, TimeSpan timeout, string expression) where T : IJsEngine => engine.TimedFn(timeout, (e) => e.Execute(expression));

}
}