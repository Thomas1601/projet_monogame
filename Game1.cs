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

    private List<SpriteMonster> _spritesSanglier;

    private List<SpriteAnimation> _spritesAnimation;

    private List<SpriteAnimation> _spritesDie;

    private List<SpriteAnimation> _spritesCascade;

    private List<SpriteMonster> _spritesGolem;

    private List<Sprite> _spritesEmpty;

    private int cptMort = 28;

    private Vector2 realPosition;

    private int lifePoints = 5; 
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
    public Texture2D logoRetro2;
    public Texture2D knight;
    public Texture2D bgGame;
    public int curMap = 0;
    public string direction;
    public Texture2D lifeBar;
    public int logoWidth = 300;
    public int logoHeight = 150;
    public int lifeWidth = 90;
    public int lifeHeight = 20;
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
      soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Waterfall"));
      soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Hurt"));
      soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Die"));
      SoundEffect.MasterVolume = 0.08f;
      //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

      _sprites = new List<Sprite>()
      {
        new Sprite(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Player/walkL"), 7) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Player/walkR"), 7) },
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Player/attackL"), 7)},
          { "AttackRight", new Animation(Content.Load<Texture2D>("Player/attackR"), 7) },
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
      _spritesSanglier[0].Speed = 6.0f;

      _spritesAnimation = new List<SpriteAnimation>()
      {
        new SpriteAnimation(new Dictionary<string, Animation>()
        {
          { "AnimL", new Animation(Content.Load<Texture2D>("Player/AnimAttack/impactL"), 7) },
          { "AnimR", new Animation(Content.Load<Texture2D>("Player/AnimAttack/impactL"), 7) },
        }, "LEFT")
        {
          Position = new Vector2(_sprites[0].Position.X-10, ground-15),
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
          Position = new Vector2(200, ground-120),
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
      _spritesGolem[0].Speed = 2.5f;

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
          { "AnimL", new Animation(Content.Load<Texture2D>("Player/DieL"), 6) },
          { "AnimR", new Animation(Content.Load<Texture2D>("Player/DieR"), 6) },
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
          { "AnimL", new Animation(Content.Load<Texture2D>("Player/DieL"), 6) },
          { "AnimR", new Animation(Content.Load<Texture2D>("Player/DieR"), 6) },
        }, "RIGHT")
        {
          Position = new Vector2(_sprites[0].Position.X, ground+20),
        },
      };
      }
    }


    void runAnimation(Vector2 posTmp)
    {
      distance = -1;
      if(curMap == 1)
      {
        if(direction == "R")
        {
          distance = (int)_spritesSanglier[0].Position.X - (int)_sprites[0].Position.X;
        }
        else
        {
          distance = (int)_sprites[0].Position.X - (int)_spritesSanglier[0].Position.X;
        }
        if(distance <= 15 && distance >= 0)
        {
          if(direction == "R")
          {
            _spritesAnimation[0].direction = "RIGHT";
          }
          else if(direction =="L")
          {
            _spritesAnimation[0].direction = "LEFT";
          }
          _spritesAnimation[0].Position = posTmp;
        }
      }
      else if(curMap == 2)
      {
        if(direction == "R")
        {
          distance = (int)_spritesGolem[0].Position.X - (int)_sprites[0].Position.X;
        }
        else
        {
          distance = (int)_sprites[0].Position.X - (int)_spritesGolem[0].Position.X;
        }
        if(distance <= 20 && distance >= 0)
        {
          if(direction == "R")
          {
            posTmp = new Vector2(_spritesGolem[0].Position.X, _spritesGolem[0].Position.Y - 40);
            _spritesAnimation[0].direction = "RIGHT";
          }
          else
          {
            posTmp = new Vector2(_spritesGolem[0].Position.X, _spritesGolem[0].Position.Y - 40);
            _spritesAnimation[0].direction = "LEFT";
          }
          _spritesAnimation[0].Position = posTmp;
        }
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

    void IsDamaged()
    {
      if(curMap == 1)
      {
        var nTmp = Distance(_spritesSanglier[0].Position, _sprites[0].Position);
        //using (StreamWriter sw = new StreamWriter("myFile.txt", true)) {
        //    sw.WriteLine(nTmp + "");
        // }
        if(nTmp < 30.0)
        {
          DecreaseLifebar();
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
        }
        if (state.IsKeyDown(Keys.Escape) && time >= 60){
          Exit();
        }
          break;
        }
        case GameState.GamePlay:
        { 
          realPosition = new Vector2(_sprites[0].Position.X+50, _sprites[0].Position.Y+30);
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
          //if(_sprites[0].Position.X >= windowWidth-50 && curMap == 2)
          //{
          //_sprites = _spritesEmpty;
          //lifeBar.Dispose();
          //}
          if(_sprites[0].Position.X >= windowWidth-50 && curMap < 2){
            bgGame = Content.Load<Texture2D>("Background/forest_stone");
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
          
          if(state.IsKeyDown(Keys.A) && compteurAttack < 8)
          {
            if(compteurAttack == 0)
            {
              posTmp = new Vector2(_spritesSanglier[0].Position.X, _spritesSanglier[0].Position.Y - 35);
            }
            compteurAttack += 1;
            runAnimation(posTmp);
            _spritesAnimation[0].Update(gameTime, _spritesAnimation);
          }
          else if(compteurAttack > 0 && compteurAttack < 8)
          {
            compteurAttack += 1;
            runAnimation(posTmp);
            _spritesAnimation[0].Update(gameTime, _spritesAnimation);
          }
          if(compteurAttack > 7)
          {
            _spritesAnimation[0].direction = "None";
            compteurAttack = 0;
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
          {
            sprite.Update(gameTime, _sprites);
          }
        if(curMap == 1)
        {
          _spritesSanglier[0].Update(gameTime, _spritesSanglier);
        }
        else if(curMap == 2)
        {
          soundCascade.Stop();
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
                    foreach (var sprite in _spritesSanglier)
                      sprite.Draw(spriteBatch);
                  }
                  else if(curMap == 2){
                    runCascade = "N";
                    foreach (var sprite in _spritesGolem)
                      sprite.Draw(spriteBatch);
                  }
                  foreach (var sprite in _spritesAnimation)
                      sprite.Draw(spriteBatch);
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
