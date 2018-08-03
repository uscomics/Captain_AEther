using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Total Score
// 1000, 10,000, 50,000, 100,000, 500,000, 1,000,000
// Total Coin
// 100, 1000, 5000, 10,000, 50,000, 100,000, 1,000,000
// Consecutive days played
// 2, 7, 14, 30, 60, 90, 120, 150, 365
// Defeated Superbrain
// 1, 10, 50, 100
// Defeated by Superbrain
// 1, 10, 50, 100

public class Badges
{
    public static string BADGE_SCORE_PARTICIPANT = "PARTICIPANT";
    public static string BADGE_SCORE_ROOKIE = "ROOKIE";
    public static string BADGE_SCORE_COMPETITOR = "COMPETITOR";
    public static string BADGE_SCORE_PLAYER = "PLAYER";
    public static string BADGE_SCORE_MAJOR_PLAYER = "MAJOR_PLAYER";
    public static string BADGE_SCORE_CHAMP = "CHAMP";
    public static string BADGE_COIN_BENJAMINS = "BENJAMINS";
    public static string BADGE_COIN_CASH_COW = "CASH_COW";
    public static string BADGE_COIN_LOOT = "LOOT";
    public static string BADGE_COIN_WAD = "WAD";
    public static string BADGE_COIN_BIG_BANK_HANK = "BIG_BANK_HANK";
    public static string BADGE_COIN_RICH = "RICH";
    public static string BADGE_COIN_MILLIONAIRE = "MILLIONAIRE";
    public static string BADGE_CONSECUTIVE_DAYS_TOURIST = "TOURIST";
    public static string BADGE_CONSECUTIVE_DAYS_VISITOR = "VISITOR";
    public static string BADGE_CONSECUTIVE_DAYS_DAY_TRIPPER = "DAY_TRIPPER";
    public static string BADGE_CONSECUTIVE_DAYS_HARD_CORE = "HARD_CORE";
    public static string BADGE_CONSECUTIVE_DAYS_ADDICTED = "ADDICTED";
    public static string BADGE_CONSECUTIVE_DAYS_FANATIC = "FANATIC";
    public static string BADGE_CONSECUTIVE_DAYS_DEVOTEE = "DEVOTEE";
    public static string BADGE_CONSECUTIVE_DAYS_ZEALOT = "ZEALOT";
    public static string BADGE_CONSECUTIVE_DAYS_GET_A_LIFE = "GET_A_LIFE";
    public static string BADGE_DEFEATED_SUPERBRAIN_VICTOR = "VICTOR";
    public static string BADGE_DEFEATED_SUPERBRAIN_HERO = "HERO";
    public static string BADGE_DEFEATED_SUPERBRAIN_SUPER_HERO = "SUPER_HERO";
    public static string BADGE_DEFEATED_SUPERBRAIN_BRAIN_BLASTER = "BRAIN_BLASTER";
    public static string BADGE_DEFEATED_BY_SUPERBRAIN_DEFEATED = "DEFEATED";
    public static string BADGE_DEFEATED_BY_SUPERBRAIN_LOSER = "LOSER";
    public static string BADGE_DEFEATED_BY_SUPERBRAIN_BROWBEATEN = "BROWBEATEN";
    public static string BADGE_DEFEATED_BY_SUPERBRAIN_BRAIN_BLASTED = "BRAIN_BLASTED";
}

[System.Serializable]
public class Badge
{
    public string id;
    public string name;
    public string description;
    public Image icon;

    public Badge(string inId, string inName, string inDescription, Image inIcon)
    {
        id = inId;
        name = inName;
        description = inDescription;
        icon = inIcon;
    }
}

[System.Serializable]
public class BadgeList<T> where T : Badge
{
    public List<T> badges;

    public BadgeList()
    {
        badges = new List<T>();
    }

    public BadgeList(List<T> inBadges)
    {
        badges = inBadges;
    }

    public T GetBadge(string inId)
    {
        for (int loop = 0; loop < badges.Count; loop++)
        {
            T badge = badges[loop];
            if (badge.id == inId) { return badge; }
        }
        return null;
    }

    public void Add(T inBadge)
    {
        badges.Add(inBadge);
    }

    public Badge Remove(string inId)
    {
        T badge = GetBadge(inId);
        if (null == badge) return null;
        badges.Remove(badge);
        return badge;
    }
}

[System.Serializable]
public class CustomerBadge : Badge
{
    public System.DateTime earned;

    public CustomerBadge(Badge inBadge)
         : base(inBadge.id, inBadge.name, inBadge.description, inBadge.icon)
    {
        earned = System.DateTime.Now;
    }

    public CustomerBadge(string inId, string inName, string inDescription, Image inIcon)
         : base(inId, inName, inDescription, inIcon)
    {
        earned = System.DateTime.Now;
    }

    public CustomerBadge(string inId, string inName, string inDescription, Image inIcon, System.DateTime inEarned)
         : base(inId, inName, inDescription, inIcon)
    {
        earned = inEarned;
    }
}

[System.Serializable]
public class BadgeNameAndIcon
{
    public string name;
    public Image icon;
};


public class BadgeManager : MonoBehaviour
{
    public MessageManager messageManager;
    public Customer customer;
    public Stats stats;
    public BadgeNameAndIcon[] badgeNamesAndIcons;

    private BadgeList<Badge> badges;

    // Use this for initialization
    void Start()
    {
        LoadBadges();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCustomerBadges(customer, stats);
    }

    public void UpdateCustomerBadgesUsingStats(Stats inStats)
    {
        UpdateCustomerBadges(customer, inStats);
    }

    public Image GetBadgeIcon(string inBadgeName)
    {
        //string resourceName = "Textures/Badges/" + inBadgeName + " Badge";
        //Texture icon = Resources.Load(resourceName, typeof(Texture2D)) as Texture;
        //return icon;

        for (int loop = 0; loop < badgeNamesAndIcons.Length; loop++)
        {
            BadgeNameAndIcon badgeInfo = badgeNamesAndIcons[loop];
            if (badgeInfo.name != inBadgeName) continue;
            Image icon = Instantiate(badgeInfo.icon, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as Image;
            return icon;
        }
        return null;
    }

    private void LoadBadges()
    {
        badges = new BadgeList<Badge>();
        badges.Add(new Badge(Badges.BADGE_SCORE_PARTICIPANT, "Participant", "You've scored 1000 points defeating enemies.", GetBadgeIcon("Participant")));
        badges.Add(new Badge(Badges.BADGE_SCORE_ROOKIE, "Rookie", "You've scored 10,000 points defeating enemies.", GetBadgeIcon("Rookie")));
        badges.Add(new Badge(Badges.BADGE_SCORE_COMPETITOR, "Competitor", "You've scored 50,000 points defeating enemies.", GetBadgeIcon("Competitor")));
        badges.Add(new Badge(Badges.BADGE_SCORE_PLAYER, "Player", "You've scored 100,000 points defeating enemies.", GetBadgeIcon("Player")));
        badges.Add(new Badge(Badges.BADGE_SCORE_MAJOR_PLAYER, "Major Player", "You've scored 500,000 points defeating enemies.", GetBadgeIcon("Major Player")));
        badges.Add(new Badge(Badges.BADGE_SCORE_CHAMP, "Champ", "You've scored 1,000,000 points defeating enemies.", GetBadgeIcon("Champ")));
        badges.Add(new Badge(Badges.BADGE_COIN_BENJAMINS, "Benjamins", "You've earned 100 in-store credits.", GetBadgeIcon("Benjamins")));
        badges.Add(new Badge(Badges.BADGE_COIN_CASH_COW, "Cash Cow", "You've earned 1,000 in-store credits.", GetBadgeIcon("Cash Cow")));
        badges.Add(new Badge(Badges.BADGE_COIN_LOOT, "Loot", "You've earned 5,000 in-store credits.", GetBadgeIcon("Loot")));
        badges.Add(new Badge(Badges.BADGE_COIN_WAD, "Wad", "You've earned 10,000 in-store credits.", GetBadgeIcon("Wad")));
        badges.Add(new Badge(Badges.BADGE_COIN_BIG_BANK_HANK, "Big Bank Hank", "You've earned 50,000 in-store credits.", GetBadgeIcon("Big Bank Hank")));
        badges.Add(new Badge(Badges.BADGE_COIN_RICH, "Rich", "You've earned 100,000 in-store credits.", GetBadgeIcon("Rich")));
        badges.Add(new Badge(Badges.BADGE_COIN_MILLIONAIRE, "Millionaire", "You've earned 1,000,000 in-store credits.", GetBadgeIcon("Millionaire")));
        badges.Add(new Badge(Badges.BADGE_CONSECUTIVE_DAYS_TOURIST, "Tourist", "You've played for 2 consecutive days.", GetBadgeIcon("Tourist")));
        badges.Add(new Badge(Badges.BADGE_CONSECUTIVE_DAYS_VISITOR, "Visitor", "You've played for 7 consecutive days.", GetBadgeIcon("Visitor")));
        badges.Add(new Badge(Badges.BADGE_CONSECUTIVE_DAYS_DAY_TRIPPER, "Day Tripper", "You've played for 14 consecutive days.", GetBadgeIcon("Day Tripper")));
        badges.Add(new Badge(Badges.BADGE_CONSECUTIVE_DAYS_HARD_CORE, "Hard Core", "You've played for 30 consecutive days.", GetBadgeIcon("Hard Core")));
        badges.Add(new Badge(Badges.BADGE_CONSECUTIVE_DAYS_ADDICTED, "Addicted", "You've played for 60 consecutive days.", GetBadgeIcon("Addicted")));
        badges.Add(new Badge(Badges.BADGE_CONSECUTIVE_DAYS_FANATIC, "Fanatic", "You've played for 90 consecutive days.", GetBadgeIcon("Fanatic")));
        badges.Add(new Badge(Badges.BADGE_CONSECUTIVE_DAYS_DEVOTEE, "Devotee", "You've played for 120 consecutive days.", GetBadgeIcon("Devotee")));
        badges.Add(new Badge(Badges.BADGE_CONSECUTIVE_DAYS_ZEALOT, "Zealot", "You've played for 150 consecutive days.", GetBadgeIcon("Zealot")));
        badges.Add(new Badge(Badges.BADGE_CONSECUTIVE_DAYS_GET_A_LIFE, "Get A Life", "You've played for 365 consecutive days.", GetBadgeIcon("Get A Life")));
        badges.Add(new Badge(Badges.BADGE_DEFEATED_SUPERBRAIN_VICTOR, "Victor", "You've defeated Superbrain", GetBadgeIcon("Victor")));
        badges.Add(new Badge(Badges.BADGE_DEFEATED_SUPERBRAIN_HERO, "Hero", "You've defeated Superbrain 10 times", GetBadgeIcon("Hero")));
        badges.Add(new Badge(Badges.BADGE_DEFEATED_SUPERBRAIN_SUPER_HERO, "Super Hero", "You've defeated Superbrain 50 times", GetBadgeIcon("Super Hero")));
        badges.Add(new Badge(Badges.BADGE_DEFEATED_SUPERBRAIN_BRAIN_BLASTER, "Brain Blaster", "You've defeated Superbrain 100 times", GetBadgeIcon("Brain Blaster")));
        badges.Add(new Badge(Badges.BADGE_DEFEATED_BY_SUPERBRAIN_DEFEATED, "Defeated", "You were defeated by Superbrain", GetBadgeIcon("Defeated")));
        badges.Add(new Badge(Badges.BADGE_DEFEATED_BY_SUPERBRAIN_LOSER, "Loser", "You were defeated by Superbrain 10 times", GetBadgeIcon("Loser")));
        badges.Add(new Badge(Badges.BADGE_DEFEATED_BY_SUPERBRAIN_BROWBEATEN, "Browbeaten", "You were defeated by Superbrain 50 times", GetBadgeIcon("Browbeaten")));
        badges.Add(new Badge(Badges.BADGE_DEFEATED_BY_SUPERBRAIN_BRAIN_BLASTED, "Brain Blasted", "You were defeated by Superbrain 100 times", GetBadgeIcon("Brain Blasted")));
    }

    private void AddBadge(Customer customer, string badgeID, string messageId)
    {
        customer.badges.Add(new CustomerBadge(badges.GetBadge(badgeID)));
        if (null == messageManager) return;
        messageManager.ShowMessage(messageId);
    }

    private void UpdateCustomerBadges(Customer customer, Stats inStats)
    {
        UpdateCustomerScoreBadges(customer, inStats);
        UpdateCustomerCoinBadges(customer, inStats);
        UpdateCustomerConsecutiveDaysBadges(customer, inStats);
        UpdateCustomerDefeatedSuperbrainBadges(customer, inStats);
        UpdateCustomerDefeatedBySuperbrainBadges(customer, inStats);
    }

    private void UpdateCustomerScoreBadges(Customer customer, Stats stats)
    {
        // 1000, 10,000, 50,000, 100,000, 500,000, 1,000,000
        if ((1000 <= stats.score) && (null == customer.badges.GetBadge(Badges.BADGE_SCORE_PARTICIPANT))) AddBadge(customer, Badges.BADGE_SCORE_PARTICIPANT, Messages.MSG_BADGE_SCORE_PARTICIPANT);
        else if ((10000 <= stats.score) && (null == customer.badges.GetBadge(Badges.BADGE_SCORE_ROOKIE))) AddBadge(customer, Badges.BADGE_SCORE_ROOKIE, Messages.MSG_BADGE_SCORE_ROOKIE);
        else if ((50000 <= stats.score) && (null == customer.badges.GetBadge(Badges.BADGE_SCORE_COMPETITOR))) AddBadge(customer, Badges.BADGE_SCORE_COMPETITOR, Messages.MSG_BADGE_SCORE_COMPETITOR);
        else if ((100000 <= stats.score) && (null == customer.badges.GetBadge(Badges.BADGE_SCORE_PLAYER))) AddBadge(customer, Badges.BADGE_SCORE_PLAYER, Messages.MSG_BADGE_SCORE_PLAYER);
        else if ((500000 <= stats.score) && (null == customer.badges.GetBadge(Badges.BADGE_SCORE_MAJOR_PLAYER))) AddBadge(customer, Badges.BADGE_SCORE_MAJOR_PLAYER, Messages.MSG_BADGE_SCORE_MAJOR_PLAYER);
        else if ((1000000 <= stats.score) && (null == customer.badges.GetBadge(Badges.BADGE_SCORE_CHAMP))) AddBadge(customer, Badges.BADGE_SCORE_CHAMP, Messages.MSG_BADGE_SCORE_CHAMP);
    }

    private void UpdateCustomerCoinBadges(Customer customer, Stats stats)
    {
        // 100, 1000, 5000, 10,000, 50,000, 100,000, 1,000,000
        if ((100 <= stats.coin) && (null == customer.badges.GetBadge(Badges.BADGE_COIN_BENJAMINS))) AddBadge(customer, Badges.BADGE_COIN_BENJAMINS, Messages.MSG_BADGE_COIN_BENJAMINS);
        else if ((1000 <= stats.coin) && (null == customer.badges.GetBadge(Badges.BADGE_COIN_CASH_COW))) AddBadge(customer, Badges.BADGE_COIN_CASH_COW, Messages.MSG_BADGE_COIN_CASH_COW);
        else if ((10000 <= stats.coin) && (null == customer.badges.GetBadge(Badges.BADGE_COIN_LOOT))) AddBadge(customer, Badges.BADGE_COIN_LOOT, Messages.MSG_BADGE_COIN_LOOT);
        else if ((50000 <= stats.coin) && (null == customer.badges.GetBadge(Badges.BADGE_COIN_WAD))) AddBadge(customer, Badges.BADGE_COIN_WAD, Messages.MSG_BADGE_COIN_WAD);
        else if ((100000 <= stats.coin) && (null == customer.badges.GetBadge(Badges.BADGE_COIN_BIG_BANK_HANK))) AddBadge(customer, Badges.BADGE_COIN_BIG_BANK_HANK, Messages.MSG_BADGE_COIN_BIG_BANK_HANK);
        else if ((500000 <= stats.coin) && (null == customer.badges.GetBadge(Badges.BADGE_COIN_RICH))) AddBadge(customer, Badges.BADGE_COIN_RICH, Messages.MSG_BADGE_COIN_RICH);
        else if ((1000000 <= stats.coin) && (null == customer.badges.GetBadge(Badges.BADGE_COIN_MILLIONAIRE))) AddBadge(customer, Badges.BADGE_COIN_MILLIONAIRE, Messages.MSG_BADGE_COIN_MILLIONAIRE);
    }

    private void UpdateCustomerConsecutiveDaysBadges(Customer customer, Stats stats)
    {
        // 2, 7, 14, 30, 60, 90, 120, 150, 365
        if ((2 <= stats.consecutiveDaysPlayingCount) && (null == customer.badges.GetBadge(Badges.BADGE_CONSECUTIVE_DAYS_TOURIST))) AddBadge(customer, Badges.BADGE_CONSECUTIVE_DAYS_TOURIST, Messages.MSG_BADGE_CONSECUTIVE_DAYS_TOURIST);
        else if ((7 <= stats.consecutiveDaysPlayingCount) && (null == customer.badges.GetBadge(Badges.BADGE_CONSECUTIVE_DAYS_VISITOR))) AddBadge(customer, Badges.BADGE_CONSECUTIVE_DAYS_VISITOR, Messages.MSG_BADGE_CONSECUTIVE_DAYS_VISITOR);
        else if ((14 <= stats.consecutiveDaysPlayingCount) && (null == customer.badges.GetBadge(Badges.BADGE_CONSECUTIVE_DAYS_DAY_TRIPPER))) AddBadge(customer, Badges.BADGE_CONSECUTIVE_DAYS_DAY_TRIPPER, Messages.MSG_BADGE_CONSECUTIVE_DAYS_DAY_TRIPPER);
        else if ((30 <= stats.consecutiveDaysPlayingCount) && (null == customer.badges.GetBadge(Badges.BADGE_CONSECUTIVE_DAYS_HARD_CORE))) AddBadge(customer, Badges.BADGE_CONSECUTIVE_DAYS_HARD_CORE, Messages.MSG_BADGE_CONSECUTIVE_DAYS_HARD_CORE);
        else if ((60 <= stats.consecutiveDaysPlayingCount) && (null == customer.badges.GetBadge(Badges.BADGE_CONSECUTIVE_DAYS_ADDICTED))) AddBadge(customer, Badges.BADGE_CONSECUTIVE_DAYS_ADDICTED, Messages.MSG_BADGE_CONSECUTIVE_DAYS_ADDICTED);
        else if ((90 <= stats.consecutiveDaysPlayingCount) && (null == customer.badges.GetBadge(Badges.BADGE_CONSECUTIVE_DAYS_FANATIC))) AddBadge(customer, Badges.BADGE_CONSECUTIVE_DAYS_FANATIC, Messages.MSG_BADGE_CONSECUTIVE_DAYS_FANATIC);
        else if ((120 <= stats.consecutiveDaysPlayingCount) && (null == customer.badges.GetBadge(Badges.BADGE_CONSECUTIVE_DAYS_DEVOTEE))) AddBadge(customer, Badges.BADGE_CONSECUTIVE_DAYS_DEVOTEE, Messages.MSG_BADGE_CONSECUTIVE_DAYS_DEVOTEE);
        else if ((150 <= stats.consecutiveDaysPlayingCount) && (null == customer.badges.GetBadge(Badges.BADGE_CONSECUTIVE_DAYS_ZEALOT))) AddBadge(customer, Badges.BADGE_CONSECUTIVE_DAYS_ZEALOT, Messages.MSG_BADGE_CONSECUTIVE_DAYS_ZEALOT);
        else if ((365 <= stats.consecutiveDaysPlayingCount) && (null == customer.badges.GetBadge(Badges.BADGE_CONSECUTIVE_DAYS_GET_A_LIFE))) AddBadge(customer, Badges.BADGE_CONSECUTIVE_DAYS_GET_A_LIFE, Messages.MSG_BADGE_CONSECUTIVE_DAYS_GET_A_LIFE);
    }

    private void UpdateCustomerDefeatedSuperbrainBadges(Customer customer, Stats stats)
    {
        // 1, 10, 50, 100
        if ((1 <= stats.killedSuperbrainCount) && (null == customer.badges.GetBadge(Badges.BADGE_DEFEATED_SUPERBRAIN_VICTOR))) AddBadge(customer, Badges.BADGE_DEFEATED_SUPERBRAIN_VICTOR, Messages.MSG_BADGE_DEFEATED_SUPERBRAIN_VICTOR);
        else if ((10 <= stats.killedSuperbrainCount) && (null == customer.badges.GetBadge(Badges.BADGE_DEFEATED_SUPERBRAIN_HERO))) AddBadge(customer, Badges.BADGE_DEFEATED_SUPERBRAIN_HERO, Messages.MSG_BADGE_DEFEATED_SUPERBRAIN_HERO);
        else if ((50 <= stats.killedSuperbrainCount) && (null == customer.badges.GetBadge(Badges.BADGE_DEFEATED_SUPERBRAIN_SUPER_HERO))) AddBadge(customer, Badges.BADGE_DEFEATED_SUPERBRAIN_SUPER_HERO, Messages.MSG_BADGE_DEFEATED_SUPERBRAIN_SUPER_HERO);
        else if ((100 <= stats.killedSuperbrainCount) && (null == customer.badges.GetBadge(Badges.BADGE_DEFEATED_SUPERBRAIN_BRAIN_BLASTER))) AddBadge(customer, Badges.BADGE_DEFEATED_SUPERBRAIN_BRAIN_BLASTER, Messages.MSG_BADGE_DEFEATED_SUPERBRAIN_BRAIN_BLASTER);
    }

    private void UpdateCustomerDefeatedBySuperbrainBadges(Customer customer, Stats stats)
    {
        // 1, 10, 50, 100
        if ((1 <= stats.killedBySuperbrainCount) && (null == customer.badges.GetBadge(Badges.BADGE_DEFEATED_BY_SUPERBRAIN_DEFEATED))) AddBadge(customer, Badges.BADGE_DEFEATED_BY_SUPERBRAIN_DEFEATED, Messages.MSG_BADGE_DEFEATED_BY_SUPERBRAIN_DEFEATED);
        else if ((10 <= stats.killedBySuperbrainCount) && (null == customer.badges.GetBadge(Badges.BADGE_DEFEATED_BY_SUPERBRAIN_LOSER))) AddBadge(customer, Badges.BADGE_DEFEATED_BY_SUPERBRAIN_LOSER, Messages.MSG_BADGE_DEFEATED_BY_SUPERBRAIN_LOSER);
        else if ((50 <= stats.killedBySuperbrainCount) && (null == customer.badges.GetBadge(Badges.BADGE_DEFEATED_BY_SUPERBRAIN_BROWBEATEN))) AddBadge(customer, Badges.BADGE_DEFEATED_BY_SUPERBRAIN_BROWBEATEN, Messages.MSG_BADGE_DEFEATED_BY_SUPERBRAIN_BROWBEATEN);
        else if ((100 <= stats.killedBySuperbrainCount) && (null == customer.badges.GetBadge(Badges.BADGE_DEFEATED_BY_SUPERBRAIN_BRAIN_BLASTED))) AddBadge(customer, Badges.BADGE_DEFEATED_BY_SUPERBRAIN_BRAIN_BLASTED, Messages.MSG_BADGE_DEFEATED_BY_SUPERBRAIN_BRAIN_BLASTED);
    }
}
