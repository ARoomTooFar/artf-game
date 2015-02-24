using UnityEngine;
using System.Collections;

using NUnit.Framework;

[TestFixture]
public class ExtensionVector3Tests{
	[Test]
	public void RoundTest(){
		Assert.AreEqual(new Vector3(0f, 0f, 0f), new Vector3(.4f, -.4f, 0f).Round());
		Assert.AreEqual(new Vector3(1f, 1f, 1f), new Vector3(.51f, 1.4999f, 1f).Round());
	}

	[Test]
	public void toCSVTest(){
		Assert.AreEqual("0,0,0", new Vector3().toCSV());
		Assert.AreEqual("0.5,0.5,0.5", new Vector3(.5f,.5f,.5f).toCSV());
		Assert.AreEqual("1,-1,0", new Vector3(1, -1, 0).toCSV());
		Assert.AreEqual("1,-1,0", new Vector3(1.0f, -1.0f, 0.0f).toCSV());
	}

	[Test]
	public void RotateToTest(){
		Vector3 vec = new Vector3(0, 0, 1);
		Assert.Throws<UnityException>(() => vec.RotateTo(DIRECTION.NorthEast));
		Assert.Throws<UnityException>(() => vec.RotateTo(DIRECTION.NonDirectional));
		Assert.AreEqual(vec, vec.RotateTo(DIRECTION.North));
		Assert.AreEqual(new Vector3(1,0,0), vec.RotateTo(DIRECTION.East));
		Assert.AreEqual(new Vector3(0,0,-1), vec.RotateTo(DIRECTION.South));
		Assert.AreEqual(new Vector3(-1,0,0), vec.RotateTo(DIRECTION.West));
		Assert.AreEqual(new Vector3(0,0,1), vec.RotateTo(DIRECTION.North));
		vec = new Vector3(2, 0, 2);
		Assert.AreEqual(new Vector3(2,0,-2), vec.RotateTo(DIRECTION.East));
	}

	[Test]
	public void getMinValsTest(){
		Vector3 vec1 = new Vector3(-1, 0, 1);
		Vector3 vec2 = new Vector3(1, 0, -1);
		Assert.AreEqual(new Vector3(-1, 0, -1), vec1.getMinVals(vec2));
		Assert.AreEqual(new Vector3(-1, 0, -1), vec2.getMinVals(vec1));
	}

	[Test]
	public void getMaxValsTest(){
		Vector3 vec1 = new Vector3(-1, 0, 1);
		Vector3 vec2 = new Vector3(1, 0, -1);
		Assert.AreEqual(new Vector3(1, 0, 1), vec1.getMaxVals(vec2));
		Assert.AreEqual(new Vector3(1, 0, 1), vec2.getMaxVals(vec1));
	}

	[Test]
	public void CopyTest(){
		Vector3 vec1 = new Vector3(3, 4, 5);
		Vector3 vec2 = vec1.Copy();
		Assert.AreEqual(vec1, vec2);
		vec2.x = 0;
		Assert.AreNotEqual(vec1, vec2);
	}
}

