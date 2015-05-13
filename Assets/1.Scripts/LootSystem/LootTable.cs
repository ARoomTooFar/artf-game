using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LootList{

	public Dictionary<string, float> lootList = new Dictionary<string, float>()
	{
		{"money", 0f},
		{"sprint", 0f},
		{"roll", 0f},
		{"charge", 0f},
		{"lunge", 0f},
		{"riotShield", 0f},
		{"nanoTriangle", 0f},
		{"shockNet", 0f},
		{"chainGrab", 0f},
		{"flare", 0f},
		{"lantern", 0f},
		{"shiv", 0f},
		{"utilityBlade", 0f},
		{"pruningBlade", 0f},
		{"sixShooter", 0f},
		{"plasmaBlade", 0f},
		{"rebarSword", 0f},
		{"longSword", 0f},
		{"machette", 0f},
		{"thinBlade", 0f},
		{"flamePike", 0f},
		{"lumberSaw", 0f},
		{"chainsawSword", 0f},
		{"copGun", 0f},
		{"hiCompressionPistol", 0f},
		{"huntingRifle", 0f},
		{"laserRifle", 0f},
		{"machineGun", 0f},
		{"automaticLaserRifle", 0f},
		{"shotgun", 0f},
		{"wallOfLead", 0f},
		{"shirtAndPants", 0f},
		{"poncho", 0f},
		{"bulletProofVest", 0f},
		{"smugglersJacket", 0f},
		{"mixedPlateUniform", 0f},
		{"mixedArmyUniform", 0f},
		{"ceramicPlate", 0f},
		{"carbonFibronicMeshSuit", 0f},
		{"delversDuster", 0f},
		{"trashHelmetLightBulb", 0f},
		{"trashHelmetBucket", 0f},
		{"trafficCone", 0f},
		{"militarySpikeHelmet", 0f},
		{"bikerHelmet", 0f},
		{"policeHelmet", 0f},
		{"comHelmet", 0f},
		{"targetingVisor", 0f},
		{"bionicEye", 0f},
		{"cyberFaceRobot", 0f},
		{"cyberFaceHorns", 0f},
		{"brainCaseVisor", 0f}
	};

	public LootList(Dictionary<string, float> items){
		foreach(var key in items.Keys){
			lootList[key] = items[key];
		}
	}
}

public static class LootTable {

	//what every monster of every tier drops, and the % chance to drop it
	public static Dictionary<string, LootList> lootTable = new Dictionary<string, LootList>(){

		//
		//BULLY TRUNK
		//
		{"BullyTrunk0", new LootList(new Dictionary<string, float>(){
				{"money", 100f},
				{"shiv", 40f},
				{"utlityBlade", 25f}
			})},
		{"BullyTrunk1", new LootList(new Dictionary<string, float>(){
				{"money", 100f},
				{"shiv", 24f},
				{"utlityBlade", 24f}
			})},
		{"BullyTrunk2", new LootList(new Dictionary<string, float>(){
				{"money", 100f},
				{"utlityBlade", 24f},
				{"huntingRifle", 24f}
			})},
		{"BullyTrunk3", new LootList(new Dictionary<string, float>(){
				{"money", 100f},
				{"machineGun", 40f},
				{"huntingRifle", 24f}
			})},
		{"BullyTrunk4", new LootList(new Dictionary<string, float>(){
				{"money", 100f},
				{"machineGun", 40f},
				{"huntingRifle", 24f}
			})},
		{"BullyTrunk5", new LootList(new Dictionary<string, float>(){
				{"money", 100f},
				{"machineGun", 40f},
				{"huntingRifle", 24f}
			})},

		//
		//FOLIANT FODDER
		//
		{"FoliantFodder0", new LootList(new Dictionary<string, float>(){
				{"money", 100f},
				{"shiv", 40f},
				{"utlityBlade", 25f},
				{"sixShooter", 25f},
				{"poncho", 24f},
				{"trashHelmetBucket", 24f},
				{"trashHelmetLightBulb", 24f},
				{"trafficCone", 24f}
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
