using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsDisplay : MonoBehaviour
{

    public Stats stats;
    public Customer customer;
    public TextMeshProUGUI score;
    public TextMeshProUGUI coin;
    public TextMeshProUGUI consecutiveDaysPlayed;
    public TextMeshProUGUI killedSuperbrain;
    public TextMeshProUGUI killedBySuperbrain;
    public RectTransform badgeArea;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((null == stats)
        || (null == customer)
        || (null == score)
        || (null == coin)
        || (null == consecutiveDaysPlayed)
        || (null == killedSuperbrain)
        || (null == killedBySuperbrain)
        || (null == badgeArea))
        {
            return;
        }
        score.text = "Score: " + stats.score;
        coin.text = "In Store Credits: " + stats.coin;
        consecutiveDaysPlayed.text = "Consecutive Days Played: " + stats.consecutiveDaysPlayingCount;
        killedSuperbrain.text = "Killed Superbrain Count: " + stats.killedSuperbrainCount;
        killedBySuperbrain.text = "Killed By Superbrain Count: " + stats.killedBySuperbrainCount;

        List<CustomerBadge> badges = customer.badges.badges;
        for (int loop = 0; loop < badges.Count; loop++)
        {
            CustomerBadge badge = badges[loop];
            badge.icon.transform.SetParent(badgeArea);
        }
    }
}
