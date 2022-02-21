﻿using System.ComponentModel.DataAnnotations;

namespace CallCenterCRM.Models
{
    public enum ReferenceSources
    {
        [Display(Name = "Телефон доверия")] helpline = 0,

    }
    public enum Genders
    { 
        [Display(Name = "мужчина")] Male = 0,
        [Display(Name = "женщина")] Female = 1
    }

    public enum Types
    {
        [Display(Name = "Юрик. лиц.")] Business = 0,
        [Display(Name = "Физ. лиц.")] Individual = 1
    }

    public enum Employments
    {
        [Display(Name = "Безработный")] Unemployed = 0,
        [Display(Name = "Пенсионер")] Retired = 1,
        [Display(Name = "Студент")] Student = 2,
        [Display(Name = "Работает")] Employed = 3,
    }

    public enum AppTypes
    {
        [Display(Name = "Заявления")] Statement = 0,
        [Display(Name = "Жалобы")] Complaint = 1,
        [Display(Name = "Предложения")] Offer = 2,
        [Display(Name = "Письмо")] Letter = 3,
    }

    public enum Regions
    {
        [Display(Name = "г. Ташкент")] TashkentCity,
        [Display(Name = "Андижан")] Andijan = 2,
        [Display(Name = "Бухара")] Bukhara = 3,
        [Display(Name = "Фергана")] Fergana = 4,
        [Display(Name = "Джизак")] Jizzakh = 5,
        [Display(Name = "Наманган")] Namangan = 6,
        [Display(Name = "Навои")] Navoiy = 7,
        [Display(Name = "Кашкадарья")] Qashqadaryo = 8,
        [Display(Name = "Самарканд")] Samarqand = 9,
        [Display(Name = "Сырдарья")] Sirdaryo = 10,
        [Display(Name = "Сурхандарьинская область")] Surxondaryo = 11,
        [Display(Name = "Ташкентская область")] TashkentRegion = 12,
        [Display(Name = "Хорезм")] Xorazm = 13,
        [Display(Name = "Каракалпакстан")] Karakalpakstan = 14,
    }
}