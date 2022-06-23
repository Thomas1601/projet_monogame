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
  public class SpriteAnimation
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


    public float Speed = 2.5f;

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


    protected virtual void SetAnimations()
    {
      if (direction == "LEFT"){
        _animationManager.Play(_animations["AnimR"]);
      }
      else if (direction == "RIGHT"){
        _animationManager.Play(_animations["AnimL"]);
      }
      else _animationManager.Stop();
    }
    
    public SpriteAnimation(Dictionary<string, Animation> animations, string direction)
    {
      this.direction = direction;
      _animations = animations;
      _animationManager = new AnimationManager(_animations.First().Value);
    }

    public SpriteAnimation(Texture2D texture)
    {
      _texture = texture;
    }

    public virtual void Update(GameTime gameTime, List<SpriteAnimation> sprites)
    {
      
      SetAnimations();

      _animationManager.Update(gameTime);

    }

    #endregion
  }
}