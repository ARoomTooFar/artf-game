// Attack Interface
// For units that can attack

using UnityEngine;
using System.Collections;

public interface IAttackable {
	void initAttack();


	void colliderStart();
	void colliderEnd();

	void specialAttack();

	void attacks();
}
