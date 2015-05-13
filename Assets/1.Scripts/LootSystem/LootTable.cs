using UnityEngine;
using System.Collections;
using System.Collections.Generic;




//holds list of all shit for a monster.
//initializes all drop rates to 0.
public class LootList{
	//item name / drop rate
	public Dictionary<string, float> lootList = new Dictionary<string, float>()
	{
		{ Items.money, 0f},
		{ Items.sprint, 0f},
		{ Items.roll, 0f},
		{ Items.charge, 0f},
		{ Items.lunge, 0f} ,
		{ Items.riotShield, 0f} ,
		{ Items.nanoTriangle, 0f} ,
		{ Items.shockNet, 0f} ,
		{ Items.chainGrab, 0f} ,
		{ Items.flare, 0f} ,
		{ Items.lantern, 0f},
		{ Items.shiv, 0f} ,
		{ Items.utilityBlade, 0f} ,
		{ Items.pruningBlade, 0f} ,
		{ Items.sixShooter, 0f} ,
		{ Items.plasmaBlade, 0f} ,
		{ Items.rebarSword, 0f} ,
		{ Items.longSword, 0f} ,
		{ Items.machette, 0f} ,
		{ Items.thinBlade, 0f} ,
		{ Items.flamePike, 0f} ,
		{ Items.lumberSaw, 0f} ,
		{ Items.chainsawSword, 0f} ,
		{ Items.copGun, 0f} ,
		{ Items.hiCompressionPistol, 0f} ,
		{ Items.huntingRifle, 0f} ,
		{ Items.laserRifle, 0f} ,
		{ Items.machineGun, 0f} ,
		{ Items.automaticLaserRifle, 0f} ,
		{ Items.shotgun, 0f} ,
		{ Items.wallOfLead, 0f} ,
		{ Items.shirtAndPants, 0f} ,
		{ Items.poncho, 0f} ,
		{ Items.bulletProofVest, 0f} ,
		{ Items.smugglersJacket, 0f} ,
		{ Items.mixedPlateUniform, 0f} ,
		{ Items.mixedArmyUniform, 0f} ,
		{ Items.ceramicPlate, 0f} ,
		{ Items.carbonFibronicMeshSuit, 0f} ,
		{ Items.delversDuster, 0f} ,
		{ Items.trashHelmetLightBulb, 0f} ,
		{ Items.trashHelmetBucket, 0f} ,
		{ Items.trafficCone, 0f} ,
		{ Items.militarySpikeHelmet, 0f} ,
		{ Items.bikerHelmet, 0f} ,
		{ Items.policeHelmet, 0f} ,
		{ Items.comHelmet, 0f} ,
		{ Items.targetingVisor, 0f} ,
		{ Items.bionicEye, 0f} ,
		{ Items.cyberFaceRobot, 0f} ,
		{ Items.cyberFaceHorns, 0f} ,
		{ Items.brainCaseVisor, 0f}
	};

	//set which ones you want to be non-zero
	public LootList(Dictionary<string, float> items){
		foreach(var key in items.Keys){
			lootList[key] = items[key];
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
	public static readonly string nanoTriangle = "nanoTriangle";
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
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.utilityBlade, 25f}
			})},
		{"BullyTrunk1", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.utilityBlade, 25f}
			})},
		{"BullyTrunk2", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.utilityBlade, 25f},
				{Items.huntingRifle, 24f}
			})},
		{"BullyTrunk3", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.machineGun, 40f},
				{Items.huntingRifle, 24f}
			})},
		{"BullyTrunk4", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.machineGun, 40f},
				{Items.huntingRifle, 24f}
			})},
		{"BullyTrunk5", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.machineGun, 40f},
				{Items.huntingRifle, 24f}
			})},

		//
		//FOLIANT FODDER
		//
		{"FoliantFodder0", new LootList(new Dictionary<string, float>(){
				{Items.money, 100f},
				{Items.shiv, 40f},
				{Items.utilityBlade, 25f},
				{Items.sixShooter, 25f},
				{Items.poncho, 24f},
				{Items.trashHelmetBucket, 24f},
				{Items.trashHelmetLightBulb, 24f},
				{Items.trafficCone, 24f}
			})},
		{"FoliantFodder1", new LootList(new Dictionary<string, float>(){

			})},
		{"FoliantFodder2", new LootList(new Dictionary<string, float>(){

			})},
		{"FoliantFodder3", new LootList(new Dictionary<string, float>(){

			})},
		{"FoliantFodder4", new LootList(new Dictionary<string, float>(){

			})},
		{"FoliantFodder5", new LootList(new Dictionary<string, float>(){

			})},


		//
		//CACKLEBRANCH
		//
		{"CackleBranch0", new LootList(new Dictionary<string, float>(){

			})},
		{"CackleBranch1", new LootList(new Dictionary<string, float>(){
				
			})},
		{"CackleBranch2", new LootList(new Dictionary<string, float>(){
				
			})},
		{"CackleBranch3", new LootList(new Dictionary<string, float>(){
				
			})},
		{"CackleBranch4", new LootList(new Dictionary<string, float>(){
				
			})},
		{"CackleBranch5", new LootList(new Dictionary<string, float>(){
				
			})},


		//
		//MIRAGE
		//
		{"Mirage0", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Mirage1", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Mirage2", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Mirage3", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Mirage4", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Mirage5", new LootList(new Dictionary<string, float>(){
				
			})},


		//
		//BUSHLING
		//
		{"Bushling0", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Bushling1", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Bushling2", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Bushling3", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Bushling4", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Bushling5", new LootList(new Dictionary<string, float>(){
				
			})},


		//
		//ARTILITREE
		//
		{"Artilitree0", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Artilitree1", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Artilitree2", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Artilitree3", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Artilitree4", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Artilitree5", new LootList(new Dictionary<string, float>(){
				
			})},


		//
		//SYNTH
		//
		{"Synth0", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Synth1", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Synth2", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Synth3", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Synth4", new LootList(new Dictionary<string, float>(){
				
			})},
		{"Synth5", new LootList(new Dictionary<string, float>(){
				
			})},



	};
}
