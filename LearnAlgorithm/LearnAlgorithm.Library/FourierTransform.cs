using MathNet.Numerics.IntegralTransforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LearnAlgorithm.Library
{
    public class FourierTransform
    {
        /// <summary>
        /// 数字离散傅立叶正向变换
        /// </summary>
        /// <param name="input">输入（时域）</param>
        /// <returns>输出（频域）</returns>
        public static double[] Forward(double[] input)
        {
            int DataLength = input.Length;
            Complex[] samples = new Complex[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                samples[i] = new Complex(input[i], 0);
            }

            //傅立叶变换
            Fourier.Forward(samples);

            //求模
            int FreqLen = DataLength / 2 + 1;
            double[] freqMod = new double[FreqLen];
            double rate = Math.Sqrt(DataLength) / 2;

            for (int i = 0; i < FreqLen; i++)
            {
                var a = samples[i].Real;
                var b = samples[i].Imaginary;
                freqMod[i] = Math.Sqrt(a * a + b * b) / rate;
            }

            return freqMod;
        }
        /*
        *  傅立叶变换原理：
        *  假设输入数据长度为2048，变换后数据长度为2048，频域数据为对称数据，仅一半有用,区间为：[0 , N/2+1]
        *  假设输入的数据采样周期为1s，基准频率Fb=2/t;输出对应的频率为0Hz+1-1024Hz，其中0HZ为直流分量，右侧数据以2048为中心轴对称
        *  如果输入的数据采样时间为0.001s，则输出频率为0Hz+1-1024KHz，依此类推。
        *  最大频率：Fmax=Fb*(N/2+1)
        *  
        *  设采样时间:t=0.02s
        *  采样频率:Fb=1/t=50Hz
        *  输出频率：Fn=Fb*(n-1)，n=[0 , N/2+1]
        *  
        *  输出的频域幅度和采样率相关，比率=根号（N）/2
        */

        /// <summary>
        /// 数字离散带通滤波
        /// 当数据长度为2的n次方时采用快速傅立叶算法，否则采用巴特沃斯算法
        /// 0 < MinFreq < MaxFreq < N/2
        /// </summary>
        /// <param name="input">输入（时域）</param>
        /// <param name="MinFreq">下限</param>
        /// <param name="MaxFreq">上限</param>
        /// <param name="Fb">基准频率（Hz）Fb=1/t</param>
        /// <returns>输出（时域）</returns>
        public static double[] BandpassFilter(double[] input, int MinFreq, int MaxFreq, int Fb)
        {
            int N = input.Length;

            Complex[] samples = new Complex[N];
            for (int i = 0; i < N; i++)
            {
                samples[i] = new Complex(input[i], 0);
            }

            //傅立叶变换
            Fourier.Forward(samples);

            //滤波
            //  n=Fn/Fs+1
            int Nmin = MinFreq / Fb + 1;
            int Nmax = MaxFreq / Fb + 1;
            for (int i = 0; i < Nmin; i++)
            {
                samples[i] = new Complex(0, 0);
            }
            for (int i = N - Nmin; i < N; i++)
            {
                samples[i] = new Complex(0, 0);
            }
            if (Nmax < N / 2)
            {
                for (int i = Nmax; i < N - Nmax; i++)
                {
                    samples[i] = new Complex(0, 0);
                }
            }

            //逆变换
            Fourier.Inverse(samples);

            //计算时域波形：由于输入虚部均为0，逆变换后虚部也为0，直接取用实部即可
            double[] sampleResult = new double[N];
            for (int i = 0; i < N; i++)
            {
                sampleResult[i] = samples[i].Real;
            }

            return sampleResult;
        }


    }
}
