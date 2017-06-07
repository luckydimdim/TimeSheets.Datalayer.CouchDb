using System;
using System.Collections.Generic;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Dtos
{
    public class TimeSheetDto
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
        /// Идентификатор наряд заказа
        /// </summary>
        public string CallOffOrderId;

        /// <summary>
        /// Идентификатор заявки на проверку (null, если не принадлежит заявке)
        /// </summary>
        public string RequestId;

        /// <summary>
        /// Дата и время создания
        /// </summary>
        public DateTime CreatedAt;

        /// <summary>
        /// Дата и время обновления
        /// </summary>
        public DateTime UpdatedAt;

        /// <summary>
        /// Примечания
        /// </summary>
        public string Notes;

        /// <summary>
        /// Период - начало
        /// </summary>
        public DateTime From;

        /// <summary>
        /// Период - окончание
        /// </summary>
        public DateTime Till;

        /// <summary>
        /// Рабочее время в разрезе работ
        /// Dictionary<{ID ставки}, IEnumerable<{время по каждому дню в месяце}>>
        /// </summary>
        public Dictionary<string, IEnumerable<double>> SpentTime;

        /// <summary>
        /// Сумма по табелю
        /// </summary>
        public double Amount;

        /// <summary>
        /// Валюта
        /// </summary>
        public string CurrencySysName;

        /// <summary>
        /// Статус
        /// </summary>
        public int Status;

        /// <summary>
        /// Вложения
        /// </summary>
        public Dictionary<string, AttachmentDto> _attachments;
    }

}