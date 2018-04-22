using LudumDare41_Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare41_Game.Content {
    class ContentManager {

        private Microsoft.Xna.Framework.Content.ContentManager contentManager;

        public ContentManager (Microsoft.Xna.Framework.Content.ContentManager _contentManager) {
            contentManager = _contentManager;
        }

        public T Load<T> (string name) {
            return contentManager.Load<T>(name);
        }

        public Animation LoadAnimation (string ID, string animation, int width, int height, float speed) {                                  //Problemsolving at its finest
            System.Console.WriteLine((ID.Contains("Tower") ? "Towers" : (ID.Contains("Entity") ? "Entities" : "FUCK!")) + "/" + ID + "/" + ID + "_" + animation + "Spritesheet");
            Texture2D temp = contentManager.Load<Texture2D>((ID.Contains("Tower") ? "Towers" : (ID.Contains("Entity") ? "Entities" : "FUCK!")) + "/" + ID + "/" + ID +"_" + animation + "Spritesheet");
            return new Animation(temp, new Vector2(width, height), temp.Width / width, speed);
        }
    }
}
