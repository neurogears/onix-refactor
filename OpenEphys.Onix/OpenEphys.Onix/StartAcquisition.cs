﻿using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Bonsai;

namespace OpenEphys.Onix
{
    public class StartAcquisition : Combinator<ContextTask, oni.Frame>
    {
        public int ReadSize { get; set; } = 28;

        public int WriteSize { get; set; } = 2048;

        public override IObservable<oni.Frame> Process(IObservable<ContextTask> source)
        {
            return source.SelectMany(context =>
            {
                return Observable.Create<oni.Frame>(observer =>
                {
                    context.Reset();
                    context.BlockReadSize = ReadSize;
                    context.BlockWriteSize = WriteSize;
                    var frameSubscription = context.FrameReceived.SubscribeSafe(observer);
                    context.Start();
                    return Disposable.Create(() =>
                    {
                        context.Stop();
                        frameSubscription.Dispose();
                    });
                });
            });
        }
    }
}
