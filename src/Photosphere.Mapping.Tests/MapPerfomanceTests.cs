﻿using System;
using System.Diagnostics;
using Photosphere.Mapping.Extensions;
using Photosphere.Mapping.Tests.TestClasses;
using Xunit;
using Xunit.Abstractions;

namespace Photosphere.Mapping.Tests
{
    public class MapPerfomanceTests
    {
        private readonly ITestOutputHelper _output;

        public MapPerfomanceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        internal void MeasurePerfomanceAndCompareWithNativeMapping()
        {
            PerformMeasuring(1000);
            PerformMeasuring(2000);
            PerformMeasuring(3000);
            PerformMeasuring(4000);
            PerformMeasuring(5000);
            PerformMeasuring(6000);
            PerformMeasuring(7000);
            PerformMeasuring(8000);
            PerformMeasuring(9000);
            PerformMeasuring(10000);
            Assert.True(true);
        }

        private void PerformMeasuring(int timesOfMeasure)
        {
            var result = new MeasureResult
            {
                Mapping = RepeatAndAccumulateMilliseconds(timesOfMeasure, (source, target) => source.MapTo(target)),
                Native = RepeatAndAccumulateMilliseconds(timesOfMeasure, LargeFoo.NativeMap)
            };
            _output.WriteLine($"Times of mapping: {timesOfMeasure}\n" + result);
        }

        private static long RepeatAndAccumulateMilliseconds(int timesToRepeat, Action<LargeFoo, LargeFoo> action)
        {
            var index = 0;
            var stopWatch = new Stopwatch();
            while (index < timesToRepeat)
            {
                var source = LargeFoo.GetRandomNew();
                var target = new LargeFoo();

                stopWatch.Start();
                action(source, target);
                stopWatch.Stop();

                index++;
            }
            return stopWatch.ElapsedMilliseconds;
        }

        private class MeasureResult
        {
            public long Mapping { get; set; }

            public long Native { get; set; }

            public override string ToString()
            {
                return $"\n{nameof(Mapping)}:\t{Mapping}\n{nameof(Native)}:\t\t{Native}\n";
            }
        }
    }
}