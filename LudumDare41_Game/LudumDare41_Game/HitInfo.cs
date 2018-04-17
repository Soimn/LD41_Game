using Microsoft.Xna.Framework;

struct HitInfo<T, S> where T : struct where S : struct {
    public Vector2 TranslationObj_1 { get; private set; }
    public Vector2 TranslationObj_2 { get; private set; }
    public T? Obj_1 { get; private set; }
    public S? Obj_2 { get; private set; }
    public bool Hit { get; private set; }

    public HitInfo (Vector2 _translationObj_1, Vector2 _translationObj_2, T? _obj_1, S? _obj_2, bool _hit = true) {
        TranslationObj_1 = _translationObj_1;
        TranslationObj_2 = _translationObj_2;
        Obj_1 = _obj_1;
        Obj_2 = _obj_2;
        Hit = _hit;
    }
}