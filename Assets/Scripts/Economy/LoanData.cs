using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents data for a loan, including total value, principal value, interest rate, installment value, and installments value.
/// </summary>
/// <param name="Total">The total value of the loan.</param>
/// <param name="Principal">The principal value of the loan.</param>
/// <param name="Rate">The interest rate of the loan, in percent.</param>
/// <param name="Installments">The installments duration of the loan.</param>
/// <param name="LoanData.Type">The type of loan.</param>
[System.Serializable]
public struct LoanData
{
    [SerializeField] private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
        private set
        {
            _name = value;
        }
    }

    [SerializeField] private float _total;
    public float Total
    {
        get
        {
            return _total;
        }
        private set
        {
            if (value <= 0)
            {
                throw new Exception("Attempted to set the Total value to negative or null");
            }
            else
            {
                _total = value;
            }
        }
    }

    [SerializeField] private float _principal;
    public float Principal
    {
        get
        {
            return _principal;
        }
        private set
        {
            if (value <= 0)
            {
                throw new Exception("Attempted to set the Principal value to negative or null");
            }
            else if (value > Total)
            {
                throw new Exception("Attempted to set the Principal value to bigger than the Total");
            }
            else
            {
                _principal = value;
            }
        }
    }

    [SerializeField] private float _rate;
    public float Rate
    {
        get
        {
            return _rate;
        }
        private set
        {
            if (value <= 0)
            {
                throw new Exception("Attempted to set the Rate value to negative or null");
            }
            else
            {
                _rate = value;
            }
        }
    }

    [SerializeField] private int _installments;
    public int Installments
    {
        get
        {
            return _installments;
        }
        private set
        {
            if (value <= 0)
            {
                throw new Exception("Attempted to set the Installments value to negative or null");
            }
            else
            {
                _installments = value;
            }
        }
    }

    public LoanData.Type LoanType;
    public static UnityEvent<LoanData> OnLoanPaymentComplete;
    public static UnityEvent<LoanData> OnInstallmentPayment;

    public readonly float InstallmentValue
    {
        get
        {
            return Total / Installments;
        }
    }

    /// <summary>
    /// Represents the type of loan, which can be Simple Interest or Compound Interest.
    /// </summary>
    public enum Type
    {
        SimpleInterest,
        CompoundInterest
    }

    public LoanData(float principal, float rate, int installments, LoanData.Type type, string name = "") : this()
    {
        switch (type)
        {
            case LoanData.Type.SimpleInterest:
                this.Total = CalculateTotalFromSimpleInterest(principal, rate);
                break;
            default:
                this.Total = CalculateTotalFromCompoundInterest(principal, rate, installments);
                break;
        }

        this.Name = name;
        this.Principal = principal;
        this.Rate = rate;
        this.Installments = installments;
        this.LoanType = type;
    }

    /// <summary>
    /// Calculates the principal amount from a given total amount and simple interest rate.
    /// </summary>
    /// <param name="total">The total amount.</param>
    /// <param name="rate">The simple interest rate.</param>
    /// <returns>The principal amount.</returns>
    public static float CalculatePrincipalFromSimpleInterest(float total, float rate)
    {
        float _addedRate = rate + 1;
        float _principal = total / _addedRate;
        return _principal;
    }

    /// <summary>
    /// Calculates the principal amount from a given total amount, compound interest rate, and number of installments.
    /// </summary>
    /// <param name="total">The total amount.</param>
    /// <param name="rate">The compound interest rate.</param>
    /// <param name="installments">The number of installments.</param>
    /// <returns>The principal amount.</returns>
    public static float CalculatePrincipalFromCompoundInterest(float total, float rate, int installments)
    {
        float _compoundInterest = CalculateCompoundInterestRate(rate, installments);
        float _principal = total / _compoundInterest;
        return _principal;
    }

    public static float CalculateCompoundInterestRate(float rate, float installments)
    {
        float _addedRate = rate + 1;
        float _compoundInterest = Mathf.Pow(_addedRate, installments);
        return _compoundInterest;
    }

    /// <summary>
    /// Calculates the compound interest based on the principal, rate, and installments.
    /// </summary>
    /// <param name="principal">The loaned amount of money, without the interest.</param>
    /// <param name="rate">The interest rate per period, in percent.</param>
    /// <param name="installments">The number of periods the interest is applied.</param>
    /// <returns>A LoanData struct with the calculated values after applying compound interest.</returns>
    public static float CalculateTotalFromCompoundInterest(float principal, float rate, int installments)
    {
        float _factor = CalculateCompoundInterestRate(rate, installments);
        float _total = principal * _factor * rate * installments / (_factor - 1);
        return _total;
    }

    /// <summary>
    /// Calculates the simple interest based on the principal and rate.
    /// </summary>
    /// <param name="principal">The loaned amount of money, without the interest.</param>
    /// <param name="rate">The interest rate per period, in percent.</param>
    /// <returns>A LoanData struct with the calculated values after applying simple interest.</returns>
    public static float CalculateTotalFromSimpleInterest(float principal, float rate)
    {
        float _addedRate = rate + 1;
        float _total = principal * _addedRate;
        return _total;
    }

    public override string ToString()
    {
        return $"Loan Data:\n" +
            $"Total Value: {Total}\n" +
            $"Principal Value: {Principal}\n" +
            $"Interest Rate: {Rate * 100}%\n" +
            $"Installments: {Installments}\n" +
            $"Loan Type: {LoanType}\n";
    }
}