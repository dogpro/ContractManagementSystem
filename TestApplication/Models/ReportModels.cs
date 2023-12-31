﻿using System;

namespace TestApplication.Models
{
    public class ReportModels
    {
        public class MoscowResident
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string DateOfBirth { get; set; }
        }

        public class ContractAmountByRussianResult
        {
            public string CompanyName { get; set; }
            public decimal TotalAmount { get; set; }
        }
    }
}