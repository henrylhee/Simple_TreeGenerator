
using Vector3 = UnityEngine.Vector3;

public struct Knot
{
    public Vector3 position { get; private set; }
    public Vector3 direction { get; private set; }
    public float thickness { get; private set; }

    public Knot(Vector3 position, Vector3 direction, float thickness)
    {
        this.position = position;
        this.direction = direction;
        this.thickness = thickness;
    }
}
