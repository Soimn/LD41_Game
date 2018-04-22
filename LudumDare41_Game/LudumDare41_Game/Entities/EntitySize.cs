namespace LudumDare41_Game.Entities {
    struct EntitySize {
        public EntityWidth Width { get; private set; }
        public EntityHeight Height { get; private set; }

        public EntitySize (EntityWidth _width, EntityHeight _height) {
            Width = _width;
            Height = _height;
        }
    }

    public enum EntityWidth { narrow = 32, medium = 64, wide = 96 }
    public enum EntityHeight { medium = 32, tall = 64 }
}
