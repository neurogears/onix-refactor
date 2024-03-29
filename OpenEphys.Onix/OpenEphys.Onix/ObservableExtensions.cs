﻿using System;
using System.Reactive.Linq;

namespace OpenEphys.Onix
{
    internal static class ObservableExtensions
    {
        public static IObservable<ContextTask> ConfigureHost(this IObservable<ContextTask> source, Func<ContextTask, IDisposable> action)
        {
            return source.Do(context => context.ConfigureHost(action));
        }

        public static IObservable<ContextTask> ConfigureLink(this IObservable<ContextTask> source, Func<ContextTask, IDisposable> action)
        {
            return source.Do(context => context.ConfigureLink(action));
        }

        public static IObservable<ContextTask> ConfigureDevice(this IObservable<ContextTask> source, Func<ContextTask, IDisposable> selector)
        {
            return source.Do(context => context.ConfigureDevice(selector));
        }
    }
}
