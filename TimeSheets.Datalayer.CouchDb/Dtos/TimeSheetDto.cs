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
        /// Год
        /// </summary>
        public int Year;

        /// <summary>
        /// Месяц
        /// </summary>
        public int Month;

        /// <summary>
        /// Рабочее время в разрезе работ
        /// Dictionary<{ID ставки}, IEnumerable<{время по каждому дню в месяце}>>
        /// </summary>
        public Dictionary<string, IEnumerable<double>> SpentTime;

        /// <summary>
        /// Статус
        /// </summary>
        public int Status;
    }

}