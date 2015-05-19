using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//holds looted items so we can send it to reward system
public static class LootedItems {
	private static List<string> loot = new List<string>();

	public static void addItemToLoot(string s){

		//may remove if gsManager doesn't neet
		loot.Add(s);

		//gsManager add
		GameObject.Find("GSManager").GetComponent<GSManager>().loot.Add(s);

		Debug.Log("Looted: " + s);
	}

	public static List<string> getList(){
		return loot;
	}
}

//holds list of all loot on a monster.
public class LootList{
	//item name / drop rate
	public Dictionary<string, float> thisMonster = new Dictionary<string, float>();

	//fill dictionary
	public LootList(Dictionary<string, float> items){
		foreach(var key in items.Keys){
			thisMonster.Add(key, items[key]);
		}
	}
}

//storing item names as variables to prevent bugs stemming from typos
public static class Items{
	public static readonly string money = "money";
	public static readonly string sprint = "sprint";
	public static readonly string roll = "roll";
	public static readonly string charge = "charge";
	public static readonly string lunge = "lunge";
	public static readonly string riotShield = "riotShield";
	public static readonly string nanoTriage = "nanoTriage";
	public static readonly string shockNet = "shockNet";
	public static readonly string chainGrab = "chainGrab";
	public static readonly string flare = "flare";
	public static readonly string lantern = "lantern";
	public static readonly string shiv = "shiv";
	public static readonly string utilityBlade = "utilityBlade";
	public static readonly string pruningBlade = "pruningBlade";
	public static readonly string sixShooter = "sixShooter";
	public static readonly string plasmaBlade = "plasmaBlade";
	public static readonly string rebarSword = "rebarSword";
	public static readonly string longSword = "longSword";
	public static readonly string machette = "machette";
	public static readonly string thinBlade = "thinBlade";
	public static readonly string flamePike = "flamePike";
	public static readonly string lumberSaw = "lumberSaw";
	public static readonly string chainsawSword = "chainsawSword";
	public static readonly string copGun = "copGun";
	public static readonly string hiCompressionPistol = "hiCompressionPistol";
	public static readonly string huntingRifle = "huntingRifle";
	public static readonly string laserRifle = "laserRifle";
	public static readonly string machineGun = "machineGun";
	public static readonly string automaticLaserRifle = "automaticLaserRifle";
	public static readonly string shotgun = "shotgun";
	public static readonly string wallOfLead = "wallOfLead";
	public static readonly string shirtAndPants = "shirtAndPants";
	public static readonly string poncho = "poncho";
	public static readonly string bulletProofVest = "bulletProofVest";
	public static readonly string smugglersJacket = "smugglersJacket";
	public static readonly string mixedPlateUniform = "mixedPlateUniform";
	public static readonly string mixedArmyUniform = "mixedArmyUniform";
	public static readonly string ceramicPlate = "ceramicPlate";
	public static readonly string carbonFibronicMeshSuit = "carbonFibronicMeshSuit";
	public static readonly string delversDuster = "delversDuster";
	public static readonly string trashHelmetLightBulb = "trashHelmetLightBulb";
	public static readonly string trashHelmetBucket = "trashHelmetBucket";
	public static readonly string trafficCone = "trafficCone";
	public static readonly string militarySpikeHelmet = "militarySpikeHelmet";
	public static readonly string bikerHelmet = "bikerHelmet";
	public static readonly string policeHelmet = "policeHelmet";
	public static readonly string comHelmet = "comHelmet";
	public static readonly string targetingVisor = "targetingVisor";
	public static readonly string bionicEye = "bionicEye";
	public static readonly string cyberFaceRobot = "cyberFaceRobot";
	public static readonly string cyberFaceHorns = "cyberFaceHorns";
	public static readonly string brainCaseVisor = "brainCaseVisor";
}


//holds permanent info
public static class LootTable {

	//what every monster of every tier drops, and the % chance to drop it
	public static readonly Dictionary<string, LootList> lootTable = new Dictionary<string, LootList>(){
		//
		//BULLY TRUNK
		//
		{"BullyTrunk0", new LootList(new Dictionary<string, float>(){
				{Items.money, 85f},
				{Items.shiv, 1f},
				{Items.utilityBlade, 1f},
				{Items.rebarSword, 1f},
				{Items.lumberSaw, 1f},
				{Items.sixShooter, 1f},
				{Items.copGun, 1f},
				{Items.shirtAndPants, 1f},
				{Items.poncho, .5f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.trafficCone, .5f},
				{Items.militarySpikeHelmet, .5f},
				{Items.bikerHelmet, .5f},
				{Items.sprint, 1f},
				{Items.charge, 1f},
				{Items.lantern, 1f},
				{Items.riotShield, 1f}
			})},
		{"BullyTrunk1", new LootList(new Dictionary<string, float>(){
				{Items.money, 74f},
				{Items.shiv, 2f},
				{Items.utilityBlade, 2f},
				{Items.rebarSword, 2f},
				{Items.longSword, 1.5f},
				{Items.lumberSaw, 2f},
				{Items.sixShooter, 1.5f},
				{Items.copGun, 1.5f},
				{Items.shirtAndPants, 1f},
				{Items.poncho, 1f},
				{Items.trashHelmetLightBulb, 1.5f},
				{Items.trashHelmetBucket, 1.5f},
				{Items.trafficCone, 1f},
				{Items.militarySpikeHelmet, 1f},
				{Items.bikerHelmet, 1f},
				{Items.policeHelmet, .5f},
				{Items.comHelmet, .5f},
				{Items.sprint, 1.25f},
				{Items.charge, 1.25f},
				{Items.lantern, 1f},
				{Items.riotShield, 1f}

			})},
		{"BullyTrunk2", new LootList(new Dictionary<string, float>(){
				{Items.money, 70f},
				{Items.shiv, 3f},
				{Items.utilityBlade, 3f},
				{Items.longSword, 3f},
				{Items.machette, 1f},
				{Items.lumberSaw, 1.5f},
				{Items.sixShooter, 1.5f},
				{Items.copGun, 1.5f},
				{Items.shotgun, 1f},
				{Items.shirtAndPants, 1f},
				{Items.poncho, 1f},
				{Items.bulletProofVest, 1f},
				{Items.trashHelmetLightBulb, 1.5f},
				{Items.trashHelmetBucket, 1.5f},
				{Items.trafficCone, 1f},
				{Items.militarySpikeHelmet, 1f},
				{Items.bikerHelmet, 1f},
				{Items.policeHelmet, .5f},
				{Items.comHelmet, .5f},
				{Items.sprint, 1.25f},
				{Items.charge, 1.25f},
				{Items.lantern, 1f},
				{Items.riotShield, 1f}

			})},
		{"BullyTrunk3", new LootList(new Dictionary<string, float>(){
				{Items.money, 70f},
				{Items.shiv, 4f},
				{Items.utilityBlade, 3f},
				{Items.machette, 2f},
				{Items.chainsawSword, 2f},
				{Items.copGun, 2f},
				{Items.huntingRifle, 2f},
				{Items.shotgun, 2f},
				{Items.bulletProofVest, 2f},
				{Items.bikerHelmet, 2f},
				{Items.policeHelmet, 2f},
				{Items.comHelmet, 1f},
				{Items.sprint, 1.5f},
				{Items.charge, 1.5f},
				{Items.lantern, 1.5f},
				{Items.riotShield, 1.5f}

			})},
		{"BullyTrunk4", new LootList(new Dictionary<string, float>(){
				{Items.money, 60f},
				{Items.utilityBlade, 3f},
				{Items.pruningBlade, 3f},
				{Items.plasmaBlade, 3f},
				{Items.machette, 3f},
				{Items.thinBlade, 2f},
				{Items.chainsawSword, 2f},
				{Items.huntingRifle, 2f},
				{Items.laserRifle, 2f},
				{Items.machineGun, 2f},
				{Items.shotgun, 2f},
				{Items.wallOfLead, 2f},
				{Items.bulletProofVest, 1f},
				{Items.smugglersJacket, 1f},
				{Items.mixedPlateUniform, 1f},
				{Items.ceramicPlate, 1f},
				{Items.carbonFibronicMeshSuit, 1f},
				{Items.targetingVisor, 1f},
				{Items.bionicEye, 2f},
				{Items.sprint, 1.5f},
				{Items.charge, 1.5f},
				{Items.lantern, 1.5f},
				{Items.riotShield, 1.5f}
			})},
		{"BullyTrunk5", new LootList(new Dictionary<string, float>(){
				{Items.money, 50f},
				{Items.pruningBlade, 4f},
				{Items.plasmaBlade, 4f},
				{Items.thinBlade, 3f},
				{Items.flamePike, 2f},
				{Items.chainsawSword, 3f},
				{Items.huntingRifle, 4f},
				{Items.laserRifle, 4f},
				{Items.machineGun, 3f},
				{Items.wallOfLead, 3f},
				{Items.smugglersJacket, 2f},
				{Items.mixedPlateUniform, 2f},
				{Items.ceramicPlate, 1f},
				{Items.carbonFibronicMeshSuit, 1f},
				{Items.targetingVisor, 1f},
				{Items.bionicEye, 2f},
				{Items.cyberFaceRobot, 1f},
				{Items.cyberFaceHorns, 1f},
				{Items.brainCaseVisor, 1f},
				{Items.sprint, 2f},
				{Items.charge, 2f},
				{Items.lantern, 2f},
				{Items.riotShield, 2f}

			})},
		
		//
		//FOLIANT HIVE
		//
		{"FoliantHive0", new LootList(new Dictionary<string, float>(){
				{Items.money, 85f},
				{Items.shiv, 2.5f},
				{Items.poncho, 2f},
				{Items.trashHelmetLightBulb, 2f},
				{Items.trashHelmetBucket, 2f},
				{Items.riotShield, 1f},
				{Items.nanoTriage, .5f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f},
				{Items.flare, 1f},
				{Items.lantern, 1f}
			})},
		{"FoliantHive1", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.lumberSaw, 25f},
				{Items.poncho, 24f},
				{Items.bulletProofVest, 26f},
				{Items.trashHelmetLightBulb, 26f},
				{Items.trashHelmetBucket, 24f},
				{Items.riotShield, 24f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f},
				{Items.flare, 24f},
				{Items.lantern, 24f}

			})},
		{"FoliantHive2", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.utilityBlade, 40f},
				{Items.machette, 40f},
				{Items.lumberSaw, 25f},
				{Items.huntingRifle, 40f},
				{Items.machineGun, 40f},
				{Items.poncho, 24f},
				{Items.bulletProofVest, 26f},
				{Items.trashHelmetLightBulb, 26f},
				{Items.trashHelmetBucket, 24f},
				{Items.trafficCone, 24f},
				{Items.militarySpikeHelmet, 24f},
				{Items.bikerHelmet, 24f},
				{Items.riotShield, 24f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f},
				{Items.flare, 24f},
				{Items.lantern, 24f}


			})},
		{"FoliantHive3", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.utilityBlade, 40f},
				{Items.pruningBlade, 40f},
				{Items.machette, 40f},
				{Items.lumberSaw, 25f},
				{Items.huntingRifle, 40f},
				{Items.machineGun, 40f},
				{Items.shotgun, 40f},
				{Items.bulletProofVest, 26f},
				{Items.smugglersJacket, 40f},
				{Items.trafficCone, 24f},
				{Items.militarySpikeHelmet, 24f},
				{Items.bikerHelmet, 24f},
				{Items.policeHelmet, 40f},
				{Items.comHelmet, 40f},
				{Items.riotShield, 24f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f},
				{Items.flare, 24f},
				{Items.lantern, 24f}
			})},
		{"FoliantHive4", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.utilityBlade, 40f},
				{Items.pruningBlade, 40f},
				{Items.thinBlade, 40f},
				{Items.lumberSaw, 25f},
				{Items.chainsawSword, 25f},
				{Items.huntingRifle, 40f},
				{Items.laserRifle, 25f},
				{Items.machineGun, 40f},
				{Items.automaticLaserRifle, 25f},
				{Items.shotgun, 40f},
				{Items.wallOfLead, 25f},
				{Items.bulletProofVest, 26f},
				{Items.smugglersJacket, 40f},
				{Items.mixedArmyUniform, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.trafficCone, 24f},
				{Items.militarySpikeHelmet, 24f},
				{Items.bikerHelmet, 24f},
				{Items.policeHelmet, 40f},
				{Items.comHelmet, 40f},
				{Items.targetingVisor, 25f},
				{Items.bionicEye, 25f},
				{Items.riotShield, 24f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f},
				{Items.flare, 24f},
				{Items.lantern, 24f}

			})},
		{"FoliantHive5", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.pruningBlade, 40f},
				{Items.thinBlade, 40f},
				{Items.chainsawSword, 25f},
				{Items.laserRifle, 25f},
				{Items.automaticLaserRifle, 25f},
				{Items.wallOfLead, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.ceramicPlate, 25f},
				{Items.carbonFibronicMeshSuit, 25f},
				{Items.comHelmet, 40f},
				{Items.targetingVisor, 25f},
				{Items.bionicEye, 25f},
				{Items.cyberFaceRobot, 25f},
				{Items.cyberFaceHorns, 25f},
				{Items.brainCaseVisor, 25f},
				{Items.riotShield, 24f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f},
				{Items.flare, 24f},
				{Items.lantern, 24f}

			})},

		//
		//FOLIANT FODDER
		//
		{"FoliantFodder0", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.sixShooter, 25f},
				{Items.shirtAndPants, 24f},
				{Items.sprint, 26f},
				{Items.charge, 24f},
				{Items.lunge, 24f},
				{Items.riotShield, 24f},
				{Items.lantern, 24f}
				
			})},
		{"FoliantFodder1", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.sixShooter, 25f},
				{Items.shirtAndPants, 24f},
				{Items.sprint, 26f},
				{Items.charge, 24f},
				{Items.lunge, 24f},
				{Items.riotShield, 24f},
				{Items.lantern, 24f}
				
			})},
		{"FoliantFodder2", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.utilityBlade, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.shirtAndPants, 24f},
				{Items.poncho, 25f},
				{Items.sprint, 26f},
				{Items.charge, 24f},
				{Items.lunge, 24f},
				{Items.riotShield, 24f},
				{Items.lantern, 24f}
				
			})},
		{"FoliantFodder3", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.utilityBlade, 25f},
				{Items.rebarSword, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.shirtAndPants, 24f},
				{Items.poncho, 25f},
				{Items.trafficCone, 25f},
				{Items.sprint, 26f},
				{Items.charge, 24f},
				{Items.lunge, 24f},
				{Items.riotShield, 24f},
				{Items.lantern, 24f}
			})},
		{"FoliantFodder4", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.utilityBlade, 25f},
				{Items.pruningBlade, 25f},
				{Items.rebarSword, 25f},
				{Items.longSword, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.machineGun, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.smugglersJacket, 25f},
				{Items.trafficCone, 25f},
				{Items.policeHelmet, 25f},
				{Items.comHelmet, 25f},
				{Items.sprint, 26f},
				{Items.charge, 24f},
				{Items.lunge, 24f},
				{Items.riotShield, 24f},
				{Items.lantern, 24f}
				
			})},
		{"FoliantFodder5", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.utilityBlade, 25f},
				{Items.pruningBlade, 25f},
				{Items.plasmaBlade, 25f},
				{Items.rebarSword, 25f},
				{Items.longSword, 25f},
				{Items.machette, 25f},
				{Items.lumberSaw, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.machineGun, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.smugglersJacket, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.policeHelmet, 25f},
				{Items.comHelmet, 25f},
				{Items.targetingVisor, 25f},
				{Items.bionicEye, 25f},
				{Items.sprint, 26f},
				{Items.charge, 24f},
				{Items.lunge, 24f},
				{Items.riotShield, 24f},
				{Items.lantern, 24f}
				
			})},


		//
		//CACKLEBRANCH
		//
		{"CackleBranch0", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.smugglersJacket, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.sprint, 26f},
				{Items.roll, 24f},
				{Items.lunge, 24f},
				{Items.shockNet, 24f}

			})},
		{"CackleBranch1", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.rebarSword, 25f},
				{Items.lumberSaw, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.shotgun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.smugglersJacket, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.sprint, 26f},
				{Items.roll, 24f},
				{Items.lunge, 24f},
				{Items.shockNet, 24f}

			})},
		{"CackleBranch2", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.rebarSword, 25f},
				{Items.lumberSaw, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.shotgun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.smugglersJacket, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.sprint, 26f},
				{Items.roll, 24f},
				{Items.lunge, 24f},
				{Items.shockNet, 24f}

			})},
		{"CackleBranch3", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.pruningBlade, 25f},
				{Items.rebarSword, 25f},
				{Items.lumberSaw, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.shotgun, 25f},
				{Items.wallOfLead, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.comHelmet, 25f},
				{Items.sprint, 26f},
				{Items.roll, 24f},
				{Items.lunge, 24f},
				{Items.shockNet, 24f}

			})},
		{"CackleBranch4", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.pruningBlade, 25f},
				{Items.longSword, 25f},
				{Items.chainsawSword, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.wallOfLead, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.comHelmet, 25f},
				{Items.targetingVisor, 25f},
				{Items.sprint, 26f},
				{Items.roll, 24f},
				{Items.lunge, 24f},
				{Items.shockNet, 24f}

			})},
		{"CackleBranch5", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.plasmaBlade, 25f},
				{Items.longSword, 25f},
				{Items.machette, 25f},
				{Items.chainsawSword, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.hiCompressionPistol, 25f},
				{Items.huntingRifle, 25f},
				{Items.laserRifle, 25f},
				{Items.wallOfLead, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.delversDuster, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.comHelmet, 25f},
				{Items.targetingVisor, 25f},
				{Items.bionicEye, 25f},
				{Items.cyberFaceRobot, 25f},
				{Items.sprint, 26f},
				{Items.roll, 24f},
				{Items.lunge, 24f},
				{Items.shockNet, 24f}

			})},


		//
		//MIRAGE
		//
		{"Mirage0", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.rebarSword, 25f},
				{Items.longSword, 25f},
				{Items.sixShooter, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.trafficCone, 25f},
				{Items.bikerHelmet, 25f},
				{Items.charge, 26f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f}

			})},
		{"Mirage1", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.rebarSword, 25f},
				{Items.longSword, 25f},
				{Items.sixShooter, 25f},
				{Items.huntingRifle, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.smugglersJacket, 25f},
				{Items.trafficCone, 25f},
				{Items.shirtAndPants, 25f},
				{Items.bikerHelmet, 25f},
				{Items.policeHelmet, 25f},
				{Items.charge, 26f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f}
			})},
		{"Mirage2", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.longSword, 25f},
				{Items.sixShooter, 25f},
				{Items.huntingRifle, 25f},
				{Items.machineGun, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.smugglersJacket, 25f},
				{Items.trafficCone, 25f},
				{Items.shirtAndPants, 25f},
				{Items.bikerHelmet, 25f},
				{Items.policeHelmet, 25f},
				{Items.comHelmet, 25f},
				{Items.charge, 26f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f}

			})},
		{"Mirage3", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.pruningBlade, 25f},
				{Items.machette, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.machineGun, 25f},
				{Items.policeHelmet, 25f},
				{Items.comHelmet, 25f},
				{Items.targetingVisor, 25f},
				{Items.charge, 26f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f}


			})},
		{"Mirage4", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.pruningBlade, 25f},
				{Items.machette, 25f},
				{Items.thinBlade, 25f},
				{Items.copGun, 25f},
				{Items.laserRifle, 25f},
				{Items.machineGun, 25f},
				{Items.automaticLaserRifle, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.targetingVisor, 25f},
				{Items.bionicEye, 25f},
				{Items.charge, 26f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f}

			})},
		{"Mirage5", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.plasmaBlade, 25f},
				{Items.machette, 25f},
				{Items.thinBlade, 25f},
				{Items.flamePike, 25f},
				{Items.hiCompressionPistol, 25f},
				{Items.laserRifle, 25f},
				{Items.automaticLaserRifle, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.delversDuster, 25f},
				{Items.bionicEye, 25f},
				{Items.cyberFaceRobot, 25f},
				{Items.brainCaseVisor, 25f},
				{Items.charge, 26f},
				{Items.nanoTriage, 24f},
				{Items.shockNet, 24f},
				{Items.chainGrab, 24f}


			})},


		//
		//BUSHLING
		//
		{"Bushling0", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.rebarSword, 25f},
				{Items.longSword, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.lunge, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},
		{"Bushling1", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.rebarSword, 25f},
				{Items.longSword, 25f},
				{Items.lumberSaw, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.lunge, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},
		{"Bushling2", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.pruningBlade, 25f},
				{Items.longSword, 25f},
				{Items.lumberSaw, 25f},
				{Items.copGun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.policeHelmet, 25f},
				{Items.comHelmet, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.lunge, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}
			})},
		{"Bushling3", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.pruningBlade, 25f},
				{Items.longSword, 25f},
				{Items.machette, 25f},
				{Items.lumberSaw, 25f},
				{Items.chainsawSword, 25f},
				{Items.copGun, 25f},
				{Items.machineGun, 25f},
				{Items.bulletProofVest, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.policeHelmet, 25f},
				{Items.comHelmet, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.lunge, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},
		{"Bushling4", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.pruningBlade, 25f},
				{Items.plasmaBlade, 25f},
				{Items.longSword, 25f},
				{Items.machette, 25f},
				{Items.thinBlade, 25f},
				{Items.chainsawSword, 25f},
				{Items.machineGun, 25f},
				{Items.shotgun, 25f},
				{Items.ceramicPlate, 25f},
				{Items.policeHelmet, 25f},
				{Items.bionicEye, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.lunge, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},
		{"Bushling5", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.pruningBlade, 25f},
				{Items.plasmaBlade, 25f},
				{Items.machette, 25f},
				{Items.thinBlade, 25f},
				{Items.flamePike, 25f},
				{Items.chainsawSword, 25f},
				{Items.machineGun, 25f},
				{Items.shotgun, 25f},
				{Items.wallOfLead, 25f},
				{Items.ceramicPlate, 25f},
				{Items.carbonFibronicMeshSuit, 25f},
				{Items.policeHelmet, 25f},
				{Items.bionicEye, 25f},
				{Items.cyberFaceHorns, 25f},
				{Items.brainCaseVisor, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.lunge, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},


		//
		//ARTILITREE
		//
		{"Artilitree0", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.rebarSword, 25f},
				{Items.longSword, 25f},
				{Items.sixShooter, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.smugglersJacket, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.riotShield, 26f},
				{Items.nanoTriage, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},
		{"Artilitree1", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.rebarSword, 25f},
				{Items.longSword, 25f},
				{Items.lumberSaw, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.shotgun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.smugglersJacket, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.policeHelmet, 25f},
				{Items.comHelmet, 25f},
				{Items.riotShield, 26f},
				{Items.nanoTriage, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},
		{"Artilitree2", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.longSword, 25f},
				{Items.lumberSaw, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.machineGun, 25f},
				{Items.shotgun, 25f},
				{Items.bulletProofVest, 25f},
				{Items.smugglersJacket, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.policeHelmet, 25f},
				{Items.comHelmet, 25f},
				{Items.riotShield, 26f},
				{Items.nanoTriage, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},
		{"Artilitree3", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.longSword, 25f},
				{Items.lumberSaw, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.machineGun, 25f},
				{Items.shotgun, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.policeHelmet, 25f},
				{Items.comHelmet, 25f},
				{Items.riotShield, 26f},
				{Items.nanoTriage, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},
		{"Artilitree4", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.pruningBlade, 25f},
				{Items.machette, 25f},
				{Items.flamePike, 25f},
				{Items.chainsawSword, 25f},
				{Items.hiCompressionPistol, 25f},
				{Items.huntingRifle, 25f},
				{Items.laserRifle, 25f},
				{Items.machineGun, 25f},
				{Items.automaticLaserRifle, 25f},
				{Items.shotgun, 25f},
				{Items.wallOfLead, 25f},
				{Items.ceramicPlate, 25f},
				{Items.targetingVisor, 25f},
				{Items.bionicEye, 25f},
				{Items.cyberFaceHorns, 25f},
				{Items.riotShield, 26f},
				{Items.nanoTriage, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},
		{"Artilitree5", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.pruningBlade, 25f},
				{Items.machette, 25f},
				{Items.flamePike, 25f},
				{Items.chainsawSword, 25f},
				{Items.hiCompressionPistol, 25f},
				{Items.laserRifle, 25f},
				{Items.automaticLaserRifle, 25f},
				{Items.wallOfLead, 25f},
				{Items.ceramicPlate, 25f},
				{Items.carbonFibronicMeshSuit, 25f},
				{Items.delversDuster, 25f},
				{Items.targetingVisor, 25f},
				{Items.bionicEye, 25f},
				{Items.cyberFaceRobot, 25f},
				{Items.cyberFaceHorns, 25f},
				{Items.brainCaseVisor, 25f},
				{Items.riotShield, 26f},
				{Items.nanoTriage, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f},
				{Items.flare, 26f},
				{Items.lantern, 26f}

			})},


		//
		//SYNTH
		//
		{"Synth0", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f}

			})},
		{"Synth1", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.machineGun, 25f},
				{Items.shotgun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f}

			})},
		{"Synth2", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.huntingRifle, 25f},
				{Items.machineGun, 25f},
				{Items.shotgun, 25f},
				{Items.shirtAndPants, 25f},
				{Items.poncho, 25f},
				{Items.bulletProofVest, 25f},
				{Items.smugglersJacket, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.trashHelmetLightBulb, 25f},
				{Items.trashHelmetBucket, 25f},
				{Items.trafficCone, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f}

			})},
		{"Synth3", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 25f},
				{Items.utilityBlade, 25f},
				{Items.sixShooter, 25f},
				{Items.copGun, 25f},
				{Items.laserRifle, 25f},
				{Items.automaticLaserRifle, 25f},
				{Items.wallOfLead, 25f},
				{Items.bulletProofVest, 25f},
				{Items.smugglersJacket, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.militarySpikeHelmet, 25f},
				{Items.bikerHelmet, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f}

			})},
		{"Synth4", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.utilityBlade, 25f},
				{Items.machette, 25f},
				{Items.lumberSaw, 25f},
				{Items.hiCompressionPistol, 25f},
				{Items.laserRifle, 25f},
				{Items.automaticLaserRifle, 25f},
				{Items.wallOfLead, 25f},
				{Items.smugglersJacket, 25f},
				{Items.mixedPlateUniform, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.ceramicPlate, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f}
			})},
		{"Synth5", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.utilityBlade, 25f},
				{Items.machette, 25f},
				{Items.thinBlade, 25f},
				{Items.flamePike, 25f},
				{Items.hiCompressionPistol, 25f},
				{Items.laserRifle, 25f},
				{Items.automaticLaserRifle, 25f},
				{Items.wallOfLead, 25f},
				{Items.mixedArmyUniform, 25f},
				{Items.ceramicPlate, 25f},
				{Items.sprint, 26f},
				{Items.roll, 26f},
				{Items.charge, 26f},
				{Items.shockNet, 26f},
				{Items.chainGrab, 26f}

			})},



	};
}
