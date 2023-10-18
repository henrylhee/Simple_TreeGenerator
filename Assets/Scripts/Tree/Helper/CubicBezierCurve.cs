using UnityEngine;


public class CubicBezierCurve
{
    CubicBezierPoints points;

    public CubicBezierCurve(CubicBezierPoints points)
    {
        this.points = points;
    }

    public Vector3 GetPosition(float t)
    {
        Vector3 b0T = (1-t)*((1-t)*points.p0 + t*points.p1) + t*((1-t)*points.p1 + t*points.p2);
        Vector3 b1T = (1-t)*((1-t)*points.p1 + t*points.p2) + t*((1-t)*points.p2 + t*points.p3);

        return (1-t)*b0T + t*b1T;
    }
}

public struct CubicBezierPoints
{
    public Vector3 p0 { get; private set; }
    public Vector3 p1 { get; private set; }
    public Vector3 p2 { get; private set; }
    public Vector3 p3 { get; private set; }

    public CubicBezierPoints(Vector3 startPos, Vector3 endPos, Vector3 startDir, Vector3 endDir, float c)
    {
        this.p0 = startPos;
        this.p1 = startPos + startDir * c;
        this.p2 = endPos - endDir * c;
        this.p3 = endPos;
    }
}

