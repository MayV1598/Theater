//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lr11_13.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ЗП_актеров
    {
        public int id { get; set; }
        public int id_контракта { get; set; }
        public int Месяц { get; set; }
        public int Премия { get; set; }
        public int Надбавка { get; set; }
        public int Итог { get; set; }

        public int ИтоговаяСумма { get
            {
                return Премия + Надбавка + Контракты.Выплаты_в_месяц;
            } 
        }
    
        public virtual Контракты Контракты { get; set; }
    }
}
