// Original header: "Adapted for Processing from "Plasma Fractal" by Giles Whitaker - Written January, 2002 by Justin Seyster (thanks for releasing into public domain, Justin)."
// Unity C# version - 4.4.2012 - mgear - http://unitycoder.com/blog
// Revision and migration to Standalone Object - 01.05.2014 - Sacristan

using UnityEngine;
using System.Collections;

public class DiamondSquare {
	Color32[] colors;
	int grain;
	int widthpheight;
	int m_x;
	int m_y;

	public DiamondSquare(int x, int y, int pgrain){
		m_x = x;
		m_y = y;

		widthpheight= m_x+m_y;
		grain = pgrain;

		colors = new Color32[m_x*m_y];
	}

	public Color32[] Draw(){
		float c1, c2, c3, c4;
		c1 = Random.value;
		c2 = Random.value;
		c3 = Random.value;
		c4 = Random.value;
		
		DivideGrid(0.0f, 0.0f, m_x , m_y , c1, c2, c3, c4);
		return colors;
	}

	float Displace(float num)
	{
		float max = num / widthpheight * grain;
		return Random.Range(-0.5f, 0.5f)* max;
	}
	
	//Returns a color based on a color value, c.
	Color ComputeColor(float c)
	{      
		float Red = 0f;
		float Green = 0f;
		float Blue = 0f;
		
		if (c < 0.5f)
		{
			Red = c * 2;
		}
		else
		{
			Red = (1.0f - c) * 2;
		}
		if (c >= 0.3 && c < 0.8f)
		{
			Green = (c - 0.3f) * 2;
		}
		else if (c < 0.3f)
		{
			Green = (0.3f - c) * 2;
		}
		else
		{
			Green = (1.3f - c) * 2;
		}
		
		if (c >= 0.5f)
		{
			Blue = (c - 0.5f) * 2;
		}
		else
		{
			Blue = (0.5f - c) * 2;
		}
		
		return new Color(Red, Green, Blue);
	}
	
	//This is the recursive function that implements the random midpoint
	//displacement algorithm.  It will call itself until the grid pieces
	//become smaller than one pixel.   
	void DivideGrid(float x, float y, float w, float h, float c1, float c2, float c3, float c4)
	{
		
		float newWidth = w * 0.5f;
		float newHeight = h * 0.5f;
		
		if (w < 1.0f || h < 1.0f)
		{
			//The four corners of the grid piece will be averaged and drawn as a single pixel.
			float c = (c1 + c2 + c3 + c4) * 0.25f;
			colors[(int)x+(int)y*m_x] = ComputeColor(c);
		}
		else
		{
			float middle =(c1 + c2 + c3 + c4) * 0.25f + Displace(newWidth + newHeight);      //Randomly displace the midpoint!
			float edge1 = (c1 + c2) * 0.5f; //Calculate the edges by averaging the two corners of each edge.
			float edge2 = (c2 + c3) * 0.5f;
			float edge3 = (c3 + c4) * 0.5f;
			float edge4 = (c4 + c1) * 0.5f;
			
			//Make sure that the midpoint doesn't accidentally "randomly displaced" past the boundaries!
			if (middle <= 0)
			{
				middle = 0;
			}
			else if (middle > 1.0f)
			{
				middle = 1.0f;
			}
			
			//Do the operation over again for each of the four new grids.                 
			DivideGrid(x, y, newWidth, newHeight, c1, edge1, middle, edge4);
			DivideGrid(x + newWidth, y, newWidth, newHeight, edge1, c2, edge2, middle);
			DivideGrid(x + newWidth, y + newHeight, newWidth, newHeight, middle, edge2, c3, edge3);
			DivideGrid(x, y + newHeight, newWidth, newHeight, edge4, middle, edge3, c4);
		}
	}
}
