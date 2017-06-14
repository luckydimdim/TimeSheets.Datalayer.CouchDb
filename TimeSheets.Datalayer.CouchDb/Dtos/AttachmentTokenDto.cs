using System;
namespace Cmas.DataLayers.CouchDb.TimeSheets.Dtos
{
    public class AttachmentTokenDto
    {
        /// <summary>
        /// Уникальный внутренний идентификатор
        /// </summary>
        public String _id;

        /// <summary>
        ///
        /// </summary>
        public String _rev;

        /// <summary>
        /// Дата и время создания
        /// </summary>
        public DateTime CreatedAt;

        public string TimeSheetId;

        public string FileName;

        public string Token;
    }
}
