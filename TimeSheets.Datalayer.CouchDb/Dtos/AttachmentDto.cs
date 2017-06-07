namespace Cmas.DataLayers.CouchDb.TimeSheets.Dtos
{
    /// <summary>
    /// Вложение
    /// </summary>
    public class AttachmentDto
    {
        /// <summary>
        /// Attachment MIME type
        /// </summary>
        public string Content_type;

        public int Revpos;

        public string Digest;

        /// <summary>
        /// Real attachment size in bytes
        /// </summary>
        public int Length;

        public bool Stub;
    }
}
