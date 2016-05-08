using UnityEngine;
using System.Collections;

public class NoiseTest : MonoBehaviour {
	public enum NoiseType {Perlin, Voronoi, DiamondSquare};
	public enum VoronoiDistanceFunction {Euclidian, Manhattan, Chebyshev};
	public enum VoronoiCombination {D0, D1_D0, D2_D0};

	public NoiseType noiseType;
	public VoronoiDistanceFunction voronoiDistanceFunction;
	public VoronoiCombination voronoiCombination;

	public int octaves = 1;
	public float frq = 100.0f;
	public float amp = 1.0f;

	public BoundaryInt seedBounds = new BoundaryInt();

	void Start(){
		Texture2D texture = new Texture2D((int)GetComponent<GUITexture>().pixelInset.width, (int)GetComponent<GUITexture>().pixelInset.height, TextureFormat.RGB24, false);
		GetComponent<GUITexture>().texture = texture;

		int seed = seedBounds.GetRandomValueWithinBounds ();
		Debug.Log ("Noise Seed Generated -> "+seed);

		//Set what distance function to use
		switch (voronoiDistanceFunction) {
			case VoronoiDistanceFunction.Euclidian:
				VoronoiNoise.SetDistanceToEuclidian();
				break;
			case VoronoiDistanceFunction.Manhattan:
				VoronoiNoise.SetDistanceToManhattan();
				break;
			case VoronoiDistanceFunction.Chebyshev:
				VoronoiNoise.SetDistanceToChebyshev();
				break;
		}

		//Set what combination to use
		switch (voronoiCombination) {
			case VoronoiCombination.D0:
				VoronoiNoise.SetCombinationTo_D0();
				break;
			case VoronoiCombination.D1_D0:
				VoronoiNoise.SetCombinationTo_D1_D0();
				break;
			case VoronoiCombination.D2_D0:
				VoronoiNoise.SetCombinationTo_D2_D0();
				break;
		}

		for (int y = 0; y < texture.height; ++y) 
		{
			for (int x = 0; x < texture.width; ++x) 
			{
				float noise = 0.0f;
				switch(noiseType){
					case NoiseType.Perlin:
						Random.seed = 0;
						PerlinNoise pNoise = new PerlinNoise(seed);
						noise = pNoise.FractalNoise2D(x, y, octaves, frq, amp);
						texture.SetPixel(x, y, new Color(noise,noise,noise,1.0f));
						break;
					case NoiseType.Voronoi:
						noise = VoronoiNoise.FractalNoise2D(x, y, octaves, frq, amp, seed);
						texture.SetPixel(x, y, new Color(noise,noise,noise,1.0f));
						break;
                }

			}
		}
		if (noiseType == NoiseType.DiamondSquare) {
			DiamondSquare diamondSquare = new DiamondSquare(texture.height, texture.width, seed);
			texture.SetPixels32(diamondSquare.Draw());
		}

		texture.Apply();

	}

}