using Hugo_LAND.Client.HugoLandServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Hugo_LAND.Client
{
    public class GameState
    {
        public SizeF GameArea;
        public World World;
        public HeroDetailsDTO Hero;
        public ItemDetailsDTO Item;
        public List<ItemDetailsDTO> _inventory;
        private readonly InventoryServiceClient _serviceInventory = new InventoryServiceClient();
        public int Attack;
        public int Armour;
        public int Level;
        public int Health;
        public int Treasure;
        public int Potions;
        public bool HasBrownKey;
        public bool HasGreenKey;
        public bool HasRedKey;
        public bool GameIsWon;

        private int _experience;
        private int _nextUpgrade;
        private Sprite _experienceSprite;
        private Sprite _attackSprite;
        private Sprite _armourSprite;
        private Sprite _healthSprite;
        private Sprite _treasureSprite;
        private Sprite _potionSprite;
        private Sprite _brownKeySprite;
        private Sprite _greenKeySprite;
        private Sprite _redKeySprite;


        private Dictionary<string, Tile> _tiles = new Dictionary<string, Tile>();

        private static Font _font = new Font("Arial", 24);
        private static Brush _brush = new SolidBrush(Color.White);
        private static Random _random = new Random();



        public GameState(SizeF gameArea, HeroDetailsDTO hero)
        {
            GameArea = gameArea;
            Hero = hero;
            //Load inventory of player.
            _inventory = _serviceInventory.GetPlayerInventory(hero.Id).ToList();

            //Load in all the tile definitions
            readTileDefinitions(@"gamedata\tilelookups.csv");

            //Create the sprites for the UI
            int y = 50;
            _experienceSprite = new Sprite(this, 580, y, _tiles["Hero"].Bitmap, _tiles["Hero"].Rectangle, _tiles["Hero"].NumberOfFrames);
            _experienceSprite.ColorKey = Color.FromArgb(75, 75, 75);
            _healthSprite = new Sprite(this, 580, y += 74, _tiles["Food1"].Bitmap, _tiles["Food1"].Rectangle, _tiles["Food1"].NumberOfFrames);
            _healthSprite.ColorKey = Color.FromArgb(75, 75, 75);
            _attackSprite = new Sprite(this, 580, y += 74, _tiles["Attack"].Bitmap, _tiles["Attack"].Rectangle, _tiles["Attack"].NumberOfFrames);
            _attackSprite.ColorKey = Color.FromArgb(75, 75, 75);
            _armourSprite = new Sprite(this, 580, y += 74, _tiles["Armour"].Bitmap, _tiles["Armour"].Rectangle, _tiles["Armour"].NumberOfFrames);
            _armourSprite.ColorKey = Color.FromArgb(75, 75, 75);
            _treasureSprite = new Sprite(this, 580, y += 74, _tiles["Treasure2"].Bitmap, _tiles["Treasure2"].Rectangle, _tiles["Treasure2"].NumberOfFrames);
            _treasureSprite.ColorKey = Color.FromArgb(75, 75, 75);
            _potionSprite = new Sprite(this, 580, y += 74, _tiles["Potion"].Bitmap, _tiles["Potion"].Rectangle, _tiles["Potion"].NumberOfFrames);
            _potionSprite.ColorKey = Color.FromArgb(75, 75, 75);
            _brownKeySprite = new Sprite(this, 580, y += 74, _tiles["KeyBrown"].Bitmap, _tiles["KeyBrown"].Rectangle, _tiles["KeyBrown"].NumberOfFrames);
            _brownKeySprite.ColorKey = Color.FromArgb(75, 75, 75);
            _greenKeySprite = new Sprite(this, 654, y, _tiles["KeyGreen"].Bitmap, _tiles["KeyGreen"].Rectangle, _tiles["KeyGreen"].NumberOfFrames);
            _greenKeySprite.ColorKey = Color.FromArgb(75, 75, 75);
            _redKeySprite = new Sprite(this, 728, y, _tiles["KeyRed"].Bitmap, _tiles["KeyRed"].Rectangle, _tiles["KeyRed"].NumberOfFrames);
            _redKeySprite.ColorKey = Color.FromArgb(75, 75, 75);
        }

        //Experience property automatically upgrades your skill as the 'set' passes
        //the level threshold
        public int Experience
        {
            get
            {
                return _experience;
            }
            set
            {
                _experience = value;
                //If we hit the upgrade threshold then increase our abilities
                if (_experience > _nextUpgrade)
                {
                    Attack++;
                    Armour++;
                    //Each upgrade is a little harder to get
                    _nextUpgrade = _nextUpgrade + 20 * Level;
                    Level++;
                }
            }
        }

        public void Draw(Graphics graphics)
        {
            World.Draw(graphics);

            //Draw the HUD
            _experienceSprite.Draw(graphics);
            _healthSprite.Draw(graphics);
            _attackSprite.Draw(graphics);
            _armourSprite.Draw(graphics);
            _treasureSprite.Draw(graphics);
            _potionSprite.Draw(graphics);
            if (HasBrownKey) _brownKeySprite.Draw(graphics);
            if (HasGreenKey) _greenKeySprite.Draw(graphics);
            if (HasRedKey) _redKeySprite.Draw(graphics);
            int y = 65;
            graphics.DrawString(Experience.ToString(), _font, _brush, 650, y);
            graphics.DrawString(Health.ToString(), _font, _brush, 650, y += 74);
            graphics.DrawString(Attack.ToString(), _font, _brush, 650, y += 74);
            graphics.DrawString(Armour.ToString(), _font, _brush, 650, y += 74);
            graphics.DrawString(Treasure.ToString(), _font, _brush, 650, y += 74);
            graphics.DrawString(Potions.ToString(), _font, _brush, 650, y += 74);

            //If the game is won then show congratulations
            if (GameIsWon)
            {
                graphics.DrawString("You Won!", _font, _brush, 200, 250);
                graphics.DrawString("Press 's' to play again", _font, _brush, 100, 300);
            }

        }

        public void Update(double gameTime, double elapsedTime)
        {
            World.Update(gameTime, elapsedTime);
        }


        public void Initialize()
        {
            //Sounds.Start();
            Random _rnd = new Random();
            //Create all the main gameobjects
            World = new World(this, _tiles);
            //Reset the game state

            int CountTreasure = GetAllOneTypeItem("Treasure").Count();
            int CountPotion = GetAllOneTypeItem("Potion").Count();
            bool GreenKey = GetAllOneTypeItem("KeyGreen").Count() > 0;
            bool BrownKey = GetAllOneTypeItem("KeyBrown").Count() > 0;
            bool RedKey = GetAllOneTypeItem("KeyRed").Count() > 0;

            //double attack2 = (pourcent * ((double)Hero.StatDex / 100) * (double)Hero.StatStr);


            Potions = CountPotion; //-------
            Armour = Hero.StatReg;   // TODO : le lien avec l'inventaire ici du Hero (donc l'inventaire DTO)
            Treasure = CountTreasure; // ------
            Attack = Hero.StatStr; // TODO il faut le m�langer probablement avec le nombre d'�p�e dans l'inventaire du H�ro
            Experience = Hero.Experience;
            Level = Hero.Level;
            _nextUpgrade = 20;
            Health = Hero.StatVitality;
            HasGreenKey = GreenKey;
            HasBrownKey = BrownKey;
            HasRedKey = RedKey;
            GameIsWon = false;
        }

        //Each line contains a comma delimited tile definition that the tile constructor understands
        private void readTileDefinitions(string tileDescriptionFile)
        {
            using (StreamReader stream = new StreamReader(tileDescriptionFile))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    //separate out the elements of the 
                    string[] elements = line.Split(',');

                    //And make the tile.
                    Tile tile = new Tile(elements);
                    _tiles.Add(tile.Name, tile);
                }
            }
        }


        public void KeyDown(Keys keys)
        {
            //If the game is not over then allow the user to play
            if (Health > 0 && !GameIsWon)
            {
                World.KeyDown(keys);
            }
            else
            {
                //If game is over then allow S to restart 
                if (keys == Keys.S)
                {
                    Initialize();
                }
            }
        }

        private List<ItemDetailsDTO> GetAllOneTypeItem(string itemName) {
            try
            {
                return _inventory.Where(i => i.Nom == itemName).ToList();
            }
            catch
            {
                return new List<ItemDetailsDTO>();
            }
        }

        
    }
}
