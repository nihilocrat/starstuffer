using UnityEngine;
using System.Collections.Generic;
using TinyJSON;

[System.Serializable]
public class GalaxyData
{
    public int width;
    public int height;

    public List<SystemData> systems = new List<SystemData>();
}

[System.Serializable]
public class BaseData
{
    public int id = 0;
    public string name;

    public void Save(string filename)
    {

    }

    public static T Load<T>(string filename)
    {
        string json = "";

        return JSON.Load(json).Make<T>();
    }
}

public class SystemData : BaseData
{
    public int x;
    public int y;
    public int z;

    public List<StarData> stars = new List<StarData>();
    public List<PlanetData> planets = new List<PlanetData>();
}

public class StarData : BaseData
{
    // from +10 to -20
    public int magnitude;

    // degress K from 2000 to 50000 for main sequence stars
    public int temperature;
}

// technicaly "satellite", anything which can orbit another body
public class PlanetData : BaseData
{
    public double radius;

    /// more accurately, semi-major axis
    public int orbitalDistance;

    /// in earth days
    public int orbitalPeriod;

    /// advances the orbit this many days at time == 0 so planets don't seem so lined-up
    public int orbitalPeriodOffset;

    public int tilt;

    /// in earth hours, negative means retrograde rotation
    public int dayLength;

    /// atmospheric pressure
    public double atmosphere;

    /// average temperature degrees kelvin
    public int temperature;

    public int mass;

    /// in m/s/s, not G
    public double gravity;

    /// can theoretically recurse indefinitely; moons of moons of moons
    public List<PlanetData> moons = new List<PlanetData>();

    public Vector3 PositionAtTime(float time, float scale = 1f)
    {
        var degreesPerUnit = (360f / orbitalPeriod) / Mathf.Max(0.1f, orbitalDistance);

        float x = Mathf.Sin(degreesPerUnit * (time + orbitalPeriodOffset)) * orbitalDistance * scale;
        float z = Mathf.Cos(degreesPerUnit * (time + orbitalPeriodOffset)) * orbitalDistance * scale;

        return new Vector3(x, 0f, z);
    }

    public Quaternion RotationAtTime(float time)
    {
        float degreesPerHour = (24f / (float)dayLength) * (360f / 24f);

        return Quaternion.Euler(tilt, degreesPerHour * time, 0f);
    }
}
