namespace LudumDare41_Game.Content {
    class ContentManager {

        private Microsoft.Xna.Framework.Content.ContentManager contentManager;

        public ContentManager (Microsoft.Xna.Framework.Content.ContentManager _contentManager) {
            contentManager = _contentManager;
        }

        public T Load<T> (string name) {
            return contentManager.Load<T>(name);
        }
    }
}
