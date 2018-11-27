private static List<double> CubicInterpolation(double[] sourceX, double[] sourceY, double[] newX)
		{
			var n = sourceX.LongLength;
			if (sourceX.LongLength != sourceY.LongLength)
			{
				return null;
			}

			if (sourceX.LongLength <= 3)
			{
				return null;
			}

			if (sourceX.LongLength >= newX.LongLength)
			{
				return null;
			}

			if (!sourceX[0].Equals(newX[0]))
			{
				return null;
			}

			if (!sourceX[sourceX.LongLength - 1].Equals(newX[newX.LongLength - 1]))
			{
				return null;
			}

			var nX = n - 1;
			var dx = new double[nX];
			var b = new double[n];
			var alfa = new double[n];
			var beta = new double[n];
			var gama = new double[n];

			var coefs = new double[4][];
			for (long i = 0; i < 4; i++)
			{
				coefs[i] = new double[nX];
			}

			for (long i = 0; i + 1 <= nX; i++)
			{
				dx[i] = sourceX[i + 1] - sourceX[i];
				if (dx[i].Equals(0.0))
				{
					return null;   
				}
			}

			for (long i = 1; i + 1 <= nX; i++)
			{
				b[i] = 3.0 * (dx[i] * ((sourceY[i] - sourceY[i - 1]) / dx[i - 1]) + dx[i - 1] * ((sourceY[i + 1] - sourceY[i]) / dx[i]));
			}

			b[0] = ((dx[0] + 2.0 * (sourceX[2] - sourceX[0])) * dx[1] * ((sourceY[1] - sourceY[0]) / dx[0]) +
						Math.Pow(dx[0], 2.0) * ((sourceY[2] - sourceY[1]) / dx[1])) / (sourceX[2] - sourceX[0]);
			b[n - 1] = (Math.Pow(dx[nX - 1], 2.0) * ((sourceY[n - 2] - sourceY[n - 3]) / dx[nX - 2]) + (2.0 * (sourceX[n - 1] - sourceX[n - 3])
				+ dx[nX - 1]) * dx[nX - 2] * ((sourceY[n - 1] - sourceY[n - 2]) / dx[nX - 1])) / (sourceX[n - 1] - sourceX[n - 3]);
			beta[0] = dx[1];
			gama[0] = sourceX[2] - sourceX[0];
			beta[n - 1] = dx[nX - 1];
			alfa[n - 1] = (sourceX[n - 1] - sourceX[n - 3]);
			for (long i = 1; i < n - 1; i++)
			{
				beta[i] = 2.0 * (dx[i] + dx[i - 1]);
				gama[i] = dx[i];
				alfa[i] = dx[i - 1];
			}

			double c;
			for (long i = 0; i < n - 1; i++)
			{
				c = beta[i];
				b[i] /= c;
				beta[i] /= c;
				gama[i] /= c;

				c = alfa[i + 1];
				b[i + 1] -= c * b[i];
				alfa[i + 1] -= c * beta[i];
				beta[i + 1] -= c * gama[i];
			}

			b[n - 1] /= beta[n - 1];
			beta[n - 1] = 1.0;
			for (var i = n - 2; i >= 0; i--)
			{
				c = gama[i];
				b[i] -= c * b[i + 1];
				gama[i] -= c * beta[i];
			}

			for (long i = 0; i < nX; i++)
			{
				var dzzdx = (sourceY[i + 1] - sourceY[i]) / Math.Pow(dx[i], 2.0) - b[i] / dx[i];
				var dzdxdx = b[i + 1] / dx[i] - (sourceY[i + 1] - sourceY[i]) / Math.Pow(dx[i], 2.0);
				coefs[0][i] = (dzdxdx - dzzdx) / dx[i];
				coefs[1][i] = (2.0 * dzzdx - dzdxdx);
				coefs[2][i] = b[i];
				coefs[3][i] = sourceY[i];
			}

			var newY = new double[newX.LongLength];
			long j = 0;
			for (long i = 0; i < n - 1; i++)
			{
				if (j >= newX.LongLength)
				{
					break;	
				}
				while (newX[j] < sourceX[i + 1])
				{
					var h = newX[j] - sourceX[i];
					newY[j] = coefs[3][i] + h * (coefs[2][i] + h * (coefs[1][i] + h * coefs[0][i] / 3.0) / 2.0);
					j++;
					if (j >= newX.LongLength)
					{
						break;
					}
				}

				if (j >= newX.LongLength)
				{
					break;
				}
			}

			newY[newY.LongLength - 1] = sourceY[n - 1];
			return newY.ToList();
}
