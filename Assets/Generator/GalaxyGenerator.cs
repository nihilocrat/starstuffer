using UnityEngine;
using System.Collections.Generic;

public class GalaxyGenerator : MonoBehaviour
{
    public int minWidth;
    public int maxWidth;
    public int minHeight;
    public int maxHeight;

    public int minSystems;
    public int maxSystems;

    public int minStars;
    public int maxStars;

    public int minPlanets;
    public int maxPlanets;

    public float minPerturbation;
    public float maxPerturbation;

    public TextAsset systemNamesFile;

    private List<string> systemNames;

    private string[] letters = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

    private string[] moonLetters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };

    [HideInInspector]
    public GalaxyData data;

    private int idIncremetor = 0;

    void Awake()
    {
        data = Generate();
    }

    private int GenerateID()
    {
        idIncremetor++;
        return idIncremetor;
    }

    public GalaxyData Generate()
    {
        systemNames = new List<string>(systemNamesFile.text.Split('\n'));

        var data = new GalaxyData();

        data.width = Random.Range(minWidth, maxWidth);
        data.height = Random.Range(minHeight, maxHeight);

        int numSystems = Random.Range(minSystems, maxSystems);

        int systemsPerRow = Mathf.FloorToInt( Mathf.Sqrt(numSystems) );

        float scale = (float)data.width / (float)systemsPerRow;

        // system placement
        for (int x=0; x < systemsPerRow; x++)
        {
            for(int y=0; y < systemsPerRow; y++)
            {
                var sys = GenerateSystem();

                sys.x = Mathf.RoundToInt( x * scale );
                sys.y = 0;
                sys.z = Mathf.RoundToInt( y * scale );

                var basePerturbation = Random.Range(minPerturbation, maxPerturbation);
                var perturbation = Random.insideUnitCircle * basePerturbation;

                sys.x += Mathf.RoundToInt(perturbation.x);
                sys.z += Mathf.RoundToInt(perturbation.y);

                data.systems.Add(sys);
            }
        }

        return data;
    }


    public SystemData GenerateSystem()
    {
        var sys = new SystemData();

        sys.id = GenerateID();
        sys.name = systemNames[Random.Range(0, systemNames.Count)];
        
        int numStars = Random.Range(minStars, maxStars);

        for (int i = 0; i < numStars; i++)
        {
            sys.stars.Add(GenerateStar());
        }

        int numPlanets = Random.Range(minPlanets, maxPlanets);

        for(int i=0; i < numPlanets; i++)
        {
            var planet = new PlanetData();

            planet.id = GenerateID();
            planet.name = sys.name + " " + letters[i];

            planet.radius = Random.Range(1,10);
            planet.orbitalDistance = ((i + 1) * 10) + Random.Range(-2,2);

            // arbitrarily choosing a range from Mercury's period to twice of Earth's
            // period should be refactored because in reality, distance affects period
            planet.orbitalPeriod = Random.Range(88, 365 * 2);
            planet.orbitalPeriodOffset = Random.Range(0, 360);
            planet.tilt = Random.Range(0,30);
            planet.dayLength = Random.Range(12,72);

            if(Random.value > 0.5f)
            {
                int moonCount = Random.Range(1,3);

                for (int j = 0; j < moonCount; j++)
                {
                    var moon = new PlanetData();

                    moon.id = GenerateID();
                    moon.name = planet.name + " " + moonLetters[j];

                    moon.radius = Random.Range(1, 2);
                    moon.orbitalDistance = (j + 1) * 10;
                    moon.orbitalPeriod = Random.Range(planet.dayLength, planet.dayLength * 2);
                    moon.orbitalPeriodOffset = Random.Range(0, 360);
                    moon.tilt = Random.Range(0, 30);
                    moon.dayLength = 1;

                    planet.moons.Add(moon);
                }
            }

            sys.planets.Add(planet);
        }

        return sys;
    }

    public StarData GenerateStar()
    {
        var star = new StarData();

        star.id = GenerateID();

        star.magnitude = Random.Range(-20, 10);
        star.temperature = Random.Range(2000, 50000);

        return star;
    }
}
