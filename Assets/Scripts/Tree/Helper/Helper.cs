using UnityEngine;


namespace Gen
{
    static class Helper
    {
        //brnchAngle  <= 90 degrees and radian angles
        public static Vector3 ChangeDir(Vector3 growDir, float phylloAngle, float braAngle)
        {
            Vector3 rotAxis = Vector3.Cross(growDir, Vector3.up);
            float rotAngle = Vector3.Angle(growDir, Vector3.up);
            Vector3 vec = new Vector3(Mathf.Cos(phylloAngle), Mathf.Sin(braAngle), Mathf.Sin(phylloAngle)).normalized;

            return Quaternion.AngleAxis(rotAngle, rotAxis) * vec;
        }

        public static Vector3 getRandomVectorInCone(float distortionCone, Vector3 direction)
        {
            Vector3 axis;
            if (direction == Vector3.up) { axis = Vector3.Cross(direction, Vector3.right); }
            else { axis = Vector3.Cross(direction, Vector3.up); }
            Vector3 result = Quaternion.AngleAxis(Random.Range(0, distortionCone), axis) * direction;
            return (Quaternion.AngleAxis(Random.Range(0, 360.0f), direction) * result).normalized;
        }

        public static void DrawSphereMesh(Vector3 position, float radius, int detail)
        {

        }
    }
}


