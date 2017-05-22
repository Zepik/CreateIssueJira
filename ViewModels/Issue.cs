using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CreateIssueJira.ViewModels
{
    public class Issue
    {  

        [Required(ErrorMessage="Pole <b>Podsumowanie</b> jest wymagane")]
        [StringLengthAttribute(200,ErrorMessage = "Pole <b>Opis</b> powinno mieć długość pomiędzy <b>{2}</b> a <b>{1}</b> znaków", MinimumLength = 5)]
        public string Summary { get; set; }
        [Required(ErrorMessage="Pole <b>Opis</b> jest wymagane")]
        [StringLengthAttribute(2000,ErrorMessage = "Pole <b>Opis</b> powinno mieć długość pomiędzy <b>{2}</b> a <b>{1}</b> znaków", MinimumLength = 10)]
        public string Description { get; set; }
        [Required(ErrorMessage="Pole <b>Zgłaszający</b> jest wymagane")]
        [Remote(action: "VerifyUser", controller: "Home")]
        public string Reporter { get; set; }

    }
    
}