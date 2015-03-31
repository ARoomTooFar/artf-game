using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPoolableResource{
	void reset();
	IPoolableResource clone();
}

