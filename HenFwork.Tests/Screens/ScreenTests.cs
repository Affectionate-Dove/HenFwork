﻿// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Screens;
using NUnit.Framework;
using System;

namespace HenFwork.Tests.Screens
{
    public class ScreenTests
    {
        [Test]
        public void TestEmptyScreenStack()
        {
            var screenStack = new ScreenStack();
            Assert.DoesNotThrow(() => screenStack.Update(0));
        }

        [Test]
        public void TestOneScreen()
        {
            var ss = new ScreenStack();
            Assert.AreEqual(null, ss.CurrentScreen);
            var screen = new Screen();
            ss.Push(screen);
            ss.Update(0);
            Assert.AreSame(screen, ss.CurrentScreen);
            ss.Pop();
            ss.Update(0);
            Assert.AreEqual(null, ss.CurrentScreen);
        }

        [Test]
        public void TestTwoScreens()
        {
            var ss = new ScreenStack();
            Assert.AreEqual(null, ss.CurrentScreen);
            var screen1 = new Screen();
            var screen2 = new Screen();

            ss.Push(screen1);
            ss.Update(0);
            Assert.AreSame(screen1, ss.CurrentScreen);

            screen1.Push(screen2);
            ss.Update(0);
            Assert.AreSame(screen2, ss.CurrentScreen);

            ss.Pop();
            ss.Update(0);
            Assert.AreSame(screen1, ss.CurrentScreen);

            ss.Pop();
            ss.Update(0);
            Assert.AreEqual(null, ss.CurrentScreen);

            Assert.Throws<InvalidOperationException>(ss.Pop);
        }
    }
}