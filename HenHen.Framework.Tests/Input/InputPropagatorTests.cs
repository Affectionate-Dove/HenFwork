﻿using HenHen.Framework.Input;
using NUnit.Framework;

namespace HenHen.Framework.Tests.Input
{
    public class InputPropagatorTests
    {
        private TestInputManager inputManager;
        private TestInputActionHandler inputActionHandler;

        [SetUp]
        public void SetUp()
        {
            inputManager = new TestInputManager();
            inputActionHandler = new TestInputActionHandler(inputManager);
        }

        [Test]
        public void BasicTest()
        {
            var listener = new TestInputListener();
            inputActionHandler.Propagator.Listeners.Add(listener);

            SimulateFewSteps();

            inputManager.SimulateKeyPress(KeyboardKey.KEY_A);
            SimulateFewSteps();

            Assert.IsTrue(listener.ReceivedPress);
            Assert.IsFalse(listener.ReceivedRelease);

            inputManager.SimulateKeyRelease(KeyboardKey.KEY_A);
            SimulateFewSteps();

            Assert.IsFalse(listener.ReceivedRelease);
            SimulateFewSteps();

            listener.HandledActions.Add(TestAction.Action1);

            inputManager.SimulateKeyPress(KeyboardKey.KEY_A);
            SimulateFewSteps();

            Assert.IsFalse(listener.ReceivedRelease);

            inputManager.SimulateKeyRelease(KeyboardKey.KEY_A);
            SimulateFewSteps();

            Assert.IsTrue(listener.ReceivedRelease);

            inputActionHandler.Propagator.Listeners.Remove(listener);
            SimulateFewSteps();
        }

        [Test]
        public void HierarchyTest()
        {
            var backgroundListener = new TestInputListener();
            var foregroundListener = new TestInputListener();
            foregroundListener.HandledActions.Add(TestAction.Action1);
            backgroundListener.HandledActions.Add(TestAction.Action2);
            inputActionHandler.Propagator.Listeners.Add(backgroundListener);
            inputActionHandler.Propagator.Listeners.Add(foregroundListener);

            SimulateFewSteps();

            inputManager.SimulateKeyPress(KeyboardKey.KEY_LEFT_CONTROL);
            SimulateFewSteps();
            inputManager.SimulateKeyPress(KeyboardKey.KEY_S);
            SimulateFewSteps();
            inputManager.SimulateKeyRelease(KeyboardKey.KEY_S);
            SimulateFewSteps();
            inputManager.SimulateKeyRelease(KeyboardKey.KEY_LEFT_CONTROL);
            SimulateFewSteps();

            Assert.IsTrue(foregroundListener.ReceivedPress);
            Assert.IsFalse(foregroundListener.ReceivedRelease);
            Assert.IsTrue(backgroundListener.ReceivedPress);
            Assert.IsTrue(backgroundListener.ReceivedRelease);
            backgroundListener.ResetFlags();
            foregroundListener.ResetFlags();

            inputManager.SimulateKeyPress(KeyboardKey.KEY_A);
            SimulateFewSteps();
            inputManager.SimulateKeyRelease(KeyboardKey.KEY_A);
            SimulateFewSteps();

            Assert.IsTrue(foregroundListener.ReceivedPress);
            Assert.IsTrue(foregroundListener.ReceivedRelease);
            Assert.IsFalse(backgroundListener.ReceivedPress);
            Assert.IsFalse(backgroundListener.ReceivedRelease);

            inputActionHandler.Propagator.Listeners.Remove(backgroundListener);
            inputActionHandler.Propagator.Listeners.Remove(foregroundListener);
            SimulateFewSteps();
        }

        [Test]
        public void NoDoubledActionsTest()
        {
            var listener = new TestInputListener();
            listener.HandledActions.Add(TestAction.Action1);
            inputActionHandler.Propagator.Listeners.Add(listener);

            SimulateFewSteps();

            inputManager.SimulateKeyPress(KeyboardKey.KEY_A);
            SimulateFewSteps();

            Assert.IsTrue(listener.ReceivedPress);
            Assert.IsFalse(listener.ReceivedRelease);
            listener.ResetFlags();

            inputManager.SimulateKeyPress(KeyboardKey.KEY_A);
            SimulateFewSteps();

            Assert.IsFalse(listener.ReceivedPress);
            Assert.IsFalse(listener.ReceivedRelease);

            inputManager.SimulateKeyRelease(KeyboardKey.KEY_A);
            SimulateFewSteps();

            Assert.IsFalse(listener.ReceivedPress);
            Assert.IsTrue(listener.ReceivedRelease);

            inputActionHandler.Propagator.Listeners.Remove(listener);
            SimulateFewSteps();
        }

        private void SimulateFewSteps()
        {
            for (var i = 0; i < 3; i++)
            {
                inputActionHandler.Update();
                inputManager.Update(1);
            }
        }
    }
}