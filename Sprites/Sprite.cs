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
  public class Sprite
  {
    #region Fields

    protected AnimationManager _animationManager;

    protected Dictionary<string, Animation> _animations;

    protected Vector2 _position;

    protected double gravity;

    protected string direction = "L";

    protected Texture2D _texture;

    protected string attackL;

    protected string attackR;

    protected string defL;

    protected string defR;

    #endregion

    #region Properties

    public Input Input;

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
      if(attackL == "Y"){
        attackL = "N";
      }
      if(attackR == "Y"){
        attackR = "N";
      }
      if(defR == "Y"){
        defR = "N";
      }
      if(defL == "Y"){
        defL = "N";
      }
      if (Keyboard.GetState().IsKeyDown(Input.Left)){
        Velocity.X = -Speed;
        direction = "L";
      }
      else if (Keyboard.GetState().IsKeyDown(Input.Right)){
        Velocity.X = Speed;
        direction = "R";
      }
      if (Keyboard.GetState().IsKeyDown(Input.Up))
        Velocity.Y = -2*Speed;
      if(Keyboard.GetState().IsKeyDown(Input.A))
      {
        if(direction == "L"){
          attackL = "Y";
          attackR = "N";
        }
        else if(direction == "R"){
          attackL = "N";
          attackR = "Y";
        }
      }
      if(Keyboard.GetState().IsKeyDown(Input.E))
      {
        if(direction == "L"){
          defL = "Y";
          defR = "N";
        }
        else if(direction == "R"){
          defL = "N";
          defR = "Y";
        }
      }
    }

    protected virtual void SetAnimations()
    {
      if (Velocity.X > 0){
        _animationManager.Play(_animations["WalkRight"]);
      }
      else if (Velocity.X < 0){
        _animationManager.Play(_animations["WalkLeft"]);
      }
      else if (attackL == "Y"){
        _animationManager.Play(_animations["AttackLeft"]);
      }
      else if (attackR == "Y"){
        _animationManager.Play(_animations["AttackRight"]);
      }
      else if (defR == "Y"){
        _animationManager.Play(_animations["DefRight"]);
      }
      else if (defL == "Y"){
        _animationManager.Play(_animations["DefLeft"]);
      }
      else _animationManager.Stop();
    }
    public Sprite(Dictionary<string, Animation> animations)
    {
      _animations = animations;
      _animationManager = new AnimationManager(_animations.First().Value);
    }

    public Sprite(Texture2D texture)
    {
      _texture = texture;
    }

    public virtual void Update(GameTime gameTime, List<Sprite> sprites)
    {
      Move();
      if(Position.Y >= 550.0){
        gravity = 0.0;
      }else if(Position.Y < 550.0){
        gravity += 0.30;
        Velocity.Y = Velocity.Y + (int)Math.Truncate(gravity);
      }

      
      SetAnimations();

      _animationManager.Update(gameTime);

      Position += Velocity;
      Velocity = Vector2.Zero;
    }

    #endregion
  }
}
