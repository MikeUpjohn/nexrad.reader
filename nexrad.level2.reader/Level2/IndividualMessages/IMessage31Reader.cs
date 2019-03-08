using nexrad.models;

namespace nexrad.reader.Level2.IndividualMessages
{
    public interface IMessage31Reader
    {
        RecordMessageRecord ReadMessage31(byte[] fileData);
        RecordMessageRecordDataBlock ReadDataBlockPointers(byte[] fileData);
        VolumeData ParseVolumeData(byte[] fileData);
        ElevationData ParseElevationData(byte[] fileData);
        RadialData ParseRadialData(byte[] fileData);
        MomentData ParseMomentData(byte[] fileData);
        float[] ParseReflectivityMomentData(byte[] fileData, float offset, float scale);
        float[] ParseVelocityMomentData(byte[] fileData, float offset, float scale);
        float[] ParseSpectrumWidthMomentData(byte[] fileData, float offset, float scale);
        float[] ParseDifferentialReflectivityMomentData(byte[] fileData, float offset, float scale);
        float[] ParseDifferentialPhaseMomentData(byte[] fileData, float offset, float scale);
        float[] ParseCorrelationCoefficientMomentData(byte[] fileData, float offset, float scale);
    }
}
