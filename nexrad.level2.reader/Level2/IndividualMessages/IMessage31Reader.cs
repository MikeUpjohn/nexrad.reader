using nexrad.models;

namespace nexrad.reader.Level2.IndividualMessages
{
    public interface IMessage31Reader
    {
        RecordMessageRecord ReadMessage31(byte[] fileData);
        RecordMessageRecordDataBlock ReadDataBlockPointers(byte[] fileData);
        VolumeData ParseVolumeData(byte[] fileData, int offset, int dbp);
        ElevationData ParseElevationData(byte[] fileData, int offset, int dbp);
        RadialData ParseRadialData(byte[] fileData, int offset, int dbp);
        MomentData ParseMomentData(byte[] fileData, int offset, int dbp);
        float[] ParseReflectivityMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp);
        float[] ParseVelocityMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp);
        float[] ParseSpectrumWidthMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp);
        float[] ParseDifferentialReflectivityMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp);
        float[] ParseDifferentialPhaseMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp);
        float[] ParseCorrelationCoefficientMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp);
    }
}
