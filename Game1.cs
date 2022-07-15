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
using System.Text;
using System.Drawing;

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
    private SpriteFont _font;
    private List<SpriteAnimation> _spritesRock;
    private int _cptImmune = 100;
    private int _cptHit = 0;
    private List<SpriteMonster> _spritesSanglier;
    private List<SpriteMonster> _spritesEmptySanglier;
    private List<SpriteAnimation> _spritesDie;
    private List<SpriteAnimation> _spritesCascade;
    private List<SpriteMonster> _spritesGolem;
    private List<Sprite> _spritesEmpty;
    private int _cptMort = 50;
    private int _cptAttackGolem = 0;
    private int _cptGolem = 0;
    private Vector2 _realPosition;
    private Vector2 _realPositionGolem;
    private Vector2 _realPositionSanglier;
    private int _lifePoints = 5;
    private int _lpSanglier = 3;
    private int _lpGolem = 5;
    private bool _dead = false;
    private bool _stored = false;
    private bool _rockThrown = false;
    private int _compteurCascade;
    public string RoadFile = "Scores.txt";
    private SoundEffectInstance _soundCascade;
    public int Ground {get; set;} = 550;
    public string RunCascade {get; set;} = "N";
    private Vector2 _posEndMap1 = new Vector2(1100, 550);
    private Vector2 _posMap2 = new Vector2(100, 550);
    private Vector2 _posRock;
    private Vector2 _posTmp;
    private Vector2 _posTarget;
    public const int WindowWidth = 1200;
    public const int WindowHeight = 800;
    private Texture2D _bgAccueil;
    private Texture2D _bgDied;
    private Texture2D _bgScores;
    private Texture2D _bgWin;
    private Texture2D _bgGame;
    private Texture2D _btnPlay;
    private Texture2D _btnCmd;
    private Texture2D _bgCmd;
    private Texture2D _btnQuit;
    private Texture2D _btnScore;
    private int _cptBtn = 0;
    private int _timer = 0;
    private int _curMap = 0;
    private string _direction;
    private Texture2D _lifeBar;
    private int _logoWidth = 300;
    private int _logoHeight = 150;
    private int _lifeWidth = 100;
    private string _curBtn = "PLAY";
    private int _lifeHeight = 25;
    private Song _song;
    private Song _songIntro;
    private int _jumpTime = 0;
    private int _attackTime = 0;
    private string _curSong = "";
    private int _time = 0;
    private List<SoundEffect> _soundEffects;
    private GameState _gameState = GameState.MainMenu;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = false;
      _soundEffects = new List<SoundEffect>();
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
      graphics.PreferredBackBufferHeight = WindowHeight;
      graphics.PreferredBackBufferWidth = WindowWidth;
      graphics.ApplyChanges();
      _direction = "L";
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
      
      _bgGame = Content.Load<Texture2D>("Background/bg_waterbridge");
      _bgAccueil = Content.Load<Texture2D>("Background/bg_accueil");
      _bgDied = Content.Load<Texture2D>("Background/youdied");
      _bgScores = Content.Load<Texture2D>("Background/bg_EcranScores");
      _bgCmd = Content.Load<Texture2D>("Background/MenuCmd");
      _lifeBar = Content.Load<Texture2D>("Player/life/full_life");
      _btnPlay = Content.Load<Texture2D>("Background/btnPlayFocus");
      _btnCmd = Content.Load<Texture2D>("Background/btnCmd");
      _btnQuit = Content.Load<Texture2D>("Background/btnQuit");
      _font = Content.Load<SpriteFont>("Score");
      _btnScore = Content.Load<Texture2D>("Background/btnScore");
      _song = Content.Load<Song>("Music/OST/Zelda_OST");
      _songIntro = Content.Load<Song>("Music/OST/Zelda_Main");
      MediaPlayer.Volume = 0.2f;
      MediaPlayer.Play(_songIntro);
      MediaPlayer.IsRepeating = true;
      _soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Jump"));
      _soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Sword"));
      _soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Waterfall"));
      _soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Hurt"));
      _soundEffects.Add(Content.Load<SoundEffect>("Music/SoundEffect/Die"));
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
          Position = new Vector2(50, Ground),
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
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Monster/Sanglier_DieL"), 4)},
        })
        {
          Position = new Vector2(1100, Ground-15),
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
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Monster/Sanglier_DieL"), 4)},
        })
        {
          Position = new Vector2(1100, Ground-15),
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
          Position = new Vector2(425, Ground-285),
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
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Monster/attackLGolem"), 7)},
        })
        {
          Position = new Vector2(1100, Ground-13),
        },
      };
      _spritesGolem[0].Speed = 2.0f;

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
          Position = new Vector2(50, Ground),
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
      if(_curMap == 1 && _lpSanglier > 0)
      {
        if(_realPositionSanglier.X <= 600){
          _spritesSanglier[0].Direction = "RIGHT";
        }
        else if(_realPositionSanglier.X >= 1101){
          _spritesSanglier[0].Direction = "LEFT";
        }
      }
      else if (_curMap == 2 && _lpGolem > 0)
      {
        var nTmp = Distance(_realPositionGolem, _realPosition);     
        if(_cptAttackGolem > 0)
        {
          _cptAttackGolem-=1;
        }
        if(_cptGolem > 0)
        {
          _spritesGolem[0].Direction = "ATTACK";
          _cptGolem -= 1;
        }
        else if(nTmp < 400.0 && _spritesGolem[0].Direction == "LEFT" && _cptAttackGolem == 0)
        {
          _spritesGolem[0].Direction = "ATTACK";
          _cptAttackGolem = 220;
          _cptGolem = 35;
          ThrowRock();
        }
        else if(_realPositionGolem.X <= 800){
          _spritesGolem[0].Direction = "RIGHT";
        }
        else if(_realPositionGolem.X >= 1101){
          _spritesGolem[0].Direction = "LEFT";
        }
        else if(_spritesGolem[0].Direction == "ATTACK")
        {
          _spritesGolem[0].Direction = "RIGHT";
        }
      }
    }

    void ThrowRock()
    {
      _rockThrown = true;
      _posRock = new Vector2((int)_realPositionGolem.X,(int)_realPositionGolem.Y-80);
      _posTarget = _realPosition;

      _spritesRock = new List<SpriteAnimation>()
      {
        new SpriteAnimation(new Dictionary<string, Animation>()
        {
          { "AnimL", new Animation(Content.Load<Texture2D>("Monster/stoneL"), 7) },
          { "AnimR", new Animation(Content.Load<Texture2D>("Monster/stoneL"), 7) },
        }, "LEFT")
        {
          Position = _posRock,
        },
      };
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
          Position = new Vector2(_sprites[0].Position.X, Ground+20),
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
          Position = new Vector2(_sprites[0].Position.X, Ground+20),
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
      if(_cptImmune == 100)
      {
        _cptImmune = 99;
        if(_lifePoints == 5)
        {
          _lifePoints -= 1;
          _lifeBar = Content.Load<Texture2D>("Player/life/semi_full");
          _soundEffects[3].CreateInstance().Play();
        }
        else if(_lifePoints == 4)
        {
          _lifePoints -= 1;
          _lifeBar = Content.Load<Texture2D>("Player/life/mid_life");
          _soundEffects[3].CreateInstance().Play();
        }
        else if(_lifePoints == 3)
        {
          _lifePoints -= 1;
          _lifeBar = Content.Load<Texture2D>("Player/life/semi_low");
          _soundEffects[3].CreateInstance().Play();
        }
        else if(_lifePoints == 2)
        {
          _lifePoints -= 1;
          _lifeBar = Content.Load<Texture2D>("Player/life/low_life");
          _soundEffects[3].CreateInstance().Play();
        }
        else if(_lifePoints == 1)
        {
          _lifePoints -= 1;
          _lifeBar = Content.Load<Texture2D>("Player/life/dead_life");
          _soundEffects[4].CreateInstance().Play();
          Dead(_direction);
          _dead = true;
        }
      }
    }

    void DecreaseMonsterLifebar()
    {
      if(_curMap == 1)
      {
        _lpSanglier -= 1;
        if(_lpSanglier <= 0)
        {
          _spritesSanglier = _spritesEmptySanglier;
        }
      }
      else if(_curMap == 2)
      {
        _lpGolem -= 1;
        if(_lpGolem <= 0)
        {
          _spritesGolem = _spritesEmptySanglier;
        }
      }
    }

    void Attack()
    {
      KeyboardState state = Keyboard.GetState();
      if(_curMap == 1)
      {
        var nTmp = Distance(_realPositionSanglier, _realPosition);
        if(nTmp < 50.0 && _lpSanglier > 0)
        {
          if(state.IsKeyDown(Keys.A)){
            if(_cptHit == 0)
            {
              DecreaseMonsterLifebar();
              _cptHit = 25;
            } 
          }  
        }
      }
      else if(_curMap == 2)
      {
        var nTmp = Distance(_realPositionGolem, _realPosition);
        if(nTmp < 40.0 && _lpGolem > 0)
        {
          if(state.IsKeyDown(Keys.A)){
            if(_cptHit == 0)
            {
              DecreaseMonsterLifebar();
              _cptHit = 25;
            } 
          }  
        }
      }
    }

    void Replay()
    {
      _lifePoints = 5;
      _lpGolem = 5;
      _lpSanglier = 3;
      _lifeBar = Content.Load<Texture2D>("Player/life/full_life");
      _direction = "L";
      _cptMort = 50;
      _dead = false;
      _stored = false;
      _timer = 0;
      _curMap = 1;
      _bgGame = Content.Load<Texture2D>("Background/bg_waterbridge");
      _spritesSanglier = new List<SpriteMonster>()
      {
        new SpriteMonster(new Dictionary<string, Animation>()
        {
          { "WalkLeft", new Animation(Content.Load<Texture2D>("Monster/Sanglier_RunL"), 8) },
          { "WalkRight", new Animation(Content.Load<Texture2D>("Monster/Sanglier_Run"), 8) },
          { "DieRight", new Animation(Content.Load<Texture2D>("Monster/Sanglier_DieR"), 4)},
          { "DieLeft", new Animation(Content.Load<Texture2D>("Monster/Sanglier_DieL"), 4)},
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Monster/attackLGolem"), 7)},
        })
        {
          Position = new Vector2(1100, Ground-15),
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
          Position = new Vector2(50, Ground),
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
          { "AttackLeft", new Animation(Content.Load<Texture2D>("Monster/attackLGolem"), 7)},
        })
        {
          Position = new Vector2(1100, Ground-13),
        },
      };
      _spritesGolem[0].Speed = 2.0f;
    }

    void StoreScore(int time, int score)
    {
      if(_stored == false)
      {
        StreamWriter sw = new StreamWriter(RoadFile, true, Encoding.ASCII);
        var timeInSec = time/60.0;
        var timeFinal = (int)Math.Round(timeInSec);
        var scr = "      "+score+"                   "+timeFinal+"\n";
        sw.Write(scr);
        sw.Close();
        _stored = true;
      }
    }

    string ReadAndOrderScores()
    {
      List<string> lstScores = new List<string>();
      StreamReader sr = new StreamReader(RoadFile);
      //Read the first line of text
      var line = sr.ReadLine();
      var cpt = 0;
      string res = "Couronnes     Secondes \n \n";
     //Continue to read until you reach end of file
      while (line != null && cpt < 10)
      {
        cpt+=1;
        var tmp = line+"\n";
        lstScores.Add(tmp);
        line = sr.ReadLine();
      }
      //close the file
      sr.Close();
      lstScores.Sort();
      lstScores.Reverse();
      foreach(string item in lstScores) {
        res = res+item+"";
    }
    return res;
    }

    void IsDamaged()
    {
      KeyboardState state = Keyboard.GetState();
      if(_curMap == 1)
      {
        var nTmp = Distance(_realPositionSanglier, _realPosition);
        if(nTmp < 40.0 && _lpSanglier > 0)
        {
          if(! state.IsKeyDown(Keys.E)){
            if(! state.IsKeyDown(Keys.A))
            {
              DecreaseLifebar();
            }
          }  
        }
      }
      else if(_curMap == 2)
      {
        if(_rockThrown == true)
        {
        var nTmp = Distance(_spritesRock[0].Position, _realPosition);
        if(nTmp < 30.0 && _lpGolem > 0)
        {
          if(! state.IsKeyDown(Keys.E)){
            DecreaseLifebar();
            _rockThrown = false;
          }
        }
        }
      }
    }

    void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(_song);
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
          if(_time < 60){
          _time +=1;
          }
          if(_curSong != "MainMenu"){
            MediaPlayer.Play(_songIntro);
            _curSong = "MainMenu";
          }
        if(state.IsKeyDown(Keys.Space)){
          if(_curBtn == "PLAY")
          {
            _curMap = 1;
          _gameState = GameState.GamePlay;
          Replay();
          }
          else if(_curBtn == "CMD")
          {
            _gameState = GameState.GameCmd;
          }
          else if(_curBtn == "SCORE")
          {
            _gameState = GameState.GameScore;
          }
          else if(_curBtn == "QUIT")
          {
            Exit();
          }
        }
        if(_cptBtn > 0)
        {
          _cptBtn-=1;
        }
        if(state.IsKeyDown(Keys.Z) && _cptBtn == 0)
        {
          _cptBtn = 20;
          if(_curBtn == "PLAY")
          {
            _curBtn = "QUIT";
            _btnQuit = Content.Load<Texture2D>("Background/btnQuitFocus");
            _btnPlay = Content.Load<Texture2D>("Background/btnPlay");
          }
          else if(_curBtn == "CMD")
          {
            _curBtn = "PLAY";
            _btnPlay = Content.Load<Texture2D>("Background/btnPlayFocus");
            _btnCmd = Content.Load<Texture2D>("Background/btnCmd");
          }
          else if(_curBtn == "SCORE")
          {
            _curBtn = "CMD";
            _btnCmd = Content.Load<Texture2D>("Background/btnCmdFocus");
            _btnScore = Content.Load<Texture2D>("Background/btnScore");
          }
          else if(_curBtn == "QUIT")
          {
            _curBtn = "SCORE";
            _btnScore = Content.Load<Texture2D>("Background/btnScoreFocus");
            _btnQuit = Content.Load<Texture2D>("Background/BtnQuit");
          }
        }
        if(state.IsKeyDown(Keys.S) && _cptBtn == 0)
        {
          _cptBtn = 20;
          if(_curBtn == "PLAY")
          {
            _curBtn = "CMD";
            _btnCmd = Content.Load<Texture2D>("Background/btnCmdFocus");
            _btnPlay = Content.Load<Texture2D>("Background/btnPlay");
          }
          else if(_curBtn == "CMD")
          {
            _curBtn = "SCORE";
            _btnScore = Content.Load<Texture2D>("Background/btnScoreFocus");
            _btnCmd = Content.Load<Texture2D>("Background/btnCmd");
          }
          else if(_curBtn == "SCORE")
          {
            _curBtn = "QUIT";
            _btnQuit = Content.Load<Texture2D>("Background/btnQuitFocus");
            _btnScore = Content.Load<Texture2D>("Background/btnScore");
          }
          else if(_curBtn == "QUIT")
          {
            _curBtn = "PLAY";
            _btnPlay = Content.Load<Texture2D>("Background/btnPlayFocus");
            _btnQuit = Content.Load<Texture2D>("Background/BtnQuit");
          }
        }
          break;
        }
        case GameState.GameDie:
        {
          if(_curSong != "GameDie"){
            MediaPlayer.Play(_songIntro);
            _curSong = "GameDie";
          }
          if(state.IsKeyDown(Keys.Escape))
          {
            _gameState = GameState.MainMenu;
          }
          break;
        }
        case GameState.GameScore:
        {
          if(state.IsKeyDown(Keys.Escape))
          {
            _gameState = GameState.MainMenu;
          }
          break;
        }
        case GameState.EndGame:
        {
          if(_curSong != "GameDie"){
            MediaPlayer.Play(_songIntro);
            _curSong = "GameDie";
          }
          if(state.IsKeyDown(Keys.Escape))
          {
            _gameState = GameState.MainMenu;
          }
          break;
        }
        case GameState.GameCmd:
        {
          if(state.IsKeyDown(Keys.Escape))
          {
            _gameState = GameState.MainMenu;
          }
          break;
        }
        case GameState.GamePlay:
        { 
          if(_rockThrown == true)
          {
            var nX = _posRock.X;
            var nY = _posRock.Y;
            var nTargetX = _posTarget.X;
            var nTargetY = _posTarget.Y;
            if(nY >= Ground+60)
            {
              _rockThrown = false;
            }
            else if((nX - nTargetX)/4 > (nTargetY - nY))
            {
              nX = nX - 15;
            }
            else
            {
              nY = nY + 5;
            }
            _posTmp = new Vector2((int)nX, (int)nY);
            _spritesRock[0].Position = _posTmp;
            _posRock = _posTmp;
            _spritesRock[0].Update(gameTime, _spritesRock);
          }
          if(_cptHit > 0)
          {
            _cptHit -= 1;
          }
          if(_curMap == 1 && _lpSanglier > 0)
          {
            _realPositionSanglier = new Vector2((int)Math.Truncate(_spritesSanglier[0].Position.X)+70, (int)Math.Truncate(_spritesSanglier[0].Position.Y)+50);
          }
          else if(_curMap == 2 && _lpGolem > 0)
          {
            _realPositionGolem = new Vector2((int)Math.Truncate(_spritesGolem[0].Position.X)+70, (int)Math.Truncate(_spritesGolem[0].Position.Y)+60);
          }
          _realPosition = new Vector2((int)Math.Truncate(_sprites[0].Position.X)+125, (int)Math.Truncate(_sprites[0].Position.Y)+80);
          if(state.IsKeyDown(Keys.A) && _direction == "L")
          {
            if(_curMap == 1 && _lpSanglier > 0)
            {
              if(_realPosition.X > _realPositionSanglier.X)
              {
                Attack();
              }
            }
            if(_curMap == 2 && _lpGolem > 0)
            {
              if(_realPosition.X > _realPositionGolem.X)
              {
                Attack();
              }
            }
            _realPosition = new Vector2(_sprites[0].Position.X+68, _sprites[0].Position.Y+25);
          }
          else if(state.IsKeyDown(Keys.A) && _direction == "R")
          {
            if(_curMap == 1 && _lpSanglier > 0)
            {
              if(_sprites[0].Position.X < _realPositionSanglier.X)
              {
                Attack();
              }
            }
            if(_curMap == 2 && _lpGolem > 0)
            {
              if(_realPosition.X < _realPositionGolem.X)
              {
                Attack();
              }
            }
          }
          _realPosition = new Vector2((int)_sprites[0].Position.X+125, (int)_sprites[0].Position.Y+60);
          if(_cptImmune == 0)
          {
            _cptImmune = 100;
          }
          else if(_cptImmune < 100)
          {
            _cptImmune -= 1;
          }
          IsDamaged();
          if(_dead == true)
          {
            if(_cptMort > 0)
            {
              _cptMort -= 1;
              _spritesDie[0].Update(gameTime, _spritesDie);
            }
            if(_cptMort == 0)
            {
              _lifeBar.Dispose();
              _gameState = GameState.GameDie;
              Replay();
            }
          }
          if(_curMap == 1)
          {
            _spritesCascade[0].Update(gameTime, _spritesCascade);
          }
          UpdateMonster();
          if(state.IsKeyDown(Keys.D))
          {
            _direction = "R";
          }
          if(state.IsKeyDown(Keys.Q))
          {
            _direction = "L";
          }
          if(_sprites[0].Position.X >= WindowWidth-50 && _curMap == 2)
          {
          _gameState = GameState.EndGame;
          }
          if(_sprites[0].Position.X >= WindowWidth-50 && _curMap < 2){
            _bgGame = Content.Load<Texture2D>("Background/forest_stone");
            if(_curMap == 1){
              _sprites[0].Position = _posMap2;
            }
            _curMap+=1;
          }
          else if(_sprites[0].Position.X <= 50 && _curMap > 1){
            _bgGame = Content.Load<Texture2D>("Background/bg_waterbridge");
            if(_curMap == 2){
              _sprites[0].Position = _posEndMap1;
            }
            _curMap-=1;
          }
          if (state.IsKeyDown(Keys.Space) && _jumpTime == 0){
            _soundEffects[0].CreateInstance().Play();
            _jumpTime += 1;
          }
          if (_jumpTime > 0){
            if(_jumpTime == 55)
              _jumpTime = 0;
            else
              _jumpTime += 1;
          }
          
          if(state.IsKeyDown(Keys.A) && _attackTime == 0)
          {
            _soundEffects[1].CreateInstance().Play();
            _attackTime += 1;
          }
          if (_attackTime > 0){
            if(_attackTime == 40)
              _attackTime = 0;
            else
              _attackTime += 1;
          }
          if(_curSong != "GamePlay"){
            _time = 0;
            MediaPlayer.Play(_song);
            _curSong = "GamePlay";
          }
          if(state.IsKeyDown(Keys.Escape)){
            _gameState = GameState.MainMenu;
        }
          //foreach (var sprite in _sprites)
          //{
        _sprites[0].Update(gameTime, _sprites);
          //}
        if(_curMap == 1)
        {
          if(_lpSanglier > 0)
            _spritesSanglier[0].Update(gameTime, _spritesSanglier);
        }
        else if(_curMap == 2)
        {
          _soundCascade.Stop();
          if(_lpGolem > 0)
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
      GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

      switch(_gameState)
            {
                case GameState.MainMenu:
                {
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(_bgAccueil, new Microsoft.Xna.Framework.Rectangle(0 ,0 ,WindowWidth ,WindowHeight), Microsoft.Xna.Framework.Color.White);
                    _spriteBatch.Draw(_btnCmd, new Microsoft.Xna.Framework.Rectangle((WindowWidth/2)-(_logoWidth/2)+75 , (WindowHeight/2)-(_logoHeight/2),200 ,42), Microsoft.Xna.Framework.Color.White);
                    _spriteBatch.Draw(_btnQuit, new Microsoft.Xna.Framework.Rectangle(WindowWidth-180, 4*(WindowHeight/5),115 ,35), Microsoft.Xna.Framework.Color.White);
                    _spriteBatch.Draw(_btnPlay, new Microsoft.Xna.Framework.Rectangle((WindowWidth/2)-(_logoWidth/2)+75 , (WindowHeight/2)-(_logoHeight/2)-150,200 ,45), Microsoft.Xna.Framework.Color.White);
                    _spriteBatch.Draw(_btnScore, new Microsoft.Xna.Framework.Rectangle((WindowWidth/2)-(_logoWidth/2)+75 , (WindowHeight/2)-(_logoHeight/2)+100,200 ,45), Microsoft.Xna.Framework.Color.White);
                    _spriteBatch.End();
                    base.Draw(gameTime);
                    break;
                }
                case GameState.GameScore:
                {
                  _spriteBatch.Begin();
                  // Draw string to screen.
                  var Scores = ReadAndOrderScores();
                  //var Scores = " 1 \n 2 \n 3 \n";
                  _spriteBatch.Draw(_bgScores, new Microsoft.Xna.Framework.Rectangle(0 ,0 ,WindowWidth ,WindowHeight), Microsoft.Xna.Framework.Color.White);
                  _spriteBatch.DrawString(_font, Scores, new Vector2(100, 250), Microsoft.Xna.Framework.Color.Black);
                  _spriteBatch.End();
                  base.Draw(gameTime);
                  break;
                }
                case GameState.GameDie:
                {
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(_bgDied, new Microsoft.Xna.Framework.Rectangle(0 ,0 ,WindowWidth ,WindowHeight), Microsoft.Xna.Framework.Color.White);
                    _spriteBatch.End();
                    base.Draw(gameTime);
                    break;
                }
                case GameState.GameCmd:
                {
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(_bgCmd, new Microsoft.Xna.Framework.Rectangle(0 ,0 ,WindowWidth ,WindowHeight), Microsoft.Xna.Framework.Color.White);
                    _spriteBatch.End();
                    base.Draw(gameTime);
                    break;
                }
                case GameState.EndGame:
                {
                  _spriteBatch.Begin();
                  if((_lpGolem + _lpSanglier) > 0)
                  {
                    if((_lpGolem <= 0) || (_lpSanglier <= 0))
                    {
                      _bgWin = Content.Load<Texture2D>("Background/partialWin");
                      StoreScore(_timer, 2);
                    }
                    else
                    {
                      _bgWin = Content.Load<Texture2D>("Background/falseWin");
                      StoreScore(_timer, 1);
                    }
                  }

                  else if((_lpGolem + _lpSanglier) <= 0)
                  {
                    _bgWin = Content.Load<Texture2D>("Background/realWin");
                    StoreScore(_timer, 3);
                  }
                  
                  _spriteBatch.Draw(_bgWin, new Microsoft.Xna.Framework.Rectangle(0 ,0 ,WindowWidth ,WindowHeight), Microsoft.Xna.Framework.Color.White);
                  _spriteBatch.End();
                  base.Draw(gameTime);
                  break;
                }
                case GameState.GamePlay:
                {
                  _timer+=1;
                  _soundCascade = _soundEffects[2].CreateInstance();
                  spriteBatch.Begin();
                  spriteBatch.Draw(_bgGame, new Microsoft.Xna.Framework.Rectangle(0 ,0 ,WindowWidth ,WindowHeight), Microsoft.Xna.Framework.Color.White);
                  if(_curMap == 1)
                  {
                    foreach (var sprite in _spritesCascade)
                      sprite.Draw(spriteBatch);
                  }
                  if(_dead != true)
                  {
                    foreach (var sprite in _sprites)
                      sprite.Draw(spriteBatch);
                  }
                  if(_curMap == 1){
                    if(RunCascade == "N")
                    {
                      _soundCascade.Play();
                      RunCascade = "Y";
                    }
                    else
                    {
                      _compteurCascade +=1;
                    }
                    if(_compteurCascade >=3400)
                    {
                      RunCascade = "N";
                      _compteurCascade = 0;
                    }
                    if(_lpSanglier > 0)
                    {
                      foreach (var sprite in _spritesSanglier)
                        sprite.Draw(spriteBatch);
                    }
                    
                  }
                  else if(_curMap == 2)
                  {
                    if(_lpGolem > 0 && _rockThrown == true)
                    {
                      foreach (var sprite in _spritesRock)
                      {
                        sprite.Draw(spriteBatch);
                      }
                        
                    }
                    RunCascade = "N";
                    foreach (var sprite in _spritesGolem)
                      sprite.Draw(spriteBatch);
                  }
                  spriteBatch.Draw(_lifeBar, new Microsoft.Xna.Framework.Rectangle((int)Math.Truncate(_realPosition.X)-65 ,(int)Math.Truncate(_realPosition.Y)-60,_lifeWidth ,_lifeHeight), Microsoft.Xna.Framework.Color.White);
                  if(_dead == true)
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
