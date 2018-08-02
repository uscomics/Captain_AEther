﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public int score;
    public int coin;
    public System.DateTime lastDayPlayed;
    public int consecutiveDaysPlayingCount;
    public int killedSuperbrainCount;
    public int killedBySuperbrainCount;

    public Stats()
    {
        score = 0;
        coin = 0;
        lastDayPlayed = System.DateTime.Now;
        consecutiveDaysPlayingCount = 0;
        killedSuperbrainCount = 0;
        killedBySuperbrainCount = 0;
    }

    public Stats(int inScore, int inCoin, System.DateTime inLastDayPlayed, int inConsecutiveDaysPlayingCount, int inKilledSuperbrainCount, int inKilledBySuperbrainCount)
    {
        score = inScore;
        coin = inCoin;
        lastDayPlayed = inLastDayPlayed;
        consecutiveDaysPlayingCount = inConsecutiveDaysPlayingCount;
        killedSuperbrainCount = inKilledSuperbrainCount;
        killedBySuperbrainCount = inKilledBySuperbrainCount;
    }

    public Stats CombineStats(Stats inStats)
    {
        Stats newStats = new Stats(score + inStats.score, coin + inStats.coin, inStats.lastDayPlayed, consecutiveDaysPlayingCount, killedSuperbrainCount + inStats.killedSuperbrainCount, killedBySuperbrainCount + inStats.killedBySuperbrainCount);
        System.DateTime yesterday = System.DateTime.Today.AddDays(-1);
        if ((yesterday.Year == lastDayPlayed.Year)
        && (yesterday.Month == lastDayPlayed.Month)
        && (yesterday.Day == lastDayPlayed.Day))
        {
            newStats.consecutiveDaysPlayingCount++;
        }
        return newStats;
    }
}
