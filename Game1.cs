using Microsoft.Xna.Framework;
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

    private int cptImmune = 100;

    private int cptHit = 0;

    private List<SpriteMonster> _spritesSanglier;

    private List<SpriteMonster> _spritesEmptySanglier;

    private List<SpriteAnimation> _spritesDie;

    private List<SpriteAnimation> _spritesCascade;

    private List<SpriteMonster> _spritesGolem;

    private List<Sprite> _spritesEmpty;

    private int cptMort = 50;

    private Vector2 realPosition;

    private int lifePoints = 5; 
    private int lpSanglier = 3;
    private int lpGolem = 5;
    private bool dead = false;
    private int compteurCascade;
    private SoundEffectInstance soundCascade;
    public int ground = 550;
    public string runCascade = "N";
    public Vector2 posMap1 = new Vector2(100, 550);
    public Vector2 posEndMap1 = new Vector2(1100, 550);
    public Vector2 posMap2 = new Vector2(100, 550);
    public Vector2 posEndMap2 = new Vector2(1100, 550);
    public Vector2 posTmp;
    public const int windowWidth = 1200;
    public const int windowHeight = 800;
    public int distance;
    public Texture2D bgAccueil;
    public Texture2D bgDied;
    public Texture2D bgWin;
    public Texture2D logoRetro2;
    public Texture2D knight;
    public Texture2D bgGame;
    public int curMap = 0;
    public string direction;
    public Texture2D lifeBar;
    public int logoWidth = 300;
    public int logoHeight = 150;
    public int lifeWidth = 100;
    public int lifeHeight = 25;
    public int compteurAttack = 0;
    public Song song;
    public Song songIntro;
    public int jumpTime = 0;
    public int attackTime = 0;
    public string curSong = "";
    public int time = 0;
    List<SoundEffect> soundEffects;
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
      //Window.AllowUserResizing = true;
      graphics.PreferredBackBufferHeight = windowHeight;
      graphics.PreferredBackBufferWidth = windowWidth;
      graphics.ApplyChanges();
      direction = "L";
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
      
      bgGame = Content.Load<Texture2D>("Background/bg_waterbridge");
      bgAccueil = Content.Load<Texture2D>("Background/bg_accueil");
      bgDied = Content.Load<Texture2D>("Background/youdied");
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
      soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Waterfall"));
      soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Hurt"));
      soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Die"));
      SoundEffect.MasterVolume = 0.06f;
      //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

      _sprites = new List<Sprite>()
      {
        new Sprite(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Player/walkL"), 7) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Player/walkR"), 7) },
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Player/attackL"), 7)},
          { "AttackRight", new Animation(Content.Load<Texture2D>("Player/attackR"), 7) },
          { "DefLeft", new Animation(Content.Load<Texture2D>("Player/DefL"), 5) },
          { "DefRight", new Animation(Content.Load<Texture2D>("Player/DefR"), 5) },
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

      _spritesSanglier = new List<SpriteMonster>()
      {
        new SpriteMonster(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Monster/Sanglier_RunL"), 8) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Monster/Sanglier_Run"), 8) },
          { "DieRight", new Animation(Content.Load<Texture2D>("Monster/Sanglier_DieR"), 4)},
          { "DieLeft", new Animation(Content.Load<Texture2D>("Monster/Sanglier_DieL"), 4)},
        })
        {
          Position = new Vector2(1100, ground-15),
        },
      };
      _spritesSanglier[0].Speed = 5.2f;

      _spritesEmptySanglier = new List<SpriteMonster>()
      {
        new SpriteMonster(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 8) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 8) },
          { "DieRight", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 8)},
          { "DieLeft", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 8)},
        })
        {
          Position = new Vector2(1100, ground-15),
        },
      };

      _spritesCascade = new List<SpriteAnimation>()
      {
        new SpriteAnimation(new Dictionary<string, Animation>()
        {
          { "AnimL", new Animation(Content.Load<Texture2D>("Background/Waterfall"), 9) },
          { "AnimR", new Animation(Content.Load<Texture2D>("Background/Waterfall"), 9) },
        }, "LEFT")
        {
          Position = new Vector2(400, ground-200),
        },
      };

      _spritesGolem = new List<SpriteMonster>()
      {
        new SpriteMonster(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Monster/Golem_walk_L"), 11) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Monster/Golem_walk_R"), 11) },
          { "DieRight", new Animation(Content.Load<Texture2D>("Monster/Golem_die_R"), 5)},
          { "DieLeft", new Animation(Content.Load<Texture2D>("Monster/Golem_die_L"), 5)},
        })
        {
          Position = new Vector2(1100, ground-13),
        },
      };
      _spritesGolem[0].Speed = 2.3f;

      _spritesEmpty = new List<Sprite>()
      {
        new Sprite(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 8) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 8) },
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 6)},
          { "AttackRight", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 6) },
          { "DieRight", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 6)},
          { "DieLeft", new Animation(Content.Load<Texture2D>("Player/spriteVide"), 6)},
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
    }


    void UpdateMonster()
    {
      if(curMap == 1)
      {
        if(_spritesSanglier[0].Position.X <= 600){
          _spritesSanglier[0].direction = "RIGHT";
        }
        else if(_spritesSanglier[0].Position.X >= 1101){
          _spritesSanglier[0].direction = "LEFT";
        }
      }
      else if (curMap == 2)
      {
        if(_spritesGolem[0].Position.X <= 600){
          _spritesGolem[0].direction = "RIGHT";
        }
        else if(_spritesGolem[0].Position.X >= 1101){
          _spritesGolem[0].direction = "LEFT";
        }
      }
    }

    void Dead(string dir)
    {
      if(dir == "L")
      {
      _spritesDie = new List<SpriteAnimation>()
      {
        new SpriteAnimation(new Dictionary<string, Animation>()
        {
          { "AnimL", new Animation(Content.Load<Texture2D>("Player/DieL"), 9) },
          { "AnimR", new Animation(Content.Load<Texture2D>("Player/DieR"), 9) },
        }, "LEFT")
        {
          Position = new Vector2(_sprites[0].Position.X, ground+20),
        },
      };
      }
      else if(dir == "R")
      {
         _spritesDie = new List<SpriteAnimation>()
      {
        new SpriteAnimation(new Dictionary<string, Animation>()
        {
          { "AnimL", new Animation(Content.Load<Texture2D>("Player/DieL"), 9) },
          { "AnimR", new Animation(Content.Load<Texture2D>("Player/DieR"), 9) },
        }, "RIGHT")
        {
          Position = new Vector2(_sprites[0].Position.X, ground+20),
        },
      };
      }
    }


    public static float Distance(Vector2 value1, Vector2 value2)
    {
      float v1 = value1.X - value2.X;
      float v2 = value1.Y - value2.Y;
	    return (float)Math.Sqrt((v1 * v1) + (v2 * v2));
    }

    void DecreaseLifebar()
    {
      if(cptImmune == 100)
      {
        cptImmune = 99;
        if(lifePoints == 5)
        {
          lifePoints -= 1;
          lifeBar = Content.Load<Texture2D>("Player/life/semi_full");
          soundEffects[3].CreateInstance().Play();
        }
        else if(lifePoints == 4)
        {
          lifePoints -= 1;
          lifeBar = Content.Load<Texture2D>("Player/life/mid_life");
          soundEffects[3].CreateInstance().Play();
        }
        else if(lifePoints == 3)
        {
          lifePoints -= 1;
          lifeBar = Content.Load<Texture2D>("Player/life/semi_low");
          soundEffects[3].CreateInstance().Play();
        }
        else if(lifePoints == 2)
        {
          lifePoints -= 1;
          lifeBar = Content.Load<Texture2D>("Player/life/low_life");
          soundEffects[3].CreateInstance().Play();
        }
        else if(lifePoints == 1)
        {
          lifePoints -= 1;
          lifeBar = Content.Load<Texture2D>("Player/life/dead_life");
          soundEffects[4].CreateInstance().Play();
          Dead(direction);
          dead = true;
        }
      }
    }

    void DecreaseMonsterLifebar()
    {
      if(curMap == 1)
      {
        lpSanglier -= 1;
        if(lpSanglier <= 0)
        {
          _spritesSanglier = _spritesEmptySanglier;
        }
      }
      else if(curMap == 2)
      {
        lpGolem -= 1;
        if(lpGolem <= 0)
        {
          _spritesGolem = _spritesEmptySanglier;
        }
      }
    }

    void Attack()
    {
      KeyboardState state = Keyboard.GetState();
      if(curMap == 1)
      {
        var nTmp = Distance(_spritesSanglier[0].Position, _sprites[0].Position);
        if(nTmp < 30.0 && lpSanglier > 0)
        {
          if(state.IsKeyDown(Keys.A)){
            if(cptHit == 0)
            {
              DecreaseMonsterLifebar();
              cptHit = 25;
            } 
          }  
        }
      }
      else if(curMap == 2)
      {
        var nTmp = Distance(_spritesGolem[0].Position, _sprites[0].Position);
        if(nTmp < 30.0 && lpGolem > 0)
        {
          if(state.IsKeyDown(Keys.A)){
            if(cptHit == 0)
            {
              DecreaseMonsterLifebar();
              cptHit = 25;
            } 
          }  
        }
      }
    }

    void Replay()
    {
      lifePoints = 5;
      lpGolem = 5;
      lpSanglier = 3;
      lifeBar = Content.Load<Texture2D>("Player/life/full_life");
      direction = "L";
      cptMort = 50;
      dead = false;
      cptMort = 50;
      curMap = 1;
      bgGame = Content.Load<Texture2D>("Background/bg_waterbridge");
      _spritesSanglier = new List<SpriteMonster>()
      {
        new SpriteMonster(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Monster/Sanglier_RunL"), 8) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Monster/Sanglier_Run"), 8) },
          { "DieRight", new Animation(Content.Load<Texture2D>("Monster/Sanglier_DieR"), 4)},
          { "DieLeft", new Animation(Content.Load<Texture2D>("Monster/Sanglier_DieL"), 4)},
        })
        {
          Position = new Vector2(1100, ground-15),
        },
      };
      _spritesSanglier[0].Speed = 5.2f;

      _sprites = new List<Sprite>()
      {
        new Sprite(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Player/walkL"), 7) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Player/walkR"), 7) },
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Player/attackL"), 7)},
          { "AttackRight", new Animation(Content.Load<Texture2D>("Player/attackR"), 7) },
          { "DefLeft", new Animation(Content.Load<Texture2D>("Player/DefL"), 5) },
          { "DefRight", new Animation(Content.Load<Texture2D>("Player/DefR"), 5) },
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

      _spritesGolem = new List<SpriteMonster>()
      {
        new SpriteMonster(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Monster/Golem_walk_L"), 11) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Monster/Golem_walk_R"), 11) },
          { "DieRight", new Animation(Content.Load<Texture2D>("Monster/Golem_die_R"), 5)},
          { "DieLeft", new Animation(Content.Load<Texture2D>("Monster/Golem_die_L"), 5)},
        })
        {
          Position = new Vector2(1100, ground-13),
        },
      };
      _spritesGolem[0].Speed = 2.3f;
    }

    void IsDamaged()
    {
      KeyboardState state = Keyboard.GetState();
      if(curMap == 1)
      {
        var nTmp = Distance(_spritesSanglier[0].Position, _sprites[0].Position);
        if(nTmp < 30.0 && lpSanglier > 0)
        {
          if(! state.IsKeyDown(Keys.E)){
            if(! state.IsKeyDown(Keys.A))
            {
              DecreaseLifebar();
            }
          }  
        }
      }
      else if(curMap == 2)
      {

      }
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
          Replay();
        }
        if (state.IsKeyDown(Keys.Escape) && time >= 60){
          Exit();
        }
          break;
        }
        case GameState.GameDie:
        {
          if(curSong != "GameDie"){
            MediaPlayer.Play(songIntro);
            curSong = "GameDie";
          }
          if(state.IsKeyDown(Keys.Escape))
          {
            _gameState = GameState.MainMenu;
          }
          break;
        }
        case GameState.EndGame:
        {
          if(curSong != "GameDie"){
            MediaPlayer.Play(songIntro);
            curSong = "GameDie";
          }
          if(state.IsKeyDown(Keys.Escape))
          {
            _gameState = GameState.MainMenu;
          }
          break;
        }
        case GameState.GamePlay:
        { 
          if(cptHit > 0)
          {
            cptHit -= 1;
          }
          if(state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Q))
          {
            realPosition = new Vector2(_sprites[0].Position.X+68, _sprites[0].Position.Y+25);
          }
          else if(state.IsKeyDown(Keys.A) && direction == "L")
          {
            if(curMap == 1 && lpSanglier > 0)
            {
              if(_sprites[0].Position.X > _spritesSanglier[0].Position.X)
              {
                Attack();
              }
            }
            if(curMap == 2 && lpGolem > 0)
            {
              if(_sprites[0].Position.X > _spritesGolem[0].Position.X)
              {
                Attack();
              }
            }
            realPosition = new Vector2(_sprites[0].Position.X+68, _sprites[0].Position.Y+25);
          }
          else if(state.IsKeyDown(Keys.A) && direction == "R")
          {
            if(curMap == 1 && lpSanglier > 0)
            {
              if(_sprites[0].Position.X < _spritesSanglier[0].Position.X)
              {
                Attack();
              }
            }
            if(curMap == 2 && lpGolem > 0)
            {
              if(_sprites[0].Position.X < _spritesGolem[0].Position.X)
              {
                Attack();
              }
            }
            realPosition = new Vector2(_sprites[0].Position.X+48, _sprites[0].Position.Y+25);
          }
          else if(direction == "R")
          {
            realPosition = new Vector2(_sprites[0].Position.X+60, _sprites[0].Position.Y+25);
          }
          else
          {
            realPosition = new Vector2(_sprites[0].Position.X+68, _sprites[0].Position.Y+25);
          }
          //realPosition = new Vector2(_sprites[0].Position.X+50, _sprites[0].Position.Y+30);
          if(cptImmune == 0)
          {
            cptImmune = 100;
          }
          else if(cptImmune < 100)
          {
            cptImmune -= 1;
          }
          IsDamaged();
          if(dead == true)
          {
            if(cptMort > 0)
            {
              cptMort -= 1;
              _spritesDie[0].Update(gameTime, _spritesDie);
            }
            if(cptMort == 0)
            {
              lifeBar.Dispose();
              _gameState = GameState.GameDie;
              Replay();
            }
          }
          if(curMap == 1)
          {
            _spritesCascade[0].Update(gameTime, _spritesCascade);
          }
          UpdateMonster();
          if(state.IsKeyDown(Keys.D))
          {
            direction = "R";
          }
          if(state.IsKeyDown(Keys.Q))
          {
            direction = "L";
          }
          if(_sprites[0].Position.X >= windowWidth-50 && curMap == 2)
          {
          _gameState = GameState.EndGame;
          }
          if(_sprites[0].Position.X >= windowWidth-50 && curMap < 2){
            bgGame = Content.Load<Texture2D>("Background/forest_stone");
            if(curMap == 1){
              _sprites[0].Position = posMap2;
            }
            curMap+=1;
          }
          else if(_sprites[0].Position.X <= 50 && curMap > 1){
            bgGame = Content.Load<Texture2D>("Background/bg_waterbridge");
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
          
          if(state.IsKeyDown(Keys.A) && attackTime == 0)
          {
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
          //foreach (var sprite in _sprites)
          //{
        _sprites[0].Update(gameTime, _sprites);
          //}
        if(curMap == 1)
        {
          if(lpSanglier > 0)
            _spritesSanglier[0].Update(gameTime, _spritesSanglier);
        }
        else if(curMap == 2)
        {
          soundCascade.Stop();
          if(lpGolem > 0)
            _spritesGolem[0].Update(gameTime, _spritesGolem);
        }
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
                case GameState.GameDie:
                {
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(bgDied, new Rectangle(0 ,0 ,windowWidth ,windowHeight), Color.White);
                    //_spriteBatch.Draw(logoRetro2, new Rectangle((windowWidth/2)-(logoWidth/2) , (windowHeight/2)-(logoHeight/2)+150,logoWidth ,logoHeight), Color.White);
                    _spriteBatch.End();
                    base.Draw(gameTime);
                    break;
                }
                case GameState.EndGame:
                {
                  _spriteBatch.Begin();
                  bgWin = Content.Load<Texture2D>("Background/realWin");
                  _spriteBatch.Draw(bgWin, new Rectangle(0 ,0 ,windowWidth ,windowHeight), Color.White);
                    //_spriteBatch.Draw(logoRetro2, new Rectangle((windowWidth/2)-(logoWidth/2) , (windowHeight/2)-(logoHeight/2)+150,logoWidth ,logoHeight), Color.White);
                  _spriteBatch.End();
                  base.Draw(gameTime);
                  break;
                }
                case GameState.GamePlay:
                {
                  soundCascade = soundEffects[2].CreateInstance();
                  spriteBatch.Begin();
                  spriteBatch.Draw(bgGame, new Rectangle(0 ,0 ,windowWidth ,windowHeight), Color.White);
                  if(curMap == 1)
                  {
                    foreach (var sprite in _spritesCascade)
                      sprite.Draw(spriteBatch);
                  }
                  if(dead != true)
                  {
                    foreach (var sprite in _sprites)
                      sprite.Draw(spriteBatch);
                  }
                  if(curMap == 1){
                    if(runCascade == "N")
                    {
                      soundCascade.Play();
                      runCascade = "Y";
                    }
                    else
                    {
                      compteurCascade +=1;
                    }
                    if(compteurCascade >=3400)
                    {
                      runCascade = "N";
                      compteurCascade = 0;
                    }
                    if(lpSanglier > 0)
                    {
                      foreach (var sprite in _spritesSanglier)
                        sprite.Draw(spriteBatch);
                    }
                    
                  }
                  else if(curMap == 2){
                    runCascade = "N";
                    foreach (var sprite in _spritesGolem)
                      sprite.Draw(spriteBatch);
                  }
                  spriteBatch.Draw(lifeBar, new Rectangle((int)Math.Truncate(realPosition.X) ,(int)Math.Truncate(realPosition.Y)-20,lifeWidth ,lifeHeight), Color.White);
                  if(dead == true)
                  {
                    foreach (var sprite in _spritesDie)
                      sprite.Draw(spriteBatch);
                  }
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
