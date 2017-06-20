namespace Cmas.DataLayers.CouchDb.TimeSheets
{
    /// <summary>
    /// Константы БД
    /// </summary>
    internal class DbConsts
    {
        /// <summary>
        /// Имя сущности
        /// </summary>
        public const string ServiceName = "time-sheets";

        /// <summary>
        /// Имя дизайн документа
        /// </summary>
        public const string DesignDocumentName = "time-sheets";

        /// <summary>
        /// Имя представления всех документов
        /// </summary>
        public const string AllDocsViewName = "all";

        /// <summary>
        /// Имя представления документов, сгрупированных по ID наряд заказа
        /// </summary>
        public const string ByCallOffOrderDocsViewName = "byCallOffOrderId";

        /// <summary>
        /// Имя представления документов, сгрупированных по ID наряд заказа и заявки на проверку
        /// </summary>
        public const string ByCallOffOrderAndRequestDocsViewName = "byCallOffOrderAndRequest";

        /// <summary>
        /// Имя представления документов, сгрупированных по ID заявки
        /// </summary>
        public const string ByRequestIdDocsViewName = "byRequestId";

        /// <summary>
        /// Имя представления идентификаторов табелей, сгрупированных по ID заявки
        /// </summary>
        public const string IdsByRequestDocsViewName = "IdsByRequest";

        /// <summary>
        /// Имя представления, которое выводит табели и их вложения
        /// </summary>
        public const string AttachmentsViewName = "attachments";

        /// <summary>
        /// Имя представления, которое выводит токены на загрузку файлов
        /// </summary>
        public const string AttachmentTokensViewName = "attachmentTokens";
    }
}
