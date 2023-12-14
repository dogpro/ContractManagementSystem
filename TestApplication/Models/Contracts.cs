using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApplication.Models
{
    public class Contracts
    {
        [Key]
        public int ContractId { get; set; }
        [ForeignKey("Counterparty")]
        public int LegalEntityId { get; set; }
        public LegalEntity Counterparty { get; set; }
        [ForeignKey("AuthorizedIndividual")]
        public int IndividualId { get; set; }
        public Individual AuthorizedIndividual { get; set; }
        public decimal ContractAmount { get; set; }
        public string Status { get; set; }
        public DateTime SigningDate { get; set; }
    }
}