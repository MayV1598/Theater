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
    
    public partial class Сотрудники
    {
        public int id_сотрудника { get; set; }
        public string ФИО { get; set; }
        public System.DateTime Дата_рождения { get; set; }
        public string Date
        {
            get => Дата_рождения.ToString("dd.MM.yyyy");
        }
        public string Номер_телефона { get; set; }
        public string Должность { get; set; }
        public string Логин { get; set; }
        public string Пароль { get; set; }
    
        public virtual Актеры Актеры { get; set; }
    }
}
