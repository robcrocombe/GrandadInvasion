using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Weapons;


namespace GranddadInvasionNS
{

    public class GrandadInvasionGame : Microsoft.Xna.Framework.Game
    {
        static GameDifficulty difficulty = new GameDifficulty();
        static Random rnd = new Random();

        static GraphicsDeviceManager graphics;
        static SpriteBatch spriteBatch;
        public static Texture2D bGround;

        static List<Enemy> e = new List<Enemy>();
        static Enemy enemy = new Enemy();

        static List<Wire> wire = new List<Wire>();
        public static int wireCount = 0;

        static ParticleEngine engine;
        static Texture2D[] engineTex = new Texture2D[3];

        static GameState gameState;

        static UI UserInt = new UI();

        public static Weapon currentWeapon = new Weapon(WeaponType.Handgun);// Default to handgun

        public static Weapon handgun = new Weapon(WeaponType.Handgun);
        public static Weapon rifle = new Weapon(WeaponType.Rifle);
        public static Weapon machineGun = new Weapon(WeaponType.MachineGun);
        public static Weapon flameThrower = new Weapon(WeaponType.Flamethrower);
        public static Weapon grenade = new Weapon(WeaponType.Grenade);
        public static Weapon rpg = new Weapon(WeaponType.RPG);


        static public int getRandom(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public GrandadInvasionGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            TargetElapsedTime = TimeSpan.FromTicks(333333);

            InactiveSleepTime = TimeSpan.FromSeconds(1);

            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 480;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Init(Content);
            GameMenu.Initialize();
            EndGame.Initialize();

            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bGround = Content.Load<Texture2D>("background");
            engineTex[0] = Content.Load<Texture2D>("Blood/Blood16");
            engineTex[1] = Content.Load<Texture2D>("Blood/Blood8");
            engineTex[2] = Content.Load<Texture2D>("Blood/Blood4");
            engine = new ParticleEngine(engineTex, new Vector2(-10, -10));

            UserInt.Load(Content);
            GameMenu.LoadContent(Content);
            EndGame.LoadContent(Content);
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (gameState == GameState.menu)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    this.Exit();
                }
            }
            else if(gameState == GameState.game)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    gameState = GameState.menu;
                }

                UserInt.Update(gameTime);
            }

            if (gameState == GameState.endgame)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    EndGame.highscoreSet = false;
                    gameState = GameState.menu;
                }
            }

            if (gameState == GameState.game)
            {
                enemy.Update(difficulty, e, gameTime, Content, engine);
                for (int i = 0; i < e.Count; i++)
                {
                    e[i].Update(difficulty, e, gameTime, Content, engine);
                    if ((e[i].getBound().Y + e[i].getBound().Height) == 800 && wire.Count == 0)
                    {
                        gameState = GameState.endgame;
                        EndGame.Update(ScoreManager.GetScore());
                    }
                }
                if (wire.Count > 0 && e.Count > 0)
                {
                    wire[wire.Count - 1].Update(e, wire);
                }

                if (wire.Count > 0)
                {
                    wire[0].updateCount();
                }
                
            }
            else
            {
                if (gameState == GameState.menu || gameState == GameState.settings || gameState == GameState.highscores || gameState == GameState.about || gameState == GameState.intro)
                {
                    GameMenu.Update(ref gameState, Content);
                }
                else
                {
                    if(gameState == GameState.endgame)
                    {
                        EndGame.Update(ScoreManager.GetScore());
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (gameState == GameState.game)
            {
                spriteBatch.Draw(bGround, new Vector2(0, 0), Color.White);

                for (int i = e.Count - 1; i > -1; i--)
                {
                    e[i].Draw(spriteBatch, gameTime);
                }

                foreach (Wire w in wire)
                {
                    w.Draw(spriteBatch);
                }
                engine.Draw(spriteBatch);

                UserInt.Draw(spriteBatch, gameTime);
            }
            else
            {
                if (gameState == GameState.menu || gameState == GameState.settings || gameState == GameState.highscores || gameState == GameState.about || gameState == GameState.intro)
                {
                    GameMenu.Draw(spriteBatch, gameState, gameTime);
                }
                else
                {
                    if(gameState == GameState.endgame)
                    {
                        EndGame.Draw(spriteBatch);
                    }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void Init(ContentManager Content)
        {
            difficulty = GameDifficulty.Easy; //DEFAULT AT THE MOMENT
            enemy.Load(Content);
            Enemy temp = new Enemy();
            temp.Init();
            temp.Load(Content);
            e.Add(temp);

            wire.Clear();
            Wire w1 = new Wire();
            w1.Init(new Vector2(0, 650));
            w1.Load(Content);
            wire.Add(w1);
            Wire w2 = new Wire();
            w2.Init(new Vector2(0, 610));
            w2.Load(Content);
            wire.Add(w2);
            Wire w3 = new Wire();
            w3.Init(new Vector2(0, 570));
            w3.Load(Content);
            wire.Add(w3);

            gameState = GameState.menu;

            UserInt.UIInitialise();
        }
    }
}
