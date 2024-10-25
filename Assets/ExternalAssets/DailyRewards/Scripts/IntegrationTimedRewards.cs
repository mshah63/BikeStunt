﻿/***************************************************************************\
Project:      Daily Rewards
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using NiobiumStudios;

/** 
 * This is just a snippet of code to integrate Timed Rewards into your project
 * 
 * Copy / Paste the code below
 **/
public class IntegrationTimedRewards : MonoBehaviour
{
    public GameObject MainMenuManager;
    void OnEnable()
    {
        TimedRewards.GetInstance().onClaimPrize += OnClaimPrizeTimedRewards;
    }

    void OnDisable()
    {
    if(TimedRewards.GetInstance())
        TimedRewards.GetInstance().onClaimPrize -= OnClaimPrizeTimedRewards;
    }

    // this is your integration function. Can be on Start or simply a function to be called
    public void OnClaimPrizeTimedRewards(int index)
    {
        // This returns a Reward object
        Reward myReward = TimedRewards.GetInstance().GetReward(index);


        // And you can access any property
        print(myReward.unit);   // This is your reward Unit name
        print(myReward.reward); // This is your reward count

		var rewardsCount = PlayerPrefs.GetInt ("MY_REWARD_KEY", 0);
		rewardsCount += myReward.reward;
        //Add And Show Coins
        SaveData.Instance.Coins += myReward.reward;
        GN_MainMenu.instance.checkCoins();
		
		PlayerPrefs.SetInt ("MY_REWARD_KEY", rewardsCount);
		PlayerPrefs.Save ();
    }


}