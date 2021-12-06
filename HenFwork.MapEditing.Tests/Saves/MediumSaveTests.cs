// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.MapEditing.Saves;
using HenFwork.Worlds.Functional.Mediums;
using NUnit.Framework;

namespace HenFwork.MapEditing.Tests.Saves
{
    [TestOf(typeof(MediumSave))]
    public class MediumSaveTests
    {
        private Triangle3 triangle;
        private MediumType mediumType;
        private MediumSave mediumSave;

        [SetUp]
        public void SetUp()
        {
            triangle = new Triangle3(new(-2.4f, 0, 1.1123f), new(4.1f, 2.1f, 4.5f), new(1, 2, 4));
            mediumType = MediumType.Air;
            mediumSave = new MediumSave(triangle, mediumType);
        }

        [Test]
        public void PropertiesInitializationTest() => ValidateProperties(mediumSave);

        [Test]
        public void SerializationDeserializationTest()
        {
            var deserializedMediumSave = new MediumSave(mediumSave.ToStringData());
            Assert.AreEqual(mediumSave, deserializedMediumSave);
            ValidateProperties(deserializedMediumSave);
        }

        private void ValidateProperties(MediumSave mediumSave)
        {
            Assert.AreEqual(triangle, mediumSave.Triangle);
            Assert.AreEqual(mediumType, mediumSave.Type);
        }
    }
}