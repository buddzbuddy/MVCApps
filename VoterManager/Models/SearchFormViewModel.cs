using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Models
{
    public class SearchFormViewModel
    {
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "ФИО")]
        public virtual string FullName
        {
            get
            {
                return LastName + " " + FirstName + " " + MiddleName;
            }
        }

        [Display(Name = "Сдал биометрические данные?")]
        public GaveBiometricDataType GaveBiometricData { get { return defValue; } set { defValue = value; } }

        private GaveBiometricDataType defValue = GaveBiometricDataType.all;
        public enum GaveBiometricDataType
        {
            yes, no, all
        }

        [Display(Name = "Возраст (лет)")]
        public int? Age { get; set; }
        public AgeExpressionType AgeExpression { get { return ageExpressionDefault; } set { ageExpressionDefault = value; } }
        private AgeExpressionType ageExpressionDefault = AgeExpressionType.Equal;
        public IEnumerable<SelectListItem> SelectAgeExpressions = from a in Enum.GetNames(typeof(AgeExpressionType))
                                                                  select new SelectListItem
                                                                  {
                                                                      Text = GetAgeExpressionText((AgeExpressionType)Enum.Parse(typeof(AgeExpressionType), a, true)),
                                                                      Selected = a == "Equal",
                                                                      Value = a
                                                                  };
        public enum AgeExpressionType
        {
            Equal = 0, GreatThen = 1, LessThen = 2, GreatEqual = 3, LessEqual = 4
        }
        private static string GetAgeExpressionText(AgeExpressionType aExpType)
        {
            if (aExpType == AgeExpressionType.Equal) return "равно";
            if (aExpType == AgeExpressionType.GreatEqual) return "больше/равно";
            if (aExpType == AgeExpressionType.GreatThen) return "больше";
            if (aExpType == AgeExpressionType.LessEqual) return "меньше/равно";
            if (aExpType == AgeExpressionType.LessThen) return "меньше";
            return "пусто";
        }
        #region Address
        [Display(Name = "Район проживания")]
        public int? DistrictId { get; set; }

        [Display(Name = "Населенный пункт проживания")]
        public int? LocalityId { get; set; }

        [Display(Name = "Улица проживания")]
        public int? StreetId { get; set; }

        [Display(Name = "Дом проживания")]
        public int? HouseId { get; set; }

        [Display(Name = "Квартира проживания")]
        public string Apartment { get; set; }

        [Display(Name = "УИК")]
        public int? PrecinctId { get; set; }
        #endregion

        [Display(Name = "Политический взгляд")]
        public int? PartyId { get; set; }

        public List<VoterViewModel> SearchResult { get; set; }
        public int SearchResultCount
        {
            get { return SearchResult != null ? SearchResult.Count : 0; }
        }
        public string SearchResultText
        {
            get { return "Результаты запроса: (" + (SearchResultCount >= 5 || SearchResultCount == 0 ? "найдено " + SearchResultCount + " записей" : SearchResultCount == 1 ? "найдена " + SearchResultCount + " запись" : "найдены " + SearchResultCount + " записи") + ")"; }
        }
    }
}