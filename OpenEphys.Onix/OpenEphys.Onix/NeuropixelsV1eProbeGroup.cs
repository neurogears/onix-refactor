using System;
using System.Text;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using OpenEphys.ProbeInterface;
using Newtonsoft.Json;
using Bonsai;

namespace OpenEphys.Onix
{
    [GeneratedCode("Bonsai.Sgen", "0.3.0.0 (Newtonsoft.Json v13.0.0.0)")]
    [Combinator]
    [WorkflowElementCategory(ElementCategory.Source)]
    public class NeuropixelsV1eProbeGroup : ProbeGroup
    {
        private const int ChannelCount = 384;
        private const int ElectrodeCount = 960;

        public NeuropixelsV1eProbeGroup()
            : base("probeinterface", "0.2.21",
                  new List<Probe>()
                  {
                      new(ProbeNdim._2,
                          ProbeSiUnits.Um,
                          new ProbeAnnotations("Neuropixels 1.0e", "IMEC"),
                          new ContactAnnotations(new string[0]),
                          DefaultContactPositions(ElectrodeCount),
                          Probe.DefaultContactPlaneAxes(ElectrodeCount),
                          Probe.DefaultContactShapes(ElectrodeCount, ContactShape.Square),
                          Probe.DefaultSquareParams(ElectrodeCount, 12.0f),
                          DefaultProbePlanarContour(),
                          DefaultDeviceChannelIndices(ChannelCount, ElectrodeCount),
                          Probe.DefaultContactIds(ElectrodeCount),
                          Probe.DefaultShankIds(ElectrodeCount))
                  }.ToArray())
        {
        }


        [JsonConstructor]
        public NeuropixelsV1eProbeGroup(string specification, string version, Probe[] probes)
            : base(specification, version, probes)
        {
        }

        public NeuropixelsV1eProbeGroup(NeuropixelsV1eProbeGroup probeGroup)
            : base(probeGroup)
        {
        }

        public static float[][] DefaultContactPositions(int numberOfChannels)
        {
            if (numberOfChannels % 2 != 0)
            {
                throw new ArgumentException("Invalid number of channels given; must be a multiple of two");
            }

            float[][] contactPositions = new float[numberOfChannels][];

            for (int i = 0; i < numberOfChannels; i++)
            {
                contactPositions[i] = new float[2] { ContactPositionX(i), i / 2 * 20 };
            }

            return contactPositions;
        }

        private static float ContactPositionX(int index)
        {
            bool even = index % 2 == 0;
            bool twoOrThree = (index / 2) % 2 == 0;

            if (even && !twoOrThree)
            {
                return 27.0f;
            }
            else if (!even && !twoOrThree)
            {
                return 59.0f;
            }
            else if (even && twoOrThree)
            {
                return 11.0f;
            }
            else
            {
                return 43.0f;
            }
        }

        /// <summary>
        /// Generates a default planar contour for the probe, based on the given probe index
        /// </summary>
        /// <returns></returns>
        public static float[][] DefaultProbePlanarContour()
        {
            float[][] probePlanarContour = new float[6][];

            probePlanarContour[0] = new float[2] { 0f, -15f };
            probePlanarContour[1] = new float[2] { 35f, -60f };
            probePlanarContour[2] = new float[2] { 70f, -15f };
            probePlanarContour[3] = new float[2] { 70f, 9600f };
            probePlanarContour[4] = new float[2] { 0f, 9600f };
            probePlanarContour[5] = new float[2] { 0f, -15f };

            return probePlanarContour;
        }

        /// <summary>
        /// Override of the DefaultDeviceChannelIndices function, which initializes a portion of the
        /// device channel indices, and leaves the rest at -1 to indicate they are not actively recorded
        /// </summary>
        /// <param name="channelCount">Number of contacts that are connected for recording</param>
        /// <param name="electrodeCount">Total number of physical contacts on the probe</param>
        /// <returns></returns>
        public static int[] DefaultDeviceChannelIndices(int channelCount, int electrodeCount)
        {
            int[] deviceChannelIndices = new int[electrodeCount];

            for (int i = 0; i < channelCount; i++)
            {
                deviceChannelIndices[i] = i;
            }

            for (int i = channelCount; i < electrodeCount; i++)
            {
                deviceChannelIndices[i] = -1;
            }

            return deviceChannelIndices;
        }

        public IObservable<NeuropixelsV1eProbeGroup> Process()
        {
            return Observable.Defer(() => Observable.Return(new NeuropixelsV1eProbeGroup(this)));
        }

        public IObservable<NeuropixelsV1eProbeGroup> Process<TSource>(IObservable<TSource> source)
        {
            return Observable.Select(source, _ => new NeuropixelsV1eProbeGroup(this));
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
            if (PrintMembers(stringBuilder))
            {
                stringBuilder.Append(" ");
            }
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}
