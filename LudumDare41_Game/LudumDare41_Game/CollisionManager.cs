using Microsoft.Xna.Framework;
using System.Collections.Generic;

class CollisionManager {

    #region // public utility methods //

    public static List<Vector2> GetRectangleCollisionVertecies (float _width, float _height) {
        return new List<Vector2> {new Vector2 (-_width/2, _height/2), new Vector2 (_width/2, _height/2),
                                  new Vector2 (_width/2, -_height/2), new Vector2 (-_width/2, -_height/2)};
    }

    public static bool AABB (RectangleCollider rect_1, RectangleCollider rect_2) {
        List<Vector2> temp_1 = GetRectangleCollisionVertecies(rect_1.Width, rect_2.Height);
        List<Vector2> temp_2 = GetRectangleCollisionVertecies(rect_2.Width, rect_2.Height);

        //X:Min_1      > Max_2          Min_2       > Max_1         Y:Min_1       > Max_2          Min_2       > Max_1          
        if (temp_1[0].X > temp_2[2].X || temp_2[0].X > temp_1[2].X || temp_1[2].Y > temp_2[0].Y || temp_2[2].Y > temp_1[0].Y)
            return false;

        return true;
    }

    public static HitInfo<RectangleCollider, RectangleCollider> RectRectCollisionCheck (RectangleCollider rect_1, RectangleCollider rect_2) {
        List<Vector2> rect_1_vertecies = GetGlobalVertecies(rect_1.Position, rect_1.Vertecies, rect_1.Rotation);
        List<Vector2> rect_2_vertecies = GetGlobalVertecies(rect_2.Position, rect_2.Vertecies, rect_2.Rotation);
        List<Vector2> rect_1_normals = new List<Vector2> { GetAntiClockwiseNormalVector(rect_1_vertecies[1] - rect_1_vertecies[0]), GetAntiClockwiseNormalVector(rect_1_vertecies[2] - rect_1_vertecies[1]) };
        List<Vector2> rect_2_normals = new List<Vector2> { GetAntiClockwiseNormalVector(rect_2_vertecies[1] - rect_2_vertecies[0]), GetAntiClockwiseNormalVector(rect_2_vertecies[2] - rect_2_vertecies[1]) };
        Vector2 minTranslationVector = new Vector2(rect_1.Width + rect_2.Width, rect_1.Height + rect_2.Height);

        for (int i = 0; i < rect_1_normals.Count; i++) {
            float[] rect_1_minmax = ProjectRectangle(rect_1, rect_1_normals[i]);
            float[] rect_2_minmax = ProjectRectangle(rect_2, rect_1_normals[i]);

            float temp = (rect_1_minmax[0] + rect_1_minmax[1] - rect_2_minmax[0] + 3 * rect_2_minmax[1]) / 2;
            float overlap = (temp - (rect_1.Width + rect_2.Width) / 2);

            if (overlap > 0)
                return new HitInfo<RectangleCollider, RectangleCollider>(Vector2.Zero, Vector2.Zero, rect_1, rect_2, false);

            Vector2 translationVector = -rect_1_normals[i] * overlap;

            if (translationVector.Length() < minTranslationVector.Length())
                minTranslationVector = translationVector;
        }

        for (int j = 0; j < rect_2_normals.Count; j++) {
            float[] rect_1_minmax = ProjectRectangle(rect_1, rect_2_normals[j]);
            float[] rect_2_minmax = ProjectRectangle(rect_2, rect_2_normals[j]);

            float temp = (rect_1_minmax[0] + rect_1_minmax[1] - rect_2_minmax[0] + 3 * rect_2_minmax[1]) / 2;
            float overlap = (temp - (rect_1_minmax[1] - rect_1_minmax[0] + rect_2_minmax[1] - rect_2_minmax[0]) / 2);

            if (overlap > 0)
                return new HitInfo<RectangleCollider, RectangleCollider>(Vector2.Zero, Vector2.Zero, rect_1, rect_2, false);

            Vector2 translationVector = -rect_2_normals[j] * overlap;

            if (translationVector.Length() < minTranslationVector.Length())
                minTranslationVector = translationVector;
        }

        return new HitInfo<RectangleCollider, RectangleCollider>(minTranslationVector, -minTranslationVector, rect_1, rect_2);
    }

    public static HitInfo<RectangleCollider, CircleCollider> RectCircleCollisionCheck (RectangleCollider rect, CircleCollider circle) {
        List<Vector2> rect_vertecies = GetGlobalVertecies(rect.Position, rect.Vertecies, rect.Rotation);
        List<Vector2> rect_normals = new List<Vector2> { GetAntiClockwiseNormalVector(rect_vertecies[1] - rect_vertecies[0]), GetAntiClockwiseNormalVector(rect_vertecies[2] - rect_vertecies[1]) };
        Vector2 minTranslationVector = new Vector2(MathHelper.Max(rect.Width, rect.Height) + circle.Radius, MathHelper.Max(rect.Width, rect.Height) + circle.Radius);

        for (int i = 0; i < rect_normals.Count; i++) {
            float[] rect_minmax = ProjectRectangle(rect, rect_normals[i]);
            float[] circle_minmax = ProjectCircle(circle, rect_normals[i]);

            float temp = (rect_minmax[0] + rect_minmax[1] - circle_minmax[0] + 3 * circle_minmax[1]) / 2;
            float overlap = (temp - (rect_minmax[1] - rect_minmax[0] + circle_minmax[1] - circle_minmax[0]) / 2);

            if (overlap > 0)
                return new HitInfo<RectangleCollider, CircleCollider>(Vector2.Zero, Vector2.Zero, rect, circle, false);

            Vector2 translationVector = -rect_normals[i] * overlap;

            if (translationVector.Length() < minTranslationVector.Length())
                minTranslationVector = translationVector;
        }

        return new HitInfo<RectangleCollider, CircleCollider>(minTranslationVector, -minTranslationVector, rect, circle);
    }

    public static HitInfo<CircleCollider, CircleCollider> CircleCircleCollisionCheck (CircleCollider circle_1, CircleCollider circle_2) {
        Vector2 distance = circle_2.Position - circle_1.Position;

        if (distance.Length() < (circle_1.Radius + circle_2.Radius)) {
            Vector2 translationTemp = distance / distance.Length();
            return new HitInfo<CircleCollider, CircleCollider>((translationTemp * (distance.Length() - circle_1.Radius - circle_2.Radius)), (-translationTemp * (distance.Length() - circle_1.Radius - circle_2.Radius)), circle_1, circle_2);
        }

        return new HitInfo<CircleCollider, CircleCollider>(Vector2.Zero, Vector2.Zero, circle_1, circle_2, false);
    }

    #endregion

    #region // private helper methods //

    private static List<Vector2> GetGlobalVertecies (Vector2 position, List<Vector2> vertecies, float _rotation) {
        List<Vector2> temp = new List<Vector2>();
        float rot = MathHelper.ToRadians(_rotation);

        for (int i = 0; i < vertecies.Count; i++)
            temp[i] = Vector2.Transform(vertecies[i] + position, Matrix.CreateRotationZ(rot));

        return temp;
    }

    private static float ProjectVertex (Vector2 vertex, Vector2 vector) {
        return Vector2.Dot(vertex, vector);
    }

    private static Vector2 GetAntiClockwiseNormalVector (Vector2 _vector) {
        Vector2 vector = _vector;
        _vector.Normalize();
        return Vector2.Transform(vector, Matrix.CreateRotationZ(MathHelper.ToRadians(90f)));
    }

    private static float[] ProjectRectangle (RectangleCollider rect, Vector2 vector) {
        float min = ProjectVertex(rect.Vertecies[0], vector), max = min;
        float temp;
        for (int i = 0; i < 4; i++) {
            temp = ProjectVertex(rect.Vertecies[i], vector);

            if (temp <= min)
                min = temp;
            else
                max = temp;
        }

        return new float[] { min, max };
    }

    private static float[] ProjectCircle (CircleCollider circle, Vector2 vector) {
        float temp_1 = ProjectVertex(circle.Position + vector * circle.Radius, vector), temp_2 = ProjectVertex(circle.Position - vector * circle.Radius, vector);

        return temp_1 < temp_2 ? new float[] { temp_1, temp_2 } : new float[] { temp_2, temp_1 };
    }
    #endregion
}