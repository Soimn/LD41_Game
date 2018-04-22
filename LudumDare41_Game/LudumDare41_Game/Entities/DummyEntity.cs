using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare41_Game.Entities {
    class DummyEntity : Entity {
        public override Vector2 Position { get; }

        public override List<Vector2> Path => throw new NotImplementedException();

        public override EntityHealth Health => throw new NotImplementedException();

        public override float Speed => throw new NotImplementedException();

        public override EntitySize Size => throw new NotImplementedException();

        public override EntityAnimationState AnimationState => throw new NotImplementedException();

        public DummyEntity (Vector2 _position, out DummyEntity dummyEntity) {
            Position = _position;
            dummyEntity = this;
        }

        public override void Init (Vector2 _position) {
            throw new NotImplementedException();
        }

        public override void Update (GameTime gameTime) {
            throw new NotImplementedException();
        }

        public override void Draw (SpriteBatch spriteBatch) {
            throw new NotImplementedException();
        }
    }
}
