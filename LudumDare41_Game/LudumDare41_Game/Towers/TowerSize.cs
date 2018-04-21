namespace LudumDare41_Game.Towers {
    struct TowerSize {
        public TowerWidth Width { get; private set; }
        public TowerHeight Height { get; private set; }

        public TowerSize (TowerWidth _width, TowerHeight _height) {
            Width = _width;
            Height = _height;
        }
    }

    public enum TowerWidth { narrow = 32, medium = 64, wide = 96}
    public enum TowerHeight { medium = 32, tall = 64}
}
