namespace BLL.DTOs.Track
{
    public class TrackGetDTO
    {
        public long Id { get; set; }
        public long HikeId { get; set; }
        public string FileName { get; set; }
        public byte[] GpxFile { get; set; }
    }
}
