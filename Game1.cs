﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using projet_MonoGame.Models;
using projet_MonoGame.Sprites;
using System;
using System.IO;

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
    Dictionary<string, string> listMap = new Dictionary<string, string>();
    public Vector2 posMap1 = new Vector2(100, 550);
    public Vector2 posEndMap1 = new Vector2(1100,550);
    public Vector2 posMap2 = new Vector2(100, 600);
    public Vector2 posEndMap2 = new Vector2(1100,600);
    public const int windowWidth = 1200;
    public const int windowHeight = 800;
    public int ground = 550;
    public Texture2D bgAccueil;
    public Texture2D logoRetro2;
    public Texture2D knight;
    public Texture2D bgGame;
    public int curMap = 0;
    public Texture2D lifeBar;
    public int logoWidth = 300;
    public int logoHeight = 150;
    public int lifeWidth = 90;
    public int lifeHeight = 20;
    public Song song;
    public Song songIntro;
    public int jumpTime = 0;
    public int attackTime = 0;
    public string curSong = "";
    public int time = 0;
    List<SoundEffect> soundEffects;
    public Vector2 position = new Vector2(0, 0);
    private GameState _gameState = GameState.MainMenu;
    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = false;
      soundEffects = new List<SoundEffect>();
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
        { "AttakR", new Animation(Content.Load<Texture2D>("Player/AttakR"), 6)},
        { "DieR", new Animation(Content.Load<Texture2D>("Player/DieR"), 6)},
        { "DieL", new Animation(Content.Load<Texture2D>("Player/DieL"), 6)},
      };
      bgGame = Content.Load<Texture2D>("Background/testGamePlay");
      bgAccueil = Content.Load<Texture2D>("Background/bg_accueil");
      logoRetro2 = Content.Load<Texture2D>("logoRetro2");
      knight = Content.Load<Texture2D>("Player/Knight");
      lifeBar = Content.Load<Texture2D>("Player/life/full_life");
      song = Content.Load<Song>("Music/OST/Zelda_OST");
      songIntro = Content.Load<Song>("Music/OST/Zelda_Main");
      MediaPlayer.Volume = 0.2f;
      MediaPlayer.Play(songIntro);
      MediaPlayer.IsRepeating = true;
      soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Jump"));
      soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Sword"));
      SoundEffect.MasterVolume = 0.2f;
      //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

      _sprites = new List<Sprite>()
      {
        new Sprite(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Player/RunL"), 8) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Player/RunR"), 8) },
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Player/AttakL2"), 6) },
          { "AttackRight", new Animation(Content.Load<Texture2D>("Player/AttakR"), 6) },
          { "DieRight", new Animation(Content.Load<Texture2D>("Player/DieR"), 6)},
          { "DieLeft", new Animation(Content.Load<Texture2D>("Player/DieL"), 6)},
        })
        {
          Position = new Vector2(50, ground),
          Input = new Input()
          {
            Left = Keys.Q,
            Right = Keys.D,
            Up = Keys.Space,
            A = Keys.A,
            E = Keys.E,
          },
        },
      };
      initMap();
    }

    void initMap()
    {
      listMap.Add("1", "Background/gamleplay_1");
      listMap.Add("2", "Background/gamleplay_2");
    }

    void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
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
          if(time < 60){
          time +=1;
          }
          if(curSong != "MainMenu"){
            MediaPlayer.Play(songIntro);
            curSong = "MainMenu";
          }
        if(state.IsKeyDown(Keys.Space)){
          curMap = 1;
          _gameState = GameState.GamePlay;
        }
        if (state.IsKeyDown(Keys.Escape) && time >= 60){
          Exit();
        }
          break;
        }
        case GameState.GamePlay:
        { 
          if(_sprites[0].Position.X >= windowWidth-50 && curMap < 2){
            bgGame = Content.Load<Texture2D>("Background/gameplay_2");
            if(curMap == 1){
              _sprites[0].Position = posMap2;
            }
            curMap+=1;
          }
          else if(_sprites[0].Position.X <= 50 && curMap > 1){
            bgGame = Content.Load<Texture2D>("Background/gameplay_1");
            if(curMap == 2){
              _sprites[0].Position = posEndMap1;
            }
            curMap-=1;
          }
          if (state.IsKeyDown(Keys.Space) && jumpTime == 0){
            soundEffects[0].CreateInstance().Play();
            jumpTime += 1;
          }
          if (jumpTime > 0){
            if(jumpTime == 55)
              jumpTime = 0;
            else
              jumpTime += 1;
          }
          if(state.IsKeyDown(Keys.A) && attackTime == 0){
            soundEffects[1].CreateInstance().Play();
            attackTime += 1;
          }
          if (attackTime > 0){
            if(attackTime == 40)
              attackTime = 0;
            else
              attackTime += 1;
          }
          if(curSong != "GamePlay"){
            time = 0;
            MediaPlayer.Play(song);
            curSong = "GamePlay";
          }
          if(state.IsKeyDown(Keys.Escape)){
            _gameState = GameState.MainMenu;
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
                    _spriteBatch.Draw(bgAccueil, new Rectangle(0 ,0 ,windowWidth ,windowHeight), Color.White);
                    //_spriteBatch.Draw(logoRetro2, new Rectangle((windowWidth/2)-(logoWidth/2) , (windowHeight/2)-(logoHeight/2)+150,logoWidth ,logoHeight), Color.White);
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
                  spriteBatch.Draw(lifeBar, new Rectangle((int)Math.Truncate(_sprites[0].Position.X)-10 ,(int)Math.Truncate(_sprites[0].Position.Y)-10 ,lifeWidth ,lifeHeight), Color.White);
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
