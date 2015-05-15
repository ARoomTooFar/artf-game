// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class SpawnBehaviour : EnemyBehaviour {
	
	protected ShootFodderBall spawn;
	protected FoliantHive hive;
	
	public virtual void SetVar(ShootFodderBall spawn, FoliantHive hive) {
		this.spawn = spawn;
		this.hive = hive;
	}
}