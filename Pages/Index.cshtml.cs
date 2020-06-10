using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace cf_loan_app.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public double LoanAmount { get; set; } = 15000;
        [BindProperty]
        public double LoanTermLength { get; set; } = 24;
        [BindProperty]
        public double LoanInterestRate { get; set; } = 3.75;

        public IEnumerable<MonthData> LoanSchedule { get; set; }
        public class MonthData
        {
            public int paymentNumber {get; set;}
            public double paymentAMT { get; set; }
            public double interestAMT {get; set;}
            public double principalAMT {get; set;}
            public double loanBalance {get; set;}
            public double interestTotal {get; set;}
        }


        public void OnGet()
        {

        }

        public void OnPost()
        {
            List<MonthData> ScheduleData = new List<MonthData>();

            double MonthlyInterestRate = (LoanInterestRate / 100) / 12;

            double MonthlyPaymentAmount = Math.Round(LoanAmount * ( MonthlyInterestRate + ( MonthlyInterestRate / ( Math.Pow((1 + MonthlyInterestRate), LoanTermLength) -1 ) ) ), 2);

            double CalculatedPaymentsTotal = ((LoanInterestRate / 1200) * LoanAmount * LoanTermLength) / (1 - Math.Pow((1 + LoanInterestRate / 1200), LoanTermLength * -1));

            double LoanBalance = LoanAmount;

            double InterestTotal = 0;

            double ActualPaymentsTotal = 0;

            int paymentNumber = 1;

            while (paymentNumber <= LoanTermLength)
            {
                var newMonth = new MonthData();

                newMonth.paymentAMT = MonthlyPaymentAmount;
                newMonth.paymentNumber = paymentNumber;
                newMonth.interestAMT = LoanBalance * MonthlyInterestRate;
                newMonth.principalAMT = newMonth.paymentAMT - newMonth.interestAMT;
                newMonth.interestTotal = InterestTotal + newMonth.interestAMT;
                newMonth.loanBalance = LoanBalance - newMonth.principalAMT;

                InterestTotal += newMonth.interestAMT;
                ActualPaymentsTotal += newMonth.paymentAMT;
                LoanBalance = newMonth.loanBalance;
                paymentNumber += 1;

                ScheduleData.Add(newMonth);
            }

            LoanSchedule = ScheduleData;
        }
    }
}
