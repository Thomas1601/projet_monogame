using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using projet_MonoGame.Models;
using projet_MonoGame.Sprites;
using System;

namespace projet_MonoGame
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    private SpriteBatch _spriteBatch;

    private List<Sprite> _sprites;

    public const int windowWidth = 1200;
        public const int windowHeight = 800;
        public Texture2D bgTexture;
        public Texture2D logoRetro2;
        public Texture2D knight;
        public static double gravity = 0;
        public Texture2D bgGame;
        public int ground = 550;
        public int logoWidth = 300;
        public int logoHeight = 150;
        public Vector2 position = new Vector2(0, 0);
        private GameState _gameState = GameState.MainMenu;
    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = false;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here
      graphics.PreferredBackBufferHeight = windowHeight;
      graphics.PreferredBackBufferWidth = windowWidth;
      graphics.ApplyChanges();
      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      // NOTE: I no-longer use this reference as it affects different objects if being used multiple times!
      var animations = new Dictionary<string, Animation>()
      {
        { "RunL", new Animation(Content.Load<Texture2D>("Player/RunL"), 8) },
        { "RunR", new Animation(Content.Load<Texture2D>("Player/RunR"), 8) },
        { "AttakL2", new Animation(Content.Load<Texture2D>("Player/AttakL2"), 6)},
      };
      bgTexture = Content.Load<Texture2D>("Background/bg");
      bgGame = Content.Load<Texture2D>("Background/testGamePlay");
      logoRetro2 = Content.Load<Texture2D>("logoRetro2");
      knight = Content.Load<Texture2D>("Player/Knight");

      _sprites = new List<Sprite>()
      {
        new Sprite(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Player/RunL"), 8) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Player/RunR"), 8) },
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Player/AttakL2"), 6) },
        })
        {
          Position = new Vector2(50, ground),
          Input = new Input()
          {
            Left = Keys.Q,
            Right = Keys.D,
            Up = Keys.Space,
            A = Keys.A,
          },
        },
      };
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      KeyboardState state = Keyboard.GetState();
      switch(_gameState)
      {
        case GameState.MainMenu:
        {
        if(state.IsKeyDown(Keys.Space)){
          _gameState = GameState.GamePlay;
        }
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
          Exit();
          break;
        }
        case GameState.GamePlay:
        { 
          if((int)position.Y < ground){
            gravity += 0.20;
            position.Y += (int)Math.Truncate(gravity);
          }
          if((int)position.Y >= ground){
            gravity = 0.0;
          }
          foreach (var sprite in _sprites)
            sprite.Update(gameTime, _sprites);
          break;
        }
        default: break;
      }
      
      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      switch(_gameState)
            {
                case GameState.MainMenu:
                {
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(bgTexture, new Rectangle(0 ,0 ,1200 ,800), Color.White);
                    _spriteBatch.Draw(logoRetro2, new Rectangle((windowWidth/2)-(logoWidth/2) , (windowHeight/2)-(logoHeight/2),logoWidth ,logoHeight), Color.White);
                    _spriteBatch.End();
                    base.Draw(gameTime);
                    break;
                }
                case GameState.GamePlay:
                {
                  spriteBatch.Begin();
                  spriteBatch.Draw(bgGame, new Rectangle(0 ,0 ,windowWidth ,windowHeight), Color.White);
                  foreach (var sprite in _sprites)
                    sprite.Draw(spriteBatch);
                  spriteBatch.End();
                  base.Draw(gameTime);
                  break;
                }
                default: break;
            
            }

      spriteBatch.Begin();

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
