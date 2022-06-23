using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using projet_MonoGame.Managers;
using projet_MonoGame.Models;

namespace projet_MonoGame.Sprites
{
  public class SpriteMonster
  {
    #region Fields

    protected AnimationManager _animationManager;

    protected Dictionary<string, Animation> _animations;

    protected Vector2 _position;

    public string state { get; set; }

    public string direction { get; set; }

    protected Texture2D _texture;

    #endregion

    #region Properties

    public Vector2 Position
    {
      get { return _position; }
      set
      {
        _position = value;

        if (_animationManager != null)
          _animationManager.Position = _position;
      }
    }


    public float Speed = 4.2f;

    public Vector2 Velocity;

    #endregion
    
    #region Methods

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      if (_texture != null)
        spriteBatch.Draw(_texture, Position, Color.White);
      else if (_animationManager != null)
        _animationManager.Draw(spriteBatch);
      else throw new Exception("This ain't right..!");
    }

    public virtual void Move()
    {
      if (direction == "LEFT"){
        Velocity.X = -Speed;
      }
      else if (direction == "RIGHT"){
        Velocity.X = Speed;
      }
      //if (Keyboard.GetState().IsKeyDown(Input.Up))
      //  Velocity.Y = -2*Speed;
      //if(Keyboard.GetState().IsKeyDown(Input.A)){
      //}
    //if(Keyboard.GetState().IsKeyDown(Input.E)){
     // }
    }

    protected virtual void SetAnimations()
    {
      if (Velocity.X > 0){
        _animationManager.Play(_animations["WalkRight"]);
      }
      else if (Velocity.X < 0){
        _animationManager.Play(_animations["WalkLeft"]);
      }
      //else if (dieR == "Y"){
      //  _animationManager.Play(_animations["DieRight"]);
      //}
      //else if (dieL == "Y"){
       // _animationManager.Play(_animations["DieLeft"]);
      //}
      else _animationManager.Stop();
    }
    public SpriteMonster(Dictionary<string, Animation> animations)
    {
      this.direction = "LEFT";
      _animations = animations;
      _animationManager = new AnimationManager(_animations.First().Value);
    }

    public SpriteMonster(Texture2D texture)
    {
      _texture = texture;
    }

    public virtual void Update(GameTime gameTime, List<SpriteMonster> sprites)
    {
      Move();
      
      SetAnimations();

      _animationManager.Update(gameTime);

      Position += Velocity;
      Velocity = Vector2.Zero;
    }

    #endregion
  }
}