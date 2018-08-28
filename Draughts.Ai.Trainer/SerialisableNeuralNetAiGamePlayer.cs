using RichTea.Common;
using RichTea.NeuralNetLib.Serialisation;

namespace Draughts.Ai.Trainer
{
    public class SerialisableNeuralNetAiGamePlayer
    {
        public SerialisedNet Net { get; set; }

        public int Generation { get; set; }

        public NeuralNetAiGamePlayer CreateNeuralNetAiGamePlayer()
        {
            var net = Net.CreateNet();
            var neuralNetAiGamePlayer = new NeuralNetAiGamePlayer(net, Generation);
            return neuralNetAiGamePlayer;
        }

        public override string ToString()
        {
            return new ToStringBuilder<SerialisableNeuralNetAiGamePlayer>(this)
                .Append(p => p.Net)
                .Append(p => p.Generation)
                .ToString();
        }

        public override bool Equals(object that)
        {
            var otherSerialisableNeuralNetAiGamePlayer = that as SerialisableNeuralNetAiGamePlayer;
            return new EqualsBuilder<SerialisableNeuralNetAiGamePlayer>(this, that)
                .Append(Net, otherSerialisableNeuralNetAiGamePlayer?.Net)
                .Append(Generation, otherSerialisableNeuralNetAiGamePlayer?.Generation)
                .AreEqual;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder<SerialisableNeuralNetAiGamePlayer>(this)
                .Append(Net)
                .Append(Generation)
                .HashCode;
        }

    }
}
