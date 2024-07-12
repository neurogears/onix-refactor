using System;
using System.Text;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using OpenEphys.ProbeInterface;
using Newtonsoft.Json;

namespace OpenEphys.Onix
{
    [GeneratedCodeAttribute("Bonsai.Sgen", "0.3.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public class Rhs2116ProbeGroup : ProbeGroup
    {
        public const int NumberOfChannelsPerProbe = 16;

        public Rhs2116ProbeGroup()
            : base("probeinterface", "0.2.21",
                new List<Probe>()
                {
                    new(
                        ProbeNdim._2,
                        ProbeSiUnits.Mm,
                        new ProbeAnnotations("Rhs2116A", ""),
                        new ContactAnnotations(new string[0]),
                        DefaultContactPositions(NumberOfChannelsPerProbe, 0),
                        Probe.DefaultContactPlaneAxes(NumberOfChannelsPerProbe),
                        Probe.DefaultContactShapes(NumberOfChannelsPerProbe, ContactShape.Circle),
                        Probe.DefaultCircleParams(NumberOfChannelsPerProbe, 0.3f),
                        DefaultProbePlanarContour(0),
                        Probe.DefaultDeviceChannelIndices(NumberOfChannelsPerProbe, 0),
                        Probe.DefaultContactIds(NumberOfChannelsPerProbe),
                        Probe.DefaultShankIds(NumberOfChannelsPerProbe)),
                    new(
                        ProbeNdim._2,
                        ProbeSiUnits.Mm,
                        new ProbeAnnotations("Rhs2116B", ""),
                        new ContactAnnotations(new string[0]),
                        DefaultContactPositions(NumberOfChannelsPerProbe, 1),
                        Probe.DefaultContactPlaneAxes(NumberOfChannelsPerProbe),
                        Probe.DefaultContactShapes(NumberOfChannelsPerProbe, ContactShape.Circle),
                        Probe.DefaultCircleParams(NumberOfChannelsPerProbe, 0.3f),
                        DefaultProbePlanarContour(1),
                        Probe.DefaultDeviceChannelIndices(NumberOfChannelsPerProbe, NumberOfChannelsPerProbe),
                        Probe.DefaultContactIds(NumberOfChannelsPerProbe),
                        Probe.DefaultShankIds(NumberOfChannelsPerProbe))
                }.ToArray())
        {
        }

        [JsonConstructor]
        public Rhs2116ProbeGroup(string specification, string version, Probe[] probes)
            : base(specification, version, probes)
        {
        }

        public Rhs2116ProbeGroup(Rhs2116ProbeGroup probeGroup)
            : base(probeGroup)
        {
        }

        public static float[][] DefaultContactPositions(int numberOfChannels, int probeIndex)
        {
            float[][] contactPositions = new float[numberOfChannels][];

            if (probeIndex == 0)
            {
                for (int i = 0; i < numberOfChannels; i++)
                {
                    contactPositions[i] = new float[2] { numberOfChannels - i, 3.0f };
                }
            }
            else if (probeIndex == 1)
            {
                for (int i = 0; i < numberOfChannels; i++)
                {
                    contactPositions[i] = new float[2] { i + 1.0f, 1.0f };
                }
            }
            else
            {
                throw new InvalidOperationException($"Probe {probeIndex} is invalid for getting default contact positions for {nameof(Rhs2116ProbeGroup)}");
            }

            return contactPositions;
        }

        /// <summary>
        /// Generates a default planar contour for the probe, based on the given probe index
        /// </summary>
        /// <param name="probeIndex">Zero-based index of the probe to generate a contour for</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Probe indices less than 0 and greater than 1 are not allowed.</exception>
        public static float[][] DefaultProbePlanarContour(int probeIndex)
        {
            float[][] probePlanarContour = new float[5][];

            if (probeIndex == 0)
            {
                probePlanarContour[0] = new float[2] { 0.5f, 2.5f };
                probePlanarContour[1] = new float[2] { 16.5f, 2.5f };
                probePlanarContour[2] = new float[2] { 16.5f, 3.5f };
                probePlanarContour[3] = new float[2] { 0.5f, 3.5f };
                probePlanarContour[4] = new float[2] { 0.5f, 2.5f };
            }
            else if (probeIndex == 1)
            {
                probePlanarContour[0] = new float[2] { 0.5f, 0.5f };
                probePlanarContour[1] = new float[2] { 16.5f, 0.5f };
                probePlanarContour[2] = new float[2] { 16.5f, 1.5f };
                probePlanarContour[3] = new float[2] { 0.5f, 1.5f };
                probePlanarContour[4] = new float[2] { 0.5f, 0.5f };
            }
            else
            {
                throw new InvalidOperationException($"Probe {probeIndex} is invalid for getting default probe planar contours for {nameof(Rhs2116ProbeGroup)}");
            }

            return probePlanarContour;
        }

        public IObservable<Rhs2116ProbeGroup> Process()
        {
            return Observable.Defer(() => Observable.Return(new Rhs2116ProbeGroup(this)));
        }

        public IObservable<Rhs2116ProbeGroup> Process<TSource>(IObservable<TSource> source)
        {
            return Observable.Select(source, _ => new Rhs2116ProbeGroup(this));
        }

        protected virtual bool PrintMembers(StringBuilder stringBuilder)
        {
            stringBuilder.Append("specification = " + Specification + ", ");
            stringBuilder.Append("version = " + Version + ", ");
            stringBuilder.Append("probes = " + Probes);
            return true;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(GetType().Name);
            stringBuilder.Append(" { ");
            if (PrintMembers(stringBuilder) )
            {
                stringBuilder.Append(" ");
            }
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}
