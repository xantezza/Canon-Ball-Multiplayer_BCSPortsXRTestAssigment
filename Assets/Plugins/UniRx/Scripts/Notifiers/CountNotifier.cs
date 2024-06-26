﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UniRx
{
    /// <summary>Event kind of CountNotifier.</summary>
    public enum CountChangedStatus
    {
        /// <summary>Count incremented.</summary>
        Increment,

        /// <summary>Count decremented.</summary>
        Decrement,

        /// <summary>Count is zero.</summary>
        Empty,

        /// <summary>Count arrived max.</summary>
        Max
    }

    /// <summary>
    ///     Notify event of count flag.
    /// </summary>
    public class CountNotifier : IObservable<CountChangedStatus>
    {
        private readonly object lockObject = new();
        private readonly Subject<CountChangedStatus> statusChanged = new();

        public int Max { get; }

        public int Count { get; private set; }

        /// <summary>
        ///     Setup max count of signal.
        /// </summary>
        public CountNotifier(int max = int.MaxValue)
        {
            if (max <= 0)
            {
                throw new ArgumentException("max");
            }

            this.Max = max;
        }

        /// <summary>
        ///     Increment count and notify status.
        /// </summary>
        public IDisposable Increment(int incrementCount = 1)
        {
            if (incrementCount < 0)
            {
                throw new ArgumentException("incrementCount");
            }

            lock (lockObject)
            {
                if (Count == Max) return Disposable.Empty;
                if (incrementCount + Count > Max) Count = Max;
                else Count += incrementCount;

                statusChanged.OnNext(CountChangedStatus.Increment);
                if (Count == Max) statusChanged.OnNext(CountChangedStatus.Max);

                return Disposable.Create(() => Decrement(incrementCount));
            }
        }

        /// <summary>
        ///     Decrement count and notify status.
        /// </summary>
        public void Decrement(int decrementCount = 1)
        {
            if (decrementCount < 0)
            {
                throw new ArgumentException("decrementCount");
            }

            lock (lockObject)
            {
                if (Count == 0) return;
                if (Count - decrementCount < 0) Count = 0;
                else Count -= decrementCount;

                statusChanged.OnNext(CountChangedStatus.Decrement);
                if (Count == 0) statusChanged.OnNext(CountChangedStatus.Empty);
            }
        }

        /// <summary>
        ///     Subscribe observer.
        /// </summary>
        public IDisposable Subscribe(IObserver<CountChangedStatus> observer)
        {
            return statusChanged.Subscribe(observer);
        }
    }
}