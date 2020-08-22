using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class consoleScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable ModuleSelectable;
    public TextMesh AllText;

    public Material[] Mats;
    public GameObject Back;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    private KeyCode[] TheKeys =
	{
        KeyCode.Backspace, KeyCode.Return,
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P,
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M,
        KeyCode.Space
	};
    string TheLetters = "<Eqwertyuiopasdfghjklzxcvbnm ";
    private bool focused = false;
    private string textOnModule = "";
    string input = "";
    string solveMessage = "";

    int chosenEnemy = -1;
    int chosenHero = -1;
    int chosenLocation = -1;
    private List<int> chosenItems = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
    private List<int> chosenWeapons = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

    // Deliberately random as shit numbers
    const float INF =   8164259999999999f;
    const float NA =  0.0000000000118428f;
    const float nINF = -8164259999999999f;

    public class Enemy
    {
        public string NAME { get; set; }
         public float  INT { get; set; }
         public float  PWR { get; set; }
         public float  DEF { get; set; }
         public float  MBL { get; set; }
         public float   HP { get; set; }
         public float  STL { get; set; }

        public Enemy(string Name, float Intelligence, float Power, float Defense, float Mobility, float Health, float Stealth )
        {
            NAME = Name;
             INT = Intelligence;
             PWR = Power;
             DEF = Defense;
             MBL = Mobility;
              HP = Health;
             STL = Stealth;
        }
    }

    Enemy theEnemy = new Enemy ("Dummy", 0, 0, 0, 0, 0, 0);

    public class Hero
    {
        public string NAME { get; set; }
         public float  HGT { get; set; }
         public float  WGT { get; set; }
         public float  AGE { get; set; }
         public float  LDN { get; set; }
         public float  RDA { get; set; }
         public float  ORG_LAT { get; set; }
         public float  ORG_LONG { get; set; }

        public Hero(string Name, float Height, float Weight, float Age, float Loudness, float Radioactivity, float Origin_Latitude, float Origin_Longitude )
        {
            NAME = Name;
             HGT = Height;
             WGT = Weight;
             AGE = Age;
             LDN = Loudness;
             RDA = Radioactivity;
             ORG_LAT = Origin_Latitude;
             ORG_LONG = Origin_Longitude;
        }
    }

    Hero theHero = new Hero ("Dummy", 0, 0, 0, 0, 0, 0, 0);

    public class Location
    {
        public string NAME { get; set; }
         public float  TMP { get; set; }
         public float  HUM { get; set; }
         public float  PSI { get; set; }
         public float  WND_DIR { get; set; }
         public float  WND_SPD { get; set; }
         public float  PRC { get; set; }
         public float  ALT { get; set; }

        public Location(string Name, float Temperature, float Humidity, float Pressure, float Wind_Direction, float Wind_Speed, float Precipitation, float Altitude )
        {
            NAME = Name;
             TMP = Temperature;
             HUM = Humidity;
             PSI = Pressure;
             WND_DIR = Wind_Direction;
             WND_SPD = Wind_Speed;
             PRC = Precipitation;
             ALT = Altitude;
        }
    }

    Location theLocation = new Location ("Dummy", 0, 0, 0, 0, 0, 0, 0);

    public class Item {
        public string NAME { get; set; }
        public bool CANUSE { get; set; }

        public Item(string Name, bool CanUse )
        {
            NAME = Name;
            CANUSE = CanUse;
        }
    }

    private List<bool> itemBools = new List<bool> { };
    private List<string> theItemNames = new List<string> { };
    private List<bool> theItemBools = new List<bool> { };
    bool isItGood = true;

    public class Weapon {
        public string NAME { get; set; }
        public float SCORE { get; set; }

        public Weapon(string Name, float Score )
        {
            NAME = Name;
            SCORE = Score;
        }
    }

    float currentWeaponScore = 0;
    private List<float> weaponScores = new List<float> { };
    private List<string> theWeaponNames = new List<string> { };
    private List<float> theWeaponScores = new List<float> { };
    string correctWeapon = "";

    void Awake () {
        moduleId = moduleIdCounter++;
        ModuleSelectable.OnFocus += delegate () { focused = true; };
        ModuleSelectable.OnDefocus += delegate () { focused = false; };
    }

    // Use this for initialization
    void Start () {
        textOnModule = "> $";

        Enemy enemy0 = new Enemy("Def", 905, 90000000, 11, 0, 1.2f, 20000000000);
        Enemy enemy1 = new Enemy("Sentinel of Kanye", 420, INF, 5, 9.8f, nINF, 1000000);
        Enemy enemy2 = new Enemy("Bananaduck", 80, 7, 1, 3, 0.01f, 127);
        Enemy enemy3 = new Enemy("Lil Uzi Vert", 99, 42, 8, 7.6f, 0.05f, 665);
        Enemy enemy4 = new Enemy("Shoebill", -1, -1, -1, -1, -1, -1);
        Enemy enemy5 = new Enemy("Module Rick", 60, 69, 3, 0, 1, 21);
        Enemy enemy6 = new Enemy("Giant Enemy Spider", 100, 100, nINF, 0.1f, 0.5f, 8);
        Enemy enemy7 = new Enemy("Enemy Spider", 10, 1, 7, 1, 0.375f, 4);
        Enemy enemy8 = new Enemy("Small Enemy Spider", 1, 0.01f, INF, 10, 0.25f, 2);
        Enemy enemy9 = new Enemy("Microscopic Enemy Spider", 0.1f, 0.0001f, NA, 100, 0.125f, 1);
        Enemy enemy10 = new Enemy("Shrieking White-Hot\nSphere of Pure Rage", 140, 1000000, 1, 100, 0, 20000000000);
        Enemy enemy11 = new Enemy("ObjCount", -1, 10000000, 7, 1, 0.029f, 5000000000);
        Enemy enemy12 = new Enemy("The Impostor", 135, 500, 5, 4, 0.06f, 25);
        Enemy enemy13 = new Enemy("The Z-Fighter", 68, 1000, 5, 3, 0.6f, 100000);
        Enemy enemy14 = new Enemy("DC 'Splode", 300, 10000000, INF, 0.01f, 0, 6900);
        Enemy enemy15 = new Enemy("Unicorn", 1, 2, 3, 4, 5, 6);
        Enemy enemy16 = new Enemy("Ablert Iensten", 160, 42, 4, 0.001f, 0.02f, 56);
        Enemy enemy17 = new Enemy("Brain Shoe", 88, 2560, 9, 1, 0.01f, 51);
        Enemy enemy18 = new Enemy("Armadill0", 47, 2, 11, 3, 0.2f, 62);
        Enemy enemy19 = new Enemy("Usaying bolt", 92, 256, 5, 10.4f, -0.01f, 73);
        Enemy enemy20 = new Enemy("Ken Tank", 61, 0.001f, 0, 0.06f, 0.01f, 39);
        Enemy enemy21 = new Enemy("Ninja (Not That One)", 86, 4, 6, 40, 0.04f, 0);
        Enemy enemy22 = new Enemy("TradeMark", NA, NA, NA, NA, NA, NA);
        Enemy enemy23 = new Enemy("AlfaGolf", INF, 0, 0, 0, 0, 1000);
        Enemy enemy24 = new Enemy("aeiou", 1, 5, 9, 15, 21, 25);
        Enemy enemy25 = new Enemy("spekvil", 66, 66, 6, 6.66f, 666, 66);
        Enemy enemy26 = new Enemy("stick bug lol", 98, 7, 1, 2, 0.01f, 60);

        chosenEnemy = UnityEngine.Random.Range(0,27);

        switch (chosenEnemy) {
            case 0: theEnemy = enemy0; break;
            case 1: theEnemy = enemy1; break;
            case 2: theEnemy = enemy2; break;
            case 3: theEnemy = enemy3; break;
            case 4: theEnemy = enemy4; break;
            case 5: theEnemy = enemy5; break;
            case 6: theEnemy = enemy6; break;
            case 7: theEnemy = enemy7; break;
            case 8: theEnemy = enemy8; break;
            case 9: theEnemy = enemy9; break;
            case 10: theEnemy = enemy10; break;
            case 11: theEnemy = enemy11; break;
            case 12: theEnemy = enemy12; break;
            case 13: theEnemy = enemy13; break;
            case 14: theEnemy = enemy14; break;
            case 15: theEnemy = enemy15; break;
            case 16: theEnemy = enemy16; break;
            case 17: theEnemy = enemy17; break;
            case 18: theEnemy = enemy18; break;
            case 19: theEnemy = enemy19; break;
            case 20: theEnemy = enemy20; break;
            case 21: theEnemy = enemy21; break;
            case 22: theEnemy = enemy22; break;
            case 23: theEnemy = enemy23; break;
            case 24: theEnemy = enemy24; break;
            case 25: theEnemy = enemy25; break;
            case 26: theEnemy = enemy26; break;
            default: Debug.Log("Anus"); break;
        }

        Hero hero0 = new Hero("Tim Wi", 1.8f, 82, 30, 60, 80, 52, 13);
        Hero hero1 = new Hero("Kanye West", 1.73f, 75, 43, 180, 1000, NA, NA);
        Hero hero2 = new Hero("Bob", 1.828f, 67, 23, 70, 0, 42.33f, -83.12f);
        Hero hero3 = new Hero("Simon", 1.69f, 53, 19, 100, 10, 41.8f, -88.15f);
        Hero hero4 = new Hero("Zean", 1.7f, 57, 15, 16, 0.1f, 0, 0);
        Hero hero5 = new Hero("RobWald", 2.72f, 79, 89, 66, 12, 42, -180);
        Hero hero6 = new Hero("ok fatty", 1.2f, 442, 88, 54, 43, 13, 52);
        Hero hero7 = new Hero("Henry Stick", 1.5f, 2, 24, 0, 0, 1, 2);
        Hero hero8 = new Hero("F0rk", 0.2f, 0.01f, NA, 0, 0, 35.8f, -104.2f);
        Hero hero9 = new Hero("lambrghiiiiiii", 1, 907, 57, 50, 100, 41.9f, -12.6f);
        Hero hero10 = new Hero("PILL", 0.01f, 0.001f, NA, 0, 3, NA, NA);
        Hero hero11 = new Hero("Mad", 0.3f, 60, 21, 0, 0.1f, 56.1f, 106.3f);
        Hero hero12 = new Hero("Fatrlick77", 1.8f, 70, 16, 77, 7, 32, -100);
        Hero hero13 = new Hero("Drunkle Squeaky", 1.9f, 64, 17, 100, 1000, 41.6f, -71.4f);
        Hero hero14 = new Hero("The small child", 0.5f, 5, 0, 57, 120, -18.7f, -35.5f);
        Hero hero15 = new Hero("Mikell Stephens", 1.73f, 70, 34, 60, 22, 38.4f, -94.3f);
        Hero hero16 = new Hero("Cap'n Disl", 1.79f, 68, 51, 44, 429, 61.5f, -105.3f);
        Hero hero17 = new Hero("U three Hs", 1.6f, 80, 25, 1, 69, 46.2f, -2.2f);
        Hero hero18 = new Hero("Y'darn kids", 1.52f, 59, 100, 30, 5, -25.3f, -133.8f);
        Hero hero19 = new Hero("BIG BIRD", 2, 101, 51, 60, 2, 123, 456);
        Hero hero20 = new Hero("wee woo wee woo", 65, 555, 2, 200, 200000, nINF, nINF);
        Hero hero21 = new Hero("hi", 1.55f, 54, 18, 15, 1, 32.2f, -82.9f);
        Hero hero22 = new Hero("Skai", 1.65f, 55, 22, 101, 161, 54.1f, -2.5f);
        Hero hero23 = new Hero("Uncle Sam", 2, 80, 208, 7.4f, 1776, 38.9f, -77);
        Hero hero24 = new Hero("dQw4w9WgXcQ", 1.77f, 58, 54, 84, 49, 55.4f, -1.2f);
        Hero hero25 = new Hero("HOW", INF, INF, INF, INF, INF, INF, INF);

        chosenHero = UnityEngine.Random.Range(0,26);

        switch (chosenHero) {
            case 0: theHero = hero0; break;
            case 1: theHero = hero1; break;
            case 2: theHero = hero2; break;
            case 3: theHero = hero3; break;
            case 4: theHero = hero4; break;
            case 5: theHero = hero5; break;
            case 6: theHero = hero6; break;
            case 7: theHero = hero7; break;
            case 8: theHero = hero8; break;
            case 9: theHero = hero9; break;
            case 10: theHero = hero10; break;
            case 11: theHero = hero11; break;
            case 12: theHero = hero12; break;
            case 13: theHero = hero13; break;
            case 14: theHero = hero14; break;
            case 15: theHero = hero15; break;
            case 16: theHero = hero16; break;
            case 17: theHero = hero17; break;
            case 18: theHero = hero18; break;
            case 19: theHero = hero19; break;
            case 20: theHero = hero20; break;
            case 21: theHero = hero21; break;
            case 22: theHero = hero22; break;
            case 23: theHero = hero23; break;
            case 24: theHero = hero24; break;
            case 25: theHero = hero25; break;
            default: Debug.Log("Anus"); break;
        }

        Location location0 = new Location("Room A9", 37, 0, 0, 0, 0, 0, 400);
        Location location1 = new Location("Tugo", 31, 4, 15, 92, 6, 5, 358);
        Location location2 = new Location("North Jersey", 100, 0, 100, 318, 2, 0, -10);
        Location location3 = new Location("Detriot", 400, 100, 200, 90, 100, 400, -500);
        Location location4 = new Location("Brizal", 999, 100, 999, 0, 999, 999, 999);
        Location location5 = new Location("noisnemiD rorriM", -22, -10, -9.8f, 90, -13, -3, -180);
        Location location6 = new Location("Haway", 38, 0, 1, 0, 0, 0, 1000);
        Location location7 = new Location("Big Blue C", 21, 200, INF, NA, NA, INF, -29029);
        Location location8 = new Location("Jup", 49, 50, 100, 11, 111, 1000, NA);
        Location location9 = new Location("Shikagoo", 20, 5, 11, 273, 87, 4, 2);
        Location location10 = new Location("memebigboy", 4, 7, 24, 2, INF, 2299, NA);
        Location location11 = new Location("3Vic3Rom3SieTan", -6, 1, -1, 42, 111, 15, 29029);
        Location location12 = new Location("where no one\ncan hear you m", -1, 0, -6, 111, 7, 0, INF);
        Location location13 = new Location("Smoot", 57, 57, 57, 57, 57, 57, 57);
        Location location14 = new Location("A town called", 21, 6, 9, 68, 14, 2, 487);
        Location location15 = new Location("place go brrrr", -2, 0, 58, 720, 1000, 0, 58);
        Location location16 = new Location("Peed", 27.5f, 22, 5, 76, 87, 3, 98);
        Location location17 = new Location("corton", 100, 100, 7, 1, 5, 16, 5);
        Location location18 = new Location("Not York", 26, 11, 16, 216, 50, 1, 112);
        Location location19 = new Location("Your mom", 55, 2, 123, nINF, NA, 4, 489);
        Location location20 = new Location("What?", 5, 6, 7, 137, 20, 2.1f, 48);
        Location location21 = new Location("Literally just a\ncardboard box", 17, 0, 2, 0, 0, 0, NA);
        Location location22 = new Location("SUNRISE", 23, 63, 0, 301, 7, 0.4f, 77);
        Location location23 = new Location("ffp", 22.2f, 500, 19.5f, 19, 87, 228.6f, 2.286f);
        Location location24 = new Location("Goergia", 22, 10, 9.8f, 270, 13, 3, 180);

        chosenLocation = UnityEngine.Random.Range(0,25);

        switch (chosenLocation) {
            case 0: theLocation = location0; break;
            case 1: theLocation = location1; break;
            case 2: theLocation = location2; break;
            case 3: theLocation = location3; break;
            case 4: theLocation = location4; break;
            case 5: theLocation = location5; break;
            case 6: theLocation = location6; break;
            case 7: theLocation = location7; break;
            case 8: theLocation = location8; break;
            case 9: theLocation = location9; break;
            case 10: theLocation = location10; break;
            case 11: theLocation = location11; break;
            case 12: theLocation = location12; break;
            case 13: theLocation = location13; break;
            case 14: theLocation = location14; break;
            case 15: theLocation = location15; break;
            case 16: theLocation = location16; break;
            case 17: theLocation = location17; break;
            case 18: theLocation = location18; break;
            case 19: theLocation = location19; break;
            case 20: theLocation = location20; break;
            case 21: theLocation = location21; break;
            case 22: theLocation = location22; break;
            case 23: theLocation = location23; break;
            case 24: theLocation = location24; break;
            default: Debug.Log("Anus"); break;
        }

        if (theEnemy.STL < 100) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theHero.LDN > 80) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theEnemy.STL > 20 && theEnemy.STL < 10000) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theHero.AGE < 18 && theHero.WGT > 203) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theHero.HGT < 2) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theHero.WGT < 70) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theHero.AGE < 18 || theHero.AGE > 100) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theHero.LDN < 80) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theHero.RDA > 15) { itemBools.Add(true); } else { itemBools.Add(false); }
        if ((theHero.ORG_LAT > 25 && theHero.ORG_LAT < 45) && (theHero.ORG_LONG > -19 && theHero.ORG_LONG < -37)) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theEnemy.INT < 70) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theEnemy.PWR < 500) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theEnemy.DEF > 6) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theEnemy.MBL > 6) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theEnemy.HP >= 0.06f) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theEnemy.STL > 10000) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theLocation.TMP > 38) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theLocation.HUM > 40) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theLocation.PSI < 15) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theLocation.WND_SPD < 20) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theLocation.PRC > 75) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theLocation.ALT < 100) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theEnemy.INT < 100 && 50 < theEnemy.INT) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theLocation.PRC == 0) { itemBools.Add(true); } else { itemBools.Add(false); }
        if (theLocation.WND_DIR % 45 == 0) { itemBools.Add(true); } else { itemBools.Add(false); }

        Item item0 = new Item("3D Glasses", itemBools[0]);
        Item item1 = new Item("Server Mute", itemBools[1]);
        Item item2 = new Item("Eggalyzer", itemBools[2]);
        Item item3 = new Item("Quantum Crack", itemBools[3]);
        Item item4 = new Item("Stepstool", itemBools[4]);
        Item item5 = new Item("Dumbbell", itemBools[5]);
        Item item6 = new Item("Time Machine", itemBools[6]);
        Item item7 = new Item("Megaphone", itemBools[7]);
        Item item8 = new Item("Geiger Counter", itemBools[8]);
        Item item9 = new Item("Bald Eagle", itemBools[9]);
        Item item10 = new Item("Dunce Cap", itemBools[10]);
        Item item11 = new Item("Stop Sign", itemBools[11]);
        Item item12 = new Item("Power Glove", itemBools[12]);
        Item item13 = new Item("Stun Gun", itemBools[13]);
        Item item14 = new Item("Breathalyzer", itemBools[14]);
        Item item15 = new Item("Sunglasses", itemBools[15]);
        Item item16 = new Item("Hand Warmers", itemBools[16]);
        Item item17 = new Item("Dehumidifier", itemBools[17]);
        Item item18 = new Item("Bike Pump", itemBools[18]);
        Item item19 = new Item("Fan", itemBools[19]);
        Item item20 = new Item("Umbrella", itemBools[20]);
        Item item21 = new Item("Springboard", itemBools[21]);
        Item item22 = new Item("Elmo", itemBools[22]);
        Item item23 = new Item("2 of Spades", itemBools[23]);
        Item item24 = new Item("Robin's Movie Ticket", itemBools[24]);

        chosenItems.Shuffle(); //First 10 are the chosens
        for (int i = 0; i < 10; i++) {
            switch (chosenItems[i]) {
                case 0: theItemNames.Add(item0.NAME); theItemBools.Add(item0.CANUSE); break;
                case 1: theItemNames.Add(item1.NAME); theItemBools.Add(item1.CANUSE); break;
                case 2: theItemNames.Add(item2.NAME); theItemBools.Add(item2.CANUSE); break;
                case 3: theItemNames.Add(item3.NAME); theItemBools.Add(item3.CANUSE); break;
                case 4: theItemNames.Add(item4.NAME); theItemBools.Add(item4.CANUSE); break;
                case 5: theItemNames.Add(item5.NAME); theItemBools.Add(item5.CANUSE); break;
                case 6: theItemNames.Add(item6.NAME); theItemBools.Add(item6.CANUSE); break;
                case 7: theItemNames.Add(item7.NAME); theItemBools.Add(item7.CANUSE); break;
                case 8: theItemNames.Add(item8.NAME); theItemBools.Add(item8.CANUSE); break;
                case 9: theItemNames.Add(item9.NAME); theItemBools.Add(item9.CANUSE); break;
                case 10: theItemNames.Add(item10.NAME); theItemBools.Add(item10.CANUSE); break;
                case 11: theItemNames.Add(item11.NAME); theItemBools.Add(item11.CANUSE); break;
                case 12: theItemNames.Add(item12.NAME); theItemBools.Add(item12.CANUSE); break;
                case 13: theItemNames.Add(item13.NAME); theItemBools.Add(item13.CANUSE); break;
                case 14: theItemNames.Add(item14.NAME); theItemBools.Add(item14.CANUSE); break;
                case 15: theItemNames.Add(item15.NAME); theItemBools.Add(item15.CANUSE); break;
                case 16: theItemNames.Add(item16.NAME); theItemBools.Add(item16.CANUSE); break;
                case 17: theItemNames.Add(item17.NAME); theItemBools.Add(item17.CANUSE); break;
                case 18: theItemNames.Add(item18.NAME); theItemBools.Add(item18.CANUSE); break;
                case 19: theItemNames.Add(item19.NAME); theItemBools.Add(item19.CANUSE); break;
                case 20: theItemNames.Add(item20.NAME); theItemBools.Add(item20.CANUSE); break;
                case 21: theItemNames.Add(item21.NAME); theItemBools.Add(item21.CANUSE); break;
                case 22: theItemNames.Add(item22.NAME); theItemBools.Add(item22.CANUSE); break;
                case 23: theItemNames.Add(item23.NAME); theItemBools.Add(item23.CANUSE); break;
                case 24: theItemNames.Add(item24.NAME); theItemBools.Add(item24.CANUSE); break;
                default: Debug.Log("Anus"); break;
            }
        }


        if (theEnemy.NAME.Contains("Spider")) { currentWeaponScore = 69; } weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theLocation.WND_SPD + 2; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        if (theHero.WGT < 1) { currentWeaponScore = 85; } if (theLocation.ALT < 0) { currentWeaponScore += 31; } weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        if (theEnemy.DEF <= 3) { currentWeaponScore = 61; } if (theLocation.TMP > 10) { currentWeaponScore += 65; } weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        if (theHero.LDN >= 70) { currentWeaponScore = 21; } if (theEnemy.HP >= 0.1f) { currentWeaponScore += 99; } weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        if (theHero.RDA > 99) { currentWeaponScore = 67; } if (theHero.ORG_LONG < 0) { currentWeaponScore += 54; } weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        if (theHero.WGT < 60) { currentWeaponScore = 47; } if (theHero.ORG_LAT < 0) { currentWeaponScore += 81; } weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = 100 - theLocation.TMP; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = 25 * theEnemy.NAME.Count(Char.IsWhiteSpace); weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        if (theEnemy.NAME == "stick bug lol") { currentWeaponScore = 50; } if (theHero.NAME == "HOW") { currentWeaponScore += 50; } if (theLocation.NAME == "place go brrrr") { currentWeaponScore += 50; } weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theHero.ORG_LAT + theHero.ORG_LONG; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = 100 * theEnemy.HP; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = 10 * theHero.HGT; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theLocation.PRC; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        for (int i = 0; i < theLocation.NAME.Length; i++) { if (Char.ToLower(theLocation.NAME[i])  == 'a' || Char.ToLower(theLocation.NAME[i]) == 'e' || Char.ToLower(theLocation.NAME[i]) == 'i' || Char.ToLower(theLocation.NAME[i]) == 'o' || Char.ToLower(theLocation.NAME[i]) == 'u') { currentWeaponScore += 10; }} weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theHero.AGE * theHero.HGT; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theHero.HGT + theHero.WGT + theHero.AGE + theHero.LDN + theHero.RDA; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theHero.NAME.Length * 7; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theEnemy.INT + theEnemy.PWR + theEnemy.DEF + theEnemy.MBL + theEnemy.HP + theEnemy.STL; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        for (int i = 0; i < theEnemy.NAME.Length; i++) { if (Char.ToLower(theEnemy.NAME[i])  == 'a' || Char.ToLower(theEnemy.NAME[i]) == 'b' || Char.ToLower(theEnemy.NAME[i]) == 'c' || Char.ToLower(theEnemy.NAME[i]) == 'd' || Char.ToLower(theEnemy.NAME[i]) == 'e' || Char.ToLower(theEnemy.NAME[i]) == 'f' || Char.ToLower(theEnemy.NAME[i]) == 'g' || Char.ToLower(theEnemy.NAME[i]) == 'h' || Char.ToLower(theEnemy.NAME[i]) == 'i' || Char.ToLower(theEnemy.NAME[i]) == 'j' || Char.ToLower(theEnemy.NAME[i]) == 'k' || Char.ToLower(theEnemy.NAME[i]) == 'l' || Char.ToLower(theEnemy.NAME[i]) == 'm' || Char.ToLower(theEnemy.NAME[i]) == 'n' || Char.ToLower(theEnemy.NAME[i]) == 'o' || Char.ToLower(theEnemy.NAME[i]) == 'p' || Char.ToLower(theEnemy.NAME[i]) == 'q' || Char.ToLower(theEnemy.NAME[i]) == 'r' || Char.ToLower(theEnemy.NAME[i]) == 's' || Char.ToLower(theEnemy.NAME[i]) == 't' || Char.ToLower(theEnemy.NAME[i]) == 'u' || Char.ToLower(theEnemy.NAME[i]) == 'v' || Char.ToLower(theEnemy.NAME[i]) == 'w' || Char.ToLower(theEnemy.NAME[i]) == 'x' || Char.ToLower(theEnemy.NAME[i]) == 'y' || Char.ToLower(theEnemy.NAME[i]) == 'z') { currentWeaponScore += 5; }} weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theEnemy.DEF * 25; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        if (theEnemy.STL < 10) { currentWeaponScore = 89; } if (theEnemy.STL > 1000) { currentWeaponScore = 121; } weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theLocation.ALT - theLocation.HUM; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theHero.AGE - 87; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;
        currentWeaponScore = theLocation.TMP + theLocation.HUM + theLocation.PSI + theLocation.WND_DIR + theLocation.WND_SPD + theLocation.PRC + theLocation.ALT; weaponScores.Add(currentWeaponScore); currentWeaponScore = 0;

        Weapon weapon0= new Weapon("Giant Enemy Bugspray", weaponScores[0]);
        Weapon weapon1= new Weapon("Market Gardener", weaponScores[1]);
        Weapon weapon2= new Weapon("Rock", weaponScores[2]);
        Weapon weapon3= new Weapon("Paper", weaponScores[3]);
        Weapon weapon4= new Weapon("Scissors", weaponScores[4]);
        Weapon weapon5= new Weapon("Lizard", weaponScores[5]);
        Weapon weapon6= new Weapon("Spock", weaponScores[6]);
        Weapon weapon7= new Weapon("Firey Fist O' Pain", weaponScores[7]);
        Weapon weapon8= new Weapon("Sand gun", weaponScores[8]);
        Weapon weapon9= new Weapon("Laughter", weaponScores[9]);
        Weapon weapon10= new Weapon("Anonymous Monsplode", weaponScores[10]);
        Weapon weapon11= new Weapon("Bed", weaponScores[11]);
        Weapon weapon12= new Weapon("T-Posing Godzilla", weaponScores[12]);
        Weapon weapon13= new Weapon("Water Staff Thing", weaponScores[13]);
        Weapon weapon14= new Weapon("Argonian Dictionary", weaponScores[14]);
        Weapon weapon15= new Weapon("DISTRACT", weaponScores[15]);
        Weapon weapon16= new Weapon("Mine Turtle", weaponScores[16]);
        Weapon weapon17= new Weapon("Markscript", weaponScores[17]);
        Weapon weapon18= new Weapon("Tylerwon", weaponScores[18]);
        Weapon weapon19= new Weapon("Gun Sand", weaponScores[19]);
        Weapon weapon20= new Weapon("Trumpet", weaponScores[20]);
        Weapon weapon21= new Weapon("Creep", weaponScores[21]);
        Weapon weapon22= new Weapon("Pineapple", weaponScores[22]);
        Weapon weapon23= new Weapon("Mrgrt Thtchr", weaponScores[23]);
        Weapon weapon24= new Weapon("Bunni", weaponScores[24]);

        chosenWeapons.Shuffle(); //First 5 are the chosens
        for (int i = 0; i < 5; i++) {
            switch (chosenWeapons[i]) {
                case 0: theWeaponNames.Add(weapon0.NAME); theWeaponScores.Add(weapon0.SCORE); break;
                case 1: theWeaponNames.Add(weapon1.NAME); theWeaponScores.Add(weapon1.SCORE); break;
                case 2: theWeaponNames.Add(weapon2.NAME); theWeaponScores.Add(weapon2.SCORE); break;
                case 3: theWeaponNames.Add(weapon3.NAME); theWeaponScores.Add(weapon3.SCORE); break;
                case 4: theWeaponNames.Add(weapon4.NAME); theWeaponScores.Add(weapon4.SCORE); break;
                case 5: theWeaponNames.Add(weapon5.NAME); theWeaponScores.Add(weapon5.SCORE); break;
                case 6: theWeaponNames.Add(weapon6.NAME); theWeaponScores.Add(weapon6.SCORE); break;
                case 7: theWeaponNames.Add(weapon7.NAME); theWeaponScores.Add(weapon7.SCORE); break;
                case 8: theWeaponNames.Add(weapon8.NAME); theWeaponScores.Add(weapon8.SCORE); break;
                case 9: theWeaponNames.Add(weapon9.NAME); theWeaponScores.Add(weapon9.SCORE); break;
                case 10: theWeaponNames.Add(weapon10.NAME); theWeaponScores.Add(weapon10.SCORE); break;
                case 11: theWeaponNames.Add(weapon11.NAME); theWeaponScores.Add(weapon11.SCORE); break;
                case 12: theWeaponNames.Add(weapon12.NAME); theWeaponScores.Add(weapon12.SCORE); break;
                case 13: theWeaponNames.Add(weapon13.NAME); theWeaponScores.Add(weapon13.SCORE); break;
                case 14: theWeaponNames.Add(weapon14.NAME); theWeaponScores.Add(weapon14.SCORE); break;
                case 15: theWeaponNames.Add(weapon15.NAME); theWeaponScores.Add(weapon15.SCORE); break;
                case 16: theWeaponNames.Add(weapon16.NAME); theWeaponScores.Add(weapon16.SCORE); break;
                case 17: theWeaponNames.Add(weapon17.NAME); theWeaponScores.Add(weapon17.SCORE); break;
                case 18: theWeaponNames.Add(weapon18.NAME); theWeaponScores.Add(weapon18.SCORE); break;
                case 19: theWeaponNames.Add(weapon19.NAME); theWeaponScores.Add(weapon19.SCORE); break;
                case 20: theWeaponNames.Add(weapon20.NAME); theWeaponScores.Add(weapon20.SCORE); break;
                case 21: theWeaponNames.Add(weapon21.NAME); theWeaponScores.Add(weapon21.SCORE); break;
                case 22: theWeaponNames.Add(weapon22.NAME); theWeaponScores.Add(weapon22.SCORE); break;
                case 23: theWeaponNames.Add(weapon23.NAME); theWeaponScores.Add(weapon23.SCORE); break;
                case 24: theWeaponNames.Add(weapon24.NAME); theWeaponScores.Add(weapon24.SCORE); break;
                default: Debug.Log("Anus"); break;
            }
        }

        Debug.LogFormat("[The Console #{0}] Enemy Name: {1}", moduleId, theEnemy.NAME);
        Debug.LogFormat("[The Console #{0}] INT: {1} IQ", moduleId, theEnemy.INT);
        Debug.LogFormat("[The Console #{0}] PWR: {1} N", moduleId, theEnemy.PWR);
        Debug.LogFormat("[The Console #{0}] DEF: {1} Mohs", moduleId, theEnemy.DEF);
        Debug.LogFormat("[The Console #{0}] MBL: {1} m/s²", moduleId, theEnemy.MBL);
        Debug.LogFormat("[The Console #{0}] HP: {1} BAC", moduleId, theEnemy.HP);
        Debug.LogFormat("[The Console #{0}] STL: {1} lm", moduleId, theEnemy.STL);

        Debug.LogFormat("[The Console #{0}] Hero Name: {1}", moduleId, theHero.NAME);
        Debug.LogFormat("[The Console #{0}] HGT: {1} m", moduleId, theHero.HGT);
        Debug.LogFormat("[The Console #{0}] WGT: {1} kg", moduleId, theHero.WGT);
        Debug.LogFormat("[The Console #{0}] AGE: {1} y", moduleId, theHero.AGE);
        Debug.LogFormat("[The Console #{0}] LDN: {1} db", moduleId, theHero.LDN);
        Debug.LogFormat("[The Console #{0}] RDA: {1} mSv", moduleId, theHero.RDA);
        Debug.LogFormat("[The Console #{0}] ORG: ({1}°, {2}°)", moduleId, theHero.ORG_LAT, theHero.ORG_LONG);

        Debug.LogFormat("[The Console #{0}] Location Name: {1}", moduleId, theLocation.NAME);
        Debug.LogFormat("[The Console #{0}] TMP: {1}° C", moduleId, theLocation.TMP);
        Debug.LogFormat("[The Console #{0}] HUM: {1}% RH", moduleId, theLocation.HUM);
        Debug.LogFormat("[The Console #{0}] PSI: {1} kPa", moduleId, theLocation.PSI);
        Debug.LogFormat("[The Console #{0}] WND: {1}° & {2} km/s²", moduleId, theLocation.WND_DIR, theLocation.WND_SPD);
        Debug.LogFormat("[The Console #{0}] PRC: {1} cm", moduleId, theLocation.PRC);
        Debug.LogFormat("[The Console #{0}] ALT: {1} m", moduleId, theLocation.ALT);

        Debug.LogFormat("[The Console #{0}] Items:", moduleId);
        Debug.LogFormat("[The Console #{0}] A) {1} | {2}", moduleId, theItemNames[0], theItemBools[0] == true ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] B) {1} | {2}", moduleId, theItemNames[1], theItemBools[1] == true ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] C) {1} | {2}", moduleId, theItemNames[2], theItemBools[2] == true ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] D) {1} | {2}", moduleId, theItemNames[3], theItemBools[3] == true ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] E) {1} | {2}", moduleId, theItemNames[4], theItemBools[4] == true ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] F) {1} | {2}", moduleId, theItemNames[5], theItemBools[5] == true ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] G) {1} | {2}", moduleId, theItemNames[6], theItemBools[6] == true ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] H) {1} | {2}", moduleId, theItemNames[7], theItemBools[7] == true ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] I) {1} | {2}", moduleId, theItemNames[8], theItemBools[8] == true ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] J) {1} | {2}", moduleId, theItemNames[9], theItemBools[9] == true ? "USE" : "do not use");

        Debug.LogFormat("[The Console #{0}] Weapons:", moduleId);
        Debug.LogFormat("[The Console #{0}] A) {1} | {2}; {3}", moduleId, theWeaponNames[0], theWeaponScores[0], (theWeaponScores[0] >= theWeaponScores[1] && theWeaponScores[0] >= theWeaponScores[2]) && (theWeaponScores[0] >= theWeaponScores[3] && theWeaponScores[0] >= theWeaponScores[4]) ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] B) {1} | {2}; {3}", moduleId, theWeaponNames[1], theWeaponScores[1], (theWeaponScores[1] >= theWeaponScores[0] && theWeaponScores[1] >= theWeaponScores[2]) && (theWeaponScores[1] >= theWeaponScores[3] && theWeaponScores[1] >= theWeaponScores[4]) ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] C) {1} | {2}; {3}", moduleId, theWeaponNames[2], theWeaponScores[2], (theWeaponScores[2] >= theWeaponScores[0] && theWeaponScores[2] >= theWeaponScores[1]) && (theWeaponScores[2] >= theWeaponScores[3] && theWeaponScores[2] >= theWeaponScores[4]) ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] D) {1} | {2}; {3}", moduleId, theWeaponNames[3], theWeaponScores[3], (theWeaponScores[3] >= theWeaponScores[0] && theWeaponScores[3] >= theWeaponScores[1]) && (theWeaponScores[3] >= theWeaponScores[2] && theWeaponScores[3] >= theWeaponScores[4]) ? "USE" : "do not use");
        Debug.LogFormat("[The Console #{0}] E) {1} | {2}; {3}", moduleId, theWeaponNames[4], theWeaponScores[4], (theWeaponScores[4] >= theWeaponScores[0] && theWeaponScores[4] >= theWeaponScores[1]) && (theWeaponScores[4] >= theWeaponScores[2] && theWeaponScores[4] >= theWeaponScores[3]) ? "USE" : "do not use");

        Debug.LogFormat("[The Console #{0}] Actions on the module:", moduleId);
    }

	// Update is called once per frame
	void Update () {
        AllText.text = textOnModule.Replace("$", input).Replace("8.16426E+15", "∞").Replace("1.18428E-11", "N/A");
        for (int i = 0; i < TheKeys.Count(); i++) {
            if (Input.GetKeyDown(TheKeys[i])) {
                if (TheLetters[i].ToString() == "<".ToString()) {
                    handleBack();
                } else if (TheLetters[i].ToString() == "E".ToString()) {
                    handleEnter();
                } else {
                    handleKey(TheLetters[i]);
                }
            }
        }
	}

    void handleKey (char c) {
        if (focused) {
        if (input.Length != 23) {
            input = input + c;
        }
        }
    }

    void handleBack () {
        if (focused) {
        if (input.Length != 0) {
            input = input.Substring(0, input.Length - 1);
        }
        }
    }

    void handleEnter () {
        if (focused) {
        if (input == "view enemy") {
            textOnModule = String.Format("Your enemy:\n{0}\n\nINT: {1} IQ\nPWR: {2} N\nDEF: {3} Mohs\nMBL: {4} m/s²\n HP: {5} BAC\nSTL: {6} lm\n\n> $", theEnemy.NAME, theEnemy.INT, theEnemy.PWR, theEnemy.DEF, theEnemy.MBL, theEnemy.HP, theEnemy.STL );
        } else if (input == "view hero") {
            textOnModule = String.Format("Your hero:\n{0}\n\nHGT: {1} m\nWGT: {2} kg\nAGE: {3} y\nLDN: {4} db\nRDA: {5} mSv\nORG: ({6}°, {7}°)\n\n> $", theHero.NAME, theHero.HGT, theHero.WGT, theHero.AGE, theHero.LDN, theHero.RDA, theHero.ORG_LAT, theHero.ORG_LONG );
        } else if (input == "view location") {
            textOnModule = String.Format("Your location:\n{0}\n\nTMP: {1}° C\nHUM: {2}% RH\nPSI: {3} kPa\nWND: {4}° & {5} km/s²\nPRC: {6} cm\nALT: {7} m\n\n> $", theLocation.NAME, theLocation.TMP, theLocation.HUM, theLocation.PSI, theLocation.WND_DIR, theLocation.WND_SPD, theLocation.PRC, theLocation.ALT );
        } else if (input == "view items") {
            textOnModule = String.Format("A) {0}\nB) {1}\nC) {2}\nD) {3}\nE) {4}\nF) {5}\nG) {6}\nH) {7}\nI) {8}\nJ) {9}\n\n> $", theItemNames[0], theItemNames[1], theItemNames[2], theItemNames[3], theItemNames[4], theItemNames[5], theItemNames[6], theItemNames[7], theItemNames[8], theItemNames[9]);
        } else if (input.StartsWith("use item ")) {
            switch (input) {
                case "use item a": if (theItemBools[0] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item A, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[0] = false; theItemNames[0] = theItemNames[0] + "!"; Debug.LogFormat("[The Console #{0}] You used item A, which was correct.", moduleId); } break;
                case "use item b": if (theItemBools[1] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item B, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[1] = false; theItemNames[1] = theItemNames[1] + "!"; Debug.LogFormat("[The Console #{0}] You used item B, which was correct.", moduleId); } break;
                case "use item c": if (theItemBools[2] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item C, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[2] = false; theItemNames[2] = theItemNames[2] + "!"; Debug.LogFormat("[The Console #{0}] You used item C, which was correct.", moduleId); } break;
                case "use item d": if (theItemBools[3] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item D, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[3] = false; theItemNames[3] = theItemNames[3] + "!"; Debug.LogFormat("[The Console #{0}] You used item D, which was correct.", moduleId); } break;
                case "use item e": if (theItemBools[4] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item E, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[4] = false; theItemNames[4] = theItemNames[4] + "!"; Debug.LogFormat("[The Console #{0}] You used item E, which was correct.", moduleId); } break;
                case "use item f": if (theItemBools[5] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item F, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[5] = false; theItemNames[5] = theItemNames[5] + "!"; Debug.LogFormat("[The Console #{0}] You used item F, which was correct.", moduleId); } break;
                case "use item g": if (theItemBools[6] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item G, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[6] = false; theItemNames[6] = theItemNames[6] + "!"; Debug.LogFormat("[The Console #{0}] You used item G, which was correct.", moduleId); } break;
                case "use item h": if (theItemBools[7] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item H, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[7] = false; theItemNames[7] = theItemNames[7] + "!"; Debug.LogFormat("[The Console #{0}] You used item H, which was correct.", moduleId); } break;
                case "use item i": if (theItemBools[8] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item I, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[8] = false; theItemNames[8] = theItemNames[8] + "!"; Debug.LogFormat("[The Console #{0}] You used item I, which was correct.", moduleId); } break;
                case "use item j": if (theItemBools[9] == false) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used item J, which was incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else { theItemBools[9] = false; theItemNames[9] = theItemNames[9] + "!"; Debug.LogFormat("[The Console #{0}] You used item J, which was correct.", moduleId); } break;
                default: Debug.Log("y'houre fool"); break;
            }
            textOnModule = String.Format("A) {0}\nB) {1}\nC) {2}\nD) {3}\nE) {4}\nF) {5}\nG) {6}\nH) {7}\nI) {8}\nJ) {9}\n\n> $", theItemNames[0], theItemNames[1], theItemNames[2], theItemNames[3], theItemNames[4], theItemNames[5], theItemNames[6], theItemNames[7], theItemNames[8], theItemNames[9]);
        } else if (input == "view weapons") {
            textOnModule = String.Format("A) {0}\nB) {1}\nC) {2}\nD) {3}\nE) {4}\n\n\n\n\n\n\n> $", theWeaponNames[0], theWeaponNames[1], theWeaponNames[2], theWeaponNames[3], theWeaponNames[4]);
        } else if (input.StartsWith("use weapon ")) {
            isItGood = true;
            for (int i = 0; i < 10; i++) {
                if (theItemBools[i] == true) {
                    isItGood = false;
                }
            }
            switch (input) {
                case "use weapon a": if (!isItGood) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon A, but you haven't used all the correct items. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else {
                    if ((theWeaponScores[0] >= theWeaponScores[1] && theWeaponScores[0] >= theWeaponScores[2]) && (theWeaponScores[0] >= theWeaponScores[3] && theWeaponScores[0] >= theWeaponScores[4])) {
                        GetComponent<KMBombModule>().HandlePass(); Debug.LogFormat("[The Console #{0}] You used weapon A, which is correct. Module solved!", moduleId); correctWeapon = theWeaponNames[0]; StartCoroutine(SolveAnim());
                    } else {
                        GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon A, which is incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim());
                    }
                }
                break;
                case "use weapon b": if (!isItGood) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon B, but you haven't used all the correct items. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else {
                    if ((theWeaponScores[0] >= theWeaponScores[2] && theWeaponScores[1] >= theWeaponScores[2]) && (theWeaponScores[1] >= theWeaponScores[3] && theWeaponScores[1] >= theWeaponScores[4])) {
                        GetComponent<KMBombModule>().HandlePass(); Debug.LogFormat("[The Console #{0}] You used weapon B, which is correct. Module solved!", moduleId); correctWeapon = theWeaponNames[1]; StartCoroutine(SolveAnim());
                    } else {
                        GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon B, which is incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim());
                    }
                }
                break;
                case "use weapon c": if (!isItGood) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon C, but you haven't used all the correct items. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else {
                    if ((theWeaponScores[2] >= theWeaponScores[0] && theWeaponScores[2] >= theWeaponScores[1]) && (theWeaponScores[2] >= theWeaponScores[3] && theWeaponScores[2] >= theWeaponScores[4])) {
                        GetComponent<KMBombModule>().HandlePass(); Debug.LogFormat("[The Console #{0}] You used weapon C, which is correct. Module solved!", moduleId); correctWeapon = theWeaponNames[2]; StartCoroutine(SolveAnim());
                    } else {
                        GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon C, which is incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim());
                    }
                }
                break;
                case "use weapon d": if (!isItGood) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon D, but you haven't used all the correct items. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else {
                    if ((theWeaponScores[3] >= theWeaponScores[0] && theWeaponScores[3] >= theWeaponScores[1]) && (theWeaponScores[3] >= theWeaponScores[2] && theWeaponScores[3] >= theWeaponScores[4])) {
                        GetComponent<KMBombModule>().HandlePass(); Debug.LogFormat("[The Console #{0}] You used weapon D, which is correct. Module solved!", moduleId); correctWeapon = theWeaponNames[3]; StartCoroutine(SolveAnim());
                    } else {
                        GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon D, which is incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim());
                    }
                }
                break;
                case "use weapon e": if (!isItGood) { GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon E, but you haven't used all the correct items. Strike!", moduleId); StartCoroutine(StrikeAnim()); } else {
                    if ((theWeaponScores[4] >= theWeaponScores[0] && theWeaponScores[4] >= theWeaponScores[1]) && (theWeaponScores[4] >= theWeaponScores[2] && theWeaponScores[4] >= theWeaponScores[3])) {
                        GetComponent<KMBombModule>().HandlePass(); Debug.LogFormat("[The Console #{0}] You used weapon E, which is correct. Module solved!", moduleId); correctWeapon = theWeaponNames[4]; StartCoroutine(SolveAnim());
                    } else {
                        GetComponent<KMBombModule>().HandleStrike(); Debug.LogFormat("[The Console #{0}] You used weapon E, which is incorrect. Strike!", moduleId); StartCoroutine(StrikeAnim());
                    }
                }
                break;
            }
        }
        input = "";
    }
    }


    IEnumerator StrikeAnim () {
        Back.GetComponent<MeshRenderer>().material = Mats[1];
        yield return new WaitForSeconds(0.5f);
        Back.GetComponent<MeshRenderer>().material = Mats[0];
    }

    IEnumerator SolveAnim () {
        Back.GetComponent<MeshRenderer>().material = Mats[2];
        textOnModule = "";
        solveMessage = String.Format("{0}\ndefeated\n{1}\nwith\n{2}\nin\n{3}\n\n> $", theHero.NAME, theEnemy.NAME, correctWeapon, theLocation.NAME);
        for (int i = 0; i < solveMessage.Length; i++) {
            textOnModule = textOnModule += solveMessage[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
}
