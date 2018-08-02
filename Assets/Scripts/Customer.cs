using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public string id;
    public string firstName;
    public string lastName;
    public string displayName;
    public System.DateTime joined;
    public string avitarURL;
    public BadgeList<CustomerBadge> badges;

    public Customer()
    {
        badges = new BadgeList<CustomerBadge>();
    }

    public Customer(string inId, string inFirstName, string inLastName, string inDisplayName, string inAvitarURL)
    {
        id = inId;
        firstName = inFirstName;
        lastName = inLastName;
        displayName = inDisplayName;
        avitarURL = inAvitarURL;
        badges = new BadgeList<CustomerBadge>();
    }

    public Customer(string inId, string inFirstName, string inLastName, string inDisplayName, string inAvitarURL, BadgeList<CustomerBadge> inBadges)
    {
        id = inId;
        firstName = inFirstName;
        lastName = inLastName;
        displayName = inDisplayName;
        avitarURL = inAvitarURL;
        badges = inBadges;
        badges = new BadgeList<CustomerBadge>();
    }
}
