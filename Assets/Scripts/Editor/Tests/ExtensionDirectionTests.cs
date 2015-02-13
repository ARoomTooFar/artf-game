using UnityEngine;
using System.Collections;

using NUnit.Framework;

[TestFixture]
public class ExtensionDirectionTests{
	[Test]
	public void OppositeTest(){
		Assert.AreEqual(DIRECTION.North, DIRECTION.South.Opposite());
		Assert.AreEqual(DIRECTION.South, DIRECTION.North.Opposite());
		Assert.AreEqual(DIRECTION.East, DIRECTION.West.Opposite());
		Assert.AreEqual(DIRECTION.West, DIRECTION.East.Opposite());

		Assert.AreEqual(DIRECTION.NorthWest, DIRECTION.SouthEast.Opposite());
		Assert.AreEqual(DIRECTION.NorthEast, DIRECTION.SouthWest.Opposite());
		Assert.AreEqual(DIRECTION.SouthEast, DIRECTION.NorthWest.Opposite());
		Assert.AreEqual(DIRECTION.SouthWest, DIRECTION.NorthEast.Opposite());

		Assert.AreEqual(DIRECTION.NonDirectional, DIRECTION.NonDirectional.Opposite());
	}

	[Test]
	public void CardinalTest(){
		Assert.IsTrue(DIRECTION.North.isCardinal());
		Assert.IsTrue(DIRECTION.South.isCardinal());
		Assert.IsTrue(DIRECTION.East.isCardinal());
		Assert.IsTrue(DIRECTION.West.isCardinal());

		Assert.IsFalse(DIRECTION.NorthEast.isCardinal());
		Assert.IsFalse(DIRECTION.NorthWest.isCardinal());
		Assert.IsFalse(DIRECTION.SouthEast.isCardinal());
		Assert.IsFalse(DIRECTION.SouthWest.isCardinal());

		Assert.IsFalse(DIRECTION.NonDirectional.isCardinal());
	}

	[Test]
	public void OrdinalTest(){
		Assert.IsFalse(DIRECTION.North.isOrdinal());
		Assert.IsFalse(DIRECTION.South.isOrdinal());
		Assert.IsFalse(DIRECTION.East.isOrdinal());
		Assert.IsFalse(DIRECTION.West.isOrdinal());
		
		Assert.IsTrue(DIRECTION.NorthEast.isOrdinal());
		Assert.IsTrue(DIRECTION.NorthWest.isOrdinal());
		Assert.IsTrue(DIRECTION.SouthEast.isOrdinal());
		Assert.IsTrue(DIRECTION.SouthWest.isOrdinal());
		
		Assert.IsFalse(DIRECTION.NonDirectional.isOrdinal());
	}
}
