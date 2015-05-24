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
				{Items.money, 75f},
				{Items.shiv, 3f},
				{Items.lumberSaw, 2.5f},
				{Items.poncho, 3f},
				{Items.bulletProofVest, 2f},
				{Items.trashHelmetLightBulb, 3f},
				{Items.trashHelmetBucket, 3f},
				{Items.riotShield, 1.5f},
				{Items.nanoTriage, 1f},
				{Items.shockNet, 1.5f},
				{Items.chainGrab, 1.5f},
				{Items.flare, 1.5f},
				{Items.lantern, 1.5f}

			})},
		{"FoliantHive2", new LootList(new Dictionary<string, float>(){
				{Items.money, 60f},
				{Items.shiv, 4f},
				{Items.utilityBlade, 1f},
				{Items.machette, 1f},
				{Items.lumberSaw, 3f},
				{Items.huntingRifle, 1f},
				{Items.machineGun, 2f},
				{Items.poncho, 3f},
				{Items.bulletProofVest, 2f},
				{Items.trashHelmetLightBulb, 3f},
				{Items.trashHelmetBucket, 3f},
				{Items.trafficCone, 2f},
				{Items.militarySpikeHelmet, 2f},
				{Items.bikerHelmet, 2f},
				{Items.riotShield, 2f},
				{Items.nanoTriage, 1.5f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f},
				{Items.flare, 1.5f},
				{Items.lantern, 2f}


			})},
		{"FoliantHive3", new LootList(new Dictionary<string, float>(){
				{Items.money, 55f},
				{Items.shiv, 5f},
				{Items.utilityBlade, 2f},
				{Items.pruningBlade, 1f},
				{Items.machette, 2f},
				{Items.lumberSaw, 4f},
				{Items.huntingRifle, 2f},
				{Items.machineGun, 3f},
				{Items.shotgun, 1f},
				{Items.bulletProofVest, 2f},
				{Items.smugglersJacket, 2f},
				{Items.trafficCone, 3f},
				{Items.militarySpikeHelmet, 2f},
				{Items.bikerHelmet, 2f},
				{Items.policeHelmet, 1f},
				{Items.comHelmet, 1f},
				{Items.riotShield, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f},
				{Items.flare, 2f},
				{Items.lantern, 2f}
			})},
		{"FoliantHive4", new LootList(new Dictionary<string, float>(){
				{Items.money, 50f},
				{Items.utilityBlade, 2f},
				{Items.pruningBlade, 2f},
				{Items.thinBlade, 1f},
				{Items.lumberSaw, 3f},
				{Items.chainsawSword, 2f},
				{Items.huntingRifle, 3f},
				{Items.laserRifle, 1f},
				{Items.machineGun, 2f},
				{Items.automaticLaserRifle, 1f},
				{Items.shotgun, 2f},
				{Items.wallOfLead, 1f},
				{Items.bulletProofVest, 2f},
				{Items.smugglersJacket, 2f},
				{Items.mixedArmyUniform, 1f},
				{Items.mixedPlateUniform, 1f},
				{Items.trafficCone, 3f},
				{Items.militarySpikeHelmet, 2f},
				{Items.bikerHelmet, 2f},
				{Items.policeHelmet, 2f},
				{Items.comHelmet, 2f},
				{Items.targetingVisor, .5f},
				{Items.bionicEye, .5f},
				{Items.riotShield, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f},
				{Items.flare, 2f},
				{Items.lantern, 2f}

			})},
		{"FoliantHive5", new LootList(new Dictionary<string, float>(){
				{Items.money, 40f},
				{Items.pruningBlade, 6f},
				{Items.thinBlade, 4f},
				{Items.chainsawSword, 4f},
				{Items.laserRifle, 4f},
				{Items.automaticLaserRifle, 3f},
				{Items.wallOfLead, 3f},
				{Items.mixedArmyUniform, 3f},
				{Items.mixedPlateUniform, 3f},
				{Items.ceramicPlate, 2f},
				{Items.carbonFibronicMeshSuit, 2f},
				{Items.comHelmet, 3f},
				{Items.targetingVisor, 1f},
				{Items.bionicEye, 1f},
				{Items.cyberFaceRobot, 1f},
				{Items.cyberFaceHorns, 1f},
				{Items.brainCaseVisor, 1f},
				{Items.riotShield, 3f},
				{Items.nanoTriage, 3f},
				{Items.shockNet, 3f},
				{Items.chainGrab, 3f},
				{Items.flare, 3f},
				{Items.lantern, 3f}

			})},

		//
		//FOLIANT FODDER
		//
		{"FoliantFodder0", new LootList(new Dictionary<string, float>(){
				{Items.money, 92f},
				{Items.shiv, 1f},
				{Items.sixShooter, 1f},
				{Items.shirtAndPants, 1f},
				{Items.sprint, 1f},
				{Items.charge, 1f},
				{Items.lunge, 1f},
				{Items.riotShield, 1f},
				{Items.lantern, 1f}
				
			})},
		{"FoliantFodder1", new LootList(new Dictionary<string, float>(){
				{Items.money, 89f},
				{Items.shiv, 2f},
				{Items.sixShooter, 2f},
				{Items.shirtAndPants, 2f},
				{Items.sprint, 1f},
				{Items.charge, 1f},
				{Items.lunge, 1f},
				{Items.riotShield, 1f},
				{Items.lantern, 1f}
				
			})},
		{"FoliantFodder2", new LootList(new Dictionary<string, float>(){
				{Items.money, 80f},
				{Items.shiv, 3f},
				{Items.utilityBlade, 2f},
				{Items.sixShooter, 3f},
				{Items.copGun, 2f},
				{Items.shirtAndPants, 3f},
				{Items.poncho, 2f},
				{Items.sprint, 1f},
				{Items.charge, 1f},
				{Items.lunge, 1f},
				{Items.riotShield, 1f},
				{Items.lantern, 1f}
				
			})},
		{"FoliantFodder3", new LootList(new Dictionary<string, float>(){
				{Items.money, 75f},
				{Items.shiv, 4f},
				{Items.utilityBlade, 3f},
				{Items.rebarSword, 1f},
				{Items.sixShooter, 3f},
				{Items.copGun, 2f},
				{Items.shirtAndPants, 3f},
				{Items.poncho, 2f},
				{Items.trafficCone, 2f},
				{Items.sprint, 1f},
				{Items.charge, 1f},
				{Items.lunge, 1f},
				{Items.riotShield, 1f},
				{Items.lantern, 1f}
			})},
		{"FoliantFodder4", new LootList(new Dictionary<string, float>(){
				{Items.money, 70f},
				{Items.utilityBlade, 3f},
				{Items.pruningBlade, 1f},
				{Items.rebarSword, 2f},
				{Items.longSword, 1f},
				{Items.copGun, 2f},
				{Items.huntingRifle, 2f},
				{Items.machineGun, 1f},
				{Items.poncho, 2f},
				{Items.bulletProofVest, 1f},
				{Items.smugglersJacket, 1f},
				{Items.trafficCone, 2f},
				{Items.policeHelmet, 1f},
				{Items.comHelmet, 1f},
				{Items.sprint, 2f},
				{Items.charge, 2f},
				{Items.lunge, 2f},
				{Items.riotShield, 2f},
				{Items.lantern, 2f}
				
			})},
		{"FoliantFodder5", new LootList(new Dictionary<string, float>(){
				{Items.money, 60f},
				{Items.utilityBlade, 4f},
				{Items.pruningBlade, 2f},
				{Items.plasmaBlade, .5f},
				{Items.rebarSword, 2f},
				{Items.longSword, 2f},
				{Items.machette, 1f},
				{Items.lumberSaw, .5f},
				{Items.copGun, 2f},
				{Items.huntingRifle, 3f},
				{Items.machineGun, 1f},
				{Items.poncho, 2f},
				{Items.bulletProofVest, 1f},
				{Items.smugglersJacket, 1f},
				{Items.mixedPlateUniform, 1f},
				{Items.mixedArmyUniform, 1f},
				{Items.trafficCone, 2f},
				{Items.militarySpikeHelmet, 1f},
				{Items.bikerHelmet, .5f},
				{Items.policeHelmet, 1f},
				{Items.comHelmet, .5f},
				{Items.targetingVisor, .5f},
				{Items.bionicEye, .5f},
				{Items.sprint, 2f},
				{Items.charge, 2f},
				{Items.lunge, 2f},
				{Items.riotShield, 2f},
				{Items.lantern, 2f}
				
			})},


		//
		//CACKLEBRANCH
		//
		{"CackleBranch0", new LootList(new Dictionary<string, float>(){
				{Items.money, 90f},
				{Items.shiv, 1f},
				{Items.sixShooter, 1f},
				{Items.copGun, 1f},
				{Items.shirtAndPants, 1f},
				{Items.poncho, 1f},
				{Items.smugglersJacket, 1f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.sprint, .5f},
				{Items.roll, .5f},
				{Items.lunge, .5f},
				{Items.shockNet, .5f}

			})},
		{"CackleBranch1", new LootList(new Dictionary<string, float>(){
				{Items.money, 85f},
				{Items.shiv, 2f},
				{Items.rebarSword, 1f},
				{Items.lumberSaw, 1f},
				{Items.sixShooter, 1f},
				{Items.copGun, 1f},
				{Items.shotgun, 1f},
				{Items.shirtAndPants, 2f},
				{Items.poncho, 1f},
				{Items.smugglersJacket, 1f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.sprint, .5f},
				{Items.roll, .5f},
				{Items.lunge, .5f},
				{Items.shockNet, .5f}

			})},
		{"CackleBranch2", new LootList(new Dictionary<string, float>(){
				{Items.money, 80f},
				{Items.shiv, 2f},
				{Items.rebarSword, 2f},
				{Items.lumberSaw, 2f},
				{Items.sixShooter, 2f},
				{Items.copGun, 2f},
				{Items.shotgun, 1f},
				{Items.shirtAndPants, 1f},
				{Items.poncho, 1f},
				{Items.smugglersJacket, 1f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.lunge, 1f},
				{Items.shockNet, 1f}

			})},
		{"CackleBranch3", new LootList(new Dictionary<string, float>(){
				{Items.money, 75f},
				{Items.shiv, 2f},
				{Items.pruningBlade, 1f},
				{Items.rebarSword, 2f},
				{Items.lumberSaw, 2f},
				{Items.sixShooter, 2f},
				{Items.copGun, 2f},
				{Items.huntingRifle, 1f},
				{Items.shotgun, 1f},
				{Items.wallOfLead, 1f},
				{Items.shirtAndPants, 1f},
				{Items.poncho, 1f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.trafficCone, 2f},
				{Items.comHelmet, 1f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.lunge, 1f},
				{Items.shockNet, 1f}

			})},
		{"CackleBranch4", new LootList(new Dictionary<string, float>(){
				{Items.money, 70f},
				{Items.pruningBlade, 2f},
				{Items.longSword, 1f},
				{Items.chainsawSword, 1f},
				{Items.sixShooter, 2f},
				{Items.copGun, 2f},
				{Items.huntingRifle, 2f},
				{Items.wallOfLead, 2f},
				{Items.shirtAndPants, 3f},
				{Items.poncho, 2f},
				{Items.mixedArmyUniform, 1f},
				{Items.trashHelmetLightBulb, 2f},
				{Items.trashHelmetBucket, 2f},
				{Items.trafficCone, 2f},
				{Items.comHelmet, 1.5f},
				{Items.targetingVisor, .5f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.lunge, 1f},
				{Items.shockNet, 1f}

			})},
		{"CackleBranch5", new LootList(new Dictionary<string, float>(){
				{Items.money, 60f},
				{Items.plasmaBlade, 1f},
				{Items.longSword, 2f},
				{Items.machette, 1f},
				{Items.chainsawSword, 2f},
				{Items.sixShooter, 2f},
				{Items.copGun, 2f},
				{Items.hiCompressionPistol, 1f},
				{Items.huntingRifle, 2f},
				{Items.laserRifle, 1f},
				{Items.wallOfLead, 2f},
				{Items.shirtAndPants, 3f},
				{Items.poncho, 2f},
				{Items.mixedArmyUniform, 1f},
				{Items.delversDuster, 1f},
				{Items.trashHelmetLightBulb, 2f},
				{Items.trashHelmetBucket, 2f},
				{Items.trafficCone, 2f},
				{Items.comHelmet, 1f},
				{Items.targetingVisor, 1f},
				{Items.bionicEye, .5f},
				{Items.cyberFaceRobot, .5f},
				{Items.sprint, 2f},
				{Items.roll, 2f},
				{Items.lunge, 2f},
				{Items.shockNet, 2f}

			})},


		//
		//MIRAGE
		//
		{"Mirage0", new LootList(new Dictionary<string, float>(){
				{Items.money, 80f},
				{Items.shiv, 3f},
				{Items.rebarSword, 1f},
				{Items.longSword, 1f},
				{Items.sixShooter, 2f},
				{Items.shirtAndPants, 3f},
				{Items.poncho, 3f},
				{Items.trafficCone, 2f},
				{Items.bikerHelmet, 1f},
				{Items.charge, 1f},
				{Items.nanoTriage, 1f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f}

			})},
		{"Mirage1", new LootList(new Dictionary<string, float>(){
				{Items.money, 70f},
				{Items.shiv, 4f},
				{Items.rebarSword, 2f},
				{Items.longSword, 2f},
				{Items.sixShooter, 3f},
				{Items.huntingRifle, 2f},
				{Items.poncho, 3f},
				{Items.bulletProofVest, 2f},
				{Items.smugglersJacket, 2f},
				{Items.trafficCone, 2f},
				{Items.shirtAndPants, 2f},
				{Items.bikerHelmet, 1f},
				{Items.policeHelmet, 1f},
				{Items.charge, 1f},
				{Items.nanoTriage, 1f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f}
			})},
		{"Mirage2", new LootList(new Dictionary<string, float>(){
				{Items.money, 60f},
				{Items.shiv, 4f},
				{Items.longSword, 3f},
				{Items.sixShooter, 4f},
				{Items.huntingRifle, 3f},
				{Items.machineGun, 2f},
				{Items.poncho, 4f},
				{Items.bulletProofVest, 2f},
				{Items.smugglersJacket, 2f},
				{Items.trafficCone, 2f},
				{Items.shirtAndPants, 3f},
				{Items.bikerHelmet, 1f},
				{Items.policeHelmet, 1f},
				{Items.comHelmet, 1f},
				{Items.charge, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f}

			})},
		{"Mirage3", new LootList(new Dictionary<string, float>(){
				{Items.money, 55f},
				{Items.pruningBlade, 6f},
				{Items.machette, 5f},
				{Items.copGun, 6f},
				{Items.huntingRifle, 6f},
				{Items.machineGun, 5f},
				{Items.policeHelmet, 4f},
				{Items.comHelmet, 3f},
				{Items.targetingVisor, 2f},
				{Items.charge, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f}


			})},
		{"Mirage4", new LootList(new Dictionary<string, float>(){
				{Items.money, 50f},
				{Items.pruningBlade, 6f},
				{Items.machette, 5f},
				{Items.thinBlade, 4f},
				{Items.copGun, 5f},
				{Items.laserRifle, 4f},
				{Items.machineGun, 5f},
				{Items.automaticLaserRifle, 3f},
				{Items.mixedArmyUniform, 4f},
				{Items.targetingVisor, 3f},
				{Items.bionicEye, 3f},
				{Items.charge, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f}

			})},
		{"Mirage5", new LootList(new Dictionary<string, float>(){
				{Items.money, 30f},
				{Items.plasmaBlade, 6f},
				{Items.machette, 6f},
				{Items.thinBlade, 6f},
				{Items.flamePike, 6f},
				{Items.hiCompressionPistol, 3f},
				{Items.laserRifle, 6f},
				{Items.automaticLaserRifle, 3f},
				{Items.mixedArmyUniform, 5f},
				{Items.delversDuster, 3f},
				{Items.bionicEye, 4f},
				{Items.cyberFaceRobot, 2f},
				{Items.brainCaseVisor, 2f},
				{Items.charge, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f}


			})},


		//
		//BUSHLING
		//
		{"Bushling0", new LootList(new Dictionary<string, float>(){
				{Items.money, 83f},
				{Items.shiv, 1f},
				{Items.utilityBlade, 1f},
				{Items.rebarSword, 1f},
				{Items.longSword, 1f},
				{Items.sixShooter, 1f},
				{Items.copGun, 1f},
				{Items.shirtAndPants, 1f},
				{Items.poncho, 1f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.trafficCone, 1f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.charge, 1f},
				{Items.lunge, 1f},
				{Items.flare, 1f},
				{Items.lantern, 1f}

			})},
		{"Bushling1", new LootList(new Dictionary<string, float>(){
				{Items.money, 75f},
				{Items.shiv, 2f},
				{Items.utilityBlade, 1f},
				{Items.rebarSword, 1f},
				{Items.longSword, 1f},
				{Items.lumberSaw, 1f},
				{Items.sixShooter, 1f},
				{Items.copGun, 1f},
				{Items.shirtAndPants, 2f},
				{Items.poncho, 2f},
				{Items.bulletProofVest, 1f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.trafficCone, 2f},
				{Items.militarySpikeHelmet, 1f},
				{Items.bikerHelmet, 1f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.charge, 1f},
				{Items.lunge, 1f},
				{Items.flare, 1f},
				{Items.lantern, 1f}

			})},
		{"Bushling2", new LootList(new Dictionary<string, float>(){
				{Items.money, 65f},
				{Items.shiv, 2f},
				{Items.utilityBlade, 2f},
				{Items.pruningBlade, 1f},
				{Items.longSword, 2f},
				{Items.lumberSaw, 2f},
				{Items.copGun, 2f},
				{Items.shirtAndPants, 2f},
				{Items.poncho, 2f},
				{Items.bulletProofVest, 2f},
				{Items.mixedPlateUniform, 1f},
				{Items.mixedArmyUniform, 1f},
				{Items.trashHelmetLightBulb, 2f},
				{Items.trashHelmetBucket, 2f},
				{Items.trafficCone, 2f},
				{Items.militarySpikeHelmet, 1f},
				{Items.bikerHelmet, 1f},
				{Items.policeHelmet, 1f},
				{Items.comHelmet, 1f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.charge, 1f},
				{Items.lunge, 1f},
				{Items.flare, 1f},
				{Items.lantern, 1f}
			})},
		{"Bushling3", new LootList(new Dictionary<string, float>(){
				{Items.money, 60f},
				{Items.shiv, 3f},
				{Items.utilityBlade, 2f},
				{Items.pruningBlade, 2f},
				{Items.longSword, 3f},
				{Items.machette, 1f},
				{Items.lumberSaw, 2f},
				{Items.chainsawSword, 1f},
				{Items.copGun, 3f},
				{Items.machineGun, 1f},
				{Items.bulletProofVest, 2f},
				{Items.mixedPlateUniform, 2f},
				{Items.mixedArmyUniform, 2f},
				{Items.trafficCone, 2f},
				{Items.militarySpikeHelmet, 2f},
				{Items.bikerHelmet, 2f},
				{Items.policeHelmet, 2f},
				{Items.comHelmet, 2f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.charge, 1f},
				{Items.lunge, 1f},
				{Items.flare, 1f},
				{Items.lantern, 1f}

			})},
		{"Bushling4", new LootList(new Dictionary<string, float>(){
				{Items.money, 55f},
				{Items.pruningBlade, 4f},
				{Items.plasmaBlade, 2f},
				{Items.longSword, 5f},
				{Items.machette, 4f},
				{Items.thinBlade, 2f},
				{Items.chainsawSword, 3f},
				{Items.machineGun, 4f},
				{Items.shotgun, 2f},
				{Items.ceramicPlate, 2f},
				{Items.policeHelmet, 3f},
				{Items.bionicEye, 2f},
				{Items.sprint, 2f},
				{Items.roll, 2f},
				{Items.charge, 2f},
				{Items.lunge, 2f},
				{Items.flare, 2f},
				{Items.lantern, 2f}

			})},
		{"Bushling5", new LootList(new Dictionary<string, float>(){
				{Items.money, 50f},
				{Items.pruningBlade, 5f},
				{Items.plasmaBlade, 2f},
				{Items.machette, 4f},
				{Items.thinBlade, 2f},
				{Items.flamePike, 1f},
				{Items.chainsawSword, 3f},
				{Items.machineGun, 5f},
				{Items.shotgun, 2f},
				{Items.wallOfLead, 1f},
				{Items.ceramicPlate, 2f},
				{Items.carbonFibronicMeshSuit, 2f},
				{Items.policeHelmet, 4f},
				{Items.bionicEye, 3f},
				{Items.cyberFaceHorns, 1f},
				{Items.brainCaseVisor, 1f},
				{Items.sprint, 2f},
				{Items.roll, 2f},
				{Items.charge, 2f},
				{Items.lunge, 2f},
				{Items.flare, 2f},
				{Items.lantern, 2f}

			})},


		//
		//ARTILITREE
		//
		{"Artilitree0", new LootList(new Dictionary<string, float>(){
				{Items.money, 80f},
				{Items.shiv, 2f},
				{Items.utilityBlade, 1f},
				{Items.rebarSword, 1f},
				{Items.longSword, 1f},
				{Items.sixShooter, 2f},
				{Items.shirtAndPants, 2f},
				{Items.poncho, 2f},
				{Items.bulletProofVest, .5f},
				{Items.smugglersJacket, .5f},
				{Items.trashHelmetLightBulb, .5f},
				{Items.trashHelmetBucket, .5f},
				{Items.trafficCone, .5f},
				{Items.militarySpikeHelmet, .5f},
				{Items.riotShield, 1f},
				{Items.nanoTriage, 1f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f},
				{Items.flare, 1f},
				{Items.lantern, 1f}

			})},
		{"Artilitree1", new LootList(new Dictionary<string, float>(){
				{Items.money, 70f},
				{Items.shiv, 2f},
				{Items.utilityBlade, 2f},
				{Items.rebarSword, 2f},
				{Items.longSword, 2f},
				{Items.lumberSaw, .5f},
				{Items.sixShooter, 2f},
				{Items.copGun, 1f},
				{Items.huntingRifle, 1f},
				{Items.shotgun, .5f},
				{Items.shirtAndPants, 2.5f},
				{Items.poncho, 1f},
				{Items.bulletProofVest, 1f},
				{Items.smugglersJacket, 1f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.trafficCone, 1f},
				{Items.militarySpikeHelmet, 1f},
				{Items.bikerHelmet, .5f},
				{Items.policeHelmet, .5f},
				{Items.comHelmet, .5f},
				{Items.riotShield, 1f},
				{Items.nanoTriage, 1f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f},
				{Items.flare, 1f},
				{Items.lantern, 1f}

			})},
		{"Artilitree2", new LootList(new Dictionary<string, float>(){
				{Items.money, 60f},
				{Items.shiv, 3f},
				{Items.utilityBlade, 3f},
				{Items.longSword, 3f},
				{Items.lumberSaw, 1f},
				{Items.sixShooter, 3f},
				{Items.copGun, 2f},
				{Items.huntingRifle, 1f},
				{Items.machineGun, 1f},
				{Items.shotgun, 1f},
				{Items.bulletProofVest, 1f},
				{Items.smugglersJacket, 1f},
				{Items.mixedPlateUniform, .5f},
				{Items.mixedArmyUniform, .5f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.trafficCone, 1f},
				{Items.militarySpikeHelmet, 1f},
				{Items.bikerHelmet, 1f},
				{Items.policeHelmet, 1f},
				{Items.comHelmet, 1f},
				{Items.riotShield, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f},
				{Items.flare, 2f},
				{Items.lantern, 2f}

			})},
		{"Artilitree3", new LootList(new Dictionary<string, float>(){
				{Items.money, 55f},
				{Items.shiv, 4f},
				{Items.utilityBlade, 3f},
				{Items.longSword, 4f},
				{Items.lumberSaw, 2f},
				{Items.copGun, 4f},
				{Items.huntingRifle, 2f},
				{Items.machineGun, 2f},
				{Items.shotgun, 2f},
				{Items.mixedPlateUniform, 1f},
				{Items.mixedArmyUniform, 1f},
				{Items.militarySpikeHelmet, 2f},
				{Items.bikerHelmet, 2f},
				{Items.policeHelmet, 2f},
				{Items.comHelmet, 2f},
				{Items.riotShield, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f},
				{Items.flare, 2f},
				{Items.lantern, 2f}

			})},
		{"Artilitree4", new LootList(new Dictionary<string, float>(){
				{Items.money, 50f},
				{Items.pruningBlade, 2f},
				{Items.machette, 3f},
				{Items.flamePike, 2f},
				{Items.chainsawSword, 2f},
				{Items.hiCompressionPistol, 3f},
				{Items.huntingRifle, 4f},
				{Items.laserRifle, 2f},
				{Items.machineGun, 4f},
				{Items.automaticLaserRifle, 2f},
				{Items.shotgun, 4f},
				{Items.wallOfLead, 2f},
				{Items.ceramicPlate, 2f},
				{Items.targetingVisor, 2f},
				{Items.bionicEye, 2f},
				{Items.cyberFaceHorns, 2f},
				{Items.riotShield, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f},
				{Items.flare, 2f},
				{Items.lantern, 2f}

			})},
		{"Artilitree5", new LootList(new Dictionary<string, float>(){
				{Items.money, 30f},
				{Items.pruningBlade, 4f},
				{Items.machette, 6f},
				{Items.flamePike, 4f},
				{Items.chainsawSword, 4f},
				{Items.hiCompressionPistol, 5f},
				{Items.laserRifle, 4f},
				{Items.automaticLaserRifle, 3f},
				{Items.wallOfLead, 4f},
				{Items.ceramicPlate, 4f},
				{Items.carbonFibronicMeshSuit, 3f},
				{Items.delversDuster, 3f},
				{Items.targetingVisor, 4f},
				{Items.bionicEye, 4f},
				{Items.cyberFaceRobot, 2f},
				{Items.cyberFaceHorns, 2f},
				{Items.brainCaseVisor, 2f},
				{Items.riotShield, 2f},
				{Items.nanoTriage, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f},
				{Items.flare, 2f},
				{Items.lantern, 2f}

			})},


		//
		//SYNTH
		//
		{"Synth0", new LootList(new Dictionary<string, float>(){
				{Items.money, 80f},
				{Items.shiv, 2f},
				{Items.utilityBlade, 2f},
				{Items.sixShooter, 2f},
				{Items.copGun, 1f},
				{Items.shirtAndPants, 2f},
				{Items.poncho, 2f},
				{Items.trashHelmetLightBulb, 1f},
				{Items.trashHelmetBucket, 1f},
				{Items.trafficCone, 1f},
				{Items.militarySpikeHelmet, 1f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.charge, 1f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f}

			})},
		{"Synth1", new LootList(new Dictionary<string, float>(){
				{Items.money, 70f},
				{Items.shiv, 3f},
				{Items.utilityBlade, 2f},
				{Items.sixShooter, 2f},
				{Items.copGun, 2f},
				{Items.huntingRifle, 1f},
				{Items.machineGun, 1f},
				{Items.shotgun, 1f},
				{Items.shirtAndPants, 2f},
				{Items.poncho, 2f},
				{Items.bulletProofVest, 1f},
				{Items.trashHelmetLightBulb, 2f},
				{Items.trashHelmetBucket, 2f},
				{Items.trafficCone, 2f},
				{Items.militarySpikeHelmet, 1f},
				{Items.bikerHelmet, 1f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.charge, 1f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f}

			})},
		{"Synth2", new LootList(new Dictionary<string, float>(){
				{Items.money, 60f},
				{Items.shiv, 3f},
				{Items.utilityBlade, 3f},
				{Items.sixShooter, 2f},
				{Items.copGun, 2f},
				{Items.huntingRifle, 2f},
				{Items.machineGun, 2f},
				{Items.shotgun, 2f},
				{Items.shirtAndPants, 3f},
				{Items.poncho, 3f},
				{Items.bulletProofVest, 2f},
				{Items.smugglersJacket, 1f},
				{Items.mixedPlateUniform, 1f},
				{Items.mixedArmyUniform, 1f},
				{Items.trashHelmetLightBulb, 2f},
				{Items.trashHelmetBucket, 2f},
				{Items.trafficCone, 2f},
				{Items.militarySpikeHelmet, 1f},
				{Items.bikerHelmet, 1f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.charge, 1f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f}

			})},
		{"Synth3", new LootList(new Dictionary<string, float>(){
				{Items.money, 55f},
				{Items.shiv, 5f},
				{Items.utilityBlade, 5f},
				{Items.sixShooter, 4f},
				{Items.copGun, 4f},
				{Items.laserRifle, 3f},
				{Items.automaticLaserRifle, 2f},
				{Items.wallOfLead, 2f},
				{Items.bulletProofVest, 3f},
				{Items.smugglersJacket, 2f},
				{Items.mixedPlateUniform, 2f},
				{Items.mixedArmyUniform, 2f},
				{Items.militarySpikeHelmet, 3f},
				{Items.bikerHelmet, 3f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.charge, 1f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f}

			})},
		{"Synth4", new LootList(new Dictionary<string, float>(){
				{Items.money, 50f},
				{Items.utilityBlade, 5f},
				{Items.machette, 5f},
				{Items.lumberSaw, 5f},
				{Items.hiCompressionPistol, 3f},
				{Items.laserRifle, 4f},
				{Items.automaticLaserRifle, 3f},
				{Items.wallOfLead, 4f},
				{Items.smugglersJacket, 4f},
				{Items.mixedPlateUniform, 4f},
				{Items.mixedArmyUniform, 4f},
				{Items.ceramicPlate, 4f},
				{Items.sprint, 1f},
				{Items.roll, 1f},
				{Items.charge, 1f},
				{Items.shockNet, 1f},
				{Items.chainGrab, 1f}
			})},
		{"Synth5", new LootList(new Dictionary<string, float>(){
				{Items.money, 45f},
				{Items.utilityBlade, 6f},
				{Items.machette, 6f},
				{Items.thinBlade, 3f},
				{Items.flamePike, 3f},
				{Items.hiCompressionPistol, 4f},
				{Items.laserRifle, 6f},
				{Items.automaticLaserRifle, 4f},
				{Items.wallOfLead, 4f},
				{Items.mixedArmyUniform, 5f},
				{Items.ceramicPlate, 4f},
				{Items.sprint, 2f},
				{Items.roll, 2f},
				{Items.charge, 2f},
				{Items.shockNet, 2f},
				{Items.chainGrab, 2f}

			})},



	};
}
