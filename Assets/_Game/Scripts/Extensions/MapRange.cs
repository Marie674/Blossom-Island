using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapRangeExtension {
	public static float MapRange (this float value, float initialRangeMin, float initialRangeMax, float targetRangeMin, float targetRangeMax) {
		return ((value - initialRangeMin) / (initialRangeMax - initialRangeMin) * (targetRangeMax - targetRangeMin) + targetRangeMin);
	}
}
