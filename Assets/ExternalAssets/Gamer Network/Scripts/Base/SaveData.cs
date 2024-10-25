using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class SaveData{

	public static SaveData Instance;
	public bool FirstTime = true;
	public bool isRated = false;
	public bool RemoveAds = false;
	public bool UnlockCars = false;
	public bool UnlockLevels = false;
	public bool UnlockEverything = false;
	public int Level = 1;
    public int Coins = 0;
    public PlayerCar[] Cars;
    public bool IsSoundOn = true;
    

	public string hashOfSaveData;

	//Constructor to save actual GameData
	public SaveData(){}

	//Constructor to check any tampering with the SaveData
	public SaveData(bool firsttime, bool israted, bool ads, bool unlockCar, bool unlockLevel, bool unlockEverything, int levels, int coins, PlayerCar [] playCars ,bool isSoundOn)
	{
		FirstTime = firsttime;
		isRated = israted;
		RemoveAds = ads;
		UnlockCars = unlockCar;
		UnlockLevels = unlockLevel;
		UnlockEverything = unlockEverything;
		Level = levels;
		Coins = coins;
		Cars = playCars;
        IsSoundOn = isSoundOn;
	}
    public void InitializeCars(int totalCars, bool[] unlock)
    {
        Cars = new PlayerCar[totalCars];
        for (int i = 0; i < totalCars; i++)
        {
            Cars[i] = new PlayerCar(unlock[i]);
        }
    }

}

[Serializable]
public class PlayerCar
{
	public bool unlocked;

	public PlayerCar(bool unlock)
	{
		unlocked = unlock;
	}
}