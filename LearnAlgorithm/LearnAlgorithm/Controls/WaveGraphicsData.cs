using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnAlgorithm.Controls
{
    public class WaveGraphicsData
    {
        public GraphicsType GraphicsType { get; set; }

        public double[]? WaveData { get; set; } = null;
    }

    public enum GraphicsType
    {
        Wave = 0,
        Frequency = 1,
    }
}
